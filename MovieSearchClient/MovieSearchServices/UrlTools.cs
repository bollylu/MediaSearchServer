﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace MovieSearchClientServices {
  public static class UrlTools {
    public static string ToUrl(this string source) {
      return WebUtility.UrlEncode(source);
    }

    public static string FromUrl(this string source) {
      return WebUtility.UrlDecode(source);
    }
  }
}
