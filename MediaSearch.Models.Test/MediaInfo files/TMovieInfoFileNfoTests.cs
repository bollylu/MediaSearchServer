using System.Xml.Linq;

namespace MediaSearch.Models.MediaInfoContent.Test;

[TestClass]
public class TMovieInfoFileNfoTests {

  [TestMethod]
  public void Instanciate_TMovieInfoFileNfo() {
    TMovieInfoFileNfo Target = new();

    Assert.IsNotNull(Target);
    Assert.IsNotInstanceOfType(Target, typeof(IToXml));

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void Instanciate_TMovieInfoFileNfo_FillData() {
    TMovieInfoFileNfo Target = new() {
      Name = "testfile",
      Description = "Description",
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    TMovieInfoContentNfo TargetContent = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    TargetContent.Genres.Add("Guerre");
    TargetContent.Genres.Add("WW2");
    TargetContent.Genres.Add("Comédie");

    Target.Content = TargetContent;

    Assert.IsNotNull(Target);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoFileNfo_Save_Test_Remove() {
    TMovieInfoFileNfo Target = new() {
      Name = "testfile",
      Description = "Description",
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    TMovieInfoContentNfo TargetContent = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    TargetContent.Genres.Add("Guerre");
    TargetContent.Genres.Add("WW2");
    TargetContent.Genres.Add("Comédie");

    Target.Content = TargetContent;

    Assert.IsFalse(Target.Exists());

    Target.Write();
    Assert.IsTrue(Target.Exists());
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());


  }

  [TestMethod]
  public void TMovieInfoFile_Read() {
    TMovieInfoFileNfo Source = new() {
      Name = "testfile",
      Description = "Description",
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    TMovieInfoContentNfo SourceContent = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    SourceContent.Genres.Add("Guerre");
    SourceContent.Genres.Add("WW2");
    SourceContent.Genres.Add("Comédie");

    Source.Content = SourceContent;

    Source.Write();
    Assert.IsTrue(Source.Exists());

    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceMessage("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileNfo Target = new TMovieInfoFileNfo() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    Target.Read();

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMovieInfoFileNfo_Save_Test_Remove_Async() {
    TMovieInfoFileNfo Target = new() {
      Name = "testfile",
      Description = "Description",
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    TMovieInfoContentNfo TargetContent = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    TargetContent.Genres.Add("Guerre");
    TargetContent.Genres.Add("WW2");
    TargetContent.Genres.Add("Comédie");

    Target.Content = TargetContent;

    Assert.IsFalse(Target.Exists());

    await Target.WriteAsync(CancellationToken.None);
    Assert.IsTrue(Target.Exists());
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMovieInfoFile_Read_Async() {
    TMovieInfoFileNfo Source = new() {
      Name = "testfile",
      Description = "Description",
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    TMovieInfoContentNfo SourceContent = new() {
      Title = "La grande vadrouille",
      Description = "Pendant la WW2, ...",
      Country = "FR",
      CreationYear = 1966
    };
    SourceContent.Genres.Add("Guerre");
    SourceContent.Genres.Add("WW2");
    SourceContent.Genres.Add("Comédie");

    Source.Content = SourceContent;

    await Source.WriteAsync(CancellationToken.None);
    Assert.IsTrue(Source.Exists());

    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source.ToString());
    TraceMessage("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileNfo Target = new TMovieInfoFileNfo() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    await Target.ReadAsync(CancellationToken.None);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }
}