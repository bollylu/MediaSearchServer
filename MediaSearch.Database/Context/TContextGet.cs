namespace MediaSearch.Database;
public class TContextGet<RECORD> : IContextOperation<RECORD> where RECORD : IRecord {
  public EContextOperation Operation { get; } = EContextOperation.Get;
  public RECORD? Record { get; }

  public string Key { get; }

  public TContextGet(string key) {
    Key = key;
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Operation)} = {Operation}", indent);
    if (Record is not null) {
      RetVal.AppendIndent($"- {nameof(Record)}", indent);
      RetVal.AppendIndent(Record?.ToString(indent) ?? "", indent + 2);
    } else {
      RetVal.AppendIndent($"- {nameof(Record)} is null");
    }
    RetVal.AppendIndent($"- {nameof(Key)} = {Key}");
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
}
