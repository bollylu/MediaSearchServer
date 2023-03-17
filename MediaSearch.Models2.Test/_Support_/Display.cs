using BLTools.Text;

namespace MediaSearch.Test.Support;
public static class Display {

  public static void TraceMessage(string title, string message) {
    message ??= "";
    Console.WriteLine(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
  }

  public static void TraceMessage(string? title, object item) {
    if (item is null) {
      Console.WriteLine($"null object".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
    } else {
      Console.WriteLine(item.ToString() ?? "".BoxFixedWidth(title ?? "", GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }

  public static void TraceMediasName(IEnumerable<IMedia> medias) {
    if (medias is null) {
      TraceMessage("Medias", "Medias are null");
    } else {
      StringBuilder MediasList = new();
      foreach (IMedia MediaItem in medias) {
        MediasList.AppendLine(MediaItem.Name);
      }
      TraceMessage("Medias", MediasList);
    }
  }

  public static void TraceMedias(IEnumerable<IMedia> medias) {
    if (medias is null) {
      TraceMessage("Medias", "Medias are null");
    } else {
      foreach (IMedia MediaItem in medias) {
        TraceMessage("Media", MediaItem);
      }
    }
  }
}
