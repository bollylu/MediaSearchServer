namespace MediaSearch.Models;

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
      return $"{Size:N3} MB";
    }

    if (size > KB) {
      double Size = (double)size / (double)KB;
      return $"{Size:N3} KB";
    }

    return $"{size} B";
  }

}
