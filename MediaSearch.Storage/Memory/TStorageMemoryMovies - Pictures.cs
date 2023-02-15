namespace MediaSearch.Storage;
public partial class TStorageMemoryMovies : AStorageMemory, IStorageMovie {

  public Task<byte[]?> GetMoviePictureAsync(string movieId, string pictureName, int width = 0, int height = 0) {
    return Task.FromResult(GetPicture(movieId, pictureName));
  }

  public Task<byte[]?> GetMoviePictureAsync(IMovie movie, string pictureName, int width = 0, int height = 0) {
    return Task.FromResult(GetPicture(movie.Id, pictureName));
  }

  public async ValueTask<bool> AddMoviePictureAsync(IMovie movie, string pictureName, byte[] picture) {
    return await ValueTask.FromResult(AddPicture(movie.Id, pictureName, picture)).ConfigureAwait(false);
  }

  public async ValueTask<bool> AddMoviePictureAsync(string movieId, string pictureName, byte[] picture) {
    return await ValueTask.FromResult(AddPicture(movieId, pictureName, picture)).ConfigureAwait(false);
  }

  public async ValueTask<bool> RemovePictureAsync(string movieId, string pictureName) {
    return await ValueTask.FromResult(RemovePicture(movieId, pictureName)).ConfigureAwait(false);
  }

  public async ValueTask<bool> RemovePictureAsync(IMovie movie, string pictureName) {
    return await ValueTask.FromResult(RemovePicture(movie.Id, pictureName)).ConfigureAwait(false);
  }

  public Task<IDictionary<string, byte[]>> GetMoviePicturesAsync(string movieId) {
    return Task.FromResult(GetAllPictures(movieId));
  }

  public Task<IDictionary<string, byte[]>> GetMoviePicturesAsync(IMovie movie) {
    return Task.FromResult(GetAllPictures(movie.Id));
  }

  public ValueTask<int> GetMoviePictureCountAsync(string movieId) {
    return ValueTask.FromResult(GetPicturesCount(movieId));
  }

  public ValueTask<int> GetMoviePictureCountAsync(IMovie movie) {
    return ValueTask.FromResult(GetPicturesCount(movie.Id));
  }


}
