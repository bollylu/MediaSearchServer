using BLTools.Text;

namespace MediaSearch.Models.Test;

[TestClass]
public class TAboutSerializationTest {

  //[ClassInitialize]
  //public static async Task ClassInitialize(TestContext context) {
  //  await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  //}

  [TestMethod]
  public void SerializeAboutWithoutConverter() {

    IAbout Source = new TAbout() {
      Name = "LibTest",
      Description = "Test library",
      CurrentVersion = new Version(0,0,1), 
      ChangeLog="1st version"
    };
    TraceMessage("Source", Source);

    string Target = Source.ToJson();

    TraceMessage("Target", Target);

    Assert.IsNotNull(Target);


  }

  [TestMethod]
  public void DeserializeAboutWithoutConverter() {

    string Source = "{" +
      "\"Name\":\"libtest\"," +
      "\"Description\":\"Test library\"," +
      "\"CurrentVersion\":\"0.0.1\"," +
      " \"ChangeLog\":\"No news\"" +
      "}";

    TraceMessage("Source", Source);

    TAbout? Target = IJson<TAbout>.FromJson(Source);
    Assert.IsNotNull(Target);
    TraceMessage("Target", Target);

    Assert.AreEqual(Target.CurrentVersion.ToString(), "0.0.1");
    Assert.AreEqual(Target.ChangeLog, "No news");

  }

}