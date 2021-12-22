using Microsoft.Extensions.Configuration;

namespace MediaSearch.Server.Support;

public static class ConfigurationExtensions {

  public static string DumpConfig(this IConfiguration configuration) {
    StringBuilder RetVal = new StringBuilder();
    foreach ( var ConfigItem in configuration.AsEnumerable().OrderBy(c => c.Key) ) {
      RetVal.AppendLine($"{ConfigItem.Key} = {ConfigItem.Value}");
    }
    return RetVal.ToString();
  }

}
