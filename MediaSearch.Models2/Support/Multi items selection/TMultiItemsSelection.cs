using System.Collections.Concurrent;
using System.Globalization;

using BLTools.Diagnostic;

namespace MediaSearch.Models;
public class TMultiItemsSelection : IMultiItemsSelection, IToStringIndent {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMultiItemsSelection() { }
  public TMultiItemsSelection(IMultiItemsSelection source) {
    foreach (string ItemItem in source.Items) {
      Items.Add(ItemItem);
    }
    Selection = source.Selection;
  }

  public TMultiItemsSelection(EFilterType selection, params string[] items) {
    foreach (string ItemItem in items) {
      Items.Add(ItemItem);
    }
    Selection = selection;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent = 2) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(Selection)} = {Selection}", indent);
    if (Items.Any()) {
      RetVal.AppendIndent($"- {nameof(Items)} = {Items.Select(i => i.WithQuotes()).CombineToString()}", indent + 2);
    } else {
      RetVal.AppendIndent($"- {nameof(Items)} = (empty)", indent + 2);
    }
    return RetVal.ToString();
  }
  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------


  public ConcurrentBag<string> Items { get; } = new();
  public EFilterType Selection { get; set; } = EFilterType.Any;

  [DoNotDump]
  private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  public bool IsMatch(IEnumerable<string> target, CompareOptions options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) {
    if (target.IsEmpty()) {
      return false;
    }

    try {
      _lock.EnterReadLock();

      if (Items.IsEmpty()) {
        return false;
      }

      CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

      switch (Selection) {

        case EFilterType.All:
          return Items.All(i => target.Any(t => CI.IndexOf(t, i, options) > -1));

        case EFilterType.Any:
        default:
          return Items.Any(i => target.Any(t => CI.IndexOf(t, i, options) > -1));
      }
    } finally {
      _lock.ExitReadLock();
    }
  }

  public bool IsMatch(string target, CompareOptions options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) {
    if (target.IsEmpty()) {
      return false;
    }

    try {
      _lock.EnterReadLock();

      if (Items.IsEmpty()) {
        return false;
      }

      CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

      switch (Selection) {

        case EFilterType.All:
          return Items.All(i => CI.IndexOf(target, i, options) > -1);

        case EFilterType.Any:
        default:
          return Items.Any(i => CI.IndexOf(target, i, options) > -1);
      }
    } finally {
      _lock.ExitReadLock();
    }
  }

  public void Clear() {
    try {
      _lock.EnterWriteLock();
      Items.Clear();
      Selection = EFilterType.Any;
    } finally {
      _lock.ExitWriteLock();
    }
  }


}
