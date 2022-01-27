using System.Collections.Generic;
using System.Globalization;
using System.Xml.Linq;

namespace MediaSearch.Models;

public static class IEnumerableMovieExtensions {

  public static IEnumerable<IMovie> WithFilter(this IEnumerable<IMovie> movies, TFilter filter) {
    IEnumerable<IMovie> Filtered = movies.FilterByDays(filter)
                                         .FilterByOutputDate(filter)
                                         .FilterByKeywords(filter)
                                         .FilterByTags(filter)
                                         .FilterByGroupsOnly(filter)
                                         .FilterByGroup(filter)
                                         .FilterBySubGroup(filter);

    return Filtered;
  }

  public static IEnumerable<IMovie> OrderedBy(this IEnumerable<IMovie> movies, TFilter filter) {

    switch (filter.SortOrder) {
      default:
      case EFilterSortOrder.Name:
        return movies.OrderBy(m => m.Name).ThenBy(m => m.OutputYear);
      case EFilterSortOrder.OutputYear:
        return movies.OrderBy(m => m.OutputYear).ThenBy(m => m.Name);
      case EFilterSortOrder.Group:
        return movies.OrderBy(m => m.Group).ThenBy(m => m.SubGroup).ThenBy(m => m.OutputYear).ThenBy(m => m.Name);
    }
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
                                        FilterTags.Any(ft =>
                                                         m.Tags.Any(t =>
                                                                      CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1
                                                                   )
                                                  )
                                     ),
      EFilterType.All => movies.Where(m =>
                                        FilterTags.All(ft =>
                                                         m.Tags.Any(t =>
                                                                      CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1
                                                                   )
                                                      )
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
    IList<IMovie> RetVal = movies.Where(m => m.DateAdded >= Limit).ToList();

    return RetVal;
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

  public static IEnumerable<IMovie> FilterByGroupsOnly(this IEnumerable<IMovie> movies, TFilter filter) {
    if (filter is null) {
      return movies;
    }
    if (filter.GroupOnly) {
      return movies.Where(m => m.IsGroupMember);
    } else {
      return movies;
    }
  }
  public static IEnumerable<IMovie> FilterByGroup(this IEnumerable<IMovie> movies, TFilter filter) {
    if (filter is null || string.IsNullOrWhiteSpace(filter.Group)) {
      return movies;
    }

    return movies.Where(m => m.Group.Equals(filter.Group, StringComparison.CurrentCultureIgnoreCase));
  }

  public static IEnumerable<IMovie> FilterBySubGroup(this IEnumerable<IMovie> movies, TFilter filter) {
    if (filter is null || string.IsNullOrWhiteSpace(filter.SubGroup)) {
      return movies;
    }

    return movies.Where(m => m.SubGroup.Equals(filter.SubGroup, StringComparison.CurrentCultureIgnoreCase));
  }

  public static IEnumerable<string> GetGroups(this IEnumerable<IMovie> movies) {
    return movies.Where(m => m.IsGroupMember)
                 .Select(m => m.Group)
                 .Distinct()
                 .OrderBy(x => x);
  }

  public static IEnumerable<string> GetSubGroups(this IEnumerable<IMovie> movies, string group) {
    return movies.Where(m => m.IsGroupMember)
                 .Where(m => m.Group.Equals(group, StringComparison.CurrentCultureIgnoreCase))
                 .Where(m => !string.IsNullOrWhiteSpace(m.SubGroup))
                 .Select(m => m.SubGroup)
                 .Distinct()
                 .OrderBy(x => x);
  }
}
