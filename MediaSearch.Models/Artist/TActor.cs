namespace MediaSearch.Models;
public class TActor : IArtist {
  public string Id { get; init; } = "";
  public string Name { get; set; } = "";
  public string FirstName { get; set; } = "";
  public string LastName { get; set; } = "";
  public string Alias { get; set; } = "";
  public string Alias2 { get; set; } = "";
  public string Description { get; set; } = "";

  public override string ToString() {
    return ToString(0);
  }

  public string ToString(int indent) {
    throw new NotImplementedException();
  }
}
