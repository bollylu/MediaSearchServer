namespace MediaSearch.Models;
public class TLanguageTextInfos : ILanguageTextInfos, IMediaSearchLoggable<TLanguageTextInfos> {

  public IMediaSearchLogger<TLanguageTextInfos> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TLanguageTextInfos>();

  private readonly List<ILanguageTextInfo> _Items = new();
  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    string IndentSpace = new string(' ', indent);
    foreach (ILanguageTextInfo LanguageTextInfoItem in _Items) {
      RetVal.AppendLine($"{IndentSpace}{LanguageTextInfoItem.ToString()}");
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Add(ILanguageTextInfo languageTextInfo) {
    try {
      _LockData.EnterWriteLock();
      _Items.Add(languageTextInfo);
      if (HasMoreThanOnePrincipal()) {
        Logger.LogWarningBox($"More than one {nameof(ILanguageTextInfo)} is principal", languageTextInfo);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Add(ELanguage language, string value, bool isPrincipal = false) {
    try {
      _LockData.EnterWriteLock();
      ILanguageTextInfo NewItem = new TLanguageTextInfo(language, value, isPrincipal);
      _Items.Add(NewItem);
      if (HasMoreThanOnePrincipal()) {
        Logger.LogWarningBox($"More than one {nameof(ILanguageTextInfo)} is principal", NewItem);
      }
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

  public ILanguageTextInfo? Get(ELanguage language) {
    try {
      _LockData.EnterReadLock();
      return _Items.FirstOrDefault(x => x.Language == language);
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public ILanguageTextInfo? GetPrincipal() {
    try {
      _LockData.EnterReadLock();
      return _Items.FirstOrDefault(x => x.IsPrincipal);
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public IEnumerable<ILanguageTextInfo> GetAll() {
    try {
      _LockData.EnterReadLock();
      return _Items.AsEnumerable();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  private bool HasMoreThanOnePrincipal() {
    return _Items.Count(x => x.IsPrincipal) > 1;
  }

  private bool HasMoreThanOneTextPerLanguage() {
    return _Items.GroupBy(x => x.Language).Where(x => x.Count() > 1).Any();
  }

}
