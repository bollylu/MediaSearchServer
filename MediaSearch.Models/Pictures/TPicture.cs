namespace MediaSearch.Models;
public class TPicture : ALoggable, IPicture {
  public string Name { get; init; } = "";
  public byte[] Data { get; private set; }
  public EPictureType PictureType { get; init; } = EPictureType.Unknown;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TPicture() {
    Data = Array.Empty<byte>();
  }

  public TPicture(string name, byte[] data, EPictureType pictureType = EPictureType.Unknown) {
    Name = name;
    Data = data;
    PictureType = pictureType;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Name)} = {Name}");
    RetVal.AppendLine($"{nameof(PictureType)} = {PictureType}");
    RetVal.AppendLine($"{nameof(Data)} = {Data.Length} byte(s)");
    return RetVal.ToString();
  }

  public async Task<bool> LoadAsync(string location) {
    try {
      Data = await File.ReadAllBytesAsync(location);
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to load picture from {location.WithQuotes()}", ex);
      return false;
    }
  }

  public byte[] Resize(int width, int height) {
    throw new NotImplementedException();
  }
}
