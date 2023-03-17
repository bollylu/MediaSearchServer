namespace MediaSearch.Models;
public class TMediaSource : AMediaSource {
  //public static IMediaSource? Create(string rootStorage, Type mediaType) {
  //  return mediaType.Name switch {
  //    nameof(IMovie) => new TMediaSourceMovie(rootStorage),
  //    _ => throw new ApplicationException($"Unable to create MediaSource of type {mediaType.Name}"),
  //  };
  //}
  //public static IMediaSource? Create(string? mediaType) {
  //  return mediaType switch {
  //    nameof(IMovie) => new TMediaSourceMovie(),
  //    _ => throw new ApplicationException($"Unable to create MediaSource of type {mediaType}"),
  //  };
  //}

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSource() : base() { }
  public TMediaSource(IMediaSource mediaSource) : base(mediaSource) {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}

