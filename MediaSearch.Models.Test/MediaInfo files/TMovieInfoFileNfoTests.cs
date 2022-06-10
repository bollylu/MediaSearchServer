using BLTools.Storage.Xml;
namespace MediaSearch.Models.MediaInfoContent.Test;

[TestClass]
public class TMovieInfoFileNfoTests {

  [TestMethod]
  public void Instanciate_TMovieInfoFileNfo() {
    TMovieInfoFileNfo Target = new();

    Assert.IsNotNull(Target);
    Assert.IsNotInstanceOfType(Target, typeof(IToXml));

    Dump(Target);
  }

  [TestMethod]
  public void Instanciate_TMovieInfoFileNfo_FillData() {
    TMovieInfoFileNfo Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    Target.NfoContent.Title = "La grande vadrouille";
    Target.NfoContent.Country = "FR";
    Target.NfoContent.Description = "Comédie de guerre";
    Target.NfoContent.CreationYear = 1966;
    Target.NfoContent.Genres.Add("Guerre");
    Target.NfoContent.Genres.Add("WW2");
    Target.NfoContent.Genres.Add("Comédie");

    Assert.IsNotNull(Target);

    Dump(Target);
  }

  [TestMethod]
  public void TMovieInfoFileNfo_Save_Test_Remove() {
    TMovieInfoFileNfo Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    Target.NfoContent.Title = "La grande vadrouille";
    Target.NfoContent.Country = "FR";
    Target.NfoContent.Description = "Comédie de guerre";
    Target.NfoContent.CreationYear = 1966;
    Target.NfoContent.Genres.Add("Guerre");
    Target.NfoContent.Genres.Add("WW2");
    Target.NfoContent.Genres.Add("Comédie");

    Assert.IsFalse(Target.Exists());

    Target.Write();
    Assert.IsTrue(Target.Exists());
    MessageBox($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());
    Ok();
  }

  [TestMethod]
  public void TMovieInfoFile_Read() {
    TMovieInfoFileNfo Source = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    Source.NfoContent.Title = "La grande vadrouille";
    Source.NfoContent.Country = "FR";
    Source.NfoContent.Description = "Comédie de guerre";
    Source.NfoContent.CreationYear = 1966;
    Source.NfoContent.Genres.Add("Guerre");
    Source.NfoContent.Genres.Add("WW2");
    Source.NfoContent.Genres.Add("Comédie");

    Dump(Source);

    Assert.IsTrue(Source.Write());
    Assert.IsTrue(Source.Exists());

    MessageBox("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileNfo Target = new TMovieInfoFileNfo() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    Assert.IsTrue(Target.Read());

    Dump(Target);

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMovieInfoFileNfo_Save_Test_Remove_Async() {
    TMovieInfoFileNfo Target = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    Target.NfoContent.Title = "La grande vadrouille";
    Target.NfoContent.Country = "FR";
    Target.NfoContent.Description = "Comédie de guerre";
    Target.NfoContent.CreationYear = 1966;
    Target.NfoContent.Genres.Add("Guerre");
    Target.NfoContent.Genres.Add("WW2");
    Target.NfoContent.Genres.Add("Comédie");

    Assert.IsFalse(Target.Exists());

    await Target.WriteAsync(CancellationToken.None);
    Assert.IsTrue(Target.Exists());
    MessageBox($"{nameof(Target)} : {Target.GetType().Name}", File.ReadAllText(Target.FullStorageName));

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMovieInfoFile_Read_Async() {
    TMovieInfoFileNfo Source = new() {
      StoragePath = Path.GetTempPath(),
      StorageName = $"{Random.Shared.Next()}.nfo"
    };

    Source.NfoContent.Title = "La grande vadrouille";
    Source.NfoContent.Country = "FR";
    Source.NfoContent.Description = "Comédie de guerre";
    Source.NfoContent.CreationYear = 1966;
    Source.NfoContent.Genres.Add("Guerre");
    Source.NfoContent.Genres.Add("WW2");
    Source.NfoContent.Genres.Add("Comédie");

    await Source.WriteAsync(CancellationToken.None);
    Assert.IsTrue(Source.Exists());

    Dump(Source);
    MessageBox("Source file raw content", File.ReadAllText(Source.FullStorageName));

    TMovieInfoFileNfo Target = new TMovieInfoFileNfo() {
      StoragePath = Source.StoragePath,
      StorageName = Source.StorageName
    };

    Assert.IsTrue(Target.Exists());
    await Target.ReadAsync(CancellationToken.None);

    Dump(Target);

    Target.Remove();
    Assert.IsFalse(Target.Exists());

    Ok();
  }
}