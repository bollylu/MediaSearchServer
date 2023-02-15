using BLTools;

using MediaSearch.Models;

namespace BuildMoviesJson;
public static class Support {

  public static List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

  public static IEnumerable<IFileInfo> FetchFiles(string rootStoragePath, CancellationToken token) {

    DirectoryInfo RootFolder = new DirectoryInfo(rootStoragePath);

    foreach (IFileInfo FileInfoItem in RootFolder.EnumerateFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                 .Where(f => f.Extension.ToLowerInvariant().IsIn(MoviesExtensions))
                                                 .Select(f => new TFileInfo(f))) {

      if (token.IsCancellationRequested) {
        yield break;
      }

      yield return FileInfoItem;
    }
  }
}
