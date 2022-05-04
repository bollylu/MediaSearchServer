namespace MediaSearch.Models.Databases;
public interface IMSTableJson<T, R> where T : IMSTable<R> where R : IMedia {

  public bool Save(T table);
  public bool Save(R record);

}
