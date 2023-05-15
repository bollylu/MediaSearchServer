namespace MediaSearch.Models2.Test.Serialization;

[TestClass]
public class ListWithPrincipalSerializationTest {
  [ClassInitialize]
  public static void ClassInitialize(TestContext context) {
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<string>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<int>());
    IJson.DefaultJsonSerializerOptions.WriteIndented = true;
  }

  [TestMethod]
  public void Serialize_String() {
    Message("Instanciate source");
    TListWithPrincipal<string> Source = new TListWithPrincipal<string>();
    Source.Add("Hello");
    Source.Add("World");
    Source.Add("and");
    Source.Add("Friends");
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
    TListWithPrincipal<int> Source = new TListWithPrincipal<int>();
    Source.Add(10);
    Source.Add(100);
    Source.Add(200);
    Source.Add(38);
    Source.SetPrincipal(200);
    MessageBox($"{nameof(Source)} : {Source.GetType().GetNameEx()}", Source.ToString());
    Message("Serialize to Json");
    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }


  [TestMethod]
  public void Deserialize_String() {
    Message("Instanciate source");
    TListWithPrincipal<string> Source = new TListWithPrincipal<string>();
    Source.Add("Hello");
    Source.Add("World");
    Source.Add("and");
    Source.Add("Friends");
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
  public void Deserialize_Int32() {
    Message("Instanciate source");
    TListWithPrincipal<int> Source = new TListWithPrincipal<int>();
    Source.Add(10);
    Source.Add(100);
    Source.Add(200);
    Source.Add(38);
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
}
