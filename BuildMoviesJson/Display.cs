using System.Collections;
using System.Text;

using BLTools.Text;

using MediaSearch.Models;

namespace BuildMoviesJson;

public static class Display {

  public static void TraceMessage(string? title, string? message) {
    message ??= "";
    Console.WriteLine(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
  }

  public static void TraceMessage(string? title, object? item) {

    if (item is null) {
      Console.WriteLine($"null object".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
      return;
    }

    switch (item) {
      case string StringItem: {
          Console.WriteLine(StringItem.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
          return;
        }

      case IEnumerable EnumerableItems: {
          foreach(var ItemItem in EnumerableItems) {
            Console.WriteLine(ItemItem.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
          }
          return;
        }

      default: {
          Console.WriteLine(item.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
          return;
        }
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

  public static void TraceMedias(IEnumerable<IMedia> medias) {
    if (medias is null) {
      TraceMessage("Media", "Medias are null");
    } else {
      foreach (IMedia MediaItem in medias) {
        TraceMessage(MediaItem.GetType().Name, MediaItem);
      }
    }
  }
}
