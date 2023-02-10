

using Microsoft.Extensions.Logging;

namespace MediaSearch.Storage;

public class TMovieContext : DbContext {

  public string DbFullName { get; private set; } = "";

  public TMovieContext(string dbFullName) : base() {
    DbFullName = dbFullName;
  }


  public override string ToString() {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendLine($"{nameof(Database.ProviderName)} = {Database.ProviderName}");
    RetVal.AppendLine($"{nameof(DbFullName)} = {DbFullName.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Movies)} = {Movies?.EntityType}");
    return RetVal.ToString();
  }

  protected override void OnConfiguring(DbContextOptionsBuilder options) {
    options.UseSqlite($"Data Source={DbFullName};")
           .EnableDetailedErrors()
           .LogTo(Console.WriteLine, LogLevel.Information)
           ;
  }

  public DbSet<TMovie> Movies => Set<TMovie>();

}
