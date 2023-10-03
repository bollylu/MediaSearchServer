namespace MediaSearch.Models;
/// <summary>
/// Allow to contain pictures and to manipulate them
/// </summary>
public interface IPictureContainer {

  IMediaPicture? GetPicture(string pictureName);

  IEnumerable<IMediaPicture> GetPictures();
  IEnumerable<IMediaPicture> GetPictures(EPictureType pictureType);

  bool AddPicture(IMediaPicture picture);
  bool AddPicture(string pictureName, byte[] pictureContent, EPictureType pictureType = EPictureType.Unknown);

  bool DeletePicture(string pictureName);
  bool DeletePicture(IMediaPicture picture);

  Task<bool> LoadPicture(IMediaSource mediaSource);
  Task<bool> LoadPicture(IMediaSource mediaSource, string pictureName, EPictureType pictureType, int width, int height);

}
