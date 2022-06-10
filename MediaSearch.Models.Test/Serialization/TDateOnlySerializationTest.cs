namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TDateOnlySerializationTest {

  [TestMethod]
  public void Serialize() {

    DateOnly Source = DateOnly.FromDateTime(DateTime.Parse("2022-02-22 22:22:22"));
    Dump(Source);

    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    Dump(Target);

    Assert.IsNotNull(Target);


  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"""20220222""";

    Dump(Source);

    DateOnly? Target = JsonSerializer.Deserialize<DateOnly>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(DateOnly.Parse("2022-02-22"), Target);

  }

}