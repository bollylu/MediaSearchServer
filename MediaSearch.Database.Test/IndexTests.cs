﻿namespace MediaSearch.Test.Database;

[TestClass]
public class IndexTests {
  [TestMethod]
  public void Intanciate_TMSIndex_Empty() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();
    Assert.IsNotNull(Target);

    Dump(Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_AddKeys() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");

    Dump(Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_AddKeys_DuplicateAllowed() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");

    Dump(Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_GetKey_KeyExist() {
    IMSIndex<Mockup_Record_IID, string, string> Source = new TMSIndex<Mockup_Record_IID, string, string>();

    Source.Add("value1", "1234567890");
    Source.Add("value1", "1234567891");

    Dump(Source);

    string? Target = Source.Get("value1");
    Dump(Target);
    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void TMSIndex_GetKey_KeyDoesNotExist_UseDefault() {
    IMSIndex<Mockup_Record_IID, string, string> Source = new TMSIndex<Mockup_Record_IID, string, string>();

    Source.Add("value1", "1234567890");
    Source.Add("value1", "1234567891");

    Dump(Source);

    string? Target = Source.Get("value2");
    Dump(Target);
    Assert.IsNull(Target);

    Target = Source.Get("value2", "000000");
    Dump(Target);
    Assert.AreEqual(Target, "000000");
  }

  [TestMethod]
  public void TMSIndex_Clear() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");
    Dump(Target);

    Target.Clear();
    Dump(Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");
    Dump(Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value1");
    Dump(Target);

    Assert.AreEqual(1, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete_ValueMissing() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");
    Dump(Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value3");
    Dump(Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete_ValueDuplicate() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");
    Dump(Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value1");
    Dump(Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }
}
