namespace MediaSearch.Server.Services;

public class TMovieCache : AMovieCache {

  public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

  protected override IEnumerable<IFileInfo> _FetchFiles(CancellationToken token) {

    DirectoryInfo RootFolder = new DirectoryInfo(RootStoragePath);

    foreach ( IFileInfo FileInfoItem in RootFolder.EnumerateFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                 .Where(f => MoviesExtensions.Contains(f.Extension.ToLowerInvariant()))
                                                 .Select(f => new TFileInfo(f)) ) {

      if ( token.IsCancellationRequested ) {
        yield break;
      }

      yield return FileInfoItem;
    }
  }

  public override Task Parse(CancellationToken token) {

    Logger.Log("Initializing movies cache");

    if ( string.IsNullOrWhiteSpace(RootStoragePath) ) {
      throw new ApplicationException("Storage is missing. Cannot process movies");
    }

    Clear();

    int Progress = 0;

    foreach ( IFileInfo MovieInfoItem in _FetchFiles(token) ) {
      Progress++;

      try {
        IMovie NewMovie = _ParseEntry(MovieInfoItem);
        NewMovie.DateAdded = DateOnly.FromDateTime(MovieInfoItem.ModificationDate);
        Logger.LogDebugEx($"Found {MovieInfoItem.FullName}");
        AddMovie(NewMovie);
      } catch ( Exception ex ) {
        Logger.LogWarning($"Unable to parse movie {MovieInfoItem} : {ex.Message}");
        if ( ex.InnerException is not null ) {
          Logger.LogWarning($"  {ex.InnerException.Message}");
        }
      }

      if ( Progress % 250 == 0 ) {
        Logger.Log($"Processed {Progress} movies...");
      }

    }

    Logger.Log($"Cache initialized successfully : {Progress} movies");

    return Task.CompletedTask;
  }

}