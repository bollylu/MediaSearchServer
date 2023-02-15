namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TUserAccountSecretSerializationTest {

  [TestMethod]
  public void Serialize() {

    IUserAccountSecret Source = new TUserAccountSecret() {
      Name = "testuser",
      MustChangePassword = true,
      PasswordHash = "blabla",
      Token = new TUserToken() {
        TokenId = Guid.NewGuid().ToString(),
        Expiration = DateTime.Now
      }
    };
    Dump(Source, "Source");

    string Target = Source.ToJson();

    Dump(Target, "Target");

    Assert.IsNotNull(Target);


  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"
{                                              
  ""Name"": ""testuser"",
  ""PasswordHash"": ""YmxhYmxh"",
  ""MustChangePassword"": false,               
  ""Token"": {
      ""TokenId"": ""(none)"",                     
      ""Expiration"": ""0001-01-01T00:00:00""
  }
}                                              
";

    Dump(Source, "Source");

    TUserAccountSecret? Target = IJson<TUserAccountSecret>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.AreEqual("blabla", Target.PasswordHash);
    Assert.AreEqual("testuser", Target.Name);

  }

}