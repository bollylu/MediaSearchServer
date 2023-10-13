using SkiaSharp;

namespace MediaSearch.Models;

public class TMediaPictures : ALoggable, IMediaPictures {

  protected List<IMediaPicture> MediaPictures = new List<IMediaPicture>();
  private readonly ReaderWriterLockSlim _Lock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaPictures() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaPictures>();
  }
  public TMediaPictures(IMediaPicture picture) : this() {
    MediaPictures.Add(picture);
  }
  public TMediaPictures(IMediaPictures mediaPictures) : this() {
    foreach (IMediaPicture Item in mediaPictures.GetAll()) {
      MediaPictures.Add(Item);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- IPictureContainer --------------------------------------------
  public const int MIN_PICTURE_WIDTH = 128;
  public const int MAX_PICTURE_WIDTH = 1024;
  public const int MIN_PICTURE_HEIGHT = 160;
  public const int MAX_PICTURE_HEIGHT = 1280;

  public const string DEFAULT_PICTURE_NAME = "folder.jpg";

  public static int TIMEOUT_TO_CONVERT_IN_MS = 5000;

  protected readonly Dictionary<string, IMediaPicture> Pictures = new Dictionary<string, IMediaPicture>();
  protected readonly ReaderWriterLockSlim _LockPictures = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  public IMediaPicture? GetPicture(string pictureName) {
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

  public IEnumerable<IMediaPicture> GetMediaPictures() {
    try {
      _LockPictures.EnterReadLock();
      return Pictures.Values;
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to get pictures", ex);
      return Array.Empty<IMediaPicture>();
    } finally {
      _LockPictures.ExitReadLock();
    }
  }

  public IEnumerable<IMediaPicture> GetPictures(EPictureType pictureType) {
    try {
      _LockPictures.EnterReadLock();
      return Pictures.Values.Where(x => x.PictureType == pictureType);
    } catch (Exception ex) {
      Logger.LogErrorBox("Unable to get pictures", ex);
      return Array.Empty<IMediaPicture>();
    } finally {
      _LockPictures.ExitReadLock();
    }
  }

  public bool AddMediaPicture(IMediaPicture picture) {
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

  public bool AddMediaPicture(string pictureName, byte[] pictureContent, EPictureType pictureType = EPictureType.Unknown) {
    return AddMediaPicture(new TMediaPicture(pictureName, pictureContent, pictureType));
  }

  public bool RemoveMediaPicture(string pictureName) {
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

  public bool RemoveMediaPicture(IMediaPicture picture) {
    return RemoveMediaPicture(picture.Name);
  }

  public async Task<bool> LoadPicture(IMediaSourceVirtual mediaSource) {
    return await LoadPicture(mediaSource, DEFAULT_PICTURE_NAME, EPictureType.Front, MIN_PICTURE_WIDTH, MIN_PICTURE_HEIGHT).ConfigureAwait(false);
  }

  public async Task<bool> LoadPicture(IMediaSourceVirtual mediaSource, string pictureName, EPictureType pictureType = EPictureType.Front, int width = MIN_PICTURE_WIDTH, int height = MIN_PICTURE_HEIGHT) {
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
        return AddMediaPicture(ParamPictureName, OutputStream.ToArray(), pictureType);
      }
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to fetch picture {FullPicturePath.WithQuotes()}", ex);
      return false;
    }
  }


  #endregion --- IPictureContainer --------------------------------------------

  #region --- IMediaPictures --------------------------------------------
  public IMediaPicture? GetDefault() {
    throw new NotImplementedException();
  }

  public IMediaPicture? Get(ELanguage language) {
    throw new NotImplementedException();
  }

  public IEnumerable<IMediaPicture> GetAll() {
    try {
      _Lock.EnterReadLock();
      return MediaPictures.AsEnumerable();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Add(IMediaPicture mediaPicture) {
    try {
      _Lock.EnterWriteLock();
      MediaPictures.Add(mediaPicture);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaPicture", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool AddRange(IEnumerable<IMediaPicture> mediaPictures) {
    try {
      _Lock.EnterWriteLock();
      MediaPictures.AddRange(mediaPictures);
      return true;
    } catch (Exception ex) {
      LogErrorBox("Unable to add MediaPictures", ex);
      return false;
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public bool Remove(IMediaPicture mediaPicture) {
    throw new NotImplementedException();
  }

  public void Clear() {
    try {
      _Lock.EnterWriteLock();
      MediaPictures.Clear();
    } finally {
      _Lock.ExitWriteLock();
    }
  }

  public void SetDefault(IMediaPicture mediaPicture) {
    throw new NotImplementedException();
  }

  public bool Any() {
    try {
      _Lock.EnterReadLock();
      return MediaPictures.Any();
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool Any(Func<IMediaPicture, bool> predicate) {
    try {
      _Lock.EnterReadLock();
      return MediaPictures.Any(predicate);
    } finally {
      _Lock.ExitReadLock();
    }
  }

  public bool IsEmpty() => !Any();
  #endregion --- IMediaPictures --------------------------------------------


}
