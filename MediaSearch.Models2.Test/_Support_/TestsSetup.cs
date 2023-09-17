namespace MediaSearch.Models2;

[TestClass]
public static class TestsSetup {
  [AssemblyInitialize]
  public static async Task ClassInitialize(TestContext context) {
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<string>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<int>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<long>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<float>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<double>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<bool>());
    IJson.AddJsonConverter(new TListWithPrincipalJsonConverter<byte[]>());

    IJson.AddJsonConverter(new TMultiItemsSelectionJsonConverter());

    IJson.DefaultJsonSerializerOptions.WriteIndented = true;

    await GlobalSettings.Initialize();
  }

}
