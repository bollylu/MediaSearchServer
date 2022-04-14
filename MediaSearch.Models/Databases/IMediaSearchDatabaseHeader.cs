namespace MediaSearch.Models;
public interface IMediaSearchDatabaseHeader : IName {

  public new string Name { get; set; }
  public new string Description { get; set; }
  public DateTime LastUpdate { get; set; }

  string ToString(int indent);
}
