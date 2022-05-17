using System.Collections;

using BLTools.Text;

using MediaSearch.Database;
using MediaSearch.Models;

namespace MediaSearch.Test.Support;
public static class Display {

  public static void TraceMessage(string message) {
    Console.WriteLine($"==> {message}");
  }

  public static void TraceBox(string? title, string? message) {
    message ??= "";
    Console.WriteLine(message.BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
  }

  public static void TraceBox(string? title, object? item) {

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

      case object ObjectItem when ObjectItem.GetType().IsGenericType:
        Console.WriteLine(ObjectItem.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
        return;

      default: {
          Console.WriteLine(item.ToString().BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
          return;
        }
    }
      
  }

  


  public static void TraceMoviesName(IEnumerable<IMovie> movies) {
    if (movies is null) {
      TraceBox("Movies", "Movies are null");
    } else {
      StringBuilder MoviesList = new();
      foreach (IMovie MovieItem in movies) {
        MoviesList.AppendLine(MovieItem.Name);
      }
      TraceBox("Movies", MoviesList);
    }
  }

  public static void TraceMedias(IEnumerable<IMedia> medias) {
    if (medias is null) {
      TraceBox("Media", "Medias are null");
    } else {
      foreach (IMedia MediaItem in medias) {
        TraceBox(MediaItem.GetType().Name, MediaItem);
      }
    }
  }
}
