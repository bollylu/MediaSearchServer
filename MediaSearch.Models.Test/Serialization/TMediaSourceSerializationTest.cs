namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMediasourceSerializationTest {

  [TestMethod]
  public void Serialize() {
    Message("Create new TMediaSource");
    IMediaSource? Source = TMediaSource.Create("\\\\server\\share\\path", typeof(IMovie));
    Assert.IsNotNull(Source);
    Dump(Source);

    string Target = IJson.ToJson(Source);
    Dump(Target);

    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.Contains("\"MediaType\": \"IMovie\","));

    Ok();

  }

  [TestMethod]
  public void Deserialize() {

    string Source =
@"{
  ""MediaType"": ""IMovie"",
  ""RootStorage"": ""\\\\server\\share\\path""
}";

    Dump(Source);

    IMediaSource? Target = IJson.FromJson<IMediaSource>(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(Target.MediaType, typeof(IMovie));
    Assert.AreEqual("\\\\server\\share\\path", Target.RootStorage);

    Ok();
  }

  [TestMethod]
  public void Deserialize_InvalidMediaType_GotException() {

    string Source =
@"{
  ""MediaType"": ""ISomething"",
  ""RootStorage"": ""\\\\server\\share\\path""
}";

    Dump(Source);

    Assert.ThrowsException<JsonConverterInvalidDataException>(() => IJson.FromJson<IMediaSource>(Source));

    Ok();
  }

}