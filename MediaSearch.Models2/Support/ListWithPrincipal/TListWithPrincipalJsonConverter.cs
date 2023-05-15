using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TListWithPrincipalJsonConverter<T> : JsonConverter<TListWithPrincipal<T>>, ILoggable {

  private const string PROPERTY_TYPE = "Type";
  private const string PROPERTY_PRINCIPAL = "Principal";
  private const string PROPERTY_VALUE = "Value";
  private const string PROPERTY_VALUES = "Values";
  private const string PROPERTY_ITEM = "Item";

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TListWithPrincipalJsonConverter<T>>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TListWithPrincipal<T>);
  }

  public override TListWithPrincipal<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    Type? ListType = null;
    T? Principal = default(T);
    List<T> Values = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          if (ListType == null) {
            throw new JsonException("Invalid or unknown list type");
          }
          TListWithPrincipal<T> RetVal = new TListWithPrincipal<T>(Values);
          if (Principal is not null) {
            RetVal.SetPrincipal(Principal);
          }
          Logger.LogDebugExBox($"Converted {RetVal.GetType().GetNameEx()}", RetVal);
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case PROPERTY_TYPE:
              ListType = Type.GetType($"System.{reader.GetString() ?? ""}", false, true);
              break;

            case PROPERTY_PRINCIPAL:
              if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.InvariantCultureIgnoreCase)) {
                Principal = (T)Convert.ChangeType(reader.GetString() ?? "", typeof(string));
              } else if (typeof(T).Name.Equals(typeof(int).Name, StringComparison.InvariantCultureIgnoreCase)) {
                Principal = (T)Convert.ChangeType(reader.GetInt32(), typeof(int));
              } else if (typeof(T).Name.Equals(typeof(long).Name, StringComparison.InvariantCultureIgnoreCase)) {
                Principal = (T)Convert.ChangeType(reader.GetInt64(), typeof(long));
              } else if (typeof(T).Name.Equals(typeof(float).Name, StringComparison.InvariantCultureIgnoreCase)) {
                Principal = (T)Convert.ChangeType(reader.GetSingle(), typeof(float));
              } else if (typeof(T).Name.Equals(typeof(double).Name, StringComparison.InvariantCultureIgnoreCase)) {
                Principal = (T)Convert.ChangeType(reader.GetDouble(), typeof(double));
              }
              break;

            case PROPERTY_VALUES:
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                T? Item = default;
                if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.InvariantCultureIgnoreCase)) {
                  Item = (T)Convert.ChangeType(reader.GetString() ?? "", typeof(string));
                } else if (typeof(T).Name.Equals(typeof(int).Name, StringComparison.InvariantCultureIgnoreCase)) {
                  Item = (T)Convert.ChangeType(reader.GetInt32(), typeof(int));
                } else if (typeof(T).Name.Equals(typeof(long).Name, StringComparison.InvariantCultureIgnoreCase)) {
                  Item = (T)Convert.ChangeType(reader.GetInt64(), typeof(long));
                } else if (typeof(T).Name.Equals(typeof(float).Name, StringComparison.InvariantCultureIgnoreCase)) {
                  Item = (T)Convert.ChangeType(reader.GetSingle(), typeof(float));
                } else if (typeof(T).Name.Equals(typeof(double).Name, StringComparison.InvariantCultureIgnoreCase)) {
                  Item = (T)Convert.ChangeType(reader.GetDouble(), typeof(double));
                }
                if (Item is not null) {
                  Values.Add(Item);
                }
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      return null;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TListWithPrincipal<T> value, JsonSerializerOptions options) {
    writer.WriteStartObject();

    writer.WriteString(PROPERTY_TYPE, typeof(T).Name);

    T Principal = value.GetPrincipal();
    WriteValue(writer, PROPERTY_PRINCIPAL, Principal);

    writer.WritePropertyName(PROPERTY_VALUES);
    writer.WriteStartArray();
    foreach (T ItemItem in value) {
      if (ItemItem is null) {
        continue;
      }
      WriteValue(writer, ItemItem);
    }
    writer.WriteEndArray();


    writer.WriteEndObject();
  }

  private void WriteValue(Utf8JsonWriter writer, T value) {
    if (value is string StringItem) {
      writer.WriteStringValue(StringItem ?? "".WithQuotes());
    } else if (value is int IntItem) {
      writer.WriteNumberValue(IntItem);
    } else if (value is long LongItem) {
      writer.WriteNumberValue(LongItem);
    } else if (value is float FloatItem) {
      writer.WriteNumberValue(FloatItem);
    } else if (value is double DoubleItem) {
      writer.WriteNumberValue(DoubleItem);
    }
  }
  private void WriteValue(Utf8JsonWriter writer, ReadOnlySpan<char> propertyName, T value) {
    if (value is string StringItem) {
      writer.WriteString(propertyName, StringItem ?? "".WithQuotes());
    } else if (value is int IntItem) {
      writer.WriteNumber(propertyName, IntItem);
    } else if (value is long LongItem) {
      writer.WriteNumber(propertyName, LongItem);
    } else if (value is float FloatItem) {
      writer.WriteNumber(propertyName, FloatItem);
    } else if (value is double DoubleItem) {
      writer.WriteNumber(propertyName, DoubleItem);
    }
  }
}
