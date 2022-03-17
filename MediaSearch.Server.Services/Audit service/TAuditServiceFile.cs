using System.Runtime.CompilerServices;

namespace MediaSearch.Server.Services;
public class TAuditServiceFile : IAuditService, IDisposable {

  public const char FIELD_SEPARATOR = '|';
  public string AuditPath { get; init; } = "";
  public string AuditFilename { get; init; } = "";

  public string FullFilename { 
    get { 
      return _FullFilename ??= Path.Combine(AuditPath, AuditFilename); 
    } 
  }
  private string? _FullFilename;

  private readonly Stream _OutputStream;
  private readonly TextWriter _Writer;

  private readonly object _Lock = new object();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TAuditServiceFile() {
    _OutputStream = new FileStream(FullFilename, FileMode.Append, FileAccess.Write, FileShare.Read);
    Encoding EncodingWithoutBOM = new UTF8Encoding(false);
    _Writer = new StreamWriter(_OutputStream, EncodingWithoutBOM);
  }
  public TAuditServiceFile(string auditPath, string auditFilename) {
    AuditPath = auditPath;
    AuditFilename = auditFilename;
    _OutputStream = new FileStream(FullFilename, FileMode.Append, FileAccess.Write, FileShare.Read);
    Encoding EncodingWithoutBOM = new UTF8Encoding(false);
    _Writer = new StreamWriter(_OutputStream, EncodingWithoutBOM);
  }

  public void Dispose() {
    _Writer?.Close();
    _Writer?.Dispose();
    
    _OutputStream?.Close();
    _OutputStream?.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public void Audit(string username, string message, [CallerMemberName]string source = "") {
    lock (_Lock) {
      StringBuilder AuditLine = new();
      AuditLine.Append(DateTime.UtcNow.ToYMDHMS());
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(source);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(username);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(message);
      _Writer.WriteLine(AuditLine.ToString());
      _Writer.Flush();
    }
  }

  public void Audit(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    lock (_Lock) {
      StringBuilder AuditLine = new();
      AuditLine.Append(DateTime.UtcNow.ToYMDHMS());
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(source);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(username);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(message);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(additionalData);
      _Writer.WriteLine(AuditLine.ToString());
      _Writer.Flush();
    }
  }

  public void Audit(string username, object message, [CallerMemberName] string source = "") {
    lock (_Lock) {
      StringBuilder AuditLine = new();
      AuditLine.Append(DateTime.UtcNow.ToYMDHMS());
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(source);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(username);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(message.ToString());
      _Writer.WriteLine(AuditLine.ToString());
      _Writer.Flush();
    }
  }

  public void Audit(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    lock (_Lock) {
      StringBuilder AuditLine = new();
      AuditLine.Append(DateTime.UtcNow.ToYMDHMS());
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(source);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(username);
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(message.ToString());
      AuditLine.Append(FIELD_SEPARATOR);
      AuditLine.Append(additionalData.ToString());
      _Writer.WriteLine(AuditLine.ToString());
      _Writer.Flush();
    }
  }

}
