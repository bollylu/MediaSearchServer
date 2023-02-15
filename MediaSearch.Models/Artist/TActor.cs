namespace MediaSearch.Models;
public class TActor : IArtist {
  public string ID { get; init; } = "";
  public string Name { get; set; } = "";
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
  public string Alias { get; set; } = "";
  public string Description { get; set; } = "";
}
