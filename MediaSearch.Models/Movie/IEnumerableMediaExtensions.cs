using System.Globalization;

namespace MediaSearch.Models;

public static class IEnumerableMediaExtensions {

  public static IEnumerable<T> WithFilter<T>(this IEnumerable<T> medias, IFilter filter) where T : IMedia {
    IEnumerable<T> Filtered = medias.FilterByDays(filter)
                                         .FilterByOutputDate(filter)
                                         .FilterByKeywords(filter)
                                         .FilterByTags(filter)
                                         .FilterByGroupsOnly(filter)
                                         .FilterByGroup(filter);

    return Filtered;
  }

  public static IEnumerable<T> OrderedBy<T>(this IEnumerable<T> medias, IFilter filter) where T : IMedia {

    switch (filter.SortOrder) {
      default:
      case EFilterSortOrder.Name:
        return medias.OrderBy(m => m.Name).ThenBy(m => m.CreationYear);
      case EFilterSortOrder.CreationYear:
        return medias.OrderBy(m => m.CreationYear).ThenBy(m => m.Name);
      case EFilterSortOrder.Group:
        return medias.OrderBy(m => m.Group).ThenBy(m => m.CreationYear).ThenBy(m => m.Name);
    }
  }

  #region --- Keywords --------------------------------------------
  public static IEnumerable<T> FilterByKeywords<T>(this IEnumerable<T> medias, IFilterKeywords filter) where T : IMedia {
    if (string.IsNullOrWhiteSpace(filter.Keywords)) {
      return medias;
    }
    string[] Keywords = filter.Keywords.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

    return filter.KeywordsSelection switch {
      EFilterType.Any => medias.Where(m => Keywords.Any(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)),
      EFilterType.All => medias.Where(m => Keywords.All(k => CI.IndexOf(m.Name, k, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1)),
      _ => throw new NotImplementedException()
    };
  }
  #endregion --- Keywords --------------------------------------------

  #region --- Tags --------------------------------------------
  public static IEnumerable<T> FilterByTags<T>(this IEnumerable<T> medias, IFilterTags filter) where T : IMedia {
    if (string.IsNullOrWhiteSpace(filter.Tags)) {
      return medias;
    }
    string[] FilterTags = filter.Tags.Split(" ");
    CompareInfo CI = CultureInfo.CurrentCulture.CompareInfo;

    return filter.TagSelection switch {
      EFilterType.Any => medias.Where(m =>
                                        FilterTags.Any(ft =>
                                                         m.Tags.Any(t =>
                                                                      CI.IndexOf(t, ft, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) > -1
                                                                   )
                                                  )
                                     ),
      EFilterType.All => medias.Where(m =>
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
  public static IEnumerable<T> FilterByDays<T>(this IEnumerable<T> medias, IFilterDaysBack filter) where T : IMedia {
    if (filter.DaysBack == 0) {
      return medias;
    }
    DateOnly Limit = DateOnly.FromDateTime(DateTime.Today.AddDays(-filter.DaysBack));
    return medias.Where(m => m.DateAdded >= Limit).ToList();
  }
  #endregion --- Days since added --------------------------------------------

  #region --- Output year --------------------------------------------
  public static IEnumerable<T> FilterByOutputDate<T>(this IEnumerable<T> medias, IFilterOutputDate filter) where T : IMedia {
    if (filter.OutputDateMax < filter.OutputDateMin) {
      return medias;
    }
    return medias.Where(m => m.CreationYear >= filter.OutputDateMin && m.CreationYear <= filter.OutputDateMax);
  }
  #endregion --- Output year --------------------------------------------

  public static IEnumerable<T> FilterByGroupsOnly<T>(this IEnumerable<T> medias, IFilter filter) where T : IMedia {
    if (filter is null) {
      return medias;
    }
    if (filter.GroupOnly) {
      return medias.Where(m => m.IsGroupMember);
    } else {
      return medias;
    }
  }
  public static IEnumerable<T> FilterByGroup<T>(this IEnumerable<T> medias, IFilterGroup filter) where T : IMedia {
    if (filter is null || string.IsNullOrWhiteSpace(filter.Group)) {
      return medias;
    }

    return medias.Where(m => m.Group.Equals(filter.Group, StringComparison.CurrentCultureIgnoreCase));
  }

  //public static IEnumerable<IMedia> FilterBySubGroup(this IEnumerable<IMedia> movies, IFilterGroup filter) {
  //  if (filter is null || string.IsNullOrWhiteSpace(filter.SubGroup)) {
  //    return movies;
  //  }

  //  return movies.Where(m => m.SubGroup.Equals(filter.SubGroup, StringComparison.CurrentCultureIgnoreCase));
  //}

  public static IAsyncEnumerable<string> GetGroups<T>(this IEnumerable<T> medias) where T : IMedia {
    return medias.Where(m => m.IsGroupMember)
                 .Select(m => m.Group)
                 .Distinct()
                 .OrderBy(x => x)
                 .ToAsyncEnumerable();
  }

  //public static IEnumerable<string> GetSubGroups(this IEnumerable<IMedia> movies, string group) {
  //  return movies.Where(m => m.IsGroupMember)
  //               .Where(m => m.Group.Equals(group, StringComparison.CurrentCultureIgnoreCase))
  //               .Where(m => !string.IsNullOrWhiteSpace(m.SubGroup))
  //               .Select(m => m.SubGroup)
  //               .Distinct()
  //               .OrderBy(x => x);
  //}
}
