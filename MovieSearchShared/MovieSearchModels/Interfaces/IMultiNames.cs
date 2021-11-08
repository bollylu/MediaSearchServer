using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchModels.Interfaces {
  public interface IMultiNames {
    
    /// <summary>
    /// A list of alternate KeyValuePairs (language & name) (0 <= count <= n)
    /// </summary>
    Dictionary<string, string> AltNames { get; }

  }
}
