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
    TMovieInfoFileMeta Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    Target.MetaContent.Titles.Add(ELanguage.French, "La grande vadrouille");
    Target.MetaContent.CreationDate = new DateOnly(1966, 1, 1);
    Target.MetaContent.Size = 1_234_567_890;
    Target.MetaContent.Descriptions.Add(ELanguage.French, "Pendant la WW2, ...");
    Target.MetaContent.Tags.Add("Guerre");
    Target.MetaContent.Tags.Add("WW2");
    Target.MetaContent.Tags.Add("Comédie");

    Assert.IsNotNull(Target);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target.ToString());
  }

  [TestMethod]
  public void TMovieInfoFileMeta_Save_Test_Remove() {
    TMovieInfoFileMeta Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    Target.MetaContent.Titles.Add(ELanguage.French, "La grande vadrouille");
    Target.MetaContent.CreationDate = new DateOnly(1966, 1, 1);
    Target.MetaContent.Size = 1_234_567_890;
    Target.MetaContent.Descriptions.Add(ELanguage.French, "Pendant la WW2, ...");
    Target.MetaContent.Tags.Add("Guerre");
    Target.MetaContent.Tags.Add("WW2");
    Target.MetaContent.Tags.Add("Comédie");


    Assert.IsFalse(Target.Exists());

    Target.Write();
    Assert.IsTrue(Target.Exists());
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());

  }

  [TestMethod]
  public async Task TMovieInfoFileMeta_Save_Test_Remove_Async() {
    TMovieInfoFileMeta Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta"
    };

    Target.MetaContent.Titles.Add(ELanguage.French, "La grande vadrouille");
    Target.MetaContent.CreationDate = new DateOnly(1966, 1, 1);
    Target.MetaContent.Size = 1_234_567_890;
    Target.MetaContent.Descriptions.Add(ELanguage.French, "Pendant la WW2, ...");
    Target.MetaContent.Tags.Add("Guerre");
    Target.MetaContent.Tags.Add("WW2");
    Target.MetaContent.Tags.Add("Comédie");


    Assert.IsFalse(Target.Exists());

    await Target.WriteAsync(CancellationToken.None);
    Assert.IsTrue(Target.Exists());
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void TMovieInfoFileMeta_Read() {
    TMovieInfoFileMeta Source = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta", 
    };

    Source.MetaContent.Titles.Add(ELanguage.French, "La grande vadrouille");
    Source.MetaContent.CreationDate = new DateOnly(1966, 1, 1);
    Source.MetaContent.Size = 1_234_567_890;
    Source.MetaContent.Descriptions.Add(ELanguage.French, "Pendant la WW2, ...");
    Source.MetaContent.Tags.Add("Guerre");
    Source.MetaContent.Tags.Add("WW2");
    Source.MetaContent.Tags.Add("Comédie");

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
    TMovieInfoFileMeta Source = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.msmeta",
    };

    Source.MetaContent.Titles.Add(ELanguage.French, "La grande vadrouille");
    Source.MetaContent.CreationDate = new DateOnly(1966, 1, 1);
    Source.MetaContent.Size = 1_234_567_890;
    Source.MetaContent.Descriptions.Add(ELanguage.French, "Pendant la WW2, ...");
    Source.MetaContent.Tags.Add("Guerre");
    Source.MetaContent.Tags.Add("WW2");
    Source.MetaContent.Tags.Add("Comédie");

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
    await Target.ReadAsync(CancellationToken.None);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }
  
}