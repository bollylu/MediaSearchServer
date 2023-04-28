using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSearch.Models;

public interface IFilterKeywords {

  /// <summary>
  /// List of keywords in a string, separated by space
  /// </summary>
  string Keywords { get; }

  /// <summary>
  /// How to combine multiple keywords
  /// </summary>
  EFilterType KeywordsSelection { get; }

}
