namespace MediaSearch.Models;

public interface IMedia : IRecord,
                          IStorage, 
                          ITags, 
                          ITitles, 
                          IDescriptions, 
                          IGroupMembership, 
                          IDirty, 
                          ICreation,
                          IDisposable,
                          IAsyncDisposable {

  public EMediaSourceType MediaType { get; set; }

  public string Name { get; }

}

