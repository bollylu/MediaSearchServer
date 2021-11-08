using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchModels {
  public interface IName {

    /// <summary>
    /// The unique name of the item
    /// </summary>
    string Name { get; set; }

    /// <summary>
    /// An extended description
    /// </summary>
    string Description { get; set; }
  }
}
