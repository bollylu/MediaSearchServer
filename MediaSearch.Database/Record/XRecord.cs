namespace MediaSearch.Database;

public class XRecord : IRecord {

  public string ID { get; } = "";

  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(ID)} = {ID}", indent);
    return RetVal.ToString();
  }

  public XRecord() { }

  public override string ToString() {
    return ToString(0);
  }


}
