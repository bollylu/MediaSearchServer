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
    Dump(Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);

    Dump(Target);

    Ok();
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

    Dump(Source);

    TUserAccountSecret? Target = IJson<TUserAccountSecret>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual("blabla", Target.PasswordHash);
    Assert.AreEqual("testuser", Target.Name);

    Ok();
  }

}