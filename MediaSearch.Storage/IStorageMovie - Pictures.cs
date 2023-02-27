﻿namespace MediaSearch.Storage;

public partial interface IStorageMovie : IStorage {

  #region --- Pictures --------------------------------------------
  /// <summary>
  /// Get the movie picture
  /// </summary>
  /// <param name="movie">The movie associated with the requested picture</param>
  /// <param name="pictureName">The name of the picture</param>
  /// <param name="width">The width of the picture in pixels</param>
  /// <param name="height">The height of the pictur ein pixels</param>
  /// <returns>A binary picture or (null) if not available</returns>
  Task<byte[]?> GetMoviePictureAsync(IRecord movie, string pictureName, int width = 0, int height = 0);

  /// <summary>
  /// Get all the pictures associated with a movie
  /// </summary>
  /// <param name="movieId">The movie</param>
  /// <returns>A enumeration of the pictures associated with the movie</returns>
  Task<IDictionary<string, byte[]>> GetMoviePicturesAsync(IRecord movie);

  /// <summary>
  /// Get The count of pictures associated with the movie
  /// </summary>
  /// <param name="movie">The movie</param>
  /// <returns>A value between 0 and n of associated pictures</returns>
  ValueTask<int> GetMoviePictureCountAsync(IRecord movie);

  /// <summary>
  /// Add one picture to the movie in the storage. The movie must exists.
  /// </summary>
  /// <param name="movie">The referenced movie, must exists</param>
  /// <param name="pictureName">The name of the picture to associate with the movie</param>
  /// <param name="picture">The binary picture to associate with the movie</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> AddMoviePictureAsync(IRecord movie, string pictureName, byte[] picture);

  /// <summary>
  /// Remove a picture associated with a movie from the storage
  /// </summary>
  /// <param name="movie">The referenced movie</param>
  /// <param name="pictureName">The name of the picture</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> RemovePictureAsync(IRecord movie, string pictureName);

  /// <summary>
  /// Update one movie in the storage (based on Id, so Id cannot be altered)
  /// </summary>
  /// <param name="movie">The movie to update, with all fields containing their values for replacement</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  ValueTask<bool> UpdateMovieAsync(IRecord movie);

  #endregion --- Pictures --------------------------------------------

}
