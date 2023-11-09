using System.Runtime.CompilerServices;

namespace MediaSearch.Server.Services;
public class TAuditServiceFile : AAuditService, IAuditServiceFile, IDisposable {

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



  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TAuditServiceFile() : base() {
    _OutputStream = new FileStream(FullFilename, FileMode.Append, FileAccess.Write, FileShare.Read);
    Encoding EncodingWithoutBOM = new UTF8Encoding(false);
    _Writer = new StreamWriter(_OutputStream, EncodingWithoutBOM);
  }
  public TAuditServiceFile(string auditPath, string auditFilename) : base() {
    AuditPath = auditPath;
    AuditFilename = auditFilename;
    _OutputStream = new FileStream(FullFilename, FileMode.Append, FileAccess.Write, FileShare.Read);
    Encoding EncodingWithoutBOM = new UTF8Encoding(false);
    _Writer = new StreamWriter(_OutputStream, EncodingWithoutBOM);
  }

  public override void Dispose() {
    try {
      _Lock.Wait();
      _Writer?.Close();
      _Writer?.Dispose();
      _OutputStream?.Close();
      _OutputStream?.Dispose();
    } finally {
      _Lock.Release();
    }
    base.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override void Audit(string username, string message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      _Writer.WriteLine(CreateAuditLine(username, source, message));
      _Writer.Flush();
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      _Writer.WriteLine(CreateAuditLine(username, source, message, additionalData));
      _Writer.Flush();
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, object message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      _Writer.WriteLine(CreateAuditLine(username, source, message.ToString()));
      _Writer.Flush();
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      _Writer.WriteLine(CreateAuditLine(username, source, message.ToString(), additionalData.ToString()));
      _Writer.Flush();
    } finally {
      _Lock.Release();
    }
  }

  public override async Task AuditAsync(string username, string message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      await _Writer.WriteLineAsync(CreateAuditLine(username, source, message));
      await _Writer.FlushAsync();
    } finally {
      _Lock.Release();
    }
  }

  public override async Task AuditAsync(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      await _Writer.WriteLineAsync(CreateAuditLine(username, source, message, additionalData));
      await _Writer.FlushAsync();
    } finally {
      _Lock.Release();
    }
  }

  public override async Task AuditAsync(string username, object message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      await _Writer.WriteLineAsync(CreateAuditLine(username, source, message.ToString()));
      await _Writer.FlushAsync();
    } finally {
      _Lock.Release();
    }
  }

  public override async Task AuditAsync(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      await _Writer.WriteLineAsync(CreateAuditLine(username, source, message.ToString(), additionalData.ToString()));
      await _Writer.FlushAsync();
    } finally {
      _Lock.Release();
    }
  }

}
