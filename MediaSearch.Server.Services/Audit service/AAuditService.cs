using System.Runtime.CompilerServices;

namespace MediaSearch.Server.Services;
public abstract class AAuditService : ALoggable, IAuditService, IDisposable {

  public const char FIELD_SEPARATOR = '|';
  private const string NULL_VALUE = "(null)";

  protected readonly SemaphoreSlim _Lock = new SemaphoreSlim(1);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AAuditService() {
    Logger = GlobalSettings.LoggerPool.GetLogger<AAuditService>();
  }

  public virtual void Dispose() {
    _Lock?.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public virtual void Audit(string username, string message, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual void Audit(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual void Audit(string username, object message, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual void Audit(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual Task AuditAsync(string username, string message, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual Task AuditAsync(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual Task AuditAsync(string username, object message, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  public virtual Task AuditAsync(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    throw new NotImplementedException();
  }

  protected string CreateAuditLine(string username, string source, string? message = NULL_VALUE, string? additionalData = NULL_VALUE) {
    StringBuilder RetVal = new();
    RetVal.Append(DateTime.UtcNow.ToYMDHMS());
    RetVal.Append(FIELD_SEPARATOR);
    RetVal.Append(source);
    RetVal.Append(FIELD_SEPARATOR);
    RetVal.Append(username);
    RetVal.Append(FIELD_SEPARATOR);
    RetVal.Append(message ?? NULL_VALUE);
    RetVal.Append(FIELD_SEPARATOR);
    RetVal.Append(additionalData ?? NULL_VALUE);
    return RetVal.ToString();
  }
}
