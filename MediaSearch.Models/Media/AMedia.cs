using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMedia : ADataModel, IMedia {

  public string Id {
    get {
      return _Id ??= _BuildId();
    }
    protected set {
      _Id = value;
    }
  }
  private string? _Id;

  protected virtual string _BuildId() {
    return Name.HashToBase64();
  }

  #region --- IName --------------------------------------------
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  #endregion --- IName --------------------------------------------

  #region --- IMultiNames --------------------------------------------
  public Dictionary<string, string> AltNames { get; } = new();
  #endregion --- IMultiNames --------------------------------------------

  #region --- ITags --------------------------------------------
  public List<string> Tags { get; } = new();
  #endregion --- ITags --------------------------------------------

  public string StorageRoot { get; set; } = "";
  public string StoragePath { get; set; } = "";
  public string FileName { get; set; } = "";
  public string FileExtension { get; set; } = "";

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly DateAdded { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMedia() {
    Logger = new TConsoleLogger();
    Logger.SeverityLimit = ESeverity.Debug;
  }

  protected AMedia(IMedia media) {
    Logger = new TConsoleLogger();
    Logger.SeverityLimit = ESeverity.Debug;

    Id = media.Id;
    Name = media.Name;
    Description = media.Description;

    StorageRoot = media.StorageRoot;
    StoragePath = media.StoragePath;
    FileName = media.FileName;
    FileExtension = media.FileExtension;

    foreach (KeyValuePair<string, string> AltNameItem in media.AltNames) {
      AltNames.Add(AltNameItem.Key, AltNameItem.Value);
    }

    foreach (string TagItem in media.Tags) {
      Tags.Add(TagItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"{nameof(Id)} = \"{Id}\"");
    RetVal.AppendLine($"{nameof(Name)} = {Name}");
    RetVal.AppendLine($"{nameof(Description)} = {Description}");
    RetVal.AppendLine($"{nameof(StorageRoot)} = {StorageRoot}");
    RetVal.AppendLine($"{nameof(StoragePath)} = {StoragePath}");
    RetVal.AppendLine($"{nameof(FileName)} = {FileName}");
    RetVal.AppendLine($"{nameof(FileExtension)} = {FileExtension}");
    if (AltNames.Any()) {
      RetVal.AppendLine("Alt. names");
      foreach (KeyValuePair<string, string> AltNameItem in AltNames) {
        RetVal.AppendLine($"|- {AltNameItem.Key}:{AltNameItem.Value}");
      }
    } else {
      RetVal.AppendLine($"{nameof(AltNames)} is empty");
    }
    if (Tags.Any()) {
      RetVal.AppendLine("Tags");
      foreach (string TagItem in Tags) {
        RetVal.AppendLine($"|- {TagItem}");
      }
    } else {
      RetVal.AppendLine($"{nameof(Tags)} is empty");
    }
    return RetVal.ToString();
  }

  
}