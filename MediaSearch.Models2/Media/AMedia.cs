using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMedia :
  ARecord,
  IMedia,
  IMediaInfosContainer,
  IMediaSourcesContainer,
  IMediaPicturesContainer {

  #region --- Public properties ------------------------------------------------------------------------------
  public static ELanguage DEFAULT_LANGUAGE = ELanguage.French;

  public const string MISSING_NAME = "(no name)";

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

  public EMediaType MediaType { get; set; } = EMediaType.Unknown;

  public ELanguage DefaultLanguage { get; set; } = DEFAULT_LANGUAGE;

  public bool IsInvalid {
    get {
      if (MediaType == EMediaType.Unknown) {
        return true;
      }
      if (MediaInfos.IsEmpty()) {
        return true;
      }
      return false;
    }
  }
  public string Name {
    get {
      return MediaInfos.Get(DefaultLanguage)?.Title ?? MISSING_NAME;
    }
    set {
      MediaInfos.SetTitle(DefaultLanguage, value ?? MISSING_NAME);
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

  public IMediaInfos MediaInfos { get; } = new TMediaInfos();
  public IMediaSources MediaSources { get; } = new TMediaSources();
  public IMediaPictures MediaPictures { get; } = new TMediaPictures();

  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMedia() {
    Logger = GlobalSettings.GlobalLogger;
  }

  protected AMedia(ILogger logger) {
    Logger = logger;
  }

  protected AMedia(string name) : this() {
    Name = name;
  }

  protected AMedia(IMedia media) : this() {
    MediaType = media.MediaType;
    Id = media.Id;

    MediaInfos.AddRange(media.MediaInfos.GetAll());
    MediaSources.AddRange(media.MediaSources.GetAll());
    MediaPictures.AddRange(media.MediaPictures.GetAll());
  }


  public virtual void Dispose() {
    MediaInfos.Clear();
    MediaSources.Clear();
    MediaPictures.Clear();
  }

  public virtual ValueTask DisposeAsync() {
    MediaInfos.Clear();
    MediaSources.Clear();
    MediaPictures.Clear();
    return ValueTask.CompletedTask;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();

    if (IsInvalid) {
      RetVal.AppendIndent("- Warning : ### Media is invalid ###", indent);
    }

    RetVal.AppendIndent($"- {nameof(MediaType)} = {MediaType}", indent)
          .AppendIndent($"- {nameof(Id)} = {Id.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent);

    if (MediaInfos.Any()) {
      RetVal.AppendIndent($"- {nameof(MediaInfos)}", indent);
      foreach (var MediaInfoItem in MediaInfos.GetAll()) {
        RetVal.AppendIndent($"- {MediaInfoItem}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(MediaInfos)} is empty", indent);
    }

    if (MediaSources.Any()) {
      RetVal.AppendIndent($"- {nameof(MediaSources)}", indent);
      foreach (var MediaSourceItem in MediaSources.GetAll()) {
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