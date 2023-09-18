namespace MediaSearch.Models.Support;
public partial class TFFProbe {

  internal IDictionary<string, string> ParseStream(JsonElement jsonSource) {
    Dictionary<string, string> RetVal = new();
    foreach (JsonProperty JsonItem in jsonSource.EnumerateObject()) {
      if (JsonItem.Value.ValueKind == JsonValueKind.Object) {
        continue;
      }
      RetVal.Add(JsonItem.Name, JsonItem.Value.ValueKind switch {
        JsonValueKind.String => JsonItem.Value.GetString() ?? "(null)",
        JsonValueKind.Number => JsonItem.Value.GetRawText(),
        _ => JsonItem.Value.GetRawText()
      });
    }
    return RetVal;
  }
  internal IDictionary<string, string> ParseTags(JsonElement jsonSource) {
    Dictionary<string, string> RetVal = new();
    foreach (JsonProperty JsonItem in jsonSource.EnumerateObject()) {
      RetVal.Add(JsonItem.Name, JsonItem.Value.ValueKind switch {
        JsonValueKind.String => JsonItem.Value.GetString() ?? "(null)",
        JsonValueKind.Number => JsonItem.Value.GetRawText(),
        _ => JsonItem.Value.GetRawText()
      });
    }
    return RetVal;
  }
  internal IDictionary<string, string> ParseDisposition(JsonElement jsonSource) {
    Dictionary<string, string> RetVal = new();
    foreach (JsonProperty JsonItem in jsonSource.EnumerateObject()) {
      RetVal.Add(JsonItem.Name, JsonItem.Value.ValueKind switch {
        JsonValueKind.String => JsonItem.Value.GetString() ?? "(null)",
        JsonValueKind.Number => JsonItem.Value.GetRawText(),
        _ => JsonItem.Value.GetRawText()
      });
    }
    return RetVal;
  }
}
