namespace MediaSearch.Test.Database;
public class Mockup_Record_IID : IMSRecord {
  public string ID { get; set; } = "";
  public string Name { get; set; } = "";


  public override string ToString() {
    return ToString(0);
  }

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{ID} => {Name}");
    return RetVal.ToString();
  }
}
