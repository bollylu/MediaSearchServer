namespace MovieSearchModels;

public record RFilter {
  public string Name { get; init; }
  public DateOnly AddedAfter { get; init; }

  public static RFilter Empty => new RFilter();
}

