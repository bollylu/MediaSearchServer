using System.Globalization;

namespace MediaSearch.Models;

public static class IEnumerableMovieExtensions {

  public static IEnumerable<IMovie> FilterBy(this IEnumerable<IMovie> movies, TFilter filter) {
    IEnumerable<IMovie> ByDays = movies.FilterByDays(filter.DaysBack);
    IEnumerable<IMovie> ByName = filter.KeywordsSelection switch {
      EFilterKeywords.Any => ByDays.FilterByAnyKeywords(filter.Name),
      EFilterKeywords.All => ByDays.FilterByAllKeywords(filter.Name),
      _ => throw new NotImplementedException()
    };

    return ByName;
  }

  public static IEnumerable<IMovie> FilterByKeyword(this IEnumerable<IMovie> movies, string filter) {
    if (string.IsNullOrWhiteSpace(filter)) {
      return movies;
    }
    return movies.Where(m => m.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase));
  }

  public static IEnumerable<IMovie> FilterByAnyKeywords(this IEnumerable<IMovie> movies, string filter) {
    if (string.IsNullOrWhiteSpace(filter)) {
      return movies;
    }
    string[] Keywords = filter.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;
    return movies.Where(m => Keywords.Any(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1));
  }

  public static IEnumerable<IMovie> FilterByAllKeywords(this IEnumerable<IMovie> movies, string filter) {
    if (string.IsNullOrWhiteSpace(filter)) {
      return movies;
    }
    string[] Keywords = filter.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;
    return movies.Where(m => Keywords.All(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1));
  }


  public static IEnumerable<IMovie> FilterByAnyTag(this IEnumerable<IMovie> movies, string filter) {
    if (string.IsNullOrWhiteSpace(filter)) {
      return movies;
    }
    string[] FilterTags = filter.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;
    return movies.Where(m => m.Tags.Any(t => FilterTags.Any(ft => CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)));
  }

  public static IEnumerable<IMovie> FilterByAllTags(this IEnumerable<IMovie> movies, string filter) {
    if (string.IsNullOrWhiteSpace(filter)) {
      return movies;
    }
    string[] FilterTags = filter.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

    return movies.Where(m => 
      m.Tags.Count(t => FilterTags.Any(ft => 
        CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)
      ) == FilterTags.Count()
    );
  }



  public static IEnumerable<IMovie> FilterByDays(this IEnumerable<IMovie> movies, int daysBack) {
    if (daysBack == 0) {
      return movies;
    }
    DateOnly Limit = DateOnly.FromDateTime(DateTime.Today.AddDays(-daysBack));
    return movies.Where(m => m.DateAdded >= Limit);
  }

  public static IEnumerable<IMovie> OrderedByName(this IEnumerable<IMovie> movies) {
    return movies.OrderBy(m => m.Name).ThenBy(m => m.OutputYear);
  }
}
