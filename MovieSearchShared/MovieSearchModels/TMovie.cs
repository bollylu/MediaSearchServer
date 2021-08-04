using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public class TMovie : IMovie {
    public string LocalName { get; set; }
    public string LocalPath { get; set; }
    public string Group { get; set; }
    public long Size { get; set; }
    public string Picture { get; set; }


    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.Append($"{LocalName}");
      RetVal.Append($", [{LocalPath}]");
      RetVal.Append($", [{Group}]");
      return RetVal.ToString();
    }
  }
}
