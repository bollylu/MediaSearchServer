using System.Runtime.CompilerServices;

using MediaSearch.Database;

using SkiaSharp;

namespace MediaSearch.Server.Services;

// <summary>
/// Server Movie service. Provides access to groups, movies and pictures from NAS
/// </summary>
public class XMovieService : IMovieService, IName, IMediaSearchLoggable<XMovieService> {

  #region --- Constants --------------------------------------------
  public static int TIMEOUT_TO_SCAN_FILES_IN_MS = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
  public static int TIMEOUT_TO_CONVERT_IN_MS = 5000;
  public static char FOLDER_SEPARATOR = Path.DirectorySeparatorChar;
  #endregion --- Constants --------------------------------------------

  public IMediaSearchLogger<XMovieService> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<XMovieService>();

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

  public IMSTable<IMovie> MovieTable { get; protected set; } = new TMSTable<IMovie>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public XMovieService(IMSTable<IMovie> table) {
    MovieTable = table;
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
    using (CancellationTokenSource Timeout = new CancellationTokenSource((int)TimeSpan.FromMinutes(5).TotalMilliseconds)) {
      await ParseAsync(Timeout.Token).ConfigureAwait(false);
    }

    _IsInitialized = true;
    _IsInitializing = false;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public void Reset() {
  }

  public Task RefreshData() {
    return Task.CompletedTask;
  }

  public int GetRefreshStatus() {
    return -1;
  }

  #region --- Movies --------------------------------------------
  public async ValueTask<int> MoviesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    return MovieTable.GetFiltered(filter).Count();
  }

  public async ValueTask<int> PagesCount(IFilter filter) {
    await Initialize().ConfigureAwait(false);
    int FilteredMoviesCount = await MoviesCount(filter).ConfigureAwait(false);
    return (FilteredMoviesCount / filter.PageSize) + (FilteredMoviesCount % filter.PageSize > 0 ? 1 : 0);
  }

  public async IAsyncEnumerable<TMovie> GetAllMovies() {
    await Initialize().ConfigureAwait(false);

    foreach (TMovie MovieItem in MovieTable.GetAll()) {
      yield return MovieItem;
    }
  }

  public async Task<TMoviesPage> GetMoviesPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);

    IEnumerable<IMovie> FilteredMovies = MovieTable.GetFiltered(filter).OrderedBy(filter).Cast<IMovie>();

    TMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = filter.Page
    };

    RetVal.AvailableMovies = FilteredMovies.Count();
    RetVal.AvailablePages = (RetVal.AvailableMovies / filter.PageSize) + (RetVal.AvailableMovies % filter.PageSize > 0 ? 1 : 0);
    RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));

    return RetVal;
  }

  public async Task<TMoviesPage> GetMoviesLastPage(IFilter filter) {
    await Initialize().ConfigureAwait(false);

    IEnumerable<IMovie> FilteredMovies = MovieTable.GetFiltered(filter).OrderedBy(filter).Cast<IMovie>();
    int MoviesCount = FilteredMovies.Count();

    TMoviesPage RetVal = new TMoviesPage() {
      Source = RootStoragePath,
      Page = (MoviesCount / filter.PageSize) + (MoviesCount % filter.PageSize > 0 ? 1 : 0),
      AvailableMovies = MoviesCount,
      AvailablePages = (MoviesCount / filter.PageSize) + (MoviesCount % filter.PageSize > 0 ? 1 : 0)
    };

    RetVal.Movies.AddRange(FilteredMovies.Skip(filter.PageSize * (filter.Page - 1)).Take(filter.PageSize));

    return RetVal;
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

  public Task<byte[]> GetPicture(string id, string pictureName, int width, int height) {
    return Task.FromResult(Array.Empty<byte>());
  }

  #region --- Cache I/O --------------------------------------------
  protected IEnumerable<IFileInfo> _FetchFiles() {
    yield break;
  }

  protected async IAsyncEnumerable<IFileInfo> _FetchFilesAsync([EnumeratorCancellation] CancellationToken token) {
    await Task.Yield();
    yield break;
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
    Logger.Log($"Initializing database from {nameof(RootStoragePath)} {RootStoragePath.WithQuotes()}");

    if (string.IsNullOrWhiteSpace(RootStoragePath)) {
      throw new ApplicationException("Storage is missing. Cannot process movies");
    }

    MovieTable.Clear();

    int Progress = 0;

    await foreach (IFileInfo MovieInfoItem in _FetchFilesAsync(token)) {
      if (token.IsCancellationRequested) {
        return;
      }

      Progress++;

      try {
        IMovie NewMovie = await _ParseEntryAsync(MovieInfoItem, token).ConfigureAwait(false);
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
        IMovie NewMovie = await _ParseEntryAsync(FileItem, token).ConfigureAwait(false);
        NewMovie.DateAdded = DateOnly.FromDateTime(FileItem.ModificationDate);
        Logger.LogDebugEx($"Found {FileItem.FullName}");
        MovieTable.Add(NewMovie);
      } catch (Exception ex) {
        Logger.LogWarning($"Unable to parse movie {FileItem} : {ex.Message}");
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

  protected Task<IMovie> _ParseEntryAsync(IFileInfo item, CancellationToken token) {

    if (token.IsCancellationRequested) {
      return Task.FromCanceled<IMovie>(token);
    }

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

    return Task.FromResult(RetVal);
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
  #endregion --- Cache I/O --------------------------------------------
}

