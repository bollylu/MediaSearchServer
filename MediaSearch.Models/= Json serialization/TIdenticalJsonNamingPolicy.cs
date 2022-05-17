namespace MediaSearch.Models;
public class TIdenticalJsonNamingPolicy : JsonNamingPolicy {
  public override string ConvertName(string name) {
    return name;
  }
}
