namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TAboutSerializationTest {

  [TestMethod]
  public void Serialize() {

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
  public void Deserialize() {

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