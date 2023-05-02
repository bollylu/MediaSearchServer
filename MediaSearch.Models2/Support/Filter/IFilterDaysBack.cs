namespace MediaSearch.Models;

public interface IFilterDaysBack {

  /// <summary>
  /// How much days to go back in time
  /// </summary>
  int DaysBack { get; }

}
