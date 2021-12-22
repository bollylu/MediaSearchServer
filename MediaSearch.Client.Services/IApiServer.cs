using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearchClientServices {
  public interface IApiServer {
    Uri BaseAddress { get; set; }
  }
}
