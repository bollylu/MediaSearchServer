using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaSearch.Server.Services;

public interface IDataProvider : IName, ILoggable {

  string RootStoragePath { get; }



}
