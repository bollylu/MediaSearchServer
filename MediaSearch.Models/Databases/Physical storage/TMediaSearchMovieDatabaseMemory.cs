using System.Runtime.CompilerServices;

namespace MediaSearch.Models;

public class TMediaSearchMovieDatabaseMemory : IMediaSearchDataTable<IMovie>, IMediaSearchLoggable<TMediaSearchMovieDatabaseMemory> {

  public IMediaSearchLogger<TMediaSearchMovieDatabaseMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSearchMovieDatabaseMemory>();

  public IMSTableHeader Header { get; } = new TMSTableHeader();

  public bool IsDirty { get; set; } = false;

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  private readonly List<IMovie> _Items = new();

  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchMovieDatabaseMemory() { }

  public TMediaSearchMovieDatabaseMemory(IMediaSearchDataTable<IMovie> database) {
    Header = new TMediaSearchDatabaseHeader(database.Header);
    foreach (IMovie MediaItem in database.GetAll()) {
      MediaItem.SetDirty();
      _Items.Add(MediaItem);
    }
  }

  public TMediaSearchMovieDatabaseMemory(IEnumerable<IMovie> medias) {
    foreach (IMovie MediaItem in medias) {
      MediaItem.SetDirty();
      _Items.Add(MediaItem);
    }
  }

  public void Dispose() {
    Clear();
    _LockData?.Dispose();
  }

  public async ValueTask DisposeAsync() {
    await ClearAsync(CancellationToken.None);
    _LockData?.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    indent = indent.WithinLimits(0, int.MaxValue);
    string IndentSpace = new string(' ', indent);

    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{IndentSpace}{nameof(Header)}");
    RetVal.AppendLine($"{IndentSpace}{Header.ToString(2)}");
    RetVal.AppendLine($"{IndentSpace}Content : {_Items.Count} item(s)");
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

  public void Add(IMovie item) {
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

  }

  public void AddOrUpdate(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMovie item is missing");
    }

    item.SetDirty();

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

  }

  public void Update(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMovie item is missing");
    }

    item.SetDirty();

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMovie item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
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
  }

  public void Delete(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMovie item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMovie item to delete is not found : {item.Id}");
      }
      _Items.RemoveAt(ItemIndex);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public IEnumerable<IMovie> GetAll(int maxRecords = 0) {

    if (IsEmpty()) {
      yield break;
    }

    maxRecords = maxRecords.WithinLimits(0, int.MaxValue);

    try {
      _LockData.EnterReadLock();
      if (maxRecords == 0) {
        foreach (IMovie MediaItem in _Items) {
          yield return MediaItem;
        }
      } else {
        foreach (IMovie MediaItem in _Items.Take(maxRecords)) {
          yield return MediaItem;
        }
      }
    } finally {
      _LockData.ExitReadLock();
    }

  }

  public IEnumerable<IMovie> GetFiltered(IFilter filter, int maxRecords = 0) {

    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    maxRecords = maxRecords.WithinLimits(0, int.MaxValue);

    IList<IMovie> LocalItems;
    try {
      _LockData.EnterReadLock();
      if (maxRecords == 0) {
        LocalItems = new List<IMovie>(_Items.WithFilter(filter).OrderedBy(filter));
      } else {
        LocalItems = new List<IMovie>(_Items.WithFilter(filter).OrderedBy(filter).Take(maxRecords));
      }
    } finally {
      _LockData.ExitReadLock();
    }

    foreach (IMovie MediaItem in LocalItems) {
      yield return MediaItem;
    }

  }

  public IMovie? Get(string id) {
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
  #endregion --- Content management --------------------------------------------

  #region --- Content management async --------------------------------------------
  public Task AddAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    item.SetDirty();

    try {
      _LockData.EnterWriteLock();
      _Items.Add(item);
    } finally {
      _LockData.ExitWriteLock();
    }

    return Task.CompletedTask;
  }

  public Task AddOrUpdateAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    item.SetDirty();

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        _Items.Add(item);
      } else {
        _Items[ItemIndex] = item;
      }
    } finally {
      _LockData.ExitWriteLock();
    }

    return Task.CompletedTask;
  }

  public Task UpdateAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    item.SetDirty();

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);

    } finally {
      _LockData.ExitWriteLock();
    }

    return Task.CompletedTask;
  }

  public Task ClearAsync(CancellationToken token) {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
    } finally {
      _LockData.ExitWriteLock();
    }
    return Task.CompletedTask;
  }

  public Task DeleteAsync(IMovie item, CancellationToken token) {
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

    } finally {
      _LockData.ExitWriteLock();
    }

    return Task.CompletedTask;
  }

  public async IAsyncEnumerable<IMovie> GetAllAsync([EnumeratorCancellation] CancellationToken token) {
    if (IsEmpty()) {
      yield break;
    }

    IList<IMovie> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMovie>(_Items);
    } finally {
      _LockData.ExitReadLock();
    }

    await foreach (IMovie MediaItem in LocalItems.ToAsyncEnumerable()) {
      yield return MediaItem;
    }
  }

  public async IAsyncEnumerable<IMovie> GetFilteredAsync(TFilter filter, [EnumeratorCancellation] CancellationToken token) {
    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    IList<IMovie> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMovie>(_Items.WithFilter(filter).OrderedBy(filter));
    } finally {
      _LockData.ExitReadLock();
    }

    await foreach (IMovie MediaItem in LocalItems.ToAsyncEnumerable()) {
      yield return MediaItem;
    }
  }
  #endregion --- Content management async --------------------------------------------

  public bool Exists() {
    return true;
  }

  public bool Create() {
    return true;
  }

  public void Remove() {
    return;
  }

  public bool OpenOrCreate() {
    return true;
  }

  public void Close() {
  }

  public Task CloseAsync(CancellationToken token) {
    return Task.CompletedTask;
  }

  public bool Load() {
    Clear();
    return true;
  }

  public async Task<bool> LoadAsync(CancellationToken token) {
    await ClearAsync(token);
    return true;
  }

  public bool AutoSave { get; set; } = false;

  public bool Save() {
    try {
      _LockData.EnterWriteLock();
      _Items.ForEach(i => i.ClearDirty());
      return true;
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public async Task<bool> SaveAsync() {
    return await SaveAsync(CancellationToken.None);
  }
  public Task<bool> SaveAsync(CancellationToken token) {
    try {
      _LockData.EnterWriteLock();
      _Items.ForEach(i => i.ClearDirty());
      return Task.FromResult(true);
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  #region --- Dirty indicator --------------------------------------------
  private void SetDirty() {
    IsDirty = true;
    Header.LastUpdate = DateTime.Now;
  }

  private void ClearDirty() {
    IsDirty = false;
  }
  #endregion --- Dirty indicator --------------------------------------------
}
