namespace MediaSearch.Database.Test;
[TestClass]
public class TableTests {

  [TestMethod]
  public void Instanciate_TMSTable_NotTyped_Empty() {
    Message("Instanciate empty untyped table");
    IMSTable Target = new TMSTable() { Name = "Test table" };
    Dump(Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TMSTable_IMovie_Empty() {
    Message("Instanciate empty IMovie table");
    IMSTable Target = new TMSTable<IMovie>() { Name = "Movie table" };
    Dump(Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void TMSTable_IMovie_SetMediaSource() {
    Message("Instanciate IMovie table and add Mediasource");
    IMSTable Target = new TMSTable<IMovie>() { Name = "Movie table" };
    IMediaSource<IMovie> Source = new TMediaSource<IMovie>() {
      RootStorage = "\\\\andromeda.sharenet.priv\\movies"
    };
    Target.Header.SetMediaSource(Source);
    Dump(Target);
  }

  [TestMethod]
  public void TMSTable_IMovie_SetInvalidMediaSource() {
    Message("Instanciate IMovie table and add invalid Mediasource");
    IMSTable Target = new TMSTable<IMovie>() { Name = "Movie table" };
    Dump(Target);

    IMediaSource<IMedia> Source = new TMediaSource<IMedia>() {
      RootStorage = "\\\\andromeda.sharenet.priv\\movies"
    };
    Assert.IsFalse(Target.Header.SetMediaSource(Source));
  }

  //[TestMethod]
  //public void Instanciate_TMSDatabaseJson_WithName() {
  //  IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name="missing.db", Description = "Missing database" };
  //  TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

  //  Assert.IsFalse(Target.Exists());
  //}

  //[TestMethod]
  //public void TMSDatabaseJson_CreateThenRemove() {
  //  IMSDatabase Target = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = "missing.db" };
  //  TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

  //  Assert.IsFalse(Target.Exists());
  //  Assert.IsTrue(Target.Create());
  //  Assert.IsTrue(Target.Exists());
  //  Assert.IsTrue(Target.Remove());
  //  Assert.IsFalse(Target.Exists());
  //}
}