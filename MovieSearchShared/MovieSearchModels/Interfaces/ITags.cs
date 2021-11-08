using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public interface ITags {

    /// <summary>
    /// A list of tags (0 <= count <= n)
    /// </summary>
    List<string> Tags { get; }

  }
}
