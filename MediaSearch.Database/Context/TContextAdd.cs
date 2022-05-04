namespace MediaSearch.Database;
public class TContextAdd<RECORD> : IContextOperation<RECORD> where RECORD : IMSRecord, new() {
  public EContextOperation Operation { get; } = EContextOperation.Add;
  public RECORD Record { get; } = new();

  public TContextAdd(RECORD record) {
    Record = record;
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Operation)} = {Operation}", indent);
    RetVal.AppendIndent($"- {nameof(Record)}", indent);
    RetVal.AppendIndent(Record.ToString(indent), indent+2);
    return RetVal.ToString();
  }


  public override string ToString() {
   return ToString(0);
  }
}
