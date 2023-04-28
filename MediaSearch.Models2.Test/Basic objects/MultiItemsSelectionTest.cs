using MediaSearch.Models2.Support.Filter;

namespace MediaSearch.Models2.Test;
[TestClass]
public class MultiItemsSelectionTest {
  [TestMethod]
  public void Instanciate_Empty_MultiItemSelection() {

    Message("Instanciate MultiItemSelection");
    IMultiItemsSelection Target = new TMultiItemsSelection();
    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Instanciate_Any_MultiItemSelection() {
    const string FIRST = "first";
    const string SECOND = "second";
    const string NOTINHERE = "notinhere";

    Message("Instanciate MultiItemSelection");
    IMultiItemsSelection Target = new TMultiItemsSelection(EFilterType.Any, FIRST, SECOND);
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.AreEqual(EFilterType.Any, Target.Selection);
    Message($"Verify existence of {FIRST.WithQuotes()}");
    Assert.IsTrue(Target.Items.Contains(FIRST));
    Message($"Verify existence of {SECOND.WithQuotes()}");
    Assert.IsTrue(Target.Items.Contains(SECOND));
    Message($"Verify non existence of {NOTINHERE.WithQuotes()}");
    Assert.IsFalse(Target.Items.Contains(NOTINHERE));

    Ok();
  }

  [TestMethod]
  public void Test_AllMatch() {
    const string FIRST = "Indiana";
    const string SECOND = "Alliance";
    const string TITLE = "Indiana Jones et l'arche d'alliance";
    const string TITLE2 = "Indiana Jones et l'arche d'alliace";
    const string TITLE3 = "Indiaa Jones et l'arche d'alliace";
    const string TITLE4 = "Indiaa Jones et l'arche d'alliance";

    Message("Instanciate MultiItemSelection");
    IMultiItemsSelection Target = new TMultiItemsSelection(EFilterType.All, FIRST, SECOND);
    Assert.IsNotNull(Target);
    Dump(Target);

    Message($"Test match against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target.IsMatch(TITLE));

    Message($"Test not match against {TITLE2.WithQuotes()}");
    Assert.IsFalse(Target.IsMatch(TITLE2));

    Message($"Test not match against {TITLE3.WithQuotes()}");
    Assert.IsFalse(Target.IsMatch(TITLE3));

    Message($"Test not match against {TITLE4.WithQuotes()}");
    Assert.IsFalse(Target.IsMatch(TITLE4));

    Ok();
  }

  [TestMethod]
  public void Test_AnyMatch() {
    const string FIRST = "Indiana";
    const string SECOND = "Alliance";
    const string TITLE = "Indiana Jones et l'arche d'alliance";
    const string TITLE2 = "Indiana Jones et l'arche d'alliace";
    const string TITLE3 = "Indiaa Jones et l'arche d'alliace";
    const string TITLE4 = "Indiaa Jones et l'arche d'alliance";

    Message("Instanciate MultiItemSelection");
    IMultiItemsSelection Target = new TMultiItemsSelection(EFilterType.Any, FIRST, SECOND);
    Assert.IsNotNull(Target);
    Dump(Target);

    Message($"Test match against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target.IsMatch(TITLE));

    Message($"Test match against {TITLE2.WithQuotes()}");
    Assert.IsTrue(Target.IsMatch(TITLE2));

    Message($"Test not match against {TITLE3.WithQuotes()}");
    Assert.IsFalse(Target.IsMatch(TITLE3));

    Message($"Test match against {TITLE4.WithQuotes()}");
    Assert.IsTrue(Target.IsMatch(TITLE4));

    Ok();
  }

  [TestMethod]
  public void Test_Diacritics() {
    const string FIRST = "Ecole";
    const string SECOND = "école";
    const string THRID = "foret";
    const string FOURTH = "a l'ècolé";
    const string FIFTH = "ö l'îcolù";
    const string TITLE = "Séverine va à l'école dans la forêt";

    Message("Instanciate MultiItemSelection");
    IMultiItemsSelection Target1 = new TMultiItemsSelection(EFilterType.Any, FIRST);
    IMultiItemsSelection Target2 = new TMultiItemsSelection(EFilterType.Any, SECOND);
    IMultiItemsSelection Target3 = new TMultiItemsSelection(EFilterType.Any, THRID);
    IMultiItemsSelection Target4 = new TMultiItemsSelection(EFilterType.Any, FOURTH);
    IMultiItemsSelection Target5 = new TMultiItemsSelection(EFilterType.Any, FIFTH);
    Assert.IsNotNull(Target1);
    Assert.IsNotNull(Target2);
    Assert.IsNotNull(Target3);
    Assert.IsNotNull(Target4);
    Assert.IsNotNull(Target5);
    Dump(Target1);
    Dump(Target2);
    Dump(Target3);
    Dump(Target4);
    Dump(Target5);

    Message($"Test match {nameof(Target1)} against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target1.IsMatch(TITLE));

    Message($"Test match {nameof(Target2)} against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target2.IsMatch(TITLE));

    Message($"Test match {nameof(Target3)} against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target3.IsMatch(TITLE));

    Message($"Test match {nameof(Target4)} against {TITLE.WithQuotes()}");
    Assert.IsTrue(Target4.IsMatch(TITLE));

    Message($"Test not match {nameof(Target5)} against {TITLE.WithQuotes()}");
    Assert.IsFalse(Target5.IsMatch(TITLE));

    Ok();
  }
}
