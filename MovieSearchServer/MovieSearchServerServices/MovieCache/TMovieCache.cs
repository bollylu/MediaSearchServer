using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BLTools;
using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public class TMovieCache : AMovieCache {

    public string RootPath { get; set; } = "";
    public List<string> ExcludedExtensions { get; set; } = new List<string>() { ".nfo", ".jpg", ".vsmeta" };

    public override Task Load() {
      Console.WriteLine("Initializing movies cache");

      Clear();

      lock (_LockCache) {
        
        DirectoryInfo RootFolder = new DirectoryInfo(RootPath);

        IEnumerable<FileInfo> MoviesInfo = RootFolder.GetFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                     .Where(f => !ExcludedExtensions.Contains(f.Extension));

        foreach (FileInfo MovieInfoItem in MoviesInfo.OrderBy(f => f.FullName)) {

          string GroupName = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                        .BeforeLast(Path.DirectorySeparatorChar)
                                                        .Replace('\\', '/');

          string FormattedGroupName = $"/{GroupName}/";

          LogDebug($"Group name = {GroupName}");

          string PictureLocation = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase)
                                                              .Replace('\\', '/');

          LogDebug($"PictureLocation = {PictureLocation}");

          _Items.Add(new TMovie() {
            LocalName = MovieInfoItem.Name,
            LocalPath = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase),
            Group = FormattedGroupName,
            Size = MovieInfoItem.Length,
            Picture = PictureLocation
          });
        }
      }

      Console.WriteLine("Cache initialized successfully");
      return Task.CompletedTask;
    }
  }
}
