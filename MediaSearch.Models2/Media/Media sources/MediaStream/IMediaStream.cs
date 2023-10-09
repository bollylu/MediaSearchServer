namespace MediaSearch.Models;

public interface IMediaStream {
  string Name { get; }
  int Index { get; }
  EStreamCodecType CodecType { get; }
  string CodecName { get; }
  string CodecLongName { get; }
}
