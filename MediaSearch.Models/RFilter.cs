namespace MediaSearch.Models;

public record RFilter {

  public string Name { get; set; }
  public int DaysBack { get; set; }

  public static RFilter Empty => new RFilter();

  public string ToUrl() {
    return $"filtername={Name.ToUrl()}&daysback={DaysBack}";
  }

  public RFilter ParseUrl(string url) {
    ISplitArgs Args = new SplitArgs();
    Args.Parse(url.Replace('&',' '));
    Name = Args.GetValue(nameof(Name), "").FromUrl();
    DaysBack = Args.GetValue(nameof(DaysBack), 0);
    return this;
  }

  public static RFilter FromUrl(string url) {
    RFilter RetVal = new RFilter();
    RetVal.ParseUrl(url);
    return RetVal;
  }
}

