﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaSearch.Database.Test;
[TestClass]
public class ContextTests {

  [TestMethod]
  public void Instanciate_TContext_Empty() {
    IMSDatabase Source = new TMSDatabaseJson();

    IContext<IMSRecord> Target = new TContext<IMSRecord>(Source, "movies");
    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void Instanciate_TContextOperation_Get() {
    IContextOperation Target = new TContextGet<IMSRecord>("1234567890");

    TraceMessage($"{nameof(Target)} : {Target.GetType().GetGenericName()}", Target);

    Assert.IsNotNull(Target);
  }
}