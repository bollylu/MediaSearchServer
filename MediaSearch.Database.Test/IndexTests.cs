namespace MediaSearch.Database.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class IndexTests {
  [TestMethod]
  public void Intanciate_TMSIndex_Empty() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();
    Assert.IsNotNull(Target);

    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_AddKeys() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");

    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_AddKeys_DuplicateAllowed() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");

    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_GetKey_KeyExist() {
    IMSIndex<Mockup_Record_IID, string, string> Source = new TMSIndex<Mockup_Record_IID, string, string>();
    
    Source.Add("value1", "1234567890");
    Source.Add("value1", "1234567891");

    TraceMessage($"{nameof(Source)} : {Source.GetType().GetGenericName()}", Source);

    string? Target = Source.Get("value1");
    TraceMessage($"{nameof(Target)} : {Target?.GetType().Name}", Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void TMSIndex_GetKey_KeyDoesNotExist_UseDefault() {
    IMSIndex<Mockup_Record_IID, string, string> Source = new TMSIndex<Mockup_Record_IID, string, string>();

    Source.Add("value1", "1234567890");
    Source.Add("value1", "1234567891");

    TraceMessage($"{nameof(Source)} : {Source.GetType().GetGenericName()}", Source);

    string? Target = Source.Get("value2");
    TraceMessage($"{nameof(Target)} : {Target?.GetType().Name}", Target);
    Assert.IsNull(Target);

    Target = Source.Get("value2", "000000");
    TraceMessage($"{nameof(Target)} : {Target?.GetType().Name}", Target);
    Assert.AreEqual(Target, "000000");
  }

  [TestMethod]
  public void TMSIndex_Clear() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Target.Clear();
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value1");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(1, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete_ValueMissing() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value2", "1234567891");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value3");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(2, Target.IndexedValues.Count);
  }

  [TestMethod]
  public void TMSIndex_Delete_ValueDuplicate() {
    IMSIndex<Mockup_Record_IID, string, string> Target = new TMSIndex<Mockup_Record_IID, string, string>();

    Target.Add("value1", "1234567890");
    Target.Add("value1", "1234567891");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);
    Assert.AreEqual(2, Target.IndexedValues.Count);

    Target.Delete("value1");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.AreEqual(0, Target.IndexedValues.Count);
  }
}
