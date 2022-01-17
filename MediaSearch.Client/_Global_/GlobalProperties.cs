using System.Reflection;

namespace MediaSearch.Client;

public static class GlobalProperties {
  public static TAbout About {
    get {
      if (_About is null) {
        string SourceName = Assembly.GetExecutingAssembly()?.GetName()?.Name ?? "";
        _About = new TAbout() {
          VersionSource = $"{SourceName}._Global_.version.txt",
          ChangeLogSource = $"{SourceName}._Global_.changelog.txt"
        };
      }
      return _About;
    }
  }
  private static TAbout? _About = null;

}
