using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMedia : IMedia {

  #region --- Public properties ------------------------------------------------------------------------------
  
  public EMediaSourceType MediaType { get; set; }
  
  public string ID {
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
      if (Titles.Any()) {
        return Titles.GetPrincipal()?.Value ?? "";
      } else {
        return "";
      }
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
    MediaType = media.MediaType;
    ID = media.ID;
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

  public virtual void Dispose() {
    Titles?.Clear();
    Descriptions?.Clear();
    Tags?.Clear();
  }

  public virtual ValueTask DisposeAsync() {
    Titles?.Clear();
    Descriptions?.Clear();
    Tags?.Clear();
    return ValueTask.CompletedTask;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public virtual string ToString(int indent) {
    StringBuilder RetVal = new();

    RetVal.AppendIndent($"- {nameof(MediaType)} = {MediaType}", indent)
          .AppendIndent($"- {nameof(ID)} = {ID.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(StorageRoot)} = {StorageRoot.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(StoragePath)} = {StoragePath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(FileName)} = {FileName.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(FileExtension)} = {FileExtension.WithQuotes()}", indent);
    
    if (Titles.Any()) {
      RetVal.AppendIndent($"- {nameof(Titles)}", indent);
      foreach (var TitleItem in Titles.GetAll()) {
        RetVal.AppendIndent($"- {TitleItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Titles)} is empty", indent);
    }
    if (Descriptions.Any()) {
      RetVal.AppendIndent($"- {nameof(Descriptions)}", indent);
      foreach (var DescriptionItem in Descriptions.GetAll()) {
        RetVal.AppendIndent($"- {DescriptionItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Descriptions)} is empty", indent);
    }

    if (Tags.Any()) {
      RetVal.AppendIndent($"- {nameof(Tags)}", indent);
      foreach (string TagItem in Tags) {
        RetVal.AppendIndent($"- {TagItem.WithQuotes()}", indent+2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Tags)} is empty", indent);
    }

    if (IsGroupMember) {
      RetVal.AppendIndent($"- {nameof(Group)} = {Group.WithQuotes()}", indent);
    } else {
      RetVal.AppendIndent($"- No group membership", indent);
    }
    RetVal.AppendIndent($"- {nameof(Size)} = {Size} bytes", indent)
          .AppendIndent($"- {nameof(CreationYear)} = {CreationYear}", indent);
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