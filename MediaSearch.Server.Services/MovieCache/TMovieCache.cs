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

    Log("Initializing movies cache");

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
        LogDebugEx($"Found {MovieInfoItem.FullName}");
        AddMovie(NewMovie);
      } catch ( Exception ex ) {
        LogWarning($"Unable to parse movie {MovieInfoItem} : {ex.Message}");
        if ( ex.InnerException is not null ) {
          LogWarning($"  {ex.InnerException.Message}");
        }
      }

      if ( Progress % 250 == 0 ) {
        Log($"Processed {Progress} movies...");
      }

    }

    Log($"Cache initialized successfully : {Progress} movies");

    return Task.CompletedTask;
  }

}