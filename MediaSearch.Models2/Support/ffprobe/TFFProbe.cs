using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using BLTools.Processes;

using MediaSearch.Models.Support.ffprobe;

namespace MediaSearch.Models.Support;
public partial class TFFProbe : ALoggable, IPropertiesFinder {

  public string FFPROBE { get; private set; } = string.Empty;

  public string Filename { get; private set; } = string.Empty;

  public TMediaProperties MediaProperties { get; } = new();

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

  public async Task Init() {
    string Data = await ProcessHelper.ExecuteProcessAsync(FFPROBE, $"-v panic -print_format json -show_streams -hide_banner {Filename.WithQuotes()}");
    JsonDocument JsonData = await JsonDocument.ParseAsync(Data.ToStream());
    JsonElement JsonStreams = JsonData.RootElement.GetProperty(PROPERTY_STREAMS);

    foreach (JsonElement StreamItem in JsonStreams.EnumerateArray()) {

      try {
        Dictionary<string, string> StreamComponents = ParseStream(StreamItem);
        Dictionary<string, string> TagsComponents = new();
        Dictionary<string, string> DispositionComponents = new();

        if (StreamItem.EnumerateObject().Any(x => x.Name == PROPERTY_TAGS)) {
          TagsComponents = ParseTags(StreamItem.EnumerateObject().First(x => x.Name == PROPERTY_TAGS).Value);
        }

        if (StreamItem.EnumerateObject().Any(x => x.Name == PROPERTY_DISPOSITION)) {
          DispositionComponents = ParseDisposition(StreamItem.EnumerateObject().First(x => x.Name == PROPERTY_DISPOSITION).Value);
        }

        switch (StreamComponents[PROPERTY_CODEC_TYPE].RemoveExternalQuotes()) {

          case CODEC_TYPE_AUDIO: {
              TStreamAudioProperties Properties = new TStreamAudioProperties(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              Properties.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              Properties.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              Properties.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              Properties.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              Properties.Channels = StreamComponents.SafeGetValue(PROPERTY_CHANNELS, 0);
              Properties.ChannelsLayout = StreamComponents.SafeGetValue(PROPERTY_CHANNEL_LAYOUT, string.Empty) ?? string.Empty;
              Properties.SampleRate = StreamComponents.SafeGetValue(PROPERTY_SAMPLE_RATE, 0);
              Properties.BitRate = StreamComponents.SafeGetValue(PROPERTY_BITRATE, 0);
              Properties.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              Properties.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaProperties.StreamProperties.Add(Properties);
              LogDebug($"{Path.GetFileName(Filename)} => Found audio stream #{Properties.Index} : {Properties.CodecName} : {Properties.Language}, {Properties.Channels} channels, {Properties.ChannelsLayout}, {Properties.SampleRate} Hz, {Properties.BitRate} bps{(Properties.Default ? " [Default]" : "")}{(Properties.Forced ? " [forced]" : "")}");
              break;
            }

          case CODEC_TYPE_VIDEO: {
              TStreamVideoProperties Properties = new TStreamVideoProperties(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              Properties.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              Properties.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              Properties.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              Properties.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              Properties.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              Properties.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              Properties.Width = StreamComponents.SafeGetValue(PROPERTY_WIDTH, 0);
              Properties.Height = StreamComponents.SafeGetValue(PROPERTY_HEIGHT, 0);
              MediaProperties.StreamProperties.Add(Properties);
              LogDebug($"{Path.GetFileName(Filename)} => Found video stream #{Properties.Index} : {Properties.CodecName} ({Properties.Width}x{Properties.Height})");
              break;
            }

          case CODEC_TYPE_SUBTITLE: {
              TStreamSubripProperties Properties = new TStreamSubripProperties(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              Properties.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              Properties.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              Properties.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              Properties.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              Properties.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              Properties.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaProperties.StreamProperties.Add(Properties);
              LogDebug($"{Path.GetFileName(Filename)} => Found subtitle stream #{Properties.Index} : {Properties.CodecName} : {Properties.Language}{(Properties.Default ? " [Default]" : "")}{(Properties.Forced ? " [forced]" : "")}");
              break;
            }

          case CODEC_TYPE_DATA: {
              TStreamDataProperties Properties = new TStreamDataProperties(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              Properties.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              Properties.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              Properties.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              Properties.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaProperties.StreamProperties.Add(Properties);
              LogDebug($"{Path.GetFileName(Filename)} => Found data stream #{Properties.Index} : {Properties.CodecName}, {Properties.CodecLongName.WithQuotes()}{(Properties.Default ? " [Default]" : "")}{(Properties.Forced ? " [forced]" : "")}");
              break;
            }

          default: {
              TStreamGenericProperties Properties = new TStreamGenericProperties(StreamComponents.SafeGetValue(PROPERTY_INDEX, 0));
              Properties.CodecTypeUnknown = StreamComponents.SafeGetValue(PROPERTY_CODEC_TYPE, MISSING_CODEC_TYPE) ?? MISSING_CODEC_TYPE;
              Properties.CodecName = StreamComponents.SafeGetValue(PROPERTY_CODEC_NAME, string.Empty) ?? string.Empty;
              Properties.CodecLongName = StreamComponents.SafeGetValue(PROPERTY_CODEC_LONG_NAME, string.Empty) ?? string.Empty;
              Properties.Language = TagsComponents.SafeGetValue(PROPERTY_LANGUAGE, MISSING_LANGUAGE) ?? MISSING_LANGUAGE;
              Properties.Title = TagsComponents.SafeGetValue(PROPERTY_TITLE, string.Empty) ?? string.Empty;
              Properties.Default = DispositionComponents.SafeGetValue(PROPERTY_DEFAULT, false);
              Properties.Forced = DispositionComponents.SafeGetValue(PROPERTY_FORCED, false);
              MediaProperties.StreamProperties.Add(Properties);
              LogDebug($"{Path.GetFileName(Filename)} => Found unknown stream #{Properties.Index} : {Properties.CodecTypeUnknown} : {Properties.CodecName.WithQuotes()}, {Properties.CodecLongName.WithQuotes()}");
              break;
            }
        }

      } catch (Exception ex) {
        LogErrorBox("Error reading stream", ex);
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
