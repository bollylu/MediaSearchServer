namespace MediaSearch.Database;
public class TID : IID {
}

public class TID<T> : IID, IID<T> where T : notnull {
  public T ID { get; set; }
  public TID(T value) {
    ID = value;
  }
}
