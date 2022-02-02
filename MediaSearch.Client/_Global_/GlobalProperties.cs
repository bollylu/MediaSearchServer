using System.Reflection;

namespace MediaSearch.Client;

public static class GlobalProperties {
  public static TAbout About {
    get {
      return _About ??= new TAbout(Assembly.GetExecutingAssembly());
    }
  }
  private static TAbout? _About = null;

}
