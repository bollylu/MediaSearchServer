namespace MediaSearch.Models;

public interface IMedia : IID<string>,
                          IStorage, 
                          ITags, 
                          ITitles, 
                          IDescriptions, 
                          IGroupMembership, 
                          IDirty, 
                          IToStringIndent,
                          ICreation,
                          IDisposable,
                          IAsyncDisposable {

  public EMediaSourceType MediaType { get; set; }

  public string Name { get; }

}

