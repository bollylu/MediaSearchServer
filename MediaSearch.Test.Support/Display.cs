using BLTools.Text;

namespace MediaSearch.Test.Support;
public static class Display {

  public static void TraceMessage(string title, string? message) {
    message ??= "";
    Console.WriteLine(message.BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
  }

  public static void TraceMessage(string title, object item) {
    if (item is null) {
      Console.WriteLine($"null object of type {item.GetType().Name}".BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
    } else {
      Console.WriteLine(item.ToString().BoxFixedWidth(title, GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }
}
