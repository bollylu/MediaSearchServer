namespace MediaSearch.Models;
public interface IMediaSourceProperties {

  void AddProperty(TMediaSourceProperty property);
  void AddProperties(IEnumerable<TMediaSourceProperty> property);

  IEnumerable<TMediaSourceProperty> GetProperties();
}
