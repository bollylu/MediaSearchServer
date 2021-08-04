using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  /// <summary>
  /// Implement a movie
  /// </summary>
  public class TMovie : IMovie {
    public string LocalName { get; set; }
    public List<string> AltNames { get; } = new();

    public string LocalPath { get; set; }
    public string Group { get; set; }
    public long Size { get; set; }
    public string Picture { get; set; }

    public List<string> Tags { get; } = new();

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"{LocalName}");
      RetVal.Append($", [{LocalPath}]");
      RetVal.AppendLine($", {Size}");
      RetVal.AppendLine($" {Group}");
      return RetVal.ToString();
    }
  }
}
