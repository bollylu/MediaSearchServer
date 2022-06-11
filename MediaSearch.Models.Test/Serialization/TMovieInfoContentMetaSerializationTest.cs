namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMovieInfoContentMetaSerializationTest {

  [TestMethod]
  public void Serialize() {

    TMovieInfoContentMeta Source = new TMovieInfoContentMeta() {
      Size = 123456
    };
    Source.Titles.Add(ELanguage.French, "Le pont de la rivère Kwai");
    Source.Titles.Add(ELanguage.English, "Bridge over Kwai river");

    Source.Descriptions.Add(ELanguage.French, "Des prisoniers doivent contruire un pont sur la rivière Kwai, en pleine jungle.");

    Source.Soundtracks.Add(ELanguage.English);
    Source.Soundtracks.Add(ELanguage.FrenchFrance);

    Source.Subtitles.Add(ELanguage.French);

    Dump(Source);

    string Target = IJson.ToJson(Source);
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();

  }

  [TestMethod]
  public void Deserialize() {

    string Source = @"
      {                                                                                               
        ""Titles"": [                              
        {
            ""Language"": ""French"",                
            ""Value"": ""Le pont de la rivère Kwai"",
            ""IsPrincipal"": false
        },                                     
        {
            ""Language"": ""English"",               
            ""Value"": ""Bridge over Kwai river"",   
            ""IsPrincipal"": true
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

    Dump(Source);

    TMovieInfoContentMeta? Target = IJson.FromJson<TMovieInfoContentMeta>(Source);
    Assert.IsNotNull(Target);

    Dump(Target);

    Assert.AreEqual(2, Target.Titles.Count());
    Assert.AreEqual(2, Target.Soundtracks.Count);

    Ok();
  }

}