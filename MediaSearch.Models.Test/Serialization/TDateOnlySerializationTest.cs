namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TDateOnlySerializationTest {

  [TestMethod]
  public void Serialize() {

    Message("Creating a DateOnly");
    DateOnly Source = DateOnly.FromDateTime(DateTime.Parse("2022-02-22 22:22:22"));
    Dump(Source, "Source");

    Message("Serialize to Json");
    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    Dump(Target, "Target");

    Assert.IsNotNull(Target);

    Ok();
  }

  [TestMethod]
  public void Deserialize() {

    Message("Creating a Json DateOnly");
    string Source = @"""20220222""";

    Dump(Source, "Source");

    Message("Deserialize in a DateOnly object");
    DateOnly? Target = JsonSerializer.Deserialize<DateOnly>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.AreEqual(DateOnly.Parse("2022-02-22"), Target);

    Ok();
  }

}