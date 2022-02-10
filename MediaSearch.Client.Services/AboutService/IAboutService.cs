namespace MediaSearch.Client.Services;

public interface IAboutService {
  Task<TAbout?> GetAboutAsync(string name);

}
