namespace MediaSearch.Models;
public interface IMediaPicturesContainer {
  IMediaPictures MediaPictures { get; }
  //  IMediaPicture? GetPicture(string pictureName);

  //  IEnumerable<IMediaPicture> GetPictures();
  //  IEnumerable<IMediaPicture> GetPictures(EPictureType pictureType);

  //  bool AddPicture(IMediaPicture picture);
  //  bool AddPicture(string pictureName, byte[] pictureContent, EPictureType pictureType = EPictureType.Unknown);

  //  bool DeletePicture(string pictureName);
  //  bool DeletePicture(IMediaPicture picture);

  //  Task<bool> LoadPicture(IMediaSource mediaSource);
  //  Task<bool> LoadPicture(IMediaSource mediaSource, string pictureName, EPictureType pictureType, int width, int height);
}
