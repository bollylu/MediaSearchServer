using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using BLTools.Processes;

namespace MediaSearch.Models.Support;
public partial class TFFProbe : ALoggable, IMediaSourceStreamsFinder {

  public string FFPROBE { get; private set; } = string.Empty;

  public string Filename { get; private set; } = string.Empty;

  public TMediaStreams MediaSourceStreams { get; } = new TMediaStreams();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TFFProbe(string filename) : base() {

    Logger = new TConsoleLogger<TFFProbe>() { SeverityLimit = ESeverity.Debug };

    Filename = filename;

    if (OperatingSystem.IsWindows()) {
      FFPROBE = @".\ExternalResources\Windows\ffprobe\ffprobe.exe";
    } else if (OperatingSystem.IsLinux()) {
      FFPROBE = @".\ExternalResources\Linux\ffprobe\ffprobe";
    } else {
      throw new ApplicationException("Unable to use FFPROBE : the process is missing");
    }

    if (!File.Exists(FFPROBE)) {
      throw new ApplicationException($"{FFPROBE} does not exist");
    }

    LogDebugEx(FFPROBE);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Constant definitions for properties --------------------------------------------
  private const string PROPERTY_STREAMS = "streams";
  private const string PROPERTY_TAGS = "tags";
  private const string PROPERTY_DISPOSITION = "disposition";
  private const string PROPERTY_INDEX = "index";
  private const string PROPERTY_CODEC_TYPE = "codec_type";
  private const string PROPERTY_CODEC_NAME = "codec_name";
  private const string PROPERTY_CODEC_LONG_NAME = "codec_long_name";
  private const string PROPERTY_LANGUAGE = "language";
  private const string PROPERTY_CHANNELS = "channels";
  private const string PROPERTY_CHANNEL_LAYOUT = "channel_layout";
  private const string PROPERTY_SAMPLE_RATE = "sample_rate";
  private const string PROPERTY_BITRATE = "bit_rate";
  private const string PROPERTY_TITLE = "title";
  private const string PROPERTY_DEFAULT = "default";
  private const string PROPERTY_FORCED = "force";
  private const string PROPERTY_WIDTH = "width";
  private const string PROPERTY_HEIGHT = "height";

  private const string CODEC_TYPE_AUDIO = "audio";
  private const string CODEC_TYPE_VIDEO = "video";
  private const string CODEC_TYPE_SUBTITLE = "subtitle";
  private const string CODEC_TYPE_DATA = "data";

  private const string MISSING_LANGUAGE = "(missing language)";
  private const string MISSING_CODEC_TYPE = "(missing codec type)";
  #endregion --- Constant definitions for properties -----------------------------------------

  public async Task Init() {
    string Data = await ProcessHelper.ExecuteProcessAsync(FFPROBE, $"-v panic -print_format json -show_streams -hide_banner {Filename.WithQuotes()}");
    JsonDocument JsonData = await JsonDocument.ParseAsync(Data.ToStream());
    JsonElement JsonStreams = JsonData.RootElement.GetProperty(PROPERTY_STREAMS);

    foreach (JsonElement JsonStreamItem in JsonStreams.EnumerateArray()) {

      try {
        Dictionary<string, string> StreamComponents = ParseStream(JsonStreamItem);
        Dictionary<string, string> TagsComponents = new();
        Dictionary<string, string> DispositionComponents = new();

        if (JsonStreamItem.EnumerateObject().Any(x => x.Name == PROPERTY_TAGS)) {
          TagsComponents = ParseTags(JsonStreamItem.EnumerateObject().First(x => x.Name == PROPERTY_TAGS).Value);
        }

        if (JsonStreamItem.EnumerateObject().Any(x => x.Name == PROPERTY_DISPOSITION)) {
          DispositionComponents = ParseDisposition(JsonStreamItem.EnumerateObject().First(x => x.Name == PROPERTY_DISPOSITION).Value);
        }

        switch (StreamComponents[PROPERTY_CODEC_TYPE].RemoveExternalQuotes()) {

          case CODEC_TYPE_AUDIO: {
              TMediaStreamAudio MediaStreamAudio = new TMediaStreamAudio(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              MediaStreamAudio.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              MediaStreamAudio.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              MediaStreamAudio.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              MediaStreamAudio.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              MediaStreamAudio.Channels = StreamComponents.SafeGetValue(PROPERTY_CHANNELS, 0);
              MediaStreamAudio.ChannelsLayout = StreamComponents.SafeGetValue(PROPERTY_CHANNEL_LAYOUT, string.Empty) ?? string.Empty;
              MediaStreamAudio.SampleRate = StreamComponents.SafeGetValue(PROPERTY_SAMPLE_RATE, 0);
              MediaStreamAudio.BitRate = StreamComponents.SafeGetValue(PROPERTY_BITRATE, 0);
              MediaStreamAudio.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              MediaStreamAudio.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaSourceStreams.Add(MediaStreamAudio);
              LogDebug($"{Path.GetFileName(Filename)} => Found stream #{MediaStreamAudio.Index} ({MediaStreamAudio.CodecType}) : {MediaStreamAudio.CodecName} : {MediaStreamAudio.Language}, {MediaStreamAudio.Channels} channels, {MediaStreamAudio.ChannelsLayout}, {MediaStreamAudio.SampleRate} Hz, {MediaStreamAudio.BitRate} bps{(MediaStreamAudio.Default ? " [Default]" : "")}{(MediaStreamAudio.Forced ? " [forced]" : "")}");
              break;
            }

          case CODEC_TYPE_VIDEO: {
              TMediaStreamVideo MediaStreamVideo = new TMediaStreamVideo(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              MediaStreamVideo.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              MediaStreamVideo.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              MediaStreamVideo.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              MediaStreamVideo.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              MediaStreamVideo.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              MediaStreamVideo.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaStreamVideo.Width = StreamComponents.SafeGetValue(PROPERTY_WIDTH, 0);
              MediaStreamVideo.Height = StreamComponents.SafeGetValue(PROPERTY_HEIGHT, 0);
              MediaSourceStreams.Add(MediaStreamVideo);
              LogDebug($"{Path.GetFileName(Filename)} => Found video stream #{MediaStreamVideo.Index} : {MediaStreamVideo.CodecName} ({MediaStreamVideo.Width}x{MediaStreamVideo.Height})");
              break;
            }

          case CODEC_TYPE_SUBTITLE: {
              TMediaStreamSubTitle MediaStreamSubTitle = new TMediaStreamSubTitle(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              MediaStreamSubTitle.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              MediaStreamSubTitle.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              MediaStreamSubTitle.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              MediaStreamSubTitle.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              MediaStreamSubTitle.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              MediaStreamSubTitle.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaSourceStreams.Add(MediaStreamSubTitle);
              LogDebug($"{Path.GetFileName(Filename)} => Found stream #{MediaStreamSubTitle.Index} ({MediaStreamSubTitle.CodecType}) : {MediaStreamSubTitle.CodecName} : {MediaStreamSubTitle.Language}{(MediaStreamSubTitle.Default ? " [Default]" : "")}{(MediaStreamSubTitle.Forced ? " [forced]" : "")}");
              break;
            }

          case CODEC_TYPE_DATA: {
              TMediaStreamData MediaStreamData = new TMediaStreamData(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              MediaStreamData.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              MediaStreamData.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              MediaStreamData.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              MediaStreamData.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaSourceStreams.Add(MediaStreamData);
              LogDebug($"{Path.GetFileName(Filename)} => Found stream #{MediaStreamData.Index} ({MediaStreamData.CodecType}) : {MediaStreamData.CodecName}, {MediaStreamData.CodecLongName.WithQuotes()}{(MediaStreamData.Default ? " [Default]" : "")}{(MediaStreamData.Forced ? " [forced]" : "")}");
              break;
            }

          default: {
              TMediaStreamUnknown MediaStreamUnknown = new TMediaStreamUnknown(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              MediaStreamUnknown.CodecTypeUnknown = StreamComponents.SafeGetValue(PROPERTY_CODEC_TYPE, MISSING_CODEC_TYPE) ?? MISSING_CODEC_TYPE;
              MediaStreamUnknown.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              MediaStreamUnknown.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              MediaStreamUnknown.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              MediaStreamUnknown.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              MediaStreamUnknown.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              MediaStreamUnknown.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaSourceStreams.Add(MediaStreamUnknown);
              LogDebug($"{Path.GetFileName(Filename)} => Found stream #{MediaStreamUnknown.Index} ({MediaStreamUnknown.CodecType}) : {MediaStreamUnknown.CodecName.WithQuotes()}, {MediaStreamUnknown.CodecLongName.WithQuotes()}");
              break;
            }
        }

      } catch (Exception ex) {
        LogErrorBox("Error reading media stream", ex);
      }
    }
  }


  /**********************************************************************************************************************************/

  public async Task<string> GetVersion() {

    string FullVersion = await ProcessHelper.ExecuteProcessAsync(FFPROBE, "-version");
    string? RetVal = Regex.Match(FullVersion, @"^ffprobe.version.(\d+.\d+).*$", RegexOptions.Compiled | RegexOptions.Singleline).Groups[1].Value;
    return RetVal ?? "Invalid version";

  }

}
