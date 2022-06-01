using BLTools.Text;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TDateOnlySerializationTest {

  [TestMethod]
  public void Serialize() {

    DateOnly Source = DateOnly.FromDateTime(DateTime.Parse("2022-02-22 22:22:22"));
    TraceBox("Source", Source);

    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    TraceBox("Target", Target);

    Assert.IsNotNull(Target);


  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"""20220222""";

    TraceBox("Source", Source);

    DateOnly? Target = JsonSerializer.Deserialize<DateOnly>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    TraceBox("Target", Target);

    Assert.AreEqual(DateOnly.Parse("2022-02-22"), Target);

  }

}