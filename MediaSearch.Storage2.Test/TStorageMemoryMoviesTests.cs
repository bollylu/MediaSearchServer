using MediaSearch.Models;

namespace MediaSearch.Storage.Test;

[TestClass]
public class TStorageMemoryMediasTests {

  [TestMethod]
  public async Task Instantiate_TStorageMemoryMedias() {
    try {
      Message("Instantiate TStorageMemoryMedias");
      TStorageMemoryMedias Target = new();
      Assert.IsInstanceOfType(Target, typeof(IStorage));
      Assert.IsInstanceOfType(Target, typeof(AStorageMemory));
      Assert.IsNotNull(Target);
      Dump(Target);
      Message("Storage exists");
      Assert.IsTrue(await Target.Exists());
      Message("Storage is empty");
      Assert.IsTrue(await Target.IsEmpty());
      Assert.AreEqual(0, await Target.MediasCount());
      Ok();
    } catch (Exception ex) {
      Failed(ex.Message);
      throw;
    }
  }

  [TestMethod]
  public async Task TStorageMemoryMedias_AddSingleMedia() {
    try {
      Message("Instantiate TStorageMemoryMedias");
      TStorageMemoryMedias Target = new();
      Dump(Target);

      Message("Adding a media to the empty storage");
      TMediaSourceVirtual Source = new TMediaSourceVirtual() {
        FileName = "Le film",
        StoragePath = @"Comédie\\Le film",
        StorageRoot = @"\\andromeda.sharenet.priv\films",
        Size = 123456,
        FileExtension = "mkv",
        CreationDate = new DateOnly(2022, 1, 1)
      };
      TMediaMovie Movie = new();
      Movie.MediaSources.Add(Source);
      Dump(Movie);
      Assert.IsTrue(await Target.AddMediaAsync(Movie));

      Message("Storage contains 1 media");
      Dump(Target);
      Assert.AreEqual(1, await Target.MediasCount(TFilter.Empty));

      Ok();
    } catch (Exception ex) {
      Failed(ex.Message);
      throw;
    }
  }

  [TestMethod]
  public async Task TStorageMemoryMedias_AddMultipleMedias() {
    try {
      Message("Instantiate TStorageMemoryMedias");
      TStorageMemoryMedias Target = new();
      Dump(Target);

      Message("Adding movies to the empty storage");

      TMediaSourceVirtual Source1 = new TMediaSourceVirtual() {
        FileName = "Le film",
        StoragePath = @"Comédie\\Le film",
        StorageRoot = @"\\andromeda.sharenet.priv\films",
        Size = 123456,
        FileExtension = "mkv",
        CreationDate = new DateOnly(2022, 1, 1)
      };
      TMediaMovie Movie1 = new();
      Movie1.MediaSources.Add(Source1);

      TMediaSourceVirtual Source2 = new TMediaSourceVirtual() {
        FileName = "Le film - La suite",
        StoragePath = @"Comédie\\Le film - La suite",
        StorageRoot = @"\\andromeda.sharenet.priv\films",
        Size = 234567,
        FileExtension = "mkv",
        CreationDate = new DateOnly(2023, 1, 1)
      };
      TMediaMovie Movie2 = new();
      Movie2.MediaSources.Add(Source2);

      Dump(Movie1);
      Dump(Movie2);
      Assert.IsTrue(await Target.AddMediaAsync(Movie1));
      Assert.IsTrue(await Target.AddMediaAsync(Movie2));

      Message("Storage contains 2 movies");
      Dump(Target);
      Assert.AreEqual(2, await Target.MediasCount(TFilter.Empty));

      Ok();
    } catch (Exception ex) {
      Failed(ex.Message);
      throw;
    }
  }

  //[TestMethod]
  //public async Task TStorageMemoryMovies_RemoveMovie() {
  //  try {
  //    Message("Instantiate TStorageMemoryMovies");
  //    IStorageMovie Target = new TStorageMemoryMedias();
  //    Dump(Target);

