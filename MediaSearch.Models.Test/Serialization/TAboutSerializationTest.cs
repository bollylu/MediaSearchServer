namespace MediaSearch.Models.Test;

[TestClass]
public class TAboutSerializationTest {

  [TestMethod]
  public void SerializeAboutWithoutConverter() { 

    TAbout About = new TAbout() { CurrentVersion = new Version(0,0,1), ChangeLog="1st version"};

    string Serialized = About.ToJson();

    Assert.IsNotNull(Serialized);


  }

  [TestMethod]
  public void DeserializeAboutWithoutConverter() {

    string Source = "{\"CurrentVersion\":\"0.0.1\", \"ChangeLog\":\"No news\"}";

    TAbout? About = TAbout.FromJson(Source);

    Assert.IsNotNull(About);
    Assert.AreEqual(About.CurrentVersion.ToString(), "0.0.1");
    Assert.AreEqual(About.ChangeLog, "No news");

  }

}