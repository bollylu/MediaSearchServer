namespace MediaSearch.Models;

public interface IID { }

public interface IID<T> : IID  {
  T ID { get; }
}
