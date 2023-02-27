namespace MediaSearch.Models;


public class TMediaSource {
  public static IMediaSource? Create(string rootStorage, Type mediaType) {
    return mediaType.Name switch {
      nameof(IMovie) => new TMediaSourceMovie(rootStorage),
      _ => throw new ApplicationException($"Unable to create MediaSource of type {mediaType.Name}"),
    };
  }
  public static IMediaSource? Create(string? mediaType) {
    return mediaType switch {
      nameof(IMovie) => new TMediaSourceMovie(),
      _ => throw new ApplicationException($"Unable to create MediaSource of type {mediaType}"),
    };
  }
}