  //    Message("Adding movies to the empty storage");
  //    IMovie Movie1 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film",
  //      StoragePath = @"Comédie\\Le film",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 123456,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2022,
  //    };

  //    IMovie Movie2 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film - La suite",
  //      StoragePath = @"Comédie\\Le film - Lz duite",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 234567,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2023,
  //    };

  //    Dump(Movie1);
  //    Dump(Movie2);
  //    Assert.IsTrue(await Target.AddMovieAsync(Movie1));
  //    Assert.IsTrue(await Target.AddMovieAsync(Movie2));

  //    Message("Storage contains 2 movies");
  //    Dump(Target);
  //    Assert.AreEqual(2, await Target.MoviesCount(TFilter.Empty));

  //    Message("Remove Movie1 from storage");
  //    Assert.IsTrue(await Target.RemoveMovieAsync(Movie1));

  //    Message("Storage now contains 1 movie");
  //    Dump(Target);
  //    Assert.AreEqual(1, await Target.MoviesCount(TFilter.Empty));

  //    Ok();
  //  } catch (Exception ex) {
  //    Failed(ex.Message);
  //    throw;
  //  }
  //}

  //[TestMethod]
  //public async Task TStorageMemoryMovies_FailsToRemoveMovie() {
  //  try {
  //    Message("Instantiate TStorageMemoryMovies");
  //    IStorageMovie Target = new TStorageMemoryMedias();
  //    Dump(Target);

  //    Message("Adding movies to the empty storage");
  //    IMovie Movie1 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film",
  //      StoragePath = @"Comédie\\Le film",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 123456,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2022,
  //    };

  //    IMovie Movie2 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film - La suite",
  //      StoragePath = @"Comédie\\Le film - Lz duite",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 234567,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2023,
  //    };

  //    Dump(Movie1);
  //    Dump(Movie2);
  //    Assert.IsTrue(await Target.AddMovieAsync(Movie1));
  //    Assert.IsTrue(await Target.AddMovieAsync(Movie2));

  //    Message("Storage contains 2 movies");
  //    Dump(Target);
  //    Assert.AreEqual(2, await Target.MoviesCount(TFilter.Empty));

  //    Message("Remove Movie1 from storage");
  //    Assert.IsFalse(await Target.RemoveMovieAsync(new TMovie()));

  //    Message("Storage still contains 2 movie");
  //    Dump(Target);
  //    Assert.AreEqual(2, await Target.MoviesCount(TFilter.Empty));

  //    Ok();
  //  } catch (Exception ex) {
  //    Failed(ex.Message);
  //    throw;
  //  }
  //}

  //[TestMethod]
  //public async Task TStorageMemoryMovies_AddPicture() {

  //  const string FOLDER_JPG = "folder.jpg";
  //  const string OTHER_JPG = "other.jpg";

  //  try {
  //    Message("Instantiate TStorageMemoryMovies");
  //    IStorageMovie Storage = new TStorageMemoryMedias();
  //    Dump(Storage);

  //    Message("No movies, no pictures");

  //    Message("Add one movie");
  //    IMovie Movie1 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film",
  //      StoragePath = @"Comédie\\Le film",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 123456,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2022,
  //    };

  //    Dump(Movie1);
  //    Assert.IsTrue(await Storage.AddMovieAsync(Movie1));

  //    Message("Associate a picture");
  //    byte[] FolderJpg = File.ReadAllBytes(Path.Combine("Data", FOLDER_JPG));
  //    byte[] OtherJpg = File.ReadAllBytes(Path.Combine("Data", OTHER_JPG));

  //    Message($"Add picture folder.jpg length is {FolderJpg.Length} bytes");
  //    Assert.IsTrue(await Storage.AddMoviePictureAsync(Movie1, FOLDER_JPG, FolderJpg));

  //    Message($"Add picture other.jpg with name folder.jpg : refused");
  //    Assert.IsFalse(await Storage.AddMoviePictureAsync(Movie1, FOLDER_JPG, OtherJpg));

