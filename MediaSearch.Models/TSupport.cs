using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MediaSearch.Models;

public static class Support {

  public static byte[] GetPicture(string pictureName, string pictureExtension = ".png") {
    try {
      Assembly Asm = Assembly.GetExecutingAssembly();
      string CompleteName = $"MediaSearch.Models.Pictures.{pictureName}{pictureExtension}";
      string[] Resources = Asm.GetManifestResourceNames();
      foreach (string ResourceItem in Resources) {
        Console.WriteLine(ResourceItem);
      }
      using (Stream? ResourceStream = Asm.GetManifestResourceStream(CompleteName)) {
        if (ResourceStream is null) {
          return Array.Empty<byte>();
        }
        using (BinaryReader reader = new BinaryReader(ResourceStream)) {
          return reader.ReadBytes((int)reader.BaseStream.Length);
        }
      }
    } catch (Exception ex) {
      Trace.WriteLine($"Unable to get picture {pictureName}{pictureExtension} : {ex.Message}");
      return Array.Empty<byte>();
    }
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessage<T>(this IMediaSearchLogger<T> logger, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebug(message?.ToString() ?? "(null)", CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageEx<T>(this IMediaSearchLogger<T> logger, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugExBox(message?.ToString() ?? "(null)", CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageBox<T>(this IMediaSearchLogger<T> logger, string title, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugBox(title, message ?? "(null)", CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageExBox<T>(this IMediaSearchLogger<T> logger, string title, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugExBox(title, message ?? "(null)", CallerName);
  }


  public static string DisplayTime(this TimeSpan source) {
    if (source.TotalMilliseconds < 1_000) { // 1 second
      return $"{source.TotalMilliseconds} ms";
    }
    if (source.TotalMilliseconds < 60_000) { // 1 minute
      return $"{source.Seconds} s {source.Milliseconds} ms";
    }
    if (source.TotalMilliseconds < 3_600_000) { // 1 hour
      return $"{source.Hours} h {source.Seconds} s {source.Milliseconds} ms";
    }

    return $"{source.Days} d {source.Hours} h {source.Seconds} s {source.Milliseconds} ms";
  }

  public static StringBuilder AppendIndent(this StringBuilder builder, string source, int indent = 2) {
    string IndentSpace = new string(' ', indent);
    foreach (string LineItem in source.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)) {
      builder.AppendLine($"{IndentSpace}{LineItem}");
    }
    return builder;
  }

  public static StringBuilder AppendIndent(this StringBuilder builder, object source, int indent = 2) {
    string IndentSpace = new string(' ', indent);
    string DisplayData = source?.ToString() ?? "";
    foreach (string LineItem in DisplayData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)) {
      builder.AppendLine($"{IndentSpace}{LineItem}");
    }
    return builder;
  }
}

