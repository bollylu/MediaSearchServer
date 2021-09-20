using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using BLTools;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public class TMovieCache : AMovieCache {

    public List<string> MoviesExtensions { get; } = new() { ".mkv", ".avi", ".mp4" };

    public override Task Load() {
      Log("Initializing movies cache");

      if (string.IsNullOrWhiteSpace(Storage)) {
        throw new ApplicationException("Storage is missing. Cannot process movies");
      }

      Clear();

      lock (_LockCache) {

        DirectoryInfo RootFolder = new DirectoryInfo(Storage);

        IEnumerable<FileInfo> MoviesInfo = RootFolder.GetFiles("*.*", new EnumerationOptions() { RecurseSubdirectories = true })
                                                     .Where(f => MoviesExtensions.Contains(f.Extension.ToLowerInvariant()));

        foreach (FileInfo MovieInfoItem in MoviesInfo.OrderBy(f => f.FullName)) {

          string LocalPath = MovieInfoItem.DirectoryName.After(Storage, System.StringComparison.InvariantCultureIgnoreCase);

          string GroupName = LocalPath.BeforeLast(Path.DirectorySeparatorChar).Replace('\\', '/');

          string FormattedGroupName = $"/{GroupName}/";

          LogDebug($"Group name = {GroupName}");

          string PictureLocation = LocalPath.Replace('\\', '/');

          LogDebug($"PictureLocation = {PictureLocation}");

          _Items.Add(new TMovie() {
            Storage = Storage,
            LocalName = MovieInfoItem.Name,
            LocalPath = MovieInfoItem.DirectoryName.After(Storage, System.StringComparison.InvariantCultureIgnoreCase),
            Group = FormattedGroupName,
            Size = MovieInfoItem.Length
          });
        }
      }

      Log("Cache initialized successfully");
      return Task.CompletedTask;
    }

  }

}
