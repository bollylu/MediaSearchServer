using System.Xml.Linq;

namespace MediaSearch.Models.MediaInfoContent.Test;

[TestClass]
public class TMovieInfoContentNfoTests {

  [TestMethod]
  public void Instanciate_TMovieInfoContentNfo() {
    TMovieInfoContentNfo Target = new();

    Assert.IsInstanceOfType(Target, typeof(IToXml));
    Assert.IsNotNull(Target);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void Instanciate_TMovieInfoContentNfo_FillData() {
    TMovieInfoContentNfo Target = new() {
      Title = "La grande vadrouille",
      Description= "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    Target.Genres.Add("Guerre");
    Target.Genres.Add("WW2");
    Target.Genres.Add("Comédie");

    Assert.IsNotNull(Target);

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoContentNfo_WithData_ToXml() {
    TMovieInfoContentNfo Source = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    Source.Genres.Add("Guerre");
    Source.Genres.Add("WW2");
    Source.Genres.Add("Comédie");

    XElement Target = Source.ToXml();

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoContentNfo_FromXml() {
    string RawSource = @"
      <movie>
        <originaltitle>La grande vadrouille</originaltitle>
        <title>La grande vadrouille</title>
        <country>FR</country>
        <year>1966</year>
        <plot>Pendant la WW2, ...</plot>
        <genre>Guerre</genre>
        <genre>WW2</genre>
        <genre>Comédie</genre>
      </movie>
    ";

    XDocument Source = XDocument.Parse(RawSource);
    Assert.IsNotNull(Source);
    Assert.IsNotNull(Source.Root);

    TMovieInfoContentNfo Target = new();
    Target.FromXml(Source.Root);

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }
}