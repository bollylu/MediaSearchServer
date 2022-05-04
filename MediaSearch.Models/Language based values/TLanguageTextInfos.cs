using System.Security.Principal;

namespace MediaSearch.Models;
public class TLanguageTextInfos : ILanguageTextInfos, IMediaSearchLoggable<TLanguageTextInfos> {

  public IMediaSearchLogger<TLanguageTextInfos> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TLanguageTextInfos>();

  private readonly List<ILanguageTextInfo> _Items = new();
  private readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    try {
      _LockData.EnterReadLock();
      StringBuilder RetVal = new();
      string IndentSpace = indent == 0 ? "" : $"{new string(' ', indent - 2)}|- ";
      foreach (ILanguageTextInfo LanguageTextInfoItem in _Items) {
        RetVal.AppendLine($"{IndentSpace}{LanguageTextInfoItem.ToString()}");
      }
      return RetVal.ToString();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public void Add(ILanguageTextInfo languageTextInfo) {
    try {
      _LockData.EnterWriteLock();
      languageTextInfo.IsPrincipal = GetPrincipal() is null;
      _Items.Add(languageTextInfo);
      if (HasMoreThanOnePrincipal()) {
        Logger.LogWarningBox($"More than one {nameof(ILanguageTextInfo)} is principal", languageTextInfo);
      }
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Add(ELanguage language, string value) {
    try {
      _LockData.EnterWriteLock();
      ILanguageTextInfo NewItem = new TLanguageTextInfo(language, value, GetPrincipal() is null);
      _Items.Add(NewItem);
    } finally {
      _LockData.ExitWriteLock();
    }
  }

  public void Add(string value) {
    try {
      _LockData.EnterWriteLock();
      ILanguageTextInfo NewItem = new TLanguageTextInfo(ELanguage.Unknown, value, GetPrincipal() is null);
      _Items.Add(NewItem);
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

  public void SetPrincipal(ELanguage language) {
    try {
      _LockData.EnterUpgradeableReadLock();

      if (IsEmpty()) {
        Logger.LogWarning($"Unable to set principal to {language} : {nameof(TLanguageTextInfos)} is empty");
        return;
      }

      ILanguageTextInfo? NewPrincipal = Get(language);
      if (NewPrincipal is null) {
        Logger.LogWarning($"Unable to set principal to {language} : {language} is not found");
        return;
      }

      if (NewPrincipal.IsPrincipal) {
        Logger.LogWarning($"Unable to set principal to {language} : {language} is already the principal");
        return;
      }

      ILanguageTextInfo? ActualPrincipal = GetPrincipal();
      if (ActualPrincipal is null) {
        Logger.LogError("Incoherent data : Principal is missing");
        return;
      }

      try {
        _LockData.EnterWriteLock();
        ActualPrincipal.IsPrincipal = false;
        NewPrincipal.IsPrincipal = true;
      } finally {
        _LockData.ExitWriteLock();
      }
      
    } finally {
      _LockData.ExitUpgradeableReadLock();
    }
  }
  public void SetPrincipal(ILanguageTextInfo languageTextInfo) {
    SetPrincipal(languageTextInfo.Language);
  }

  public IEnumerable<ILanguageTextInfo> GetAll() {
    List<ILanguageTextInfo> Values;
    try {
      _LockData.EnterReadLock();
      if (_Items.IsEmpty()) {
        yield break;
      }
      Values = _Items.ToList();
    } finally {
      _LockData.ExitReadLock();
    }
    foreach (ILanguageTextInfo LanguageTextInfoItem in Values) {
      yield return LanguageTextInfoItem;
    }
  }

  public bool HasMoreThanOnePrincipal() {
    return _Items.Count(x => x.IsPrincipal) > 1;
  }

  public bool HasMoreThanOneTextPerLanguage() {
    return _Items.GroupBy(x => x.Language).Where(x => x.Count() > 1).Any();
  }

}
