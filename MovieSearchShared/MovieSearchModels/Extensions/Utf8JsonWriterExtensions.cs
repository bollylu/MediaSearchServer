using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public static class Utf8JsonWriterExtensions {

    public static void WriteRawValue(this Utf8JsonWriter writer, string utf8Json) {
      if (string.IsNullOrWhiteSpace(utf8Json)) {
        return;
      }

      using (JsonDocument Document = JsonDocument.Parse(utf8Json)) {
        Document.RootElement.WriteTo(writer);
      }
    }

  }
}
