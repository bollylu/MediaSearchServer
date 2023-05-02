﻿using System.Diagnostics;

using SkiaSharp;

namespace MediaSearch.Models;
public class TPicture : ALoggable, IPicture {

  public const int MIN_PICTURE_WIDTH = 128;
  public const int MAX_PICTURE_WIDTH = 1024;
  public const int MIN_PICTURE_HEIGHT = 160;
  public const int MAX_PICTURE_HEIGHT = 1280;

  public const string DEFAULT_PICTURE_NAME = "folder.jpg";

  public static int DEFAULT_PICTURE_WIDTH = MIN_PICTURE_WIDTH;
  public static int DEFAULT_PICTURE_HEIGHT = MIN_PICTURE_HEIGHT;

  public static int TIMEOUT_TO_CONVERT_IN_MS = 5000;

  public string Name { get; init; } = "";
  public byte[] Data { get; set; } = Array.Empty<byte>();
  public EPictureType PictureType { get; init; } = EPictureType.Unknown;
  public ELanguage Language { get; init; } = ELanguage.Unknown;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TPicture() { }

  public TPicture(string name, byte[] data, EPictureType pictureType = EPictureType.Unknown, ELanguage language = ELanguage.Unknown) {
    Name = name;
    Data = data;
    PictureType = pictureType;
    Language = language;
  }

  public TPicture(IPicture picture) {
    Name = picture.Name;
    Data = picture.Data;
    PictureType = picture.PictureType;
    Language = picture.Language;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Name)} = {Name}");
    RetVal.AppendLine($"{nameof(PictureType)} = {PictureType}");
    RetVal.AppendLine($"{nameof(Data)} = {Data.Length} byte(s)");
    return RetVal.ToString();
  }

  public async Task<bool> LoadAsync(string location, bool withResize = true) {
    try {
      Data = await File.ReadAllBytesAsync(location);
      if (withResize) {
        IPicture? Resized = Resize(DEFAULT_PICTURE_WIDTH, DEFAULT_PICTURE_HEIGHT);
        if (Resized is not null) {
          Data = Resized.Data;
        }
      }
      return true;
    } catch (Exception ex) {
      LogErrorBox($"Unable to load picture from {location.WithQuotes()}", ex);
      return false;
    }
  }

  public IPicture? Resize(int width, int height) {
    int ParamWidth = width.WithinLimits(MIN_PICTURE_WIDTH, MAX_PICTURE_WIDTH);
    int ParamHeight = height.WithinLimits(MIN_PICTURE_HEIGHT, MAX_PICTURE_HEIGHT);

    try {
      using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_TO_CONVERT_IN_MS)) {
        using (MemoryStream PictureStream = new MemoryStream(Data)) {
          PictureStream.Seek(0, SeekOrigin.Begin);
          SKImage Image = SKImage.FromEncodedData(PictureStream);
          SKBitmap Picture = SKBitmap.FromImage(Image);
          SKBitmap ResizedPicture = Picture.Resize(new SKImageInfo(ParamWidth, ParamHeight), SKFilterQuality.High);
          SKData Result = ResizedPicture.Encode(SKEncodedImageFormat.Jpeg, 100);
          using (MemoryStream OutputStream = new()) {
            Result.SaveTo(OutputStream);
            return new TPicture(Name, OutputStream.ToArray(), PictureType, Language);
          }
        }
      }
    } catch (Exception ex) {
      LogErrorBox($"Unable to resize picture {Name.WithQuotes()} to {width}x{height}", ex);
      return null;
    }
  }

  public static IPicture Default {
    get {
      return new TPicture();
    }
  }

  public static IPicture? GetPictureFromAssembly(string pictureName, string pictureExtension = ".png") {
    try {
      Assembly Asm = Assembly.GetExecutingAssembly();
      string CompleteName = $"MediaSearch.Models.Pictures.{pictureName}{pictureExtension}";
      string[] Resources = Asm.GetManifestResourceNames();
      foreach (string ResourceItem in Resources) {
        Console.WriteLine(ResourceItem);
      }
      using (Stream? ResourceStream = Asm.GetManifestResourceStream(CompleteName)) {
        if (ResourceStream is null) {
          return null;
        }
        using (BinaryReader reader = new BinaryReader(ResourceStream)) {
          return new TPicture(pictureName, reader.ReadBytes((int)reader.BaseStream.Length));
        }
      }
    } catch (Exception ex) {
      Trace.WriteLine($"Unable to get picture {pictureName}{pictureExtension} : {ex.Message}");
      return null;
    }
  }

  public static IPicture PictureMissing {
    get {
      return _PictureMissing ??= GetPictureFromAssembly("missing", ".jpg") ?? new TPicture("Picture not found (missing.jpg)", Array.Empty<byte>());
    }
  }

  private static IPicture? _PictureMissing;
}
