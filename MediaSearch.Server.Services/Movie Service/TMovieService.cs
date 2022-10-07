using System.Runtime.CompilerServices;

using SkiaSharp;

namespace MediaSearch.Server.Services;

/// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class TMovieService : IMovieService, IName, ILoggable {

  #region --- Constants --------------------------------------------
  public static int TIMEOUT_TO_SCAN_FILES_IN_MS = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
  public static int TIMEOUT_TO_CONVERT_IN_MS = 5000;
  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;
  #endregion --- Constants --------------------------------------------

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMovieService>();

  #region --- IName --------------------------------------------
  /// <summary>
  /// The name of the source
  /// </summary>
  public string Name { get; set; } = "";

  /// <summary>
  /// The description of the source
  /// </summary>
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  /// <summary>
  /// Root path to look for data
  /// </summary>
  public string RootStoragePath { get; init; } = "";

  /// <summary>
  /// The extensions of the files of interest
  /// </summary>
  public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovieService() {
  }

  public TMovieService(string storage) : this() {
    RootStoragePath = storage;
  }

  private bool _IsInitialized = false;
  private bool _IsInitializing = false;
  public async Task Initialize() {
    if (_IsInitialized) {
      return;
    }

    if (_IsInitializing) {
      return;
    }

    if (MovieTable.Any()) {
      return;
    }

    _IsInitializing = true;

    Logger.Log($"Parsing data source : {RootStoragePath}");
    using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_TO_SCAN_FILES_IN_MS)) {
      await ParseAsync(Timeout.Token).ConfigureAwait(false);
    }

    _IsInitialized = true;
    _IsInitializing = false;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();

    RetVal.AppendIndent($"{nameof(Name)} : {Name.WithQuotes()}", indent);
    RetVal.AppendIndent($"{nameof(Description)} : {Description.WithQuotes()}", indent);
    RetVal.AppendIndent($"{nameof(RootStoragePath)} : {RootStoragePath.WithQuotes()}", indent);
    RetVal.AppendIndent($"{nameof(MoviesExtensions)} : {string.Join(", ", MoviesExtensions.Select(x => x.WithQuotes()))}", indent);
    RetVal.AppendIndent($"{nameof(MovieTable)} :", indent);
    RetVal.AppendIndent($"{MovieTable.ToString(2)}", indent);

    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Reset() {
    //MovieTable.Clear();
    _IsInitialized = false;
  }

  public Task RefreshData() {
    Reset();
    return Task.Run(async () => await Initialize());
  }

  public int GetRefreshStatus() {
    if (_IsInitialized) {
      return -1;
    }
    //return (int)MovieTable.Count();
    return 0;
  }

  #region --- Movies --------------------------------------------
  public async ValueTask<int> MoviesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return MovieTable.GetFiltered(filter).Count();
  }

  public async ValueTask<int> PagesCount(IFilter filter) {
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public async IAsyncEnumerable<TMovie> GetAllMovies() {
    await Initialize().ConfigureAwait(false);

    foreach (TMovie MovieItem in MovieTable.GetAll()) {
      yield return MovieItem;
    }
  }

  public Task<TMoviesPage> GetMoviesPage(IFilter filter) {

    IList<IMovie> FilteredMovies = MovieTable.GetFiltered(filter).Cast<IMovie>().ToList();

    TMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = filter.Page
    };

    RetVal.AvailableMovies = FilteredMovies.Count;
    RetVal.AvailablePages = (RetVal.AvailableMovies / filter.PageSize) + (RetVal.AvailableMovies % filter.PageSize > 0 ? 1 : 0);
    RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));

    return Task.FromResult(RetVal);
  }

  public Task<TMoviesPage> GetMoviesLastPage(IFilter filter) {

    IList<IMovie> FilteredMovies = MovieTable.GetFiltered(filter).Cast<IMovie>().ToList();

    TMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = (FilteredMovies.Count / filter.PageSize) + (FilteredMovies.Count % filter.PageSize > 0 ? 1 : 0),
      AvailableMovies = FilteredMovies.Count,
      AvailablePages = (FilteredMovies.Count / filter.PageSize) + (FilteredMovies.Count % filter.PageSize > 0 ? 1 : 0)
    };

    RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));

    return Task.FromResult(RetVal);
  }

  public Task<IMovie?> GetMovie(string id) {
    if (string.IsNullOrWhiteSpace(id)) {
      Logger.LogWarning("Unable to retrieve movie : id is null or invalid");
      return Task.FromResult<IMovie?>(null);
    }
    IMedia? Media = MovieTable.Get(id);
    return Task.FromResult<IMovie?>(Media as IMovie);
  }
  #endregion --- Movies --------------------------------------------

  public async IAsyncEnumerable<string> GetGroups() {
    await Initialize().ConfigureAwait(false);

    await foreach (string GroupItem in MovieTable.GetAll().GetGroups().ConfigureAwait(false)) {
      yield return GroupItem;
    }
  }



  public async Task<byte[]> GetPicture(string movieId,
                                       string pictureName,
                                       int width,
                                       int height) {

    #region === Validate parameters ===
    string ParamPictureName = pictureName ?? IMovieService.DEFAULT_PICTURE_NAME;
    int ParamWidth = width.WithinLimits(IMovieService.MIN_PICTURE_WIDTH, IMovieService.MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(IMovieService.MIN_PICTURE_HEIGHT, IMovieService.MAX_PICTURE_HEIGHT);
    if (string.IsNullOrWhiteSpace(movieId)) {
      Logger.LogError("Unable to fetch picture : id is null or invalid");
      return Array.Empty<byte>();
    }
    #endregion === Validate parameters ===

    IMedia? Movie = MovieTable.Get(movieId);
    if (Movie is null) {
      Logger.LogError($"Unable to fetch picture id \"{movieId}\"");
      return Array.Empty<byte>();
    }

    string FullPicturePath = Path.Join(RootStoragePath.NormalizePath(), Movie.StoragePath.NormalizePath(), ParamPictureName);

    Logger.LogDebug($"GetPicture {FullPicturePath} : size({ParamWidth}, {ParamHeight})");
    if (!File.Exists(FullPicturePath)) {
      Logger.LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
      return Array.Empty<byte>();
    }

    try {
      using CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_TO_CONVERT_IN_MS);
      using FileStream SourceStream = new FileStream(FullPicturePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
      using MemoryStream PictureStream = new MemoryStream();
      await SourceStream.CopyToAsync(PictureStream, Timeout.Token);
      PictureStream.Seek(0, SeekOrigin.Begin);
      SKImage Image = SKImage.FromEncodedData(PictureStream);
      SKBitmap Picture = SKBitmap.FromImage(Image);
      SKBitmap ResizedPicture = Picture.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
      SKData Result = ResizedPicture.Encode(SKEncodedImageFormat.Jpeg, 100);
      using (MemoryStream OutputStream = new()) {
        Result.SaveTo(OutputStream);
        return OutputStream.ToArray();
      }
    } catch (Exception ex) {
      Logger.LogError($"Unable to fetch picture {FullPicturePath} : {ex.Message}");
      if (ex.InnerException is not null) {
        Logger.LogError($"  {ex.InnerException.Message}");
      }
      return Array.Empty<byte>();
    }
  }

  #region --- Cache I/O --------------------------------------------

  protected IEnumerable<IFileInfo> _FetchFiles() {
    DirectoryInfo RootFolder = new DirectoryInfo(RootStoragePath);

    return RootFolder.EnumerateFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                     .Where(f => f.Extension.ToLowerInvariant().IsIn(MoviesExtensions))
                     .Select(f => new TFileInfo(f));
  }

  protected async IAsyncEnumerable<IFileInfo> _FetchFilesAsync([EnumeratorCancellation] CancellationToken token) {
    DirectoryInfo RootFolder = new DirectoryInfo(RootStoragePath);

    IAsyncEnumerable<IFileInfo> Files = RootFolder.EnumerateFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
      .Where(f => f.Extension.ToLowerInvariant().IsIn(MoviesExtensions))
      .Select(f => new TFileInfo(f))
      .ToAsyncEnumerable();

    await foreach (IFileInfo FileInfoItem in Files) {

      if (token.IsCancellationRequested) {
        yield break;
      }

      yield return FileInfoItem;
    }
  }

  public void Parse() {
    Logger.Log($"Initializing database from {nameof(RootStoragePath)} : {RootStoragePath.WithQuotes()}");

    if (string.IsNullOrWhiteSpace(RootStoragePath)) {
      throw new ApplicationException("Storage is missing. Cannot process movies");
    }

    MovieTable.Clear();

    int Progress = 0;

    foreach (IFileInfo MovieInfoItem in _FetchFiles()) {

      Progress++;

      try {
        IMovie NewMovie = _ParseEntry(MovieInfoItem);
        NewMovie.DateAdded = DateOnly.FromDateTime(MovieInfoItem.ModificationDate);
        Logger.LogDebugEx($"Found {MovieInfoItem.FullName}");
        MovieTable.Add(NewMovie);
      } catch (Exception ex) {
        Logger.LogWarning($"Unable to parse movie {MovieInfoItem} : {ex.Message}");
        if (ex.InnerException is not null) {
          Logger.LogWarning($"  {ex.InnerException.Message}");
        }
      }

      if (Progress % 250 == 0) {
        Logger.Log($"Processed {Progress} movies...");
      }

    }

    Logger.Log($"Cache initialized successfully : {Progress} movies");

    return;
  }

  public async Task ParseAsync(CancellationToken token) {
    Logger.Log($"Initializing database from {nameof(RootStoragePath)} : {RootStoragePath.WithQuotes()}");

    if (string.IsNullOrWhiteSpace(RootStoragePath)) {
      throw new ApplicationException("Storage is missing. Cannot process movies");
    }

    MovieTable.Clear();

    int Progress = 0;

    await foreach (IFileInfo MovieInfoItem in _FetchFilesAsync(token).ConfigureAwait(false)) {
      if (token.IsCancellationRequested) {
        return;
      }

      Progress++;

      try {
        IMovie NewMovie = await _ParseEntryAsync(MovieInfoItem);
        NewMovie.DateAdded = DateOnly.FromDateTime(MovieInfoItem.ModificationDate);
        Logger.LogDebugEx($"Found {MovieInfoItem.FullName}");
        MovieTable.Add(NewMovie);
      } catch (Exception ex) {
        Logger.LogWarning($"Unable to parse movie {MovieInfoItem.FullName.WithQuotes()} : {ex.Message}");
        if (ex.InnerException is not null) {
          Logger.LogWarning($"  {ex.InnerException.Message}");
        }
      }

      if (Progress % 250 == 0) {
        Logger.Log($"Processed {Progress} movies...");
      }

    }

    Logger.Log($"Cache initialized successfully : {Progress} movies");

    return;
  }

  public async Task ParseAsync(IEnumerable<IFileInfo> fileSource, CancellationToken token) {
    Logger.Log($"Initializing database from {nameof(fileSource)}, {fileSource.Count()} item(s) to parse ");
    MovieTable.Clear();
    int Progress = 0;

    foreach (IFileInfo FileItem in fileSource) {
      if (token.IsCancellationRequested) {
        return;
      }

      Progress++;
      try {
        IMovie NewMovie = await _ParseEntryAsync(FileItem).ConfigureAwait(false);
        NewMovie.DateAdded = DateOnly.FromDateTime(FileItem.ModificationDate);
        Logger.LogDebugEx($"Found {FileItem.FullName}");
        MovieTable.Add(NewMovie);
      } catch (Exception ex) {
        Logger.LogWarning($"Unable to parse movie {FileItem.FullName.WithQuotes()} : {ex.Message}");
        if (ex.InnerException is not null) {
          Logger.LogWarning($"  {ex.InnerException.Message}");
        }
      }

      if (Progress % 250 == 0) {
        Logger.Log($"Processed {Progress} movies...");
      }
    }

    Logger.Log($"Cache initialized successfully : {Progress} movies");

    return;
  }

  protected IMovie _ParseEntry(IFileInfo item) {

    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    IMovie RetVal = new TMovie(ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    RetVal.StorageRoot = RootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = RetVal.StoragePath
                                      .BeforeLast(FOLDER_SEPARATOR)
                                      .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #")).Select(t => t.TrimEnd(' ', '#'));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarning($"Unable to find output year : {ex.Message} : {item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    RetVal.Size = item.Length;

    return RetVal;
  }

  protected Task<IMovie> _ParseEntryAsync(IFileInfo item) {

    // Standardize directory separator
    string ProcessedFileItem = item.FullName.NormalizePath();

    IMovie RetVal = new TMovie(Name = ProcessedFileItem.AfterLast(FOLDER_SEPARATOR).BeforeLast(" ("));

    RetVal.StorageRoot = RootStoragePath.NormalizePath();
    RetVal.StoragePath = ProcessedFileItem.BeforeLast(FOLDER_SEPARATOR).After(RetVal.StorageRoot, System.StringComparison.InvariantCultureIgnoreCase);

    RetVal.FileName = item.Name;
    RetVal.FileExtension = RetVal.FileName.AfterLast('.').ToLowerInvariant();

    IEnumerable<string> Tags = RetVal.StoragePath
                                     .BeforeLast(FOLDER_SEPARATOR)
                                     .Split(FOLDER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

    foreach (string TagItem in Tags.Where(t => !t.EndsWith(" #"))) {
      RetVal.Tags.Add(TagItem.TrimStart('[').TrimEnd(']'));
    }

    IEnumerable<string> GroupTags = Tags.Where(t => t.EndsWith(" #")).Select(t => t.TrimEnd(' ', '#'));
    RetVal.Group = string.Join("/", GroupTags);

    try {
      RetVal.CreationDate = new DateOnly(int.Parse(RetVal.FileName.AfterLast('(').BeforeLast(')')), 1, 1);
    } catch (FormatException ex) {
      Logger.LogWarning($"Unable to find output year : {ex.Message} : {item.FullName}");
      RetVal.CreationDate = DateOnly.MinValue;
    }

    RetVal.Size = item.Length;

    return Task.FromResult(RetVal);
  }
  #endregion --- Cache I/O --------------------------------------------
}

