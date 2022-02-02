
namespace MediaSearch.Models;

public class TDateOnlyJsonConverter : JsonConverter<DateOnly> {
  public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType == JsonTokenType.String) {
      string? Data = reader.GetString();
      if (Data is null || Data.Length != 8) {
        throw new JsonException($"Invalid Json data for DateOnly : \'{Data}\"");
      }
      DateOnly RetVal = new DateOnly(int.Parse(Data.Left(4)), int.Parse(Data.Substring(4, 2)), int.Parse(Data.Right(2)));
      return RetVal;
    }
    return DateOnly.MinValue;
  }

  public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) {
    writer.WriteStringValue($"{value.Year:0000}{value.Month:00}{value.Day:00}");
  }
}
