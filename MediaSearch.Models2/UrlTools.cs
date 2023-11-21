using System.Net;
using System.Text.Encodings.Web;

namespace MediaSearch.Models;

public static class UrlTools {

  public static string ToUrl(this string source) {
    return WebUtility.UrlEncode(source);
  }

  public static string ToUrl64(this string source) {
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(UrlEncoder.Default.Encode(source)));
  }

  public static string FromUrl(this string source) {
    return WebUtility.UrlDecode(source);
  }

  public static string FromUrl64(this string source) {
    return Encoding.UTF8.GetString(Convert.FromBase64String(source));
    //Encoding.UTF8.GetString(Microsoft.AspNetCore.WebUtilities.WebEncoders.Base64UrlDecode(source));
  }

  public static string NormalizePath(this string path, bool forWindows = true) {
    if (forWindows) {
      return path.Replace('/', '\\');
    } else {
      return path.Replace('\\', '/');
    }
  }
}

