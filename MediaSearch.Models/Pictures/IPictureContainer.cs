namespace MediaSearch.Models;
/// <summary>
/// Allow to contain pictures and to manipulate them
/// </summary>
public interface IPictureContainer {

  IPicture? GetPicture(string pictureName);

  IEnumerable<IPicture> GetPictures();
  IEnumerable<IPicture> GetPictures(EPictureType pictureType);

  bool AddPicture(IPicture picture);
  bool AddPicture(string pictureName, byte[] pictureContent, EPictureType pictureType = EPictureType.Unknown);

  bool DeletePicture(string pictureName);
  bool DeletePicture(IPicture picture);

  Task<bool> LoadPicture();
  Task<bool> LoadPicture(string pictureName, EPictureType pictureType, int width, int height);

}
