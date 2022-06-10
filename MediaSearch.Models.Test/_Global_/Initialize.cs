namespace MediaSearch.Models.Test;

[TestClass]
public class Initialize {
  [AssemblyInitialize]
  public static void Init(TestContext context) {
    MediaSearch.Models.GlobalSettings.Initialize().Wait();
  }
}
