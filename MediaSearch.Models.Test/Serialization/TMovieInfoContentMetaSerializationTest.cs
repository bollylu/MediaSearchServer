namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieInfoContentMetaSerializationTest {

  [TestMethod]
  public void Serialize() {

    IMovieInfoContent Source = new TMovieInfoContentMeta() {
      Size = 123456
    };
    Source.Titles.Add(ELanguage.French, "Le pont de la rivère Kwai");
    Source.Titles.Add(ELanguage.English, "Bridge over Kwai river");

    Source.Descriptions.Add(ELanguage.French, "Des prisoniers doivent contruire un pont sur la rivière Kwai, en pleine jungle.");

    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);

    Source.Subtitles.Add(ELanguage.French);

    Dump(Source, "Source");

    string Target = Source.ToJson();

    Dump(Target, "Target");

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
  ""Size"": 123456,
  ""Soundtracks"": [
      ""English"",
      ""FrenchFrance""
  ], 
  ""Subtitles"": [
      ""French""
  ]
}                                                                                               
";

    Dump(Source, "Source");

    TMovieInfoContentMeta? Target = IJson<TMovieInfoContentMeta>.FromJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target, "Target");

    Assert.AreEqual(2, Target.Titles.Count);
    Assert.AreEqual(2, Target.Soundtracks.Count);

  }

}