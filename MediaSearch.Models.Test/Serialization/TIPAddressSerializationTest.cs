using System.Net;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TIPAddressSerializationTest {

  [TestMethod]
  public void Serialize() {

    IPAddress Source = IPAddress.Parse("192.168.132.160");
    Dump(Source, "Source");

    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    Dump(Target, "Target");

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"""192.168.132.160""";

    Dump(Source, "Source");

    IPAddress? Target = JsonSerializer.Deserialize<IPAddress>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.AreEqual(IPAddress.Parse("192.168.132.160"), Target);

  }

}