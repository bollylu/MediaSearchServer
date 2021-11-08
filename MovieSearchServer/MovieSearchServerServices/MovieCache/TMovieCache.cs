using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;

using MovieSearchModels;

namespace MovieSearchServerServices.MovieService {
  public class TMovieCache : AMovieCache {

    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4", ".iso" };

    public override IEnumerable<IFileInfo> FetchFiles(CancellationToken token) {

      DirectoryInfo RootFolder = new DirectoryInfo(Storage);

      foreach (IFileInfo FileInfoItem in RootFolder.EnumerateFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                   .Where(f => MoviesExtensions.Contains(f.Extension.ToLowerInvariant()))
                                                   .Select(f=> new TFileInfo(f))) {

        if (token.IsCancellationRequested) {
          yield break;
        }

        yield return FileInfoItem;
      }
    }

    public override async Task Parse(IEnumerable<IFileInfo> fileSource, CancellationToken token) {
      Log("Initializing movies cache");

      if (string.IsNullOrWhiteSpace(Storage)) {
        throw new ApplicationException("Storage is missing. Cannot process movies");
      }

      Clear();

      int Progress = 0;

      foreach (IFileInfo MovieInfoItem in fileSource) {
        Progress++;

        try {
          IMovie NewMovie = _ParseEntry(MovieInfoItem);

          await Task.Yield();

          _Items.Add($"{NewMovie.Filename}{NewMovie.OutputYear}", NewMovie);
        } catch (Exception ex) {
          LogWarning($"Unable to parse movie {MovieInfoItem} : {ex.Message}");
          if (ex.InnerException is not null) {
            LogWarning($"  {ex.InnerException.Message}");
          }
        }

        if (Progress % 100 == 0) {
          Log($"Processed {Progress} movies...");
        }

      }

      Log($"Cache initialized successfully : {Progress} movies");

    }

  }

}
