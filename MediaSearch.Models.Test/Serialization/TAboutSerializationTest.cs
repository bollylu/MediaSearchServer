namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TAboutSerializationTest {

  [TestMethod]
  public void Serialize() {

    IAbout Source = new TAbout() {
      Name = "LibTest",
      Description = "Test library",
      CurrentVersion = new Version(0, 0, 1),
      ChangeLog = "1st version"
    };
    Dump(Source);

    string Target = Source.ToJson();

    Dump(Target);

    Assert.IsNotNull(Target);
    Ok();

  }

  [TestMethod]
  public void Deserialize() {

    string Source = "{" +
      "\"Name\":\"libtest\"," +
      "\"Description\":\"Test library\"," +
      "\"CurrentVersion\":\"0.0.1\"," +
      " \"ChangeLog\":\"No news\"" +
      "}";

    Dump(Source);

    TAbout? Target = IJson<TAbout>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(Target.CurrentVersion.ToString(), "0.0.1");
    Assert.AreEqual(Target.ChangeLog, "No news");

    Ok();
  }

}