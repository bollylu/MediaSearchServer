﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Client.Services {
  public interface IBusService<T> {
    event Action<string, T> OnMessage;
    void SendMessage(string source, T message);
  }


}
