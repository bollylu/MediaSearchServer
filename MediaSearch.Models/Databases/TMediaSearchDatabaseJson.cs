using System.Runtime.CompilerServices;

using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models;

public class TMediaSearchDatabaseJson : IMediaSearchDatabasePersistent, IMediaSearchLoggable<TMediaSearchDatabaseJson> {

  public IMediaSearchLogger<TMediaSearchDatabaseJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSearchDatabaseJson>();

  #region --- Public properties ------------------------------------------------------------------------------
  #region --- IName --------------------------------------------
  public string Name {
    get {
      return _Name ??= FullStorageFilename;
    }
    set {
      _Name = value;
    }
  }
  private string? _Name;
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  public string StoragePath { get; init; } = "";
  public string StorageFilename { get; init; } = "";

  public string FullStorageFilename => Path.Join(StoragePath, StorageFilename);
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the media in memory
  /// </summary>
  private readonly List<IMedia> _Items = new();

  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  private bool _IsOpened = false;
  private bool _IsDirty = false;
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchDatabaseJson() {
    Logger.SeverityLimit = ESeverity.DebugEx;
  }

  public TMediaSearchDatabaseJson(IMediaSearchDatabase database) : this() {
    foreach (IMedia MediaItem in database.GetAll()) {
      _Items.Add(MediaItem);
    }
    _IsDirty = true;
  }

  public TMediaSearchDatabaseJson(IEnumerable<IMedia> medias) : this() {
    foreach (IMedia MediaItem in medias) {
      _Items.Add(MediaItem);
    }
    _IsDirty = true;
  }

