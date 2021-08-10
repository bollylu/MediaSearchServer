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
    public List<string> MoviesExtensions { get; set; } = new() { ".mkv", ".avi", ".mp4" };

    public override Task Load() {
      Log("Initializing movies cache");

      Clear();

      lock (_LockCache) {

        DirectoryInfo RootFolder = new DirectoryInfo(RootPath);

        IEnumerable<FileInfo> MoviesInfo = RootFolder.GetFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                     .Where(f => MoviesExtensions.Contains(f.Extension.ToLowerInvariant()));

        foreach (FileInfo MovieInfoItem in MoviesInfo.OrderBy(f => f.FullName)) {

          string LocalPath = MovieInfoItem.DirectoryName.After(RootPath, System.StringComparison.InvariantCultureIgnoreCase);

          string GroupName = LocalPath.BeforeLast(Path.DirectorySeparatorChar).Replace('\\', '/');

          string FormattedGroupName = $"/{GroupName}/";

          LogDebug($"Group name = {GroupName}");

          string PictureLocation = LocalPath.Replace('\\', '/');

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

      Log("Cache initialized successfully");
      return Task.CompletedTask;
    }
  }
}
