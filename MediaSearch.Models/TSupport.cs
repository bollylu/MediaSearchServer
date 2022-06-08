﻿using System.Diagnostics;
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
  public static void IfDebugMessage(this ILogger logger, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebug(message?.ToString() ?? "(null)", CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageEx(this ILogger logger, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugExBox(message?.ToString() ?? "(null)", CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageBox(this ILogger logger, string title, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugBox(title, message ?? "(null)", GlobalSettings.DEBUG_BOX_WIDTH, CallerName);
  }

  [Conditional("DEBUG")]
  public static void IfDebugMessageExBox(this ILogger logger, string title, object? message, [CallerMemberName] string CallerName = "") {
    logger.LogDebugExBox(title, message ?? "(null)", GlobalSettings.DEBUG_BOX_WIDTH, CallerName);
  }

}

