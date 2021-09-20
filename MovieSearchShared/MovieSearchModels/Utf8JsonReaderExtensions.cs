using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using BLTools;
using BLTools.Text;
using static BLTools.Text.TextBox;

using BLTools.Diagnostic.Logging;
using System.CodeDom.Compiler;

namespace MovieSearch.Models {
  public static class Utf8JsonReaderExtensions {

    public static bool GetNextToken(this ref Utf8JsonReader reader, ILogger logger, int indent = 0) {

      bool RetVal = reader.Read();
      logger?.Log($"{Spaces(2 * reader.CurrentDepth)}{Spaces(indent)}{reader.CurrentDepth}. Token ({reader.BytesConsumed}) => {reader.TokenType}");

      return RetVal;
    }


    //public static string GetNextString(ref Utf8JsonReader reader) {

    //  bool Ok = reader.Read();
    //  string RetVal = reader.GetString();
    //  Logger?.Log($"{Spaces(2 * reader.CurrentDepth)}{Spaces(_Indent)}{reader.CurrentDepth}. Value ({reader.BytesConsumed}) => {reader.TokenType}");

    //  return RetVal;
    //}

  }
}
