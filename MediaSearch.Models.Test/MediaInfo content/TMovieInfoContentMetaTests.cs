using System.Xml.Linq;

namespace MediaSearch.Models.MediaInfoContent.Test;

[TestClass]
public class TMovieInfoContentMetaTests {

  [TestMethod]
  public void Instanciate_TMovieInfoContentMeta() {
    TMovieInfoContentMeta Target = new();

    Assert.IsInstanceOfType(Target, typeof(IJson));
    Assert.IsNotNull(Target);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void Instanciate_TMovieInfoContentMeta_FillData() {
    TMovieInfoContentMeta Target = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    Target.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    Target.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
    Target.Genres.Add("Guerre");
    Target.Genres.Add("WW2");
    Target.Genres.Add("Comédie");

    Assert.IsNotNull(Target);
    Assert.AreEqual(1, Target.Titles.Count());
    Assert.AreEqual(1, Target.Descriptions.Count());
    Assert.AreEqual(3, Target.Genres.Count);
    Assert.AreEqual(1966, Target.CreationYear);
    Assert.AreEqual(1234567890, Target.Size);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TMovieInfoContenMeta_WithData_ToJson() {
    TMovieInfoContentMeta Source = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    Source.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    Source.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
    Source.Genres.Add("Guerre");
    Source.Genres.Add("WW2");
    Source.Genres.Add("Comédie");

    string Target = ((IJson)Source).ToJson();

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoContentMeta_FromJson() {
    string Source = @"
{                                     
  ""Titles"": [                         
    {
      ""Language"": ""French"",
      ""Value"": ""La grande vadrouille"",
      ""IsPrincipal"": false
    }
  ],
  ""Descriptions"": [
    {
      ""Language"": ""French"",
      ""Value"": ""Pendant la WW2, ..."",
      ""IsPrincipal"": false
    }
  ],
  ""Size"": 1234567890,
  ""CreationYear"": 1966,
  ""Soundtracks"": [
    ""French""
  ],
  ""Subtitles"": [
    ""FrenchFrance""
  ],
  ""Genres"": [
    ""Guerre"",
    ""WW2"",
    ""Comédie""
  ]
}
";

    Assert.IsNotNull(Source);

    TMovieInfoContentMeta? Target = IJson<TMovieInfoContentMeta>.FromJson(Source);
    Assert.IsNotNull(Target);

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }
}