
using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

internal static class Global {

  internal static readonly IMovieCache MovieCache = XMovieCache.Instance();
  
}
