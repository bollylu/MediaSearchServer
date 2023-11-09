namespace MediaSearch.Server.Services;

public class TMovieCache : AMediaCache {

  public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };


}