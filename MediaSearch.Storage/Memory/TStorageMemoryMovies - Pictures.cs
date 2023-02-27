namespace MediaSearch.Storage;
public partial class TStorageMemoryMovies : AStorageMemory, IStorageMovie {

  public Task<byte[]?> GetMoviePictureAsync(IRecord movie, string pictureName, int width = 0, int height = 0) {
    return Task.FromResult(GetPicture(movie, pictureName));
  }

  public async ValueTask<bool> AddMoviePictureAsync(IRecord movie, string pictureName, byte[] picture) {
    return await ValueTask.FromResult(AddPicture(movie, pictureName, picture)).ConfigureAwait(false);
  }

  public async ValueTask<bool> RemovePictureAsync(IRecord movie, string pictureName) {
    return await ValueTask.FromResult(RemovePicture(movie, pictureName)).ConfigureAwait(false);
  }

  public Task<IDictionary<string, byte[]>> GetMoviePicturesAsync(IRecord movie) {
    return Task.FromResult(GetAllPictures(movie));
  }

  public ValueTask<int> GetMoviePictureCountAsync(IRecord movie) {
    return ValueTask.FromResult(GetPicturesCount(movie));
  }

  public ValueTask<bool> UpdateMovieAsync(IRecord movie) {
    throw new NotImplementedException();
  }
}
