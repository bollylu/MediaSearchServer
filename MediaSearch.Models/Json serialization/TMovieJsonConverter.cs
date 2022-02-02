using System.Drawing;
using System.Drawing.Printing;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MediaSearch.Models;

public class TMovieJsonConverter : JsonConverter<TMovie> {

  public override bool CanConvert(Type typeToConvert) {
    return base.CanConvert(typeToConvert);
  }

  public override TMovie Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    Utf8JsonReader TestReader = reader;

    //string MovieId = "";
    //while (TestReader.Read()) {
    //  JsonTokenType TokenType = reader.TokenType;

    //  if (TokenType == JsonTokenType.EndObject) {
    //    break;
    //  }

    //  if (TokenType == JsonTokenType.PropertyName) {

    //    string Property = reader.GetString();
    //    reader.Read();
    //    if (Property == nameof(IMovie.Id)) {
    //      MovieId = TestReader.GetString();
    //      break;
    //    }
    //  }
    //}

    TMovie RetVal = new TMovie();

    while (reader.Read()) {

      JsonTokenType TokenType = reader.TokenType;

      if (TokenType == JsonTokenType.EndObject) {
        return RetVal;
      }

      if (TokenType == JsonTokenType.PropertyName) {

        string? Property = reader.GetString();
        reader.Read();

        switch (Property) {

          case nameof(IMovie.Name):
            RetVal.Name = reader.GetString() ?? "";
            break;

          case nameof(IMovie.Group):
            RetVal.Group = reader.GetString() ?? "";
            break;

          case nameof(IMovie.SubGroup):
            RetVal.SubGroup = reader.GetString() ?? "";
            break;

          case nameof(IMovie.StoragePath):
            RetVal.StoragePath = reader.GetString() ?? "";
            break;

          case nameof(IMovie.FileName):
            RetVal.FileName = reader.GetString() ?? "";
            break;

          case nameof(IMovie.FileExtension):
            RetVal.FileExtension = reader.GetString() ?? "";
            break;

          case nameof(IMovie.Size):
            RetVal.Size = reader.GetInt64();
            break;

          case nameof(IMovie.OutputYear):
            RetVal.OutputYear = reader.GetInt32();
            break;

          case nameof(IMovie.DateAdded):
            RetVal.DateAdded = JsonSerializer.Deserialize<DateOnly>(ref reader, options);
            break;

          case nameof(IMovie.AltNames):
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
              string AltNameItem = reader.GetString() ?? "";
              RetVal.AltNames.Add(AltNameItem.Before('|'), AltNameItem.After('|'));
            }
            break;

          case nameof(IMovie.Tags):
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
              RetVal.Tags.Add(reader.GetString() ?? "");
            }
            break;
        }
      }
    }

    return RetVal;

  }


  public override void Write(Utf8JsonWriter writer, TMovie value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(IMovie.Id), value.Id);
    writer.WriteString(nameof(IMovie.Name), value.Name);
    writer.WriteString(nameof(IMovie.Group), value.Group);
    writer.WriteString(nameof(IMovie.SubGroup), value.SubGroup);
    writer.WriteString(nameof(IMovie.StoragePath), value.StoragePath);
    writer.WriteString(nameof(IMovie.FileName), value.FileName);
    writer.WriteString(nameof(IMovie.FileExtension), value.FileExtension);
    writer.WriteNumber(nameof(IMovie.Size), value.Size);
    writer.WriteNumber(nameof(IMovie.OutputYear), value.OutputYear);
    writer.WritePropertyName(nameof(IMovie.DateAdded));
    JsonSerializer.Serialize(writer, value.DateAdded, options);

    writer.WriteStartArray(nameof(IMovie.AltNames));
    foreach (KeyValuePair<string, string> AltNameItem in value.AltNames) {
      writer.WriteStringValue($"{AltNameItem.Key}|{AltNameItem.Value}");
    }
    writer.WriteEndArray();

    writer.WriteStartArray(nameof(IMovie.Tags));
    foreach (string TagItem in value.Tags) {
      writer.WriteStringValue(TagItem);
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
  }
}
