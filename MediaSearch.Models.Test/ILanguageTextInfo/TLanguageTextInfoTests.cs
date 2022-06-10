namespace MediaSearch.Models.LanguageTextInfo.Test;

[TestClass]
public class TLanguageTextInfoTests {
  [TestMethod]
  public void Instanciate_Empty_TLanguageTestInfo() {
    ILanguageTextInfo Target = new TLanguageTextInfo();
    Assert.IsNotNull(Target);
    Assert.AreEqual(ELanguage.Unknown, Target.Language);
    Assert.AreEqual("", Target.Value);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void Instanciate_TLanguageTestInfo_WithData() {
    ILanguageTextInfo Target = new TLanguageTextInfo(ELanguage.French, "Coucou");
    Assert.IsNotNull(Target);
    Assert.AreEqual(ELanguage.French, Target.Language);
    Assert.AreEqual("Coucou", Target.Value);
    Dump(Target);
    Ok();
  }

}
