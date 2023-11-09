using System.Net;

namespace MediaSearch.Server.Services;
public interface IAuditServiceSyslog {
  IPAddress ServerAddress { get; init; }
  int SysLogPort { get; init; }
}
