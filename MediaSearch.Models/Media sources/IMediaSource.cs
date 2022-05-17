namespace MediaSearch.Models;

public interface IMediaSource {
  public Type? MediaType { get; }

  public string RootStorage { get; set; }

  public string ToString(int indent);
}

public interface IMediaSource<T> : IMediaSource where T : IID {

}
