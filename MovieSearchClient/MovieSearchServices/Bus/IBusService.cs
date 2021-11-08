using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearchClientServices {
  public interface IBusService<T> {
    event Action<string, T> OnMessage;
    void SendMessage(string source, T message);
  }


}
