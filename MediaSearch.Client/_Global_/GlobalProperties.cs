using System.Reflection;

namespace MediaSearch.Client;

public static class GlobalProperties {
  public static TAbout About {
    get {
      if (_About is null) {
        _About = new TAbout(Assembly.GetExecutingAssembly());
      }
      return _About;
    }
  }
  private static TAbout? _About = null;

}
