
using MediaSearch.Server.Services;

namespace MediaSearch.Models.Test;

internal static class Global {

  /// <summary>
  /// Internal source of data
  /// </summary>
  internal static IMovieCache MovieCache => _MovieCache ??= XMovieCache.Instance(@"data\movies.json");
  private static IMovieCache? _MovieCache;
}
