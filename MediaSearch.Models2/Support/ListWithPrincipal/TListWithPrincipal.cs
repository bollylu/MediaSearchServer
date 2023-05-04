using System.ComponentModel.Design.Serialization;

namespace MediaSearch.Models;

public class TListWithPrincipal {
  public const int NO_PRINCIPAL = -1;
}

public class TListWithPrincipal<T> : List<T>, IListWithPrincipal<T> {

  private int _PrincipalIndex = TListWithPrincipal.NO_PRINCIPAL;

  public TListWithPrincipal() { }
  public TListWithPrincipal(IListWithPrincipal<T> source) {
    foreach (T ItemItem in source) {
      Add(ItemItem);
    }
    SetPrincipal(source.GetPrincipal());
  }

  public int SetPrincipal(T principal) {
    int Index = this.FindIndex(x => x?.Equals(principal) ?? false);
    if (Index < 0) {
      _PrincipalIndex = TListWithPrincipal.NO_PRINCIPAL;
      return TListWithPrincipal.NO_PRINCIPAL;
    }
    _PrincipalIndex = Index;
    return Index;
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


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    for (int i = 0; i < this.Count; i++) {
      if (i == _PrincipalIndex) {
        RetVal.AppendLine($"[X] {this[i]}");
      } else {
        RetVal.AppendLine($"[ ] {this[i]}");
      }
    }
    return RetVal.ToString();
  }
}
