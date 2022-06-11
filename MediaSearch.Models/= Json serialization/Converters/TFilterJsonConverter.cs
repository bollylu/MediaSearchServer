using static BLTools.Json.JsonConverterResources;

namespace MediaSearch.Models;

public class TFilterJsonConverter : JsonConverter<TFilter>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TFilterJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    if (typeToConvert == typeof(TFilter)) { return true; }
    if (typeToConvert == typeof(IFilter)) { return true; }
    return false;
  }

  public override TFilter Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TFilter RetVal = new TFilter();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.IfDebugMessageExBox($"Converted {nameof(TFilter)}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TFilter.Page):
              RetVal.Page = reader.GetInt32();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.Page)}", RetVal.Page);
              break;

            case nameof(TFilter.PageSize):
              RetVal.PageSize = reader.GetInt32();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.PageSize)}", RetVal.PageSize);
              break;

            case nameof(TFilter.DaysBack):
              RetVal.DaysBack = reader.GetInt32();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.DaysBack)}", RetVal.DaysBack);
              break;

            case nameof(TFilter.OutputDateMin):
              RetVal.OutputDateMin = reader.GetInt32();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.OutputDateMin)}", RetVal.OutputDateMin);
              break;

            case nameof(TFilter.OutputDateMax):
              RetVal.OutputDateMax = reader.GetInt32();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.OutputDateMax)}", RetVal.OutputDateMax);
              break;

            case nameof(TFilter.Keywords):
              RetVal.Keywords = reader.GetString() ?? "";
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.Keywords)}", RetVal.Keywords.WithQuotes());
              break;

            case nameof(TFilter.KeywordsSelection):
              RetVal.KeywordsSelection = JsonSerializer.Deserialize<EFilterType>(ref reader, options);
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.KeywordsSelection)}", RetVal.KeywordsSelection);
              break;

            case nameof(TFilter.Tags):
              RetVal.Tags = reader.GetString() ?? "";
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.Tags)}", RetVal.Tags.WithQuotes());
              break;

            case nameof(TFilter.TagSelection):
              RetVal.TagSelection = JsonSerializer.Deserialize<EFilterType>(ref reader, options);
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.TagSelection)}", RetVal.TagSelection);
              break;

            case nameof(TFilter.Group):
              RetVal.Group = reader.GetString() ?? "";
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.Group)}", RetVal.Group.WithQuotes());
              break;

            case nameof(TFilter.SubGroup):
              RetVal.SubGroup = reader.GetString() ?? "";
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.SubGroup)}", RetVal.SubGroup.WithQuotes());
              break;

            case nameof(TFilter.GroupOnly):
              RetVal.GroupOnly = reader.GetBoolean();
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.GroupOnly)}", RetVal.GroupOnly);
              break;

            case nameof(TFilter.SortOrder):
              RetVal.SortOrder = JsonSerializer.Deserialize<EFilterSortOrder>(ref reader, options);
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.SortOrder)}", RetVal.SortOrder);
              break;

            case nameof(TFilter.GroupMemberships):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                string? GroupMembershipItem = reader.GetString();
                if (GroupMembershipItem is not null) {
                  RetVal.GroupMemberships.Add(GroupMembershipItem);
                }
              }
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.GroupMemberships)}", string.Join(", ", RetVal.GroupMemberships));
              break;

            case nameof(TFilter.GroupFilter):
              RetVal.GroupFilter = JsonSerializer.Deserialize<EFilterGroup>(ref reader, options);
              Logger.IfDebugMessageExBox($"Found {nameof(RetVal.GroupFilter)}", RetVal.GroupFilter);
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.IfDebugMessageExBox($"Converted {nameof(TFilter)}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError(ERROR_CONVERSION, ex.Message);
      throw;
    }

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
    writer.WriteString(nameof(TFilter.Tags), value.Tags);
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

    writer.WritePropertyName(nameof(TFilter.SortOrder));
    JsonSerializer.Serialize(writer, value.SortOrder, options);

    writer.WriteEndObject();
  }
}
