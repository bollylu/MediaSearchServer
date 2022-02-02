using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace MediaSearch.Models;

public class TFilterJsonConverter : JsonConverter<TFilter> {

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert.IsAssignableFrom(typeof(TFilter));
  }

  public override TFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TFilter RetVal = new TFilter();

    while (reader.Read()) {

      JsonTokenType TokenType = reader.TokenType;

      if (TokenType == JsonTokenType.EndObject) {
        return RetVal;
      }

      if (TokenType == JsonTokenType.PropertyName) {

        string? Property = reader.GetString();
        reader.Read();

        switch (Property) {

          case nameof(TFilter.Page):
            RetVal.Page = reader.GetInt32();
            break;

          case nameof(TFilter.PageSize):
            RetVal.PageSize = reader.GetInt32();
            break;

          case nameof(TFilter.DaysBack):
            RetVal.DaysBack = reader.GetInt32();
            break;

          case nameof(TFilter.OutputDateMin):
            RetVal.OutputDateMin = reader.GetInt32();
            break;

          case nameof(TFilter.OutputDateMax):
            RetVal.OutputDateMax = reader.GetInt32();
            break;

          case nameof(TFilter.Keywords):
            RetVal.Keywords = reader.GetString() ?? "";
            break;

          case nameof(TFilter.KeywordsSelection):
            RetVal.KeywordsSelection = JsonSerializer.Deserialize<EFilterType>(ref reader, options);
            break;

          case nameof(TFilter.Tags):
            RetVal.Tags = reader.GetString() ?? "";
            break;

          case nameof(TFilter.TagSelection):
            RetVal.TagSelection = JsonSerializer.Deserialize<EFilterType>(ref reader, options);
            break;

          case nameof(TFilter.Group):
            RetVal.Group = reader.GetString() ?? "";
            break;

          case nameof(TFilter.SubGroup):
            RetVal.SubGroup = reader.GetString() ?? "";
            break;

          case nameof(TFilter.GroupOnly):
            RetVal.GroupOnly = reader.GetBoolean();
            break;

          case nameof(TFilter.GroupMemberships):
            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
              string? GroupMembershipItem = reader.GetString();
              if (GroupMembershipItem is not null) {
                RetVal.GroupMemberships.Add(GroupMembershipItem);
              }
            }
            break;
        }
      }
    }

    return RetVal;
  }

  public override void Write(Utf8JsonWriter writer, TFilter value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteNumber(nameof(TFilter.Page), value.Page);
    writer.WriteNumber(nameof(TFilter.PageSize), value.PageSize);

    writer.WriteString(nameof(TFilter.Keywords), value.Keywords);
    writer.WritePropertyName(nameof(TFilter.KeywordsSelection));
    JsonSerializer.Serialize(writer, value.KeywordsSelection, options);
    writer.WriteString(nameof(TFilter.Keywords), value.Tags);
    writer.WritePropertyName(nameof(TFilter.TagSelection));
    JsonSerializer.Serialize(writer, value.TagSelection, options);

    writer.WriteNumber(nameof(TFilter.OutputDateMin), value.OutputDateMin);
    writer.WriteNumber(nameof(TFilter.OutputDateMax), value.OutputDateMax);

    writer.WriteNumber(nameof(TFilter.DaysBack), value.DaysBack);

    writer.WriteString(nameof(TFilter.Group), value.Group);
    writer.WriteString(nameof(TFilter.SubGroup), value.SubGroup);
    writer.WriteBoolean(nameof(TFilter.GroupOnly), value.GroupOnly);
    writer.WritePropertyName(nameof(TFilter.GroupFilter));
    JsonSerializer.Serialize(writer, value.GroupFilter, options);

    writer.WriteStartArray(nameof(TFilter.GroupMemberships));
    foreach (string GroupMembershipItem in value.GroupMemberships) {
      writer.WriteStringValue(GroupMembershipItem);
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
  }
}
