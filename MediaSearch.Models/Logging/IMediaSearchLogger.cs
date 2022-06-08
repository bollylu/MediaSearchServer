
using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;

/// <summary>
/// Extension of ILogger. Allows more logging methods
/// </summary>
public interface IMediaSearchLogger : ILogger, IName {

  new string Name { get; set; }

}

/// <summary>
/// Extension of ILogger. Allows more logging methods and fills the source with the type and the method of the caller
/// </summary>
/// <typeparam name="T">The type of the source</typeparam>
public interface IMediaSearchLogger<T> : IMediaSearchLogger, ILogger<T>, ICloneable {



}
