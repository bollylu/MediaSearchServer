namespace MediaSearch.Database;
public class TID : IID {
}

public class TID<KEYID> : IID, IID<KEYID> {
  public KEYID ID { get; set; }
  public TID(KEYID value) {
    ID = value;
  }
}
