namespace MediaSearch.Storage;

public class TActorsContext : DbContext {

  public DbSet<TActor> Actors => Set<TActor>();

}
