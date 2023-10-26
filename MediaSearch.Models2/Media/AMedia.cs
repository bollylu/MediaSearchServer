using BLTools.Diagnostic;
using BLTools.Encryption;

namespace MediaSearch.Models;

public abstract class AMedia :
  ARecord,
  IMedia {

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

  #region --- IMedia --------------------------------------------
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
      IMediaInfo? MediaInfo = MediaInfos.GetDefault();
      if (MediaInfo is not null) {
        return MediaInfo.Name;
      } else {
        return MISSING_NAME;
      }
    }
    set {
      IMediaInfo? MediaInfo = MediaInfos.GetDefault();
      if (MediaInfo is not null) {
        MediaInfo.Name = value ?? MISSING_NAME;
      }
    }
  }

  [JsonConverter(typeof(TDateOnlyJsonConverter))]
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

  //[DoNotDump]
  public IMediaInfos MediaInfos { get; } = new TMediaInfos();
  //[DoNotDump]
  public IMediaSources MediaSources { get; } = new TMediaSources();
  //[DoNotDump]
  public IMediaPictures MediaPictures { get; } = new TMediaPictures();
  #endregion --- IMedia -----------------------------------------

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

    //if (IsGroupMember) {
    //  RetVal.AppendIndent($"- {nameof(Groups)} = {Groups.Select(g => g.WithQuotes()).CombineToString()}", indent + 2);
    //} else {
    //  RetVal.AppendIndent("- No group membership");
    //}

    return RetVal.ToString();
  }


  public override string ToString() {
    return ToString(0);
  }

  //public virtual string Dump() {
  //  StringBuilder RetVal = new();
  //  if (IsInvalid) {
  //    RetVal.AppendLine("- Warning : ### Media is invalid ###");
  //  }

  //  RetVal.AppendLine($"- {nameof(MediaType)} = {MediaType}")
  //        .AppendLine($"- {nameof(Id)} = {Id.WithQuotes()}")
  //        .AppendLine($"- {nameof(Name)} = {Name.WithQuotes()}");

  //  if (MediaInfos.Any()) {
  //    RetVal.AppendLine($"- {nameof(MediaInfos)} [{MediaInfos.GetAll().Count()}]");
  //    RetVal.AppendIndent(MediaInfos.Dump());
  //  } else {
  //    RetVal.AppendLine($"- {nameof(MediaInfos)} is empty");
  //  }

  //  if (MediaSources.Any()) {
  //    RetVal.AppendLine($"- {nameof(MediaSources)} [{MediaSources.GetAll().Count()}]");
  //    RetVal.AppendIndent(MediaSources.Dump());
  //  } else {
  //    RetVal.AppendLine($"- {nameof(MediaSources)} is empty");
  //  }
  //  return RetVal.ToString();
  //}
  #endregion --- Converters -------------------------------------------------------------------------------------

















}