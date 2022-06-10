namespace MediaSearch.Models.LanguageTextInfo.Test;

[TestClass]
public class TLanguageTextInfosTests {


  [TestMethod]
  public void Instanciate_Empty_TLanguageTestInfos() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Assert.IsNotNull(Target);
    Assert.AreEqual(0, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_AddValidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Dump(Target);

    Assert.AreEqual(2, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_AddInvalidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Dump(Target);
    Message("Adding invalid data, refused");
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Dump(Target);

    Assert.AreEqual(1, Target.GetAll().Count());
    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_Empty() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.SetPrincipal(ELanguage.English);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_AlreadyPrincipal() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Dump(Target);
    Message("Changing principal to already principal, refused");
    Target.SetPrincipal(ELanguage.French);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_InvalidData() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Dump(Target);
    Message("Changing principal to non-existing, refused");
    Target.SetPrincipal(ELanguage.English);
    Dump(Target);
    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipal_InvalidData2() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Hello"));
    Dump(Target);
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Dump(Target);

    Message("Set principal to English");
    Target.SetPrincipal(ELanguage.English);
    Dump(Target);
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Message("Set principal to French");
    Target.SetPrincipal(ELanguage.French);
    Dump(Target);
    Assert.AreEqual(ELanguage.French, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipalViaLanguage() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    Target.Add(new TLanguageTextInfo(ELanguage.French, "Coucou"));
    Target.Add(new TLanguageTextInfo(ELanguage.English, "Hello"));
    Dump(Target);

    Message("Set principal to English");
    Target.SetPrincipal(ELanguage.English);
    Dump(Target);

    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Ok();
  }

  [TestMethod]
  public void TLanguageTestInfos_SetPrincipalViaTLanguageTextInfo() {
    ILanguageTextInfos Target = new TLanguageTextInfos();
    ILanguageTextInfo Item1 = new TLanguageTextInfo(ELanguage.French, "Coucou");
    ILanguageTextInfo Item2 = new TLanguageTextInfo(ELanguage.English, "Hello");
    Target.Add(Item1);
    Target.Add(Item2);
    Dump(Target);

    Message("Set principal to item2");
    Target.SetPrincipal(Item2);
    Dump(Target);

    Assert.IsFalse(Target.HasMoreThanOnePrincipal());
    Assert.AreEqual(ELanguage.English, Target.GetPrincipal()?.Language ?? ELanguage.Unknown);

    Ok();
  }
}
