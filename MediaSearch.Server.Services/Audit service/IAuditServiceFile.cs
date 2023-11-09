namespace MediaSearch.Server.Services;
public interface IAuditServiceFile {
  string AuditPath { get; init; }
  string AuditFilename { get; init; }
}
