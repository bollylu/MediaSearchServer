namespace MediaSearch.Models;

public interface IID { }

public interface IID<T> : IID where T : notnull {

  T ID { get; }

}
