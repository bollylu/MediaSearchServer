using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

using BLTools.Processes;

namespace MediaSearch.Models.Support;
public partial class TFFProbe : ALoggable, IPropertiesFinder {

  public string FFPROBE { get; private set; } = string.Empty;

  private readonly string _Filename = string.Empty;

  private readonly Dictionary<string, string> MediaStreams = new Dictionary<string, string>();

  public TFFProbe(string filename) : base() {

    Logger = new TConsoleLogger<TFFProbe>() { SeverityLimit = ESeverity.DebugEx };

    _Filename = filename;

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
  private const string PROPERTY_CODEC_TYPE = "codec_type";
  private const string PROPERTY_CODEC_NAME = "codec_name";
  private const string PROPERTY_LANGUAGE = "language";
  private const string PROPERTY_TITLE = "title";
  private const string PROPERTY_CHANNELS = "channels";
  private const string PROPERTY_CHANNEL_LAYOUT = "channel_layout";
  private const string PROPERTY_SAMPLE_RATE = "sample_rate";
  private const string CODEC_TYPE_AUDIO = "audio";
  private const string CODEC_TYPE_VIDEO = "video";
  private const string CODEC_TYPE_SUBTITLE = "subtitle";

  public async Task Init() {
    string Data = await ProcessHelper.ExecuteProcessAsync(FFPROBE, $"-v panic -print_format json -show_streams -hide_banner {_Filename.WithQuotes()}");
    JsonDocument JsonData = await JsonDocument.ParseAsync(Data.ToStream());
    JsonElement JsonStreams = JsonData.RootElement.GetProperty(PROPERTY_STREAMS);
    int a = 1;
    int v = 1;
    int s = 1;
    foreach (JsonElement StreamItem in JsonStreams.EnumerateArray()) {

      IDictionary<string, string> StreamComponents = ParseStream(StreamItem);
      IDictionary<string, string> TagsComponents = ParseTags(StreamItem.EnumerateObject().First(x => x.Name == PROPERTY_TAGS).Value);
      IDictionary<string, string> DispositionComponents = ParseDisposition(StreamItem.EnumerateObject().First(x => x.Name == PROPERTY_DISPOSITION).Value);

      switch (StreamComponents[PROPERTY_CODEC_TYPE].RemoveExternalQuotes()) {
        case CODEC_TYPE_AUDIO:
          LogDebug($"Found audio stream #{a} : {StreamComponents[PROPERTY_CODEC_NAME]} : {TagsComponents[PROPERTY_LANGUAGE]}, {StreamComponents[PROPERTY_CHANNELS]} channels, {StreamComponents[PROPERTY_CHANNEL_LAYOUT]}, {StreamComponents[PROPERTY_SAMPLE_RATE]} Hz");
          MediaStreams.Add($"{CODEC_TYPE_AUDIO}#{a++}", $"{StreamItem}");
          break;
        case CODEC_TYPE_VIDEO:
          LogDebug($"Found video stream #{v} : {StreamComponents[PROPERTY_CODEC_NAME]} : {TagsComponents[PROPERTY_TITLE]}");
          MediaStreams.Add($"{CODEC_TYPE_VIDEO}#{v++}", $"{StreamItem}");
          break;
        case CODEC_TYPE_SUBTITLE:
          LogDebug($"Found subtitle stream #{s} : {StreamComponents[PROPERTY_CODEC_NAME]} : {TagsComponents[PROPERTY_LANGUAGE]}");
          MediaStreams.Add($"{CODEC_TYPE_SUBTITLE}#{s++}", $"{StreamItem}");
          break;
        default:
          LogDebugBox($"Found unknown stream", StreamItem);
          break;
      }

    }
  }

  public async Task<string> GetVersion() {

    string FullVersion = await ProcessHelper.ExecuteProcessAsync(FFPROBE, "-version");
    string? RetVal = Regex.Match(FullVersion, @"^ffprobe.version.(\d+.\d+).*$", RegexOptions.Compiled | RegexOptions.Singleline).Groups[1].Value;
    return RetVal ?? "Invalid version";

  }

  public IEnumerable<KeyValuePair<string, string>> GetVideoStreams() {
    foreach (var StreamItem in MediaStreams.Where(x => x.Key.StartsWith(CODEC_TYPE_VIDEO))) {
      yield return StreamItem;
    }
  }

  public IEnumerable<KeyValuePair<string, string>> GetAudioStreams() {
    foreach (var StreamItem in MediaStreams.Where(x => x.Key.StartsWith(CODEC_TYPE_AUDIO))) {
      yield return StreamItem;
    }
  }

  public IEnumerable<KeyValuePair<string, string>> GetSubTitleStreams() {
    foreach (var StreamItem in MediaStreams.Where(x => x.Key.StartsWith(CODEC_TYPE_SUBTITLE))) {
      yield return StreamItem;
    }
  }
}
