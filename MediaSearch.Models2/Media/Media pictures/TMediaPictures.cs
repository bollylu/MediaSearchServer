using SkiaSharp;

namespace MediaSearch.Models;

public class TMediaPictures : TLanguageDictionary<IPicture>, IMediaPictures, ILoggable {

  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TMediaPictures>();

  public IPicture? Default {
    get {
      if (IsEmpty()) {
        return null;
      }
      return this.First().Value;
    }
    set {
      this.Add(value?.Language ?? ELanguage.Unknown, value ?? TPicture.Default);
    }
  }

  public TMediaPictures() { }
  public TMediaPictures(IPicture picture) {
    Add(picture.Language, picture);
  }
  public TMediaPictures(IMediaPictures mediaPictures) {
    foreach (KeyValuePair<ELanguage, IPicture> Item in mediaPictures) {
      Add(Item.Key, Item.Value);
    }
  }

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
      Logger.LogErrorBox($"Unable to get picture {pictureName.WithQuotes()}", ex);
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
      Logger.LogErrorBox("Unable to get pictures", ex);
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
      Logger.LogErrorBox("Unable to get pictures", ex);
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
      Logger.LogErrorBox($"Unable to add picture {picture.Name.WithQuotes()}", ex);
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
      Logger.LogErrorBox($"Unable to remove picture {pictureName.WithQuotes()}", ex);
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

    Logger.LogDebug($"LoadPicture {FullPicturePath} : size({ParamWidth}, {ParamHeight})");
    if (!File.Exists(FullPicturePath)) {
      Logger.LogError($"Unable to fetch picture {FullPicturePath} : File is missing or access is denied");
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
      Logger.LogErrorBox($"Unable to fetch picture {FullPicturePath.WithQuotes()}", ex);
      return false;
    }
  }


  #endregion --- IPictureContainer --------------------------------------------
}
