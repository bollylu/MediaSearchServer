﻿using System.Globalization;
using MediaSearch.Models2.Support.Filter;

namespace MediaSearch.Models;

public static class EnumerableMediaExtensions {

  public static IEnumerable<IMedia> WithFilter(this IEnumerable<IMedia> medias, IFilter filter) {

    IEnumerable<IMedia> Filtered = medias.FilterByDays(filter)
                                         .FilterByOutputDate(filter)
                                         .FilterByKeywords(filter)
                                         .FilterByTags(filter)
                                         .FilterByGroupsOnly(filter)
                                         ;

    return Filtered;
  }

  public static IEnumerable<IMedia> OrderedBy(this IEnumerable<IMedia> medias, IFilter filter) {

    switch (filter.SortOrder) {
      default:
      case EFilterSortOrder.Name:
        return medias
          .OrderBy(m => m.Name)
          .ThenBy(m => m.CreationYear);

      case EFilterSortOrder.OutputYear:
        return medias
          .OrderBy(m => m.CreationYear)
          .ThenBy(m => m.Name);

      case EFilterSortOrder.Group:
        return medias
          .OrderBy(m => m.Group)
          .ThenBy(m => m.CreationYear)
          .ThenBy(m => m.Name);
    }
  }

  #region --- Keywords --------------------------------------------
  public static IEnumerable<IMedia> FilterByKeywords(this IEnumerable<IMedia> medias, IFilter filter) {
    if (filter.Keywords.IsEmpty()) {
      return medias;
    }

    return medias
      .Where(m => m.MediaInfos.Exists(filter.Language))
      .Where(m => filter.Keywords.IsMatch(m.MediaInfos[filter.Language].Title));
  }
  #endregion --- Keywords --------------------------------------------

  #region --- Tags --------------------------------------------
  public static IEnumerable<IMedia> FilterByTags(this IEnumerable<IMedia> medias, IFilter filter) {
    if (filter.Tags.IsEmpty()) {
      return medias;
    }

    return medias
      .Where(m => m.MediaInfos.Exists(filter.Language))
      .Where(m => filter.Tags.IsMatch(m.MediaInfos[filter.Language].Tags));
  }
  #endregion --- Tags --------------------------------------------

  #region --- Days since added --------------------------------------------
  public static IEnumerable<IMedia> FilterByDays(this IEnumerable<IMedia> medias, IFilterDaysBack filter) {
    if (filter.DaysBack == 0) {
      return medias;
    }
    DateOnly Limit = DateOnly.FromDateTime(DateTime.Today.AddDays(-filter.DaysBack));

    return medias.Where(m => m.DateAdded >= Limit);
  }
  #endregion --- Days since added --------------------------------------------

  #region --- Output year --------------------------------------------
  public static IEnumerable<IMedia> FilterByOutputDate(this IEnumerable<IMedia> medias, IFilter filter) {
    if (filter.OutputDateMax < filter.OutputDateMin) {
      return medias;
    }
    return medias
      .Where(m => m.MediaSources.Exists(filter.Language))
      .Where(m => m.MediaSources[filter.Language]
                         .CreationYear
                         .IsInRange(filter.OutputDateMin, filter.OutputDateMax));
  }
  #endregion --- Output year --------------------------------------------

  public static IEnumerable<IMedia> FilterByGroupsOnly(this IEnumerable<IMedia> medias, IFilter filter) {
    if (filter is null) {
      return medias;
    }
    if (filter.GroupOnly) {
      return medias.Where(m => m.IsGroupMember);
    } else {
      return medias;
    }
  }
  //public static IEnumerable<IMedia> FilterByGroup(this IEnumerable<IMedia> medias, IFilterGroup filter) {
  //  if (filter is null || string.IsNullOrWhiteSpace(filter.Group)) {
  //    return medias;
  //  }

  //  return medias.Where(m => m.Group.Equals(filter.Group, StringComparison.CurrentCultureIgnoreCase));
  //}

  public static IAsyncEnumerable<string> GetGroups(this IEnumerable<IMedia> movies) {
    return movies.Where(m => m.IsGroupMember)
                 .Select(m => m.Group)
                 .Distinct()
                 .OrderBy(x => x)
                 .ToAsyncEnumerable();
  }

}