namespace MediaSearch.Client.Services;
public class TBusService<T> : IBusService<T> {

  public event Action<string, T>? OnMessage;

  public void SendMessage(string source, T message) {
    OnMessage?.Invoke(source, message);
  }
}
