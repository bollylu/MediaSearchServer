using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public class XMovieCache : AMovieCache {

    public XMovieCache () {
      StorageName = "Demo";
    }

    public override Task Load() {

      _Items.Add(new TMovie() {
        Storage = @"andromeda.sharenet.priv\Films",
        LocalName = "Alien agent (2007).avi",
        LocalPath = @"Science-fiction\[Aliens, créatures, ...]\Alien agent (2007)",
        Group = @"/Science-fiction/[Aliens, créatures, ...]",
        Size = 123456
      });

      _Items.Add(new TMovie() {
        Storage = @"andromeda.sharenet.priv\Films",
        LocalName = "Godzilla (1954).mkv",
        LocalPath = @"Science-fiction\[Aliens, créatures, ...]\Godzilla #\Godzilla (1954)",
        Group = @"/Science-fiction/[Aliens, créatures, ...]",
        Size = 123457
      });

      return Task.CompletedTask;
    }
  }
}
