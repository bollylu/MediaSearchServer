using System.Net;

using BLTools.Text;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TIPAddressSerializationTest {

  [TestMethod]
  public void Serialize() {

    IPAddress Source = IPAddress.Parse("192.168.132.160");
    TraceBox("Source", Source);

    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    TraceBox("Target", Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"""192.168.132.160""";

    TraceBox("Source", Source);

    IPAddress? Target = JsonSerializer.Deserialize<IPAddress>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    TraceBox("Target", Target);

    Assert.AreEqual(IPAddress.Parse("192.168.132.160"), Target);

  }

}