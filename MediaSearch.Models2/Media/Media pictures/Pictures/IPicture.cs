namespace MediaSearch.Models;
public interface IPicture {
  string Name { get; init; }

  byte[] Data { get; }

  EPictureType PictureType { get; init; }

  Task<bool> LoadAsync(string location);

  byte[] Resize(int width, int height);
}
