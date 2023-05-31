using BLTools.Encryption;

using SkiaSharp;

namespace MediaSearch.Models;

public abstract class AMedia : ARecord, IMedia {

  public static ELanguage DEFAULT_LANGUAGE = ELanguage.French;

  #region --- IRecord --------------------------------------------
  public override string Id {
    get {
      return _Id ??= _BuildId();
    }
    protected set {
      _Id = value;
    }
  }
  protected string? _Id;

  protected virtual string _BuildId() {
    return Name.HashToBase64();
  }
  #endregion --- IRecord --------------------------------------------

  public EMediaType MediaType { get; set; }

  public ELanguage DefaultLanguage { get; set; } = DEFAULT_LANGUAGE;

  public virtual IMediaSources MediaSources { get; set; } = new TMediaSources();

  public virtual IMediaInfos MediaInfos { get; set; } = new TMediaInfos();

  public virtual IMediaPictures MediaPictures { get; set; } = new TMediaPictures();

  public bool IsInvalid {
    get {
      if (MediaInfos.IsEmpty()) {
        return true;
      }
      return false;
    }
  }
  public string Name {
    get {
      return MediaInfos.Get(DefaultLanguage)?.Title ?? "(no name)";
    }
    set {
    }
  }

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly CreationDate { get; set; } = new DateOnly();
  public int CreationYear {
    get {
      return CreationDate.Year;
    }
  }

  public string Group { get; set; } = "";
  public bool IsGroupMember => !string.IsNullOrWhiteSpace(Group);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMedia() {
    Logger = GlobalSettings.GlobalLogger;
  }

  protected AMedia(ILogger logger) {
    Logger = logger;
  }

  protected AMedia(IMedia media) : this() {
    MediaType = media.MediaType;
    Id = media.Id;

    foreach (var MediaInfoItem in media.MediaInfos) {
      MediaInfos.Add(MediaInfoItem.Key, new TMediaInfo(MediaInfoItem.Value));
    }

    MediaSources = new TMediaSources(media.MediaSources);

    foreach (var MediaPictureItem in media.MediaPictures) {
      MediaPictures.Add(MediaPictureItem.Key, new TPicture(MediaPictureItem.Value));
    }
  }


  public virtual void Dispose() {
  }

  public virtual ValueTask DisposeAsync() {
    return ValueTask.CompletedTask;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();

    RetVal.AppendIndent($"- {nameof(MediaType)} = {MediaType}", indent)
          .AppendIndent($"- {nameof(Id)} = {Id.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent);

    if (IsInvalid) {
      RetVal.AppendIndent("- ### Media is invalid ###", indent);
    }

    if (MediaInfos.Any()) {
      RetVal.AppendIndent($"- {nameof(MediaInfos)}", indent);
      foreach (var MediaInfoItem in MediaInfos) {
        RetVal.AppendIndent($"- {MediaInfoItem.Value}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(MediaInfos)} is empty", indent);
    }

    if (MediaSources.Any()) {
      RetVal.AppendIndent($"- {nameof(MediaSources)}", indent);
      foreach (var MediaSourceItem in MediaSources) {
        RetVal.AppendIndent($"- {MediaSourceItem.GetType().Name}", indent + 2);
        RetVal.AppendIndent($"{MediaSourceItem.ToString(indent + 2)}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(MediaSources)} is empty", indent);
    }

    if (IsGroupMember) {
      RetVal.AppendIndent($"- {nameof(Group)} = {Group.WithQuotes()}", indent);
    } else {
      RetVal.AppendIndent($"- No group membership", indent);
    }

    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  #endregion --- Converters -------------------------------------------------------------------------------------





}