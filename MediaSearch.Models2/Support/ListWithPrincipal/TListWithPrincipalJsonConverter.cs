using System.Collections;

using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TListWithPrincipalJsonConverter<T> : JsonConverter<TListWithPrincipal<T>>, ILoggable {

  private const string JSON_PROPERTY_TYPE = "Type";
  private const string JSON_PROPERTY_PRINCIPAL = "Principal";
  private const string JSON_PROPERTY_VALUES = "Values";

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

            case JSON_PROPERTY_TYPE:
              ListType = Type.GetType($"System.{reader.GetString() ?? ""}", false, true);
              break;

            case JSON_PROPERTY_PRINCIPAL:
              Principal = ReadValue(reader);
              break;

            case JSON_PROPERTY_VALUES:
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                T? Item = ReadValue(reader);
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

    writer.WriteString(JSON_PROPERTY_TYPE, typeof(T).Name);

    T Principal = value.GetPrincipal();
    WriteValue(writer, JSON_PROPERTY_PRINCIPAL, Principal);

    writer.WritePropertyName(JSON_PROPERTY_VALUES);
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

  private static void WriteValue(Utf8JsonWriter writer, T value) {
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
    } else if (value is bool BoolItem) {
      writer.WriteBooleanValue(BoolItem);
    } else if (value is byte[] BytesItem) {
      writer.WriteBase64StringValue(BytesItem);
    }
  }
  private static void WriteValue(Utf8JsonWriter writer, ReadOnlySpan<char> propertyName, T value) {
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
    } else if (value is bool BoolItem) {
      writer.WriteBoolean(propertyName, BoolItem);
    } else if (value is byte[] BytesItem) {
      writer.WriteBase64String(propertyName, BytesItem);
    }
  }

  private static T? ReadValue(Utf8JsonReader reader) {
    if (typeof(T).Name.Equals(typeof(string).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetString() ?? "", typeof(string));
    }

    if (typeof(T).Name.Equals(typeof(int).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetInt32(), typeof(int));
    }

    if (typeof(T).Name.Equals(typeof(long).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetInt64(), typeof(long));
    }

    if (typeof(T).Name.Equals(typeof(float).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetSingle(), typeof(float));
    }

    if (typeof(T).Name.Equals(typeof(double).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetDouble(), typeof(double));
    }

    if (typeof(T).Name.Equals(typeof(bool).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetBoolean(), typeof(bool));
    }

    if (typeof(T).Name.Equals(typeof(byte[]).Name, StringComparison.InvariantCultureIgnoreCase)) {
      return (T)Convert.ChangeType(reader.GetBytesFromBase64(), typeof(byte[]));
    }



    return default(T);
  }
}
