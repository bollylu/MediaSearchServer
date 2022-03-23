using System.Runtime.CompilerServices;

namespace MediaSearch.Models;

public class TMediaSearchDatabaseMemory : IMediaSearchDatabase, IMediaSearchLoggable<TMediaSearchDatabaseMemory> {

  public IMediaSearchLogger<TMediaSearchDatabaseMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMediaSearchDatabaseMemory>();

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

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
    foreach (IMedia MediaItem in database.GetAll()) {
      _Items.Add(MediaItem);
    }
  }

  public TMediaSearchDatabaseMemory(IEnumerable<IMedia> medias) {
    foreach (IMedia MediaItem in medias) {
      _Items.Add(MediaItem);
    }
  }

  public ValueTask DisposeAsync() {
    _Items.Clear();
    return ValueTask.CompletedTask;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    indent = indent.WithinLimits(0, int.MaxValue);
    string IndentSpace = new string(' ', indent);

    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{IndentSpace}{nameof(TMediaSearchDatabaseMemory)} : {Name}");
    if (!string.IsNullOrWhiteSpace(Description)) {
      RetVal.AppendLine($"{IndentSpace}  {nameof(Description)} : {Description}");
    }
    RetVal.AppendLine($"{IndentSpace}  Content : {_Items.Count} item(s)");
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
      
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Clear() {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
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


}
