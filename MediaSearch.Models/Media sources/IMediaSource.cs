namespace MediaSearch.Models;
public interface IMediaSource<T> where T : IMedia {

  public Type MediaType { get; }

  public string RootStorage { get; set; }

  public string ToString(int indent);
}
