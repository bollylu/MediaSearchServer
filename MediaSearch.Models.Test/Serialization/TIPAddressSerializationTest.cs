using System.Net;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TIPAddressSerializationTest {

  [TestMethod]
  public void Serialize() {

    IPAddress Source = IPAddress.Parse("192.168.132.160");
    Dump(Source);

    string Target = JsonSerializer.Serialize(Source, IJson.DefaultJsonSerializerOptions);

    Dump(Target);

    Assert.IsNotNull(Target);

    Ok();
  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"""192.168.132.160""";

    Dump(Source);

    IPAddress? Target = JsonSerializer.Deserialize<IPAddress>(Source, IJson.DefaultJsonSerializerOptions);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(IPAddress.Parse("192.168.132.160"), Target);

    Ok();

  }

}