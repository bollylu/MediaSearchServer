using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMedia : IMedia {

  #region --- Public properties ------------------------------------------------------------------------------
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

  public string Name {
    get {
      return Titles.GetPrincipal()?.Value ?? "";
    }
  }

  public ILanguageTextInfos Titles { get; } = new TLanguageTextInfos();

  public ILanguageTextInfos Descriptions { get; } = new TLanguageTextInfos();

  public List<string> Tags { get; } = new();

  public DateOnly CreationDate { get; set; } = new DateOnly();
  public int CreationYear {
    get {
      return CreationDate.Year;
    }
  }

  public string StorageRoot { get; set; } = "";
  public string StoragePath { get; set; } = "";
  public string FileName { get; set; } = "";
  public string FileExtension { get; set; } = "";
  public long Size { get; set; }

  public DateOnly DateAdded { get; set; }

  public string Group { get; set; } = "";
  public bool IsGroupMember => !string.IsNullOrWhiteSpace(Group); 
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMedia() {
  }

  protected AMedia(IMedia media) {
    Id = media.Id;
    StorageRoot = media.StorageRoot;
    StoragePath = media.StoragePath;
    FileName = media.FileName;
    FileExtension = media.FileExtension;
    CreationDate = media.CreationDate;
    Size = media.Size;
    DateAdded = media.DateAdded;
    Group = media.Group;

    foreach (var TitleItem in media.Titles.GetAll()) {
      Titles.Add(TitleItem);
    }

    foreach (var DescriptionItem in media.Descriptions.GetAll()) {
      Titles.Add(DescriptionItem);
    }

    foreach (string TagItem in media.Tags) {
      Tags.Add(TagItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public virtual string ToString(int Indent) {
    StringBuilder RetVal = new();
    string IndentSpace = new string(' ', Indent);
    RetVal.AppendLine($"{IndentSpace}{nameof(Id)} = {Id.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(Name)} = {Name.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(StorageRoot)} = {StorageRoot.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(StoragePath)} = {StoragePath.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(FileName)} = {FileName.WithQuotes()}");
    RetVal.AppendLine($"{IndentSpace}{nameof(FileExtension)} = {FileExtension.WithQuotes()}");
    if (Titles.Any()) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Titles)}");
      RetVal.AppendLine($"{IndentSpace}{Titles.ToString(2)}");
    } else {
      RetVal.AppendLine($"{IndentSpace}{nameof(Titles)} is empty");
    }
    if (Descriptions.Any()) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Descriptions)}");
      RetVal.AppendLine($"{IndentSpace}{Descriptions.ToString(2)}");
    } else {
      RetVal.AppendLine($"{IndentSpace}{nameof(Descriptions)} is empty");
    }

    if (Tags.Any()) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Tags)}");
      foreach (string TagItem in Tags) {
        RetVal.AppendLine($"{IndentSpace}|- {TagItem.WithQuotes()}");
      }
    } else {
      RetVal.AppendLine($"{IndentSpace}{nameof(Tags)} is empty");
    }

    if (IsGroupMember) {
      RetVal.AppendLine($"{IndentSpace}{nameof(Group)} = {Group.WithQuotes()}");
    } else {
      RetVal.AppendLine($"{IndentSpace}No group membership");
    }
    RetVal.AppendLine($"{IndentSpace}{nameof(Size)} = {Size} bytes");
    RetVal.AppendLine($"{IndentSpace}{nameof(CreationYear)} = {CreationYear}");
    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  #region --- IDirty --------------------------------------------
  public bool IsDirty { get; protected set; } = false;

  public virtual void SetDirty() {
    IsDirty = true;
  }

  public virtual void ClearDirty() {
    IsDirty = false;
  } 
  #endregion --- IDirty --------------------------------------------
}