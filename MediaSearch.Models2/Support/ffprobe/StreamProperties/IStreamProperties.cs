using MediaSearch.Models.Support.ffprobe;

namespace MediaSearch.Models.Support.ffprobe;
public interface IStreamProperties {
  string Name { get; }
  int Index { get; }
  EStreamCodecType CodecType { get; }
  string CodecName { get; }
  string CodecLongName { get; }
}
