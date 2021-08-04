using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovie {

    string LocalName { get; }
    string LocalPath { get; }
    string Group { get; }
    long Size { get; }
    string Picture { get; }
  }
}
