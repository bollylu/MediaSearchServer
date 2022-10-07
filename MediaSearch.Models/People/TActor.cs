namespace MediaSearch.Models;
public class TActor : IPeople, IRecord {

  public string ID { get; set; }

  public string FirstName { get; set; }
  public string LastName { get; set; }
  public string Alias1 { get; set; }
  public string Alias2 { get; set; }

  public string ToString(int indent) {
    throw new NotImplementedException();
  }


}
