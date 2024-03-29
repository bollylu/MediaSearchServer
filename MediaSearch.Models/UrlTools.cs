﻿using System.Net;

namespace MediaSearch.Models;

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
    return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
  }
}

