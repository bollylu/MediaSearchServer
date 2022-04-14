namespace MediaSearch.Models;

public interface IMedia : IStorage, 
                          ITags, 
                          ITitles, 
                          IDescriptions, 
                          IGroupMembership, 
                          IDirty, 
                          IToStringIndent,
                          ICreation {

  public string Name { get; }

  /// <summary>
  /// Identifier for this item
  /// </summary>
  string Id { get; }
  
}

