using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public class TMovies : IMovies {
    public string Name { get; set; }
    public List<TMovie> Movies { get; set; } = new List<TMovie>();
    public int Page { get; set; }
    public int AvailablePages { get; set; }
    public string Source { get; set; }


    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"{Name} / page {Page}/{AvailablePages}");
      foreach(IMovie MovieItem in Movies) {
        RetVal.AppendLine(MovieItem.ToString());
      }
      return RetVal.ToString();
    }
  }
}
