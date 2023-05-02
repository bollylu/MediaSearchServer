using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TMultiItemsSelectionJsonConverter : JsonConverter<TMultiItemsSelection>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMultiItemsSelection>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMultiItemsSelection) || typeToConvert.GetInterfaces().Any(x => x == typeof(IMultiItemsSelection));
  }

  public override TMultiItemsSelection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMultiItemsSelection RetVal = new TMultiItemsSelection();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebugExBox($"Converted {RetVal.GetType().Name}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMultiItemsSelection.Selection):
              RetVal.Selection = JsonSerializer.Deserialize<EFilterType>(ref reader, options);
              break;

            case nameof(TMultiItemsSelection.Items):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                string? ItemItem = reader.GetString();
                if (ItemItem is not null) {
                  RetVal.Items.Add(ItemItem);
                }
              }
              Logger.LogDebugExBox($"Found {nameof(TMultiItemsSelection.Items)}", RetVal.Items.Select(i => i.WithQuotes()).CombineToString());
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      Logger.LogDebugExBox($"Converted {RetVal.GetType().Name}", RetVal);
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError(ERROR_CONVERSION, ex.Message);
      throw;
    }

  }

  public override void Write(Utf8JsonWriter writer, TMultiItemsSelection value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(IMultiItemsSelection.Selection), value.Selection.ToString());

    writer.WriteStartArray(nameof(IMultiItemsSelection.Items));
    foreach (string ItemItem in value.Items) {
      writer.WriteStringValue(ItemItem);
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
  }
}
