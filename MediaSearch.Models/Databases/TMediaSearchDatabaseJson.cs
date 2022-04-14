using System.Runtime.CompilerServices;

using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models;

public class TMediaSearchDatabaseJson : IMediaSearchDatabase, IMediaSearchLoggable<TMediaSearchDatabaseJson> {

  public const int TIMEOUT_IN_MS = 5000;
  public const string JSON_HEADER = "header";
  public const string JSON_CONTENT = "medias";

  public IMediaSearchLogger<TMediaSearchDatabaseJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSearchDatabaseJson>();

  #region --- Public properties ------------------------------------------------------------------------------

  public IMediaSearchDatabaseHeader Header { get; } = new TMediaSearchDatabaseHeader();

  public string DatabasePath { get; init; } = "";
  public string DatabaseName { get; init; } = "";
  public string DatabaseFullName => Path.Join(DatabasePath, DatabaseName);

  public string HeaderFilename { get; init; } = "=header=.json";

  public bool IsDirty { get; set; } = false;
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the media in memory
  /// </summary>
  private readonly List<IMedia> _Items = new();

  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  private bool _IsOpened = false;

  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchDatabaseJson(string databasePath, string name) {
    DatabasePath = databasePath;
    DatabaseName = name;
    Header.Name = name;
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public TMediaSearchDatabaseJson(IMediaSearchDatabase database) {
    if (database is TMediaSearchDatabaseJson JsonDatabase) {
      DatabasePath = JsonDatabase.DatabasePath;
      DatabaseName = JsonDatabase.DatabaseName;
    }
    Header.Name = database.Header.Name;
    Header.Description = database.Header.Description;
    foreach (IMedia MediaItem in database.GetAll()) {
      _Items.Add(MediaItem);
    }
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public TMediaSearchDatabaseJson(IEnumerable<IMedia> medias) {
    foreach (IMedia MediaItem in medias) {
      MediaItem.SetDirty();
      _Items.Add(MediaItem);
    }
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public void Dispose() {
    Close();
    _Items.Clear();
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

    try {
      _LockData.EnterReadLock();
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"{IndentSpace}- {nameof(DatabasePath)} : {DatabasePath.WithQuotes()}");
      RetVal.AppendLine($"{IndentSpace}- {nameof(DatabaseName)} : {DatabaseName.WithQuotes()}");
      RetVal.AppendLine($"{IndentSpace}- {nameof(DatabaseFullName)} : {DatabaseFullName.WithQuotes()}");
      RetVal.AppendLine($"{IndentSpace}- {nameof(Header)}");
      RetVal.AppendLine($"{IndentSpace}{Header.ToString(2)}");
      RetVal.AppendLine($"{IndentSpace}- Content in memory : {_Items.Count} item(s)");
      RetVal.AppendLine($"{IndentSpace}- {nameof(AutoSave)} : {AutoSave}");
      return RetVal.ToString();
    } finally {
      _LockData.ExitReadLock();
    }

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

    item.SetDirty();

    try {
      _LockData.EnterWriteLock();
      _Items.Add(item);
      SetDirty();
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
      _Items.Add(item);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public void AddOrUpdate(IMedia item) {
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

      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      Save();
    }
  }

  public async Task AddOrUpdateAsync(IMedia item, CancellationToken token) {
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      Save();
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      Save();
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public void Clear() {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      Save();
    }
  }

  public async Task ClearAsync(CancellationToken token) {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
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

  public IEnumerable<IMedia> GetFiltered(IFilter filter) {

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
  public bool Load() {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    bool OldAutoSave = AutoSave;

    try {
      AutoSave = false;
      Clear();

      IEnumerable<string> Records = Directory.EnumerateFiles(DatabaseFullName, "*.json");
      foreach (string RecordItem in Records.AsParallel()) {
        string DataSourceContent = File.ReadAllText(RecordItem);
        JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
        JsonElement JsonMovie = JsonContent.RootElement;
        IMovie? Movie = IJson<TMovie>.FromJson(JsonMovie.GetRawText());
        if (Movie is not null) {
          Add(Movie);
        }
      }
      ClearDirty();
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to load data from {DatabaseFullName}", ex);
      return false;
    } finally {
      AutoSave = OldAutoSave;
    }
  }

  public async Task<bool> LoadAsync(CancellationToken token) {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    bool OldAutoSave = AutoSave;

    try {

      AutoSave = false;
      await ClearAsync(token).ConfigureAwait(false);

      IEnumerable<string> Records = Directory.EnumerateFiles(DatabaseFullName, "*.json");
      foreach (string RecordItem in Records.AsParallel()) {
        if (token.IsCancellationRequested) {
          return false;
        }
        string DataSourceContent = await File.ReadAllTextAsync(RecordItem, token);
        JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
        JsonElement JsonMovie = JsonContent.RootElement;
        IMovie? Movie = IJson<TMovie>.FromJson(JsonMovie.GetRawText());
        if (Movie is not null) {
          await AddAsync(Movie, token);
        }
      }
      ClearDirty();
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to load data from {DatabaseFullName}", ex);
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
      Logger.IfDebugMessageEx($"Saving database content to {DatabaseFullName}", $"{Count()} records");
      if (!_IsOpened) {
        return false;
      }
      if (!IsDirty) {
        return true;
      }

      try {

        foreach (IMovie MovieItem in GetAll().Where(m => m.IsDirty).AsParallel()) {
          string RecordName = $"{Path.Combine(DatabaseFullName, MovieItem.Id)}.json";
          using (FileStream OutputStream = new FileStream(RecordName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
            using (Utf8JsonWriter Writer = new Utf8JsonWriter(OutputStream, new JsonWriterOptions() { Indented = true, Encoder = IJson.DefaultJsonSerializerOptions.Encoder })) {
              JsonSerializer.Serialize(Writer, MovieItem, IJson.DefaultJsonSerializerOptions);
              Writer.Flush();
            }
          }
          MovieItem.ClearDirty();
        }
        ClearDirty();
        return true;

      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to commit changes to storage", ex);
        return false;

      }

    } finally {
      Logger.IfDebugMessageEx($"Database content saved to {DatabaseFullName}", $"{Count()} records");
    }

  }

  public async Task<bool> SaveAsync(CancellationToken token) {

    try {
      Logger.IfDebugMessageEx($"Saving database content to {DatabaseFullName}", $"{_Items.Count} records");

      if (!_IsOpened) {
        return false;
      }
      if (!IsDirty) {
        return true;
      }

      try {

        await foreach (IMovie MovieItem in GetAllAsync(token).Where(m => m.IsDirty).ConfigureAwait(false)) {
          if (token.IsCancellationRequested) {
            return false;
          }
          string RecordName = $"{Path.Combine(DatabaseFullName, MovieItem.Id)}.json";
          using (MemoryStream OutputStream = new MemoryStream()) {
            await JsonSerializer.SerializeAsync(OutputStream, MovieItem, IJson.DefaultJsonSerializerOptions, token).ConfigureAwait(false);
            await File.WriteAllBytesAsync(RecordName, OutputStream.ToArray(), token).ConfigureAwait(false);
          }
          MovieItem.ClearDirty();
        }
        ClearDirty();
        return true;

      } catch (Exception ex) {
        Logger.LogErrorBox("Unable to commit changes to storage", ex);
        return false;

      }

    } finally {
      Logger.IfDebugMessageEx($"Database content saved asynchronously to {DatabaseFullName}", $"{Count()} records");
    }

  }
  #endregion --- Save --------------------------------------------

  public bool Open() {
    if (_IsOpened) {
      return true;
    }
    try {

      Logger.IfDebugMessageEx("Opening json database", DatabaseFullName);
      if (Exists()) {
        _IsOpened = true;
        return true;
      }

      try {
        Create();
        _IsOpened = true;
        ClearDirty();
        return true;
      } catch (Exception ex) {
        _IsOpened = false;
        Logger.LogErrorBox($"Unable to create {DatabaseFullName}", ex);
        return false;
      }

    } finally {
      Logger.IfDebugMessageEx("Json database status after open", this);
    }


  }

  public void Close() {

    try {
      Logger.IfDebugMessageEx("Closing json database", DatabaseFullName);
      if (_IsOpened && IsDirty) {
        Save();
      }
      _IsOpened = false;
      ClearDirty();
    } finally {
      Logger.IfDebugMessageEx("Json database status", this);
    }

  }

  public async Task CloseAsync(CancellationToken token) {
    try {
      Logger.IfDebugMessageEx("Closing json database async", DatabaseFullName);
      if (_IsOpened && IsDirty) {
        await SaveAsync(token).ConfigureAwait(false);
      }
      _IsOpened = false;
      ClearDirty();
    } finally {
      Logger.IfDebugMessageEx("Json database status", this);
    }

  }

  public bool Create() {
    if (Exists()) {
      return true;
    }
    try {
      Directory.CreateDirectory(DatabaseFullName);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to create database {DatabaseFullName}", ex);
      return false;
    }
  }

  public void Remove() {
    try {
      Logger.IfDebugMessageEx("Removing json database", DatabaseFullName);
      if (!Exists()) {
        Logger.LogWarningBox("Unable to remove database", this);
        return;
      }
      try {
        Directory.Delete(DatabaseFullName, true);
        return;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to remove database {DatabaseFullName}", ex);
        throw;
      }
    } finally {
      Logger.IfDebugMessageEx("Json database status", $"Database exists : {Exists()}");
    }

  }

  public bool Exists() {
    if (string.IsNullOrEmpty(DatabaseFullName)) {
      Logger.LogWarning($"Unable to test for existence : Missing {nameof(DatabaseFullName)}");
      return false;
    }
    return Directory.Exists(DatabaseFullName);
  }
  #endregion --- I/O --------------------------------------------

  #region --- Dirty indicator --------------------------------------------
  public void SetDirty() {
    IsDirty = true;
    Header.LastUpdate = DateTime.Now;
  }

  public void ClearDirty() {
    IsDirty = false;
  }
  #endregion --- Dirty indicator --------------------------------------------
}
