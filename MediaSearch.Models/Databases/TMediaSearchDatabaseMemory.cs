using System.Runtime.CompilerServices;

namespace MediaSearch.Models;

public class TMediaSearchDatabaseMemory : IMediaSearchDatabase, IMediaSearchLoggable<TMediaSearchDatabaseMemory> {

  public IMediaSearchLogger<TMediaSearchDatabaseMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSearchDatabaseMemory>();

  public IMediaSearchDatabaseHeader Header { get; } = new TMediaSearchDatabaseHeader();

  public bool IsDirty { get; set; } = false;

  #region --- Internal data storage --------------------------------------------
  /// <summary>
  /// Store the movies
  /// </summary>
  private readonly List<IMedia> _Items = new();

  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSearchDatabaseMemory() { }

  public TMediaSearchDatabaseMemory(IMediaSearchDatabase database) {
    Header = new TMediaSearchDatabaseHeader(database.Header);
    foreach (IMedia MediaItem in database.GetAll()) {
      MediaItem.SetDirty();
      _Items.Add(MediaItem);
    }
  }

  public TMediaSearchDatabaseMemory(IEnumerable<IMedia> medias) {
    foreach (IMedia MediaItem in medias) {
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

  }

  public void AddOrUpdate(IMedia item) {
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
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

  }

  public void Update(IMedia item) {
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
  #endregion --- Content management --------------------------------------------

  #region --- Content management async --------------------------------------------
  public Task AddAsync(IMedia item, CancellationToken token) {
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

  public Task AddOrUpdateAsync(IMedia item, CancellationToken token) {
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

  public Task UpdateAsync(IMedia item, CancellationToken token) {
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

  public Task DeleteAsync(IMedia item, CancellationToken token) {
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

    await foreach (IMedia MediaItem in LocalItems.ToAsyncEnumerable()) {
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

    await foreach (IMedia MediaItem in LocalItems.ToAsyncEnumerable()) {
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

  public bool Open() {
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
