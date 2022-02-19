using BLTools.Text;

namespace MediaSearch.Models.Test;

[TestClass]
public class TAboutSerializationTest {

  [ClassInitialize]
  public static async Task ClassInitialize(TestContext context) {
    await MediaSearch.Models.GlobalSettings.Initialize().ConfigureAwait(false);
  }

  [TestMethod]
  public void SerializeAboutWithoutConverter() {

    TAbout Source = new TAbout() {
      Name = "LibTest",
      Description = "Test library",
      CurrentVersion = new Version(0,0,1), 
      ChangeLog="1st version"
    };
    Console.WriteLine(Source.ToString().BoxFixedWidth("Source", GlobalSettings.DEBUG_BOX_WIDTH));
    string Target = Source.ToJson();
    Console.WriteLine(Target.BoxFixedWidth("Serialized", GlobalSettings.DEBUG_BOX_WIDTH));
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

    Console.WriteLine(Source.BoxFixedWidth("Source", GlobalSettings.DEBUG_BOX_WIDTH));

    TAbout? Target = TAbout.FromJson(Source);
    Assert.IsNotNull(Target);
    Console.WriteLine(Target.ToString().BoxFixedWidth("Deserialized", GlobalSettings.DEBUG_BOX_WIDTH));

    Assert.AreEqual(Target.CurrentVersion.ToString(), "0.0.1");
    Assert.AreEqual(Target.ChangeLog, "No news");

  }

}