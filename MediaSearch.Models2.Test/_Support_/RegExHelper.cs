using System.Text.Json;

using BLTools.Json;

namespace MediaSearch.Models2;
public static class RegExHelper {

  public static IEnumerable<string> GetJsonProperties(this JsonDocument JsonSource) {
    foreach (var Item in JsonSource.RootElement.EnumerateObject()) {
      switch (Item.Value.ValueKind) {
        case JsonValueKind.Object:
          JsonDocument Doc = JsonDocument.Parse(Item.Value.ToString());
          foreach (string SubItem in Doc.GetJsonProperties()) {
            yield return SubItem;
          }
          break;
        case JsonValueKind.Array:
          yield return Item.Name;
          foreach (JsonElement ArrayItem in Item.Value.EnumerateArray()) {
            yield return $"{ArrayItem.GetRawText()}";
          }
          break;
        default:
          yield return $"{Item.Name} : {Item.Value}";
          break;
      }
    }
  }
}
