using BLTools.Text;

using MediaSearch.Models;

namespace MediaSearch.Test.Support;
public static class Display {

  public static void TraceMessage(string? title, string? message) {
    message ??= "";
    Console.WriteLine(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
  }

  public static void TraceMessage(string? title, object item) {
    if (item is null) {
      Console.WriteLine($"null object".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
    } else {
      Console.WriteLine(item.ToString() ?? "".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }

  public static void TraceMoviesName(IEnumerable<IMovie> movies) {
    if (movies is null) {
      TraceMessage("Movies", "Movies are null");
    } else {
      StringBuilder MoviesList = new();
      foreach (IMovie MovieItem in movies) {
        MoviesList.AppendLine(MovieItem.Name);
      }
      TraceMessage("Movies", MoviesList);
    }
  }

  public static void TraceMovies(IEnumerable<IMovie> movies) {
    if (movies is null) {
      TraceMessage("Movies", "Movies are null");
    } else {
      foreach (IMovie MovieItem in movies) {
        TraceMessage("Movie", MovieItem);
      }
    }
  }
}
