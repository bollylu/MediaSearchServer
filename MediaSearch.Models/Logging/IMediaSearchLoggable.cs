using BLTools.Text;

namespace MediaSearch.Models.Logging;
public interface IMediaSearchLoggable {

  IMediaSearchLogger Logger { get; }

}

public interface IMediaSearchLoggable<T> {

  IMediaSearchLogger<T> Logger { get; }

}
