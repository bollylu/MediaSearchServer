namespace MediaSearch.Models2.Test.Serialization;

[TestClass]
public class ListWithPrincipalSerializationTest {

  [TestMethod]
  public void Serialize_String() {
    Message("Instanciate source");
    TListWithPrincipal<string> Source = new() { "Hello", "World", "and", "Friends" };
    Source.SetPrincipal("World");
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Serialize_Int() {
    Message("Instanciate source");
    TListWithPrincipal<int> Source = new() { 10, 100, 200, 38 };
    Source.SetPrincipal(200);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Serialize_Float() {
    Message("Instanciate source");
    TListWithPrincipal<float> Source = new() { 10.25f, 100.314f, 200f, 38.987f };
    Source.SetPrincipal(100.314f);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Serialize_ByteArray() {
    Message("Instanciate source");
    TListWithPrincipal<byte[]> Source = new() {
      new byte[] { 10, 100, 200, 36 },
      new byte[] { 192, 168, 10, 25 },
      new byte[] { 192, 168, 11, 25 },
      new byte[] { 172, 16, 75, 80 }
    };
    Source.SetPrincipal(new byte[] { 192, 168, 10, 25 });
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Serialize_Boolean() {
    Message("Instanciate source");
    TListWithPrincipal<bool> Source = new() { true, false };
    Source.SetPrincipal(false);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Serialize_Boolean_WithDuplicate() {
    try {
      Message("Instanciate source with duplicate item, hence generate an exception");
      TListWithPrincipal<bool> Source = new() { true, false, false };
    } catch (InvalidOperationException ex) {
      MessageBox(nameof(InvalidOperationException), ex.Message);
    }


    Ok();
  }

  [TestMethod]
  public void Deserialize_String() {
    Message("Instanciate source");
    TListWithPrincipal<string> Source = new() {
      "Hello",
      "World",
      "and",
      "Friends"
    };
    Source.SetPrincipal("World");
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string JsonSource = IJson.ToJson(Source);
    Assert.IsNotNull(JsonSource);
    Dump(JsonSource);

    TListWithPrincipal<string>? Target = IJson.FromJson<TListWithPrincipal<string>>(JsonSource);
    Assert.IsNotNull(Target);
    MessageBox($"{nameof(Target)} : {Target.GetType().GetNameEx()}", Target.ToString());

    Ok();
  }

  [TestMethod]
  public void Deserialize_Int() {
    Message("Instanciate source");
    TListWithPrincipal<int> Source = new() { 10, 100, 200, 38 };
    Source.SetPrincipal(200);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string JsonSource = IJson.ToJson(Source);
    Assert.IsNotNull(JsonSource);
    Dump(JsonSource);

    TListWithPrincipal<int>? Target = IJson.FromJson<TListWithPrincipal<int>>(JsonSource);
    Assert.IsNotNull(Target);
    MessageBox($"{nameof(Target)} : {Target.GetType().GetNameEx()}", Target.ToString());

    Ok();
  }

  [TestMethod]
  public void Deserialize_Float() {
    Message("Instanciate source");
    TListWithPrincipal<float> Source = new() { 10.25f, 100.314f, 200f, 38.987f };
    Source.SetPrincipal(100.314f);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string JsonSource = IJson.ToJson(Source);
    Assert.IsNotNull(JsonSource);
    Dump(JsonSource);

    TListWithPrincipal<float>? Target = IJson.FromJson<TListWithPrincipal<float>>(JsonSource);
    Assert.IsNotNull(Target);
    MessageBox($"{nameof(Target)} : {Target.GetType().GetNameEx()}", Target.ToString());

    Ok();
  }

  [TestMethod]
  public void Deserialize_Boolean() {
    Message("Instanciate source");
    TListWithPrincipal<bool> Source = new() { true, false };
    Source.SetPrincipal(false);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string JsonSource = IJson.ToJson(Source);
    Assert.IsNotNull(JsonSource);
    Dump(JsonSource);

    TListWithPrincipal<bool>? Target = IJson.FromJson<TListWithPrincipal<bool>>(JsonSource);
    Assert.IsNotNull(Target);
    MessageBox($"{nameof(Target)} : {Target.GetType().GetNameEx()}", Target.ToString());

    Ok();
  }

  [TestMethod]
  public void Deserialize_ByteArray() {
    Message("Instanciate source");
    TListWithPrincipal<byte[]> Source = new() {
      new byte[] { 10, 100, 200, 36 },
      new byte[] { 192, 168, 10, 25 },
      new byte[] { 192, 168, 11, 25 },
      new byte[] { 172, 16, 75, 80 }
    };
    Source.SetPrincipal(new byte[] { 192, 168, 10, 25 });
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string JsonSource = IJson.ToJson(Source);
    Assert.IsNotNull(JsonSource);
    Dump(JsonSource);

    TListWithPrincipal<byte[]>? Target = IJson.FromJson<TListWithPrincipal<byte[]>>(JsonSource);
    Assert.IsNotNull(Target);
    MessageBox($"{nameof(Target)} : {Target.GetType().GetNameEx()}", Target.ToString());

    Ok();
  }
}
