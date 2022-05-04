namespace MediaSearch.Database;

public class XMSRecord : IMSRecord {

  public string ID { get; } = "";

  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(ID)} = {ID}", indent);
    return RetVal.ToString();
  }

  public XMSRecord() { }

  public override string ToString() {
    return ToString(0);
  }

  
}
