namespace MediaSearch.Models;


public class TMediaSource : AMediaSource<IMSRecord> {
  public static IMediaSource? Create(string rootStorage, Type mediaType) {
    return mediaType.Name switch {
      nameof(IMovie) => new TMediaSourceMovie(rootStorage),
      _ => throw new ApplicationException($"Unable to create MediaSource of type {mediaType.Name}"),
    };
  }
}

public class TMediaSourceMovie : AMediaSource<IMovie> {
  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourceMovie() { }

  public TMediaSourceMovie(string rootStorage) : base(rootStorage) { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------
}
