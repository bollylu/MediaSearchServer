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

  public EMediaType MediaType { get; init; }

  public ELanguage DefaultLanguage { get; init; } = DEFAULT_LANGUAGE;

  public virtual IMediaSources MediaSources { get; init; } = new TMediaSources();

  #region --- IMediaInfos --------------------------------------------
  public IMediaInfos MediaInfos { get; init; } = new TMediaInfos();

  public IMediaInfo GetInfos() {
    if (IsInvalid) {
      return new TMediaInfo();
    }

    if (MediaInfos.ContainsKey(DefaultLanguage)) {
      return MediaInfos[DefaultLanguage];
    }

    return MediaInfos.First().Value;
  }
  public IMediaInfo GetInfos(ELanguage language) {
    if (IsInvalid) {
      return new TMediaInfo();
    }

    if (MediaInfos.ContainsKey(language)) {
      return MediaInfos[language];
    }

    return GetInfos();
  }
  #endregion --- IMediaInfos --------------------------------------------

  public virtual IMediaPictures MediaPictures { get; init; } = new TMediaPictures();

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
      return GetInfos(DefaultLanguage).Title;
    }
    set {
    }
  }
  //public ILanguageTextInfos Titles { get; } = new TLanguageTextInfos();

  //public ILanguageTextInfos Descriptions { get; } = new TLanguageTextInfos();

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

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