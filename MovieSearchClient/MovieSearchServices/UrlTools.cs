using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MovieSearchClientServices {
  public static class UrlTools {
    public static string ToUrl(this string source) {
      return WebUtility.UrlEncode(source);
    }
    public static string ToUrl64(this string source) {
      return Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(source));
    }

    public static string FromUrl(this string source) {
      return WebUtility.UrlDecode(source);
    }

    public static string FromUrl64(this string source) {
      return Encoding.UTF8.GetString(Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(source));
    }

    public static string NormalizePath(this string path) {
      if (OperatingSystem.IsWindows()) {
        return path.Replace('/', '\\');
      }

      if (OperatingSystem.IsLinux()) {
        return path.Replace('\\', '/');
      }

      return path;
    }
  }
}
