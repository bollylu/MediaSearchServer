namespace MediaSearch.Models;

public interface IDuplicate<T> where T : class, new() {
  public T Duplicate();
}
