using System.Net;

namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TUserAccountSerializationTest {

  [TestMethod]
  public void Serialize() {

    IUserAccount Source = new TUserAccount() {
      Name = "testuser",
      Description = "Test user",
      RemoteIp = IPAddress.Parse("192.168.10.11"),
      LastSuccessfulLogin = DateTime.Now.AddDays(-1),
      LastFailedLogin = DateTime.Now.AddDays(-1).AddMinutes(-10)
    };
    TraceBox("Source", Source);

    string Target = Source.ToJson();

    TraceBox("Target", Target);

    Assert.IsNotNull(Target);


  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"
{                                              
  ""Name"": ""testuser"",                          
  ""Description"": ""Test user"",                  
  ""RemoteIp"": ""192.168.10.11"",                 
  ""LastSuccessfulLogin"": ""11/03/2022 13:38:02"",
  ""LastFailedLogin"": ""11/03/2022 13:28:02"",    
  ""Secret"": {
      ""Name"": """",                                
      ""Password"": """",                            
      ""MustChangePassword"": false,               
      ""Token"": {
          ""TokenId"": ""(none)"",                     
          ""Expiration"": ""0001-01-01T00:00:00""
      }
    }
  }                                              
";

    TraceBox("Source", Source);

    TUserAccount? Target = IJson<TUserAccount>.FromJson(Source);
    Assert.IsNotNull(Target);
    TraceBox("Target", Target);

    Assert.AreEqual("192.168.10.11",Target.RemoteIp.ToString());

  }

}