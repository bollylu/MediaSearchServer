using System.Net;

namespace MediaSearch.Models;

public class TIPAddressJsonConverter : JsonConverter<IPAddress> {

  public override bool CanConvert(Type objectType) {
    return objectType == typeof(IPAddress);
  }

  public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType == JsonTokenType.String) {
      string? Data = reader.GetString();
      if (Data is null) {
        throw new JsonException($"Invalid Json data for IPAddress : \"{Data}\"");
      }
      IPAddress RetVal = IPAddress.Parse(Data);
      return RetVal;
    }
    return IPAddress.Any;
  }

  public override void Write(Utf8JsonWriter writer, IPAddress value, JsonSerializerOptions options) {
    writer.WriteStringValue(value.MapToIPv4().ToString());
  }
}
