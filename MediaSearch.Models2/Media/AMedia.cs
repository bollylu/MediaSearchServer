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
    if (MediaInfos.IsEmpty()) {
      throw new ApplicationException("Invalid media : MediaInfos cannot be empty");
    }

    if (MediaInfos.ContainsKey(DefaultLanguage)) {
      return MediaInfos[DefaultLanguage];
    }

    return MediaInfos.First().Value;
  }
  public IMediaInfo GetInfos(ELanguage language) {
    if (MediaInfos.IsEmpty()) {
      throw new ApplicationException("Invalid media : MediaInfos cannot be empty");
    }

    if (MediaInfos.ContainsKey(language)) {
      return MediaInfos[language];
    }

    return GetInfos();
  }
  #endregion --- IMediaInfos --------------------------------------------

  public virtual IMediaPictures MediaPictures { get; init; } = new TMediaPictures();

  public string Name {
    get {
      if (Titles.Any()) {
        return Titles.GetPrincipal()?.Value ?? "";
      } else {
        return "";
      }
    }
    set {
      if (Titles.Any()) {
        Titles.Clear();
      }
      Titles.Add(value);
    }
  }
  public ILanguageTextInfos Titles { get; } = new TLanguageTextInfos();

  public ILanguageTextInfos Descriptions { get; } = new TLanguageTextInfos();

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
    CreationDate = media.CreationDate;
    Group = media.Group;
    foreach (var MediaInfoItem in media.MediaInfos) {
      MediaInfos.Add(MediaInfoItem.Key, new TMediaInfo(MediaInfoItem.Value));
    }
    foreach (var MediaSourceItem in media.MediaSources) {
      MediaSources.Add(MediaSourceItem.Key, new TMediaSource(MediaSourceItem.Value));
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

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new();

    RetVal.AppendIndent($"- {nameof(MediaType)} = {MediaType}", indent)
          .AppendIndent($"- {nameof(Id)} = {Id.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent);

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
        RetVal.AppendIndent($"- {MediaSourceItem.Value}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(MediaSources)} is empty", indent);
    }


    if (Tags.Any()) {
      RetVal.AppendIndent($"- {nameof(Tags)}", indent);
      foreach (string TagItem in Tags) {
        RetVal.AppendIndent($"- {TagItem.WithQuotes()}", indent + 2);
      }
    } else {
      RetVal.AppendIndent($"- {nameof(Tags)} is empty", indent);
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

  #region --- IPictureContainer --------------------------------------------
  public const int MIN_PICTURE_WIDTH = 128;
  public const int MAX_PICTURE_WIDTH = 1024;
  public const int MIN_PICTURE_HEIGHT = 160;
  public const int MAX_PICTURE_HEIGHT = 1280;

  public const string DEFAULT_PICTURE_NAME = "folder.jpg";

  public static int TIMEOUT_TO_CONVERT_IN_MS = 5000;

  protected readonly Dictionary<string, IPicture> Pictures = new Dictionary<string, IPicture>();
  protected readonly ReaderWriterLockSlim _LockPictures = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  public IPicture? GetPicture(string pictureName) {
    try {
      _LockPictures.EnterReadLock();
      return Pictures[pictureName];
    } catch (Exception ex) {
      LogErrorBox($"Unable to get picture {pictureName.WithQuotes()}", ex);
      return null;
    } finally {
      _LockPictures.ExitReadLock();
    }
  }

  public IEnumerable<IPicture> GetPictures() {
    try {
      _LockPictures.EnterReadLock();
      return Pictures.Values;
    } catch (Exception ex) {
      LogErrorBox("Unable to get pictures", ex);
      return Array.Empty<IPicture>();
    } finally {
      _LockPictures.ExitReadLock();
    }
  }

  public IEnumerable<IPicture> GetPictures(EPictureType pictureType) {
    try {
      _LockPictures.EnterReadLock();
      return Pictures.Values.Where(x => x.PictureType == pictureType);
    } catch (Exception ex) {
      LogErrorBox("Unable to get pictures", ex);
      return Array.Empty<IPicture>();
    } finally {
      _LockPictures.ExitReadLock();
    }
  }

  public bool AddPicture(IPicture picture) {
    try {
      _LockPictures.EnterWriteLock();
      Pictures.Add(picture.Name, picture);
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to add picture {picture.Name.WithQuotes()}", ex);
      return false;
    } finally {
      _LockPictures.ExitWriteLock();
    }
  }

  public bool AddPicture(string pictureName, byte[] pictureContent, EPictureType pictureType = EPictureType.Unknown) {
    return AddPicture(new TPicture(pictureName, pictureContent, pictureType));
  }

  public bool DeletePicture(string pictureName) {
    try {
      _LockPictures.EnterWriteLock();
      Pictures.Remove(pictureName);
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to remove picture {pictureName.WithQuotes()}", ex);
      return false;
    } finally {
      _LockPictures.ExitWriteLock();
    }
  }

  public bool DeletePicture(IPicture picture) {
    return DeletePicture(picture.Name);
  }

  public async Task<bool> LoadPicture(IMediaSource mediaSource) {
    return await LoadPicture(mediaSource, DEFAULT_PICTURE_NAME, EPictureType.Front, MIN_PICTURE_WIDTH, MIN_PICTURE_HEIGHT).ConfigureAwait(false);
  }

  public async Task<bool> LoadPicture(IMediaSource mediaSource, string pictureName, EPictureType pictureType = EPictureType.Front, int width = MIN_PICTURE_WIDTH, int height = MIN_PICTURE_HEIGHT) {
    #region === Validate parameters ===
    string ParamPictureName = pictureName ?? DEFAULT_PICTURE_NAME;
    int ParamWidth = width.WithinLimits(MIN_PICTURE_WIDTH, MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(MIN_PICTURE_HEIGHT, MAX_PICTURE_HEIGHT);
    #endregion === Validate parameters ===

    string FullPicturePath = Path.Join(mediaSource.StorageRoot, mediaSource.StoragePath, ParamPictureName).NormalizePath();

    LogDebug($"LoadPicture {FullPicturePath} : size({ParamWidth}, {ParamHeight})");
    if (!File.Exists(FullPicturePath)) {
      LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
      return false;
    }

    try {
      using CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_TO_CONVERT_IN_MS);
      using FileStream SourceStream = new FileStream(FullPicturePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
      using MemoryStream PictureStream = new MemoryStream();
      await SourceStream.CopyToAsync(PictureStream, Timeout.Token).ConfigureAwait(false);
      PictureStream.Seek(0, SeekOrigin.Begin);
      SKImage Image = SKImage.FromEncodedData(PictureStream);
      SKBitmap Picture = SKBitmap.FromImage(Image);
      SKBitmap ResizedPicture = Picture.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
      SKData Result = ResizedPicture.Encode(SKEncodedImageFormat.Jpeg, 100);
      using (MemoryStream OutputStream = new()) {
        Result.SaveTo(OutputStream);
        return AddPicture(ParamPictureName, OutputStream.ToArray(), pictureType);
      }
    } catch (Exception ex) {
      LogErrorBox($"Unable to fetch picture {FullPicturePath.WithQuotes()}", ex);
      return false;
    }
  }
  #endregion --- IPictureContainer --------------------------------------------



}