  public async ValueTask DisposeAsync() {
    await CloseAsync(CancellationToken.None);
    _Items.Clear();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    indent = indent.WithinLimits(0, int.MaxValue);
    string IndentSpace = new string(' ', indent);

    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{IndentSpace}{nameof(TMediaSearchDatabaseJson)} : {Name.WithQuotes()}");
    if (!string.IsNullOrWhiteSpace(Description)) {
      RetVal.AppendLine($"{IndentSpace}  {nameof(Description)} : {Description.WithQuotes()}");
    }
    RetVal.AppendLine($"{IndentSpace}  {nameof(StoragePath)} : {StoragePath.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}  {nameof(StorageFilename)} : {StorageFilename.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}  {nameof(FullStorageFilename)} : {FullStorageFilename.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}  Content in memory : {_Items.Count} item(s)");
    RetVal.AppendLine($"{IndentSpace}  {nameof(AutoSave)} : {AutoSave}");
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- Content information --------------------------------------------
  public bool Any() {
    try {
      _LockData.EnterReadLock();
      return _Items.Any();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public bool IsEmpty() {
    try {
      _LockData.EnterReadLock();
      return _Items.IsEmpty();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public int Count() {
    try {
      _LockData.EnterReadLock();
      return _Items.Count;
    } finally {
      _LockData.ExitReadLock();
    }
  }
  #endregion --- Content information --------------------------------------------

  #region --- Content management --------------------------------------------

  public void Add(IMedia item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        _Items.Add(item);
      } else {
        _Items[ItemIndex] = item;
      }
      _IsDirty = true;
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      Save();
    }
  }

  public async Task AddAsync(IMedia item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        _Items.Add(item);
      } else {
        _Items[ItemIndex] = item;
      }
      _IsDirty = true;

      if (AutoSave) {
        await SaveAsync(token).ConfigureAwait(false);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void AddOrUpdate(IMedia item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      _Items.Add(item);
      _IsDirty = true;

      if (AutoSave) {
        Save();
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public async Task AddOrUpdateAsync(IMedia item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      _Items.Add(item);
      _IsDirty = true;

      if (AutoSave) {
        await SaveAsync(token);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Update(IMedia item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);
      _IsDirty = true;

      if (AutoSave) {
        Save();
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public async Task UpdateAsync(IMedia item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);
      _IsDirty = true;

      if (AutoSave) {
        await SaveAsync(token);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Delete(IMedia item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to delete is not found : {item.Id}");
      }
      _Items.RemoveAt(ItemIndex);
      _IsDirty = true;

      if (AutoSave) {
        Save();
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public async Task DeleteAsync(IMedia item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to delete is not found : {item.Id}");
      }
      _Items.RemoveAt(ItemIndex);
      _IsDirty = true;

      if (AutoSave) {
        await SaveAsync(token).ConfigureAwait(false);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Clear() {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      if (AutoSave) {
        Save();
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public async Task ClearAsync(CancellationToken token) {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      if (AutoSave) {
        await SaveAsync(token).ConfigureAwait(false);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public IMedia? Get(string id) {
    if (string.IsNullOrWhiteSpace(id)) {
      throw new ArgumentNullException(nameof(id));
    }

    try {
      _LockData.EnterReadLock();
      return _Items.FirstOrDefault(x => x.Id == id);
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public IEnumerable<IMedia> GetAll() {

    if (IsEmpty()) {
      yield break;
    }

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMedia>(_Items);
    } finally {
      _LockData.ExitReadLock();
    }

    foreach (IMedia MediaItem in LocalItems) {
      yield return MediaItem;
    }
  }

  public async IAsyncEnumerable<IMedia> GetAllAsync([EnumeratorCancellation] CancellationToken token) {

    if (IsEmpty()) {
      yield break;
    }

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMedia>(_Items);
    } finally {
      _LockData.ExitReadLock();
    }

    await foreach (IMedia MediaItem in LocalItems.ToAsyncEnumerable().WithCancellation(token)) {
      yield return MediaItem;
    }
  }

  public IEnumerable<IMedia> GetFiltered(TFilter filter) {

    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMedia>(_Items.WithFilter(filter).OrderedBy(filter));
    } finally {
      _LockData.ExitReadLock();
    }

    foreach (IMedia MediaItem in LocalItems) {
      yield return MediaItem;
    }
  }

  public async IAsyncEnumerable<IMedia> GetFilteredAsync(TFilter filter, [EnumeratorCancellation] CancellationToken token) {

    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMedia>(_Items.WithFilter(filter).OrderedBy(filter));
    } finally {
      _LockData.ExitReadLock();
    }

    await foreach (IMedia MediaItem in LocalItems.ToAsyncEnumerable().WithCancellation(token)) {
      yield return MediaItem;
    }
  }

  #endregion --- Content management --------------------------------------------

  #region --- I/O --------------------------------------------
  public const int IO_TIMEOUT_IN_MS = 5000;

  #region --- Load --------------------------------------------
  public virtual bool Load() {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    bool OldAutoSave = AutoSave;

    try {
      _LockData?.EnterReadLock();

      AutoSave = false;
      Clear();

      string DataSourceContent = File.ReadAllText(FullStorageFilename);
      JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
      JsonElement JsonMovies = JsonContent.RootElement;
      foreach (JsonElement JsonMovieItem in JsonMovies.GetProperty("movies").EnumerateArray()) {
        IMovie? Movie = IJson<TMovie>.FromJson(JsonMovieItem.GetRawText());
        if (Movie is not null) {
          Add(Movie);
        }
      }
      _IsDirty = false;
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to load data from {FullStorageFilename}", ex);
      return false;
    } finally {
      _LockData?.ExitReadLock();
      AutoSave = OldAutoSave;
    }
  }

  public virtual async Task<bool> LoadAsync(CancellationToken token) {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    bool OldAutoSave = AutoSave;

    try {

      AutoSave = false;
      await ClearAsync(token).ConfigureAwait(false);

      string DataSourceContent = await File.ReadAllTextAsync(FullStorageFilename, token).ConfigureAwait(false);
      JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
      JsonElement JsonMovies = JsonContent.RootElement;
      foreach (JsonElement JsonMovieItem in JsonMovies.GetProperty("movies").EnumerateArray()) {
        IMovie? Movie = IJson<TMovie>.FromJson(JsonMovieItem.GetRawText());
        if (Movie is not null) {
          await AddAsync(Movie, CancellationToken.None).ConfigureAwait(false);
        }
      }
      _IsDirty = false;
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to load data from {FullStorageFilename}", ex);
      return false;
    } finally {
      AutoSave = OldAutoSave;
    }
  }
  #endregion --- Load --------------------------------------------

  #region --- Save --------------------------------------------
  public bool AutoSave { get; set; } = false;

  public bool Save() {

    try {
      Logger.IfDebugMessageEx($"Saving database content to {FullStorageFilename}", $"{Count()} records");
      if (!_IsOpened) {
        return false;
      }
      if (!_IsDirty) {
        return true;
      }

      StringBuilder RawContent = new();

      try {
        _LockData.EnterReadLock();
        RawContent.AppendLine("{\n \"medias\" : [\n");
        foreach (IJson MovieItem in _Items) {
          RawContent.Append(MovieItem.ToJson());
          RawContent.AppendLine(",");
        }
        RawContent.Truncate(1);
        RawContent.AppendLine("\n]\n}");
      } finally {
        _LockData.ExitReadLock();
      }

      try {
        File.WriteAllText(FullStorageFilename, RawContent.ToString());
        _IsDirty = false;
        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to commit changes to storage", ex);
        return false;
      }
    } finally {
      Logger.IfDebugMessageEx($"Database content saved to {FullStorageFilename}", $"{Count()} records");
    }

  }

  public async Task<bool> SaveAsync(CancellationToken token) {

    try {
      Logger.IfDebugMessageEx($"Saving database content to {FullStorageFilename}", $"{Count()} records");

      if (!_IsOpened) {
        return false;
      }
      if (!_IsDirty) {
        return true;
      }

      StringBuilder RawContent = new();

      try {
        _LockData.EnterReadLock();
        RawContent.AppendLine("{\n \"medias\" : [\n");
        foreach (IJson MovieItem in _Items) {
          RawContent.Append(MovieItem.ToJson());
          RawContent.AppendLine(",");
        }
        RawContent.Truncate(1);
        RawContent.AppendLine("\n]\n}");
      } finally {
        _LockData.ExitReadLock();
      }

      try {
        await File.WriteAllTextAsync(FullStorageFilename, RawContent.ToString(), token).ConfigureAwait(false);
        _IsDirty = false;
        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to commit changes to storage", ex);
        return false;
      }
    } finally {
      Logger.IfDebugMessageEx($"Database content saved asynchronously to {FullStorageFilename}", $"{Count()} records");
    }

  }
  #endregion --- Save --------------------------------------------

  public bool Open() {
    if (_IsOpened) {
      return true;
    }
    try {

      Logger.IfDebugMessageEx("Opening json database", FullStorageFilename);
      if (Exists()) {
        _IsOpened = true;
        _IsDirty = false;
        return true;
      }

      try {
        FileStream Db = File.Create(FullStorageFilename);
        Db.Close();
        _IsOpened = true;
        _IsDirty = false;
        return true;
      } catch (Exception ex) {
        _IsOpened = false;
        Logger.LogErrorBox($"Unable to create {FullStorageFilename}", ex);
        return false;
      }

    } finally {
      Logger.IfDebugMessageEx("Json database status", this);
    }


  }

  public void Close() {

    try {
      Logger.IfDebugMessageEx("Closing json database", FullStorageFilename);
      if (_IsOpened && _IsDirty) {
        Save();
      }
      _IsOpened = false;
      _IsDirty = false;
    } finally {
      Logger.IfDebugMessageEx("Json database status", this);
    }

  }

  public async Task CloseAsync(CancellationToken token) {
    try {
      Logger.IfDebugMessageEx("Closing json database async", FullStorageFilename);
      if (_IsOpened && _IsDirty) {
        await SaveAsync(token).ConfigureAwait(false);
      }
      _IsOpened = false;
      _IsDirty = false;
    } finally {
      Logger.IfDebugMessageEx("Json database status", this);
    }

  }

  public void Remove() {
    try {
      Logger.IfDebugMessageEx("Removing json database", FullStorageFilename);
      if (!Exists()) {
        Logger.LogWarningBox("Unable to remove database", this.ToString());
        return;
      }
      try {
        File.Delete(FullStorageFilename);
        return;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to remove database {FullStorageFilename}", ex);
        throw;
      }
    } finally {
      Logger.IfDebugMessageEx("Json database status", $"Database exists : {Exists()}");
    }

  }

  public bool Exists() {
    if (string.IsNullOrEmpty(FullStorageFilename)) {
      Logger.LogWarning($"Unable to test for existence : Missing {nameof(FullStorageFilename)}");
      return false;
    }
    return File.Exists(FullStorageFilename);
  }
  #endregion --- I/O --------------------------------------------

}
