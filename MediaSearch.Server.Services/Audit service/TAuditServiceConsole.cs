using System.Runtime.CompilerServices;

namespace MediaSearch.Server.Services;
public class TAuditServiceConsole : AAuditService, IAuditServiceConsole {

  public ConsoleColor BackgroundColor { get; init; }
  public ConsoleColor ForegroundColor { get; init; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TAuditServiceConsole() : base() {
    BackgroundColor = Console.BackgroundColor;
    ForegroundColor = Console.ForegroundColor;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override void Audit(string username, string message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message, additionalData), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, object message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message.ToString()), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
  }

  public override void Audit(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message.ToString(), additionalData.ToString()), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
  }

  public override Task AuditAsync(string username, string message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
    return Task.CompletedTask;
  }

  public override Task AuditAsync(string username, object message, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message.ToString()), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
    return Task.CompletedTask;
  }

  public override Task AuditAsync(string username, string message, string additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message, additionalData), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
    return Task.CompletedTask;
  }

  public override Task AuditAsync(string username, object message, object additionalData, [CallerMemberName] string source = "") {
    try {
      _Lock.Wait();
      WriteLineInColor(CreateAuditLine(username, source, message.ToString(), additionalData.ToString()), BackgroundColor, ForegroundColor);
    } finally {
      _Lock.Release();
    }
    return Task.CompletedTask;
  }

  private static void WriteLineInColor(string textLine, ConsoleColor backgroundColor, ConsoleColor foregroundColor) {
    ConsoleColor SavedBackgroundColor = Console.BackgroundColor;
    ConsoleColor SavedForegroundColor = Console.ForegroundColor;
    Console.BackgroundColor = backgroundColor;
    Console.ForegroundColor = foregroundColor;
    Console.WriteLine(textLine);
    Console.BackgroundColor = SavedBackgroundColor;
    Console.ForegroundColor = SavedForegroundColor;
  }
}
