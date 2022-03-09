namespace MediaSearch.Models.Test;

[TestClass]
public class TMediaInfoContentMetaTest {

  [TestMethod]
  public void Serialize() {

    IMediaInfoContent Source = new TMediaInfoContentMeta() {
      Size = 123456
    };
    Source.Titles.Add(ELanguage.French, "Le pont de la rivère Kwai");
    Source.Titles.Add(ELanguage.English, "Bridge over Kwai river");

    Source.Descriptions.Add(ELanguage.French, "Des prisoniers doivent contruire un pont sur la rivière Kwai, en pleine jungle.");

    TraceMessage("Source", Source);

    string Target = Source.ToJson();

    TraceMessage("Target", Target);

    Assert.IsNotNull(Target);

  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"
{                                                                                               
  ""Titles"": [                                                                                   
      {
        ""key"": ""French"",                                                                          
        ""value"": ""Le pont de la rivère Kwai""
      },                                                                                          
      {
        ""key"": ""English"",                                                                         
        ""value"": ""Bridge over Kwai river""
      }                                                                                           
  ],                                                                                            
  ""Descriptions"": [
      {
        ""key"": ""French"",                                                                          
        ""value"": ""Des prisoniers doivent contruire un pont sur la rivière Kwai, en pleine jungle.""
      }                                                                                           
  ],                                                                                            
  ""Size"": 123456
}                                                                                               

";

    TraceMessage("Source", Source);

  TMediaInfoContentMeta? Target = IJson<TMediaInfoContentMeta>.FromJson(Source);
  Assert.IsNotNull(Target);
    TraceMessage("Target", Target);

  Assert.AreEqual(2, Target.Titles.Count);

  }

}