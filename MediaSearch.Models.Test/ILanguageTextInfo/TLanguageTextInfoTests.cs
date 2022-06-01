namespace MediaSearch.Models.LanguageTextInfo.Test;

[TestClass]
public class TLanguageTextInfoTests {
  [TestMethod]
  public void Instanciate_TLanguageTestInfo() {
    ILanguageTextInfo Target = new TLanguageTextInfo();
    Assert.IsNotNull(Target);
    Assert.AreEqual(ELanguage.Unknown, Target.Language);
    Assert.AreEqual("", Target.Value);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TLanguageTestInfo_WithData() {
    ILanguageTextInfo Target = new TLanguageTextInfo(ELanguage.French, "Coucou");
    Assert.IsNotNull(Target);
    Assert.AreEqual(ELanguage.French, Target.Language);
    Assert.AreEqual("Coucou", Target.Value);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TLanguageTestInfos() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Assert.IsNotNull(Target);
    Assert.AreEqual(0, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.IsFalse(Target.HasMoreThanOneTextPerLanguage());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TLanguageTestInfos_AddValidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Assert.AreEqual(2, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    Assert.IsFalse(Target.HasMoreThanOneTextPerLanguage());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TLanguageTestInfos_AddInvalidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Assert.AreEqual(2, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    Assert.IsTrue(Target.HasMoreThanOneTextPerLanguage());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_Empty() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.SetPrincipal(ELanguage.English);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_AlreadyPrincipal() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Target.SetPrincipal(ELanguage.French);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_InvalidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Target.SetPrincipal(ELanguage.English);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_InvalidData2() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Target.SetPrincipal(ELanguage.English);
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Target.SetPrincipal(ELanguage.French);
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipalViaLanguage() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Target.SetPrincipal(ELanguage.English);
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipalViaTLanguageTextInfo() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    ILanguageTextInfo Item1 = new TLanguageTextInfo(ELanguage.French, "Coucou");
    ILanguageTextInfo Item2 = new TLanguageTextInfo(ELanguage.English, "Hello");
    Target.Add(Item1);
    Target.Add(Item2);
    Target.SetPrincipal(Item2);
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }
}
