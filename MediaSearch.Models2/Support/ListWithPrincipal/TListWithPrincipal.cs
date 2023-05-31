using System.Linq;

namespace MediaSearch.Models;

public class TListWithPrincipal {
  public const int NO_PRINCIPAL = -1;
}

public class TListWithPrincipal<T> : List<T>, IListWithPrincipal<T> {

  private int _PrincipalIndex = TListWithPrincipal.NO_PRINCIPAL;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TListWithPrincipal() { }
  public TListWithPrincipal(IListWithPrincipal<T> source) {
    foreach (T ItemItem in source) {
      Add(ItemItem);
    }
    SetPrincipal(source.GetPrincipal());
  }

  public TListWithPrincipal(IEnumerable<T> source) {
    foreach (T ItemItem in source) {
      Add(ItemItem);
    }
  }

  public TListWithPrincipal(params T[] source) {
    foreach (T ItemItem in source) {
      Add(ItemItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public new void Add(T item) {
    if (this.Contains(item)) {
      throw new InvalidOperationException($"Item [{item}] already exist in the list");
    }

    if (this.IsEmpty()) {
      _PrincipalIndex = 0;
    }

    base.Add(item);
  }

  public IListWithPrincipal<T> SetPrincipal(T principal) {
    int Index = -1;
    if (typeof(T).Name == typeof(byte[]).Name) {
      byte[]? BytesPrincipal = principal as byte[] ?? throw new ApplicationException("???");
      Index = this.Cast<byte[]>().ToList().FindIndex(x => x?.SequenceEqual(BytesPrincipal) ?? false);
    } else {
      Index = this.FindIndex(x => x?.Equals(principal) ?? false);
    }
    if (Index < 0) {
      _PrincipalIndex = TListWithPrincipal.NO_PRINCIPAL;
      throw new ArgumentOutOfRangeException(nameof(principal));
    }
    _PrincipalIndex = Index;
    return this;
  }

  public T GetPrincipal() {
    if (this.IsEmpty()) {
      throw new InvalidOperationException();
    }

    if (_PrincipalIndex > TListWithPrincipal.NO_PRINCIPAL) {
      return this[_PrincipalIndex];
    }

    return this.First();
  }

  public void ClearPrincipal() {
    _PrincipalIndex = TListWithPrincipal.NO_PRINCIPAL;
  }


  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString() {
    return ToString(0);
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    if (typeof(T).Name == typeof(byte[]).Name) {
      for (int i = 0; i < this.Count; i++) {
        byte[]? BytesItem = this[i] as byte[] ?? throw new ApplicationException("???");
        if (i == _PrincipalIndex) {
          RetVal.AppendIndent($"[X] {BytesItem.ToHexString()}", indent + 2);
        } else {
          RetVal.AppendIndent($"[ ] {BytesItem.ToHexString()}", indent + 2);
        }
      }
    } else {
      int Index = _PrincipalIndex == TListWithPrincipal.NO_PRINCIPAL ? 0 : _PrincipalIndex;
      for (int i = 0; i < this.Count; i++) {
        if (i == Index) {
          RetVal.AppendIndent($"[X] {this[i]}", indent + 2);
        } else {
          RetVal.AppendIndent($"[ ] {this[i]}", indent + 2);
        }
      }
    }

    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
