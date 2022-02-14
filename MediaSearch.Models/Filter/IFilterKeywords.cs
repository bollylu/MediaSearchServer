using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSearch.Models;

public interface IFilterKeywords {

  string Keywords { get; }
  EFilterType KeywordsSelection { get; }

}
