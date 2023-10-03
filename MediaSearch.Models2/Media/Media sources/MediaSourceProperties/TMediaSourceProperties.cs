namespace MediaSearch.Models;
public class TMediaSourceProperties : IMediaSourceProperties {

  private readonly List<TMediaSourceProperty> _Properties = new List<TMediaSourceProperty>();

  public void AddProperty(TMediaSourceProperty property) {
    _Properties.Add(property);
  }

  public void AddProperties(IEnumerable<TMediaSourceProperty> property) {
    foreach (var propertyItem in property) {
      _Properties.Add(propertyItem);
    }
  }

  public IEnumerable<TMediaSourceProperty> GetProperties() {
    return _Properties;
  }
}
