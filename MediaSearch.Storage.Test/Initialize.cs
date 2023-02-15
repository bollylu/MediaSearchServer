namespace MediaSearch.Storage.Test;

[TestClass]
public class Initialize {
  [AssemblyInitialize]
  public static void Init(TestContext context) {
    MediaSearch.Models.GlobalSettings.Initialize().Wait();
    MediaSearch.Storage.GlobalSettings.Initialize().Wait();
  }
}
