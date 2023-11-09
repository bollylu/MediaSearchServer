using System.Runtime.CompilerServices;

namespace MediaSearch.Server.Services;
public interface IAuditService {
  void Audit(string username, string message, [CallerMemberName] string source = "");
  void Audit(string username, string message, string additionalData, [CallerMemberName] string source = "");
  void Audit(string username, object message, [CallerMemberName] string source = "");
  void Audit(string username, object message, object additionalData, [CallerMemberName] string source = "");

  Task AuditAsync(string username, string message, [CallerMemberName] string source = "");
  Task AuditAsync(string username, string message, string additionalData, [CallerMemberName] string source = "");
  Task AuditAsync(string username, object message, [CallerMemberName] string source = "");
  Task AuditAsync(string username, object message, object additionalData, [CallerMemberName] string source = "");
}
