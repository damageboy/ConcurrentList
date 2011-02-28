// <copyright file="ConcurrentListPexTests.Contains.g.cs" company="Microsoft">Copyright � Microsoft 2011</copyright>
// <auto-generated>
// This file contains automatically generated unit tests.
// Do NOT modify this file manually.
// 
// When Pex is invoked again,
// it might remove or update any previously generated unit tests.
// 
// If the contents of this file becomes outdated, e.g. if it does not
// compile anymore, you may delete this file and invoke Pex again.
// </auto-generated>
using Microsoft.Pex.Framework;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace ConcurrentList.UnitTests
{
  public partial class ConcurrentListPexTests
  {
[Test]
[PexGeneratedBy(typeof(ConcurrentListPexTests))]
public void Contains577()
{
    ConcurrentList<int> concurrentList;
    bool b;
    concurrentList = new ConcurrentList<int>();
    b = this.Contains<int>(concurrentList, 0);
    PexAssert.AreEqual<bool>(false, b);
    PexAssert.IsNotNull((object)concurrentList);
    PexAssert.AreEqual<int>(0, concurrentList.Count);
}
  }
}
