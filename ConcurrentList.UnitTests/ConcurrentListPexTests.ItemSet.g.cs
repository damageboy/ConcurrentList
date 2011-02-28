// <copyright file="ConcurrentListPexTests.ItemSet.g.cs" company="Microsoft">Copyright � Microsoft 2011</copyright>
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
using System;
using NUnit.Framework;
using Microsoft.Pex.Framework.Generated;

namespace ConcurrentList.UnitTests
{
  public partial class ConcurrentListPexTests
  {
[Test]
[PexGeneratedBy(typeof(ConcurrentListPexTests))]
[ExpectedException(typeof(ArgumentOutOfRangeException))]
public void ItemSetThrowsArgumentOutOfRangeException816()
{
    ConcurrentList<int> concurrentList;
    concurrentList = new ConcurrentList<int>();
    this.ItemSet<int>(concurrentList, 0, 0);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListPexTests))]
[ExpectedException(typeof(ArgumentOutOfRangeException))]
public void ItemSetThrowsArgumentOutOfRangeException26()
{
    ConcurrentList<int> concurrentList;
    concurrentList = new ConcurrentList<int>();
    this.ItemSet<int>(concurrentList, int.MinValue, 0);
}
  }
}