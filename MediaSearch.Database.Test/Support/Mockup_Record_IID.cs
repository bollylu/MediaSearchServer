namespace MediaSearch.Database.Test;
public class Mockup_Record_IID : IID<string> {
  public string ID { get; set; } = "";
  public string Name { get; set; } = "";


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.Append($"{ID} => {Name}");
    return RetVal.ToString();
  }
}
