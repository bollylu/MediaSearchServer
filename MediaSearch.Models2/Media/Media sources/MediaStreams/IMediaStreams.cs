namespace MediaSearch.Models;
public interface IMediaStreams {
  bool Add(params IMediaStream[] mediaStreams);
  bool AddRange(IEnumerable<IMediaStream> mediaStreams);
  IMediaStream? Get(int streamId);
  IEnumerable<IMediaStream> GetAll();
  bool Remove(IMediaStream mediaStream);
  void Clear();

  bool IsEmpty();
  bool Any();
  bool Any(Predicate<IMediaStream> predicate);
}
