using System.Diagnostics;
using System.Reflection;

namespace MovieSearchModels;

public static class TSupport {

  public static void PrintMoviesName(IEnumerable<IMovie> movies) {
    foreach ( IMovie MovieItem in movies ) {
      Console.WriteLine(MovieItem.Name);
    }
  }

  public static byte[] GetPicture(string pictureName, string pictureExtension = ".png") {
    try {
      Assembly Asm = Assembly.GetExecutingAssembly();
      string CompleteName = $"{Asm.GetName().Name}.Pictures.{pictureName}{pictureExtension}";
      string[] Resources = Asm.GetManifestResourceNames();
      foreach ( string ResourceItem in Resources ) {
        Console.WriteLine(ResourceItem);
      }
      using ( BinaryReader reader = new BinaryReader(Asm.GetManifestResourceStream(CompleteName)) ) {
        return reader.ReadBytes((int)reader.BaseStream.Length);
      }
    } catch ( Exception ex ) {
      Trace.WriteLine($"Unable to get picture {pictureName}{pictureExtension} : {ex.Message}");
      return null;
    }
  }

}

