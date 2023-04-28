using System.Net;

namespace MediaSearch.Models;

public interface IUserAccountInfo {
  string Name { get; set; }
  string Description { get; set; }
  IPAddress RemoteIp { get; set; }
  DateTime LastSuccessfulLogin { get; set; }
  DateTime LastFailedLogin { get; set; }
}
