using BLTools.Encryption;
using BLTools.Json;

using SkiaSharp;

namespace MediaSearch.Models;

public abstract class AMedia : ALoggable, IMedia {

  public static ELanguage DEFAULT_LANGUAGE = ELanguage.French;

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

  public EMediaType MediaType { get; set; }

  #region --- IName --------------------------------------------
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
  public DateOnly DateAdded { get; set; } = DateOnly.FromDateTime(DateTime.Today);

  public DateOnly CreationDate { get; set; } = new DateOnly();
  public int CreationYear {
    get {
      return CreationDate.Year;
    }
  }

  public long Size { get; set; }

  public string Group { get; set; } = "";
  public bool IsGroupMember => !string.IsNullOrWhiteSpace(Group);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMedia() {
    Logger = GlobalSettings.GlobalLogger;
  }

  protected AMedia(IMedia media) : this() {
    MediaType = media.MediaType;
    Id = media.Id;
    //StorageRoot = media.StorageRoot;
    //StoragePath = media.StoragePath;
    //FileName = media.FileName;
    //FileExtension = media.FileExtension;
    CreationDate = media.CreationDate;
    //Size = media.Size;
    //DateAdded = media.DateAdded;
    Group = media.Group;

    foreach (ILanguageTextInfo TitleItem in media.Titles.GetAll()) {
      Titles.Add(TitleItem);
    }

    foreach (ILanguageTextInfo DescriptionItem in media.Descriptions.GetAll()) {
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

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new();

    RetVal.AppendIndent($"- {nameof(MediaType)} = {MediaType}", indent)
          .AppendIndent($"- {nameof(Id)} = {Id.WithQuotes()}", indent)
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
    RetVal.AppendIndent($"- {nameof(Size)} = {Size} bytes", indent)
          .AppendIndent($"- {nameof(CreationYear)} = {CreationYear}", indent);
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

  public async Task<bool> LoadPicture() {
    return await LoadPicture(DEFAULT_PICTURE_NAME, EPictureType.Front, MIN_PICTURE_WIDTH, MIN_PICTURE_HEIGHT).ConfigureAwait(false);
  }

  public async Task<bool> LoadPicture(string pictureName, EPictureType pictureType = EPictureType.Front, int width = MIN_PICTURE_WIDTH, int height = MIN_PICTURE_HEIGHT) {
    #region === Validate parameters ===
    string ParamPictureName = pictureName ?? DEFAULT_PICTURE_NAME;
    int ParamWidth = width.WithinLimits(MIN_PICTURE_WIDTH, MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(MIN_PICTURE_HEIGHT, MAX_PICTURE_HEIGHT);
    #endregion === Validate parameters ===

    string FullPicturePath = Path.Join(StorageRoot, StoragePath, ParamPictureName).NormalizePath();

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