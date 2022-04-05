using System.Xml.Linq;

namespace MediaSearch.Models.MediaInfoContent.Test;

[TestClass]
public class TMovieInfoFileMetaTests {

  [TestMethod]
  public void Instanciate_TMovieInfoFileMeta() {
    TMovieInfoFileMeta Target = new();

    Assert.IsNotNull(Target);
    Assert.IsInstanceOfType(Target, typeof(IJson));

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void Instanciate_TMovieInfoFileMeta_FillData() {
    TMediaInfoHeader TargetHeader = new TMediaInfoHeader() {
      Name = "testfile",
      Description = "Description"
    };
    TMovieInfoFileMeta Target = new() {
      Header = TargetHeader,
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    TMovieInfoContentMeta TargetContent = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    TargetContent.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    TargetContent.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
    TargetContent.Genres.Add("Guerre");
    TargetContent.Genres.Add("WW2");
    TargetContent.Genres.Add("Comédie");

    Target.Content = TargetContent;

    Assert.IsNotNull(Target);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoFileMeta_Save_Test_Remove() {
    TMediaInfoHeader TargetHeader = new TMediaInfoHeader() {
      Name = "testfile",
      Description = "Description"
    };
    TMovieInfoFileMeta Target = new() {
      Header = TargetHeader,
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    TMovieInfoContentMeta TargetContent = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    TargetContent.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    TargetContent.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
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
  public async Task TMovieInfoFileMeta_Save_Test_Remove_Async() {
    TMediaInfoHeader TargetHeader = new TMediaInfoHeader() {
      Name = "testfile",
      Description = "Description"
    };
    TMovieInfoFileMeta Target = new() {
      Header = TargetHeader,
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    TMovieInfoContentMeta TargetContent = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    TargetContent.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    TargetContent.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
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
  public void TMovieInfoFileMeta_Read() {
    TMediaInfoHeader SourceHeader = new TMediaInfoHeader() {
      Name = "testfile",
      Description = "Description"
    };
    TMovieInfoFileMeta Source = new() {
      Header = SourceHeader,
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta", 
    };

    TMovieInfoContentMeta SourceContent = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    SourceContent.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    SourceContent.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
    SourceContent.Genres.Add("Guerre");
    SourceContent.Genres.Add("WW2");
    SourceContent.Genres.Add("Comédie");

    Source.Content = SourceContent;

    Assert.IsFalse(Source.Exists());

    Source.Write();
    Assert.IsTrue(Source.Exists());

    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);
    TraceMessage("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileMeta Target = new TMovieInfoFileMeta() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    Target.Read();

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMovieInfoFileMeta_Read_Async() {
    TMediaInfoHeader SourceHeader = new TMediaInfoHeader() {
      Name = "testfile",
      Description = "Description"
    };
    TMovieInfoFileMeta Source = new() {
      Header = SourceHeader,
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta",
    };

    TMovieInfoContentMeta SourceContent = new() {
      CreationYear = 1966,
      Size = 1234567890
    };

    SourceContent.Titles.Add(new TLanguageTextInfo(ELanguage.French, "La grande vadrouille"));
    SourceContent.Descriptions.Add(new TLanguageTextInfo(ELanguage.French, "Pendant la WW2, ..."));
    SourceContent.Genres.Add("Guerre");
    SourceContent.Genres.Add("WW2");
    SourceContent.Genres.Add("Comédie");

    Source.Content = SourceContent;

    Assert.IsFalse(Source.Exists());

    Source.Write();
    Assert.IsTrue(Source.Exists());

    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);
    TraceMessage("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileNfo Target = new TMovieInfoFileNfo() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    await Target.ReadAsync(CancellationToken.None);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }
  
}