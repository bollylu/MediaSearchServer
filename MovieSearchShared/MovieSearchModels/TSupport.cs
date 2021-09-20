using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public static class TSupport {
    public static void PrintMoviesName(IEnumerable<IMovie> movies) {
      foreach (IMovie MovieItem in movies) {
        Console.WriteLine(MovieItem.LocalName);
      }
    }

    public static byte[] GetPicture(string pictureName, string pictureExtension = ".png") {
      try {
        using (BinaryReader reader = new BinaryReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"pictures{Path.DirectorySeparatorChar}{pictureName}{pictureExtension}"))) {
          return reader.ReadBytes((int)reader.BaseStream.Length);
        }
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to get picture {pictureName}{pictureExtension} : {ex.Message}");
        return null;
      }
    }

  }
}
