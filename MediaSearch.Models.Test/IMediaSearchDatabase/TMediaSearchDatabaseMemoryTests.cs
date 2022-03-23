namespace MediaSearch.Models.MediaSearchDatabase.Test;

[TestClass]
public class TMediaSearchDatabaseMemoryTests {

  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseMemory() {
    TMediaSearchDatabaseMemory Target = new TMediaSearchDatabaseMemory() {
      Name = "test",
      Description = "test db"
    };
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());

    TraceMessage("Target", Target.ToString());
  }

  [TestMethod]
  public void Empty_TMediaSearchDatabaseMemory_AddItems() {
    TMediaSearchDatabaseMemory Target = new TMediaSearchDatabaseMemory() {
      Name = "test",
      Description = "test db"
    };
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());

    IMedia Media = new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    };
    Target.Add(Media);

    TraceMessage("Target after one addition", Target.ToString());
    Assert.AreEqual(1, Target.GetAll().Count());

    IMedia Media2 = new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    };
    Target.Add(Media2);

    TraceMessage("Target after two additions", Target.ToString());

    Assert.AreEqual(2, Target.GetAll().Count());
  }

  [TestMethod]
  public void Duplicate_TMediaSearchDatabaseMemory() {
    TMediaSearchDatabaseMemory Source = new TMediaSearchDatabaseMemory() {
      Name = "test",
      Description = "test db"
    };

    IMedia Media = new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    };
    Source.Add(Media);

    IMedia Media2 = new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    };
    Source.Add(Media2);

    TraceMessage("Source after two additions", Source.ToString());

    TMediaSearchDatabaseMemory Target = new TMediaSearchDatabaseMemory(Source) {
      Name = "second",
      Description = "Duplicated db"
    };

    TraceMessage("Target", Target.ToString());
    Assert.AreEqual(2, Target.GetAll().Count());
  }
}