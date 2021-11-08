using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public static class JsonElementExtensions {

    public static JsonElement GetPropertyEx(this JsonElement element, string name, StringComparison comp = StringComparison.InvariantCultureIgnoreCase) {

      try {
        return element.EnumerateObject().FirstOrDefault(x => x.Name.Equals(name, comp)).Value;
      } catch {
        throw new JsonException($"Missing property {name} from Json element");
      }
    }

  }
}
