using System;

namespace MediaSearch.Client;

public static class DisplayConverters {

  public static string DisplaySizeInUnit(long size) {
    const long KB = 1024;
    const long MB = 1024 * KB;
    const long GB = 1024 * MB;

    if (size > GB) {
      double Size = (double)size / (double)GB;
      return $"{Size:N3} GB";

    }
    if (size > MB) {
      double Size = (double)size / (double)MB;
      return $"{Size:N0} MB";
    }

    if (size > KB) {
      double Size = (double)size / (double)KB;
      return $"{Size:N0} KB";
    }

    return $"{size} B";
  }

  public static string DisplayPathNormalized(string path) {

    if (path.Contains('/')) {
      string[] Components = path.BeforeLast('/').Split('/', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      return string.Join(" > ", Components);
    } else {
      string[] Components = path.BeforeLast('\\').Split('\\', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
      return string.Join(" > ", Components);
    }
  }
}
