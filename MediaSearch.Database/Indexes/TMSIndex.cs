using System.Text;

using MediaSearch.Models;

namespace MediaSearch.Database;

public class TMSIndex<RECORD, INDEX_KEY, INDEX_VALUE> : IMSIndex<RECORD, INDEX_KEY, INDEX_VALUE> 
  where RECORD : IID<string>
  where INDEX_KEY : notnull
  where INDEX_VALUE : notnull {

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type IndexType => typeof(RECORD);

  #region --- Converters -------------------------------------------------------------------------------------
  /// <summary>
  /// Transforms the index to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(IndexType)} = {this.GetType().GetGenericName()}")
          .AppendIndent($"- {nameof(LastUpdate)} = {LastUpdate}")
          .AppendIndent($"- {nameof(IndexedValues)} = {IndexedValues.Count} value(s)");
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  } 
  #endregion --- Converters -------------------------------------------------------------------------------------

  public List<KeyValuePair<INDEX_KEY, INDEX_VALUE>> IndexedValues { get; } = new();
  private readonly ReaderWriterLockSlim _Lock = new();

  public void Add(INDEX_KEY key, INDEX_VALUE id) {
    try {
      _Lock.EnterWriteLock();
      IndexedValues.Add(new KeyValuePair<INDEX_KEY, INDEX_VALUE>(key, id));
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public void Add(KeyValuePair<INDEX_KEY, INDEX_VALUE> kvp) {
    try {
      _Lock.EnterWriteLock();
      IndexedValues.Add(kvp);
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public void Delete(INDEX_KEY key) {
    try {
      _Lock.EnterWriteLock();
      IndexedValues.RemoveAll(kvp => kvp.Key.Equals(key));
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      IndexedValues.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool Exists(INDEX_VALUE id) {
    try {
      _Lock.EnterReadLock();
      return IndexedValues.Any(kvp => kvp.Value.Equals(id));
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public INDEX_VALUE? Get(INDEX_KEY key, INDEX_VALUE? defaultValue) {
    try {
      _Lock.EnterReadLock();
      int Index = IndexedValues.FindIndex(kvp => kvp.Key.Equals(key));
      return Index == -1 ? defaultValue : IndexedValues[Index].Value;
    } finally {
      _Lock.ExitReadLock();
    }
  }

}
