namespace MediaSearch.Storage;

public interface IStorage {

  string Name { get; set; }

  ValueTask<bool> Exists();
  ValueTask<bool> Create();
  ValueTask<bool> Remove();

  ValueTask<bool> Any();
  ValueTask<bool> IsEmpty();

  Task Clear();

}
