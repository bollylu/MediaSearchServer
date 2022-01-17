using System.Globalization;
using System.Xml.Linq;

namespace MediaSearch.Models;

public static class IEnumerableMovieExtensions {

  public static IEnumerable<IMovie> WithFilter(this IEnumerable<IMovie> movies, TFilter filter) {
    IEnumerable<IMovie> Filtered = movies.FilterByDays(filter)
                                         .FilterByOutputDate(filter)
                                         .FilterByKeywords(filter)
                                         .FilterByTags(filter);

    return Filtered;
  }


  #region --- Keywords --------------------------------------------
  public static IEnumerable<IMovie> FilterByKeywords(this IEnumerable<IMovie> movies, TFilter filter) {
    if (string.IsNullOrWhiteSpace(filter.Keywords)) {
      return movies;
    }
    string[] Keywords = filter.Keywords.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

    return filter.KeywordsSelection switch {
      EFilterType.Any => movies.Where(m => Keywords.Any(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)),
      EFilterType.All => movies.Where(m => Keywords.All(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)),
      _ => throw new NotImplementedException()
    };
  }
  #endregion --- Keywords --------------------------------------------

  #region --- Tags --------------------------------------------
  public static IEnumerable<IMovie> FilterByTags(this IEnumerable<IMovie> movies, TFilter filter) {
    if (string.IsNullOrWhiteSpace(filter.Tags)) {
      return movies;
    }
    string[] FilterTags = filter.Tags.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

    return filter.TagSelection switch {
      EFilterType.Any => movies.Where(m =>
                                        m.Tags.Any(t =>
                                                     FilterTags.Any(ft =>
                                                                      CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1
                                                                   )
                                                  )
                                     ),
      EFilterType.All => movies.Where(m =>
                                        m.Tags.Count(t =>
                                                       FilterTags.Any(ft =>
                                                                        CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1
                                                                     )
                                                    ) == FilterTags.Count()
                                     ),
      _ => throw new NotImplementedException()
    };

  }
  #endregion --- Tags --------------------------------------------

  #region --- Days since added --------------------------------------------
  public static IEnumerable<IMovie> FilterByDays(this IEnumerable<IMovie> movies, TFilter filter) {
    if (filter.DaysBack == 0) {
      return movies;
    }
    DateOnly Limit = DateOnly.FromDateTime(DateTime.Today.AddDays(-filter.DaysBack));
    return movies.Where(m => m.DateAdded >= Limit);
  }
  #endregion --- Days since added --------------------------------------------

  #region --- Output year --------------------------------------------
  public static IEnumerable<IMovie> FilterByOutputDate(this IEnumerable<IMovie> movies, TFilter filter) {
    if (filter.OutputDateMax < filter.OutputDateMin) {
      return movies;
    }
    return movies.Where(m => m.OutputYear >= filter.OutputDateMin && m.OutputYear <= filter.OutputDateMax);
  }
  #endregion --- Output year --------------------------------------------

  public static IEnumerable<IMovie> OrderedByName(this IEnumerable<IMovie> movies) {
    return movies.OrderBy(m => m.Name).ThenBy(m => m.OutputYear);
  }
}
