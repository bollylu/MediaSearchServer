using static BLTools.Diagnostic.TraceInfo;

namespace BuildMoviesJson;
public class TCounter {

  private int Value = 0;

  private readonly object _Lock = new object();

  public void Increment() {
    lock (_Lock) {
      Value++;
      if (Value % 100 == 0) {
        Message($"{Value} ...");
      }
    }
  }

}
