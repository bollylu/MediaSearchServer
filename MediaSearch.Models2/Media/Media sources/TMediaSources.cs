namespace MediaSearch.Models;
public class TMediaSources : List<IMediaSource>, IMediaSources {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSources() { }
  public TMediaSources(params IMediaSource[] mediaSources) {
    foreach (IMediaSource MediaSourceItem in mediaSources) {
      Add(MediaSourceItem);
    }
  }
  public TMediaSources(TMediaSources mediaSources) {
    foreach (IMediaSource MediaSourceItem in mediaSources) {
      Add(MediaSourceItem);
    }
  }

  public TMediaSources(IEnumerable<IMediaSource> mediaSources) {
    foreach (IMediaSource MediaSourceItem in mediaSources) {
      Add(MediaSourceItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    foreach (IMediaSource MediaSourceItem in this) {
      RetVal.AppendIndent($"- {MediaSourceItem}", indent);
    }
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }
}