  //    Message("Add picture folder.jpg to unknown movie : refused");
  //    Assert.IsFalse(await Storage.AddMoviePictureAsync(new TMovie() { Name = "missing" }, FOLDER_JPG, FolderJpg));

  //    Message("Storage contains 1 movie");
  //    Dump(Storage);
  //    Assert.AreEqual(1, await Storage.MoviesCount(TFilter.Empty));

  //    Message("Storage contains 1 associated picture");
  //    Byte[]? Target = await Storage.GetMoviePictureAsync(Movie1, FOLDER_JPG);
  //    Assert.IsNotNull(Target);
  //    Message($"Picture length is {Target.Length} bytes");

  //    Message("Target is identical to Picture");
  //    Assert.IsTrue(Target.SequenceEqual(FolderJpg));

  //    Ok();
  //  } catch (Exception ex) {
  //    Failed(ex.Message);
  //    throw;
  //  }

  //}

  //[TestMethod]
  //public async Task TStorageMemoryMovies_AddThenRemoveAPictures() {

  //  const string PICTURE_NAME = "folder.jpg";
  //  const string PICTURE_NAME_OTHER = "other.jpg";

  //  try {
  //    Message("Instantiate TStorageMemoryMovies");
  //    IStorageMovie Storage = new TStorageMemoryMedias();
  //    Dump(Storage);

  //    Message("No movies, no pictures");

  //    Message("Add one movie");
  //    IMovie Movie1 = new TMovie(new TMediaSourceMovie() {
  //      FileName = "Le film",
  //      StoragePath = @"Comédie\\Le film",
  //      StorageRoot = @"\\andromeda.sharenet.priv\films",
  //      Size = 123456,
  //      FileExtension = "mkv"
  //    }) {
  //      OutputYear = 2022,
  //    };

  //    Dump(Movie1);
  //    Assert.IsTrue(await Storage.AddMovieAsync(Movie1));

  //    Message("Associate a picture");
  //    byte[] FolderJpg = File.ReadAllBytes(Path.Combine("Data", PICTURE_NAME));
  //    byte[] OtherJpg = File.ReadAllBytes(Path.Combine("Data", PICTURE_NAME_OTHER));

  //    Message($"folder.jpg length is {FolderJpg.Length} bytes");
  //    Assert.IsTrue(await Storage.AddMoviePictureAsync(Movie1, PICTURE_NAME, FolderJpg));

  //    Message($"other.jpg length is {FolderJpg.Length} bytes");
  //    Assert.IsTrue(await Storage.AddMoviePictureAsync(Movie1, PICTURE_NAME_OTHER, OtherJpg));

  //    Message("Storage contains 1 movie");
  //    Dump(Storage);
  //    Assert.AreEqual(1, await Storage.MoviesCount(TFilter.Empty));

  //    Message($"Storage contains {await Storage.GetMoviePictureCountAsync(Movie1)} associated pictures");
  //    Dump(await Storage.GetMoviePicturesAsync(Movie1));

  //    Byte[]? Pic1 = await Storage.GetMoviePictureAsync(Movie1, PICTURE_NAME);
  //    Assert.IsNotNull(Pic1);
  //    Message($"Pic1 length is {Pic1.Length} bytes");
  //    Message("Pic1 is identical to folder.jpg");
  //    Assert.IsTrue(Pic1.SequenceEqual(FolderJpg));

  //    Byte[]? Pic2 = await Storage.GetMoviePictureAsync(Movie1, PICTURE_NAME_OTHER);
  //    Assert.IsNotNull(Pic2);
  //    Message($"Pic2 length is {Pic2.Length} bytes");
  //    Message("Pic2 is identical to other.jpg");
  //    Assert.IsTrue(Pic2.SequenceEqual(OtherJpg));

  //    Assert.IsTrue(await Storage.RemovePictureAsync(Movie1, PICTURE_NAME_OTHER));

  //    Ok();
  //  } catch (Exception ex) {
  //    Failed(ex.Message);
  //    throw;
  //  }

  //}
}
