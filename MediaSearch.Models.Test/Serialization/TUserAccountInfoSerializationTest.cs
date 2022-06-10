using System.Net;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TUserAccountInfoSerializationTest {

  [TestMethod]
  public void Serialize() {

    IUserAccountInfo Source = new TUserAccountInfo() {
      Name = "testuser",
      Description = "Test user",
      RemoteIp = IPAddress.Parse("192.168.10.11"),
      LastSuccessfulLogin = DateTime.Now.AddDays(-1),
      LastFailedLogin = DateTime.Now.AddDays(-1).AddMinutes(-10)
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
  ""Description"": ""Test user"",                  
  ""RemoteIp"": ""192.168.10.11"",                 
  ""LastSuccessfulLogin"": ""11/03/2022 13:38:02"",
  ""LastFailedLogin"": ""11/03/2022 13:28:02""
  }                                              
";

    Dump(Source);

    TUserAccountInfo? Target = IJson<TUserAccountInfo>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual("192.168.10.11", Target.RemoteIp.ToString());

    Ok();
  }

}