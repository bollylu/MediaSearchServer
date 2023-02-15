namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TAboutSerializationTest {

  [TestMethod]
  public void Serialize() {

    Message("Creating About");
    IAbout Source = new TAbout() {
      Name = "LibTest",
      Description = "Test library",
      CurrentVersion = new Version(0, 0, 1),
      ChangeLog = "1st version"
    };

    Dump(Source, "Source");
    Message("Serialize to Json");
    string Target = Source.ToJson();

    Dump(Target, "Target");

    Assert.IsNotNull(Target);

    Ok();

  }

  [TestMethod]
  public void Deserialize() {

    Message("Creating Json About");
    string Source = "{" +
      "\"Name\":\"libtest\"," +
      "\"Description\":\"Test library\"," +
      "\"CurrentVersion\":\"0.0.1\"," +
      " \"ChangeLog\":\"No news\"" +
      "}";

    Dump(Source, "Source");

    Message("Deserialize from Json into a TAbout");
    TAbout? Target = IJson<TAbout>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.AreEqual(Target.CurrentVersion.ToString(), "0.0.1");
    Assert.AreEqual(Target.ChangeLog, "No news");

    Ok();
  }

}