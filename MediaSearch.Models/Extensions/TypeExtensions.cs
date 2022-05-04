namespace MediaSearch.Models;
public static class TypeExtensions {
  
  public static string GetGenericName(this Type type) {
    List<string> GenericArguments = new();
    foreach (Type ItemType in type.GetGenericArguments()) {
      GenericArguments.Add(ItemType.Name);
    }
    return $"{type.Name.BeforeLast('`')}<{string.Join(", ", GenericArguments)}>";
  }

}
