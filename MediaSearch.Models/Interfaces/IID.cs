namespace MediaSearch.Models;
public interface IId<T> {

  /// <summary>
  /// String identifier
  /// </summary>
  T Id { get; }

}

public interface IIdString : IId<string> {
  public string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append(Id);
    return RetVal.ToString();
  }
}
