// <copyright file="ConcurrentListTests.Contains.g.cs" company="Microsoft">Copyright � Microsoft 2011</copyright>
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
  public partial class ConcurrentListTests
  {
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains985()
{
    int[] ints = new int[0];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains880()
{
    int[] ints = new int[1];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains740()
{
    int[] ints = new int[1];
    ints[0] = 1;
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains71()
{
    int[] ints = new int[2];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains480()
{
    int[] ints = new int[2];
    ints[0] = 1;
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains40()
{
    int[] ints = new int[2];
    this.Contains(1, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains825()
{
    int[] ints = new int[3];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains431()
{
    int[] ints = new int[9];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains600()
{
    int[] ints = new int[10];
    this.Contains(0, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains138()
{
    int[] ints = new int[9];
    this.Contains(1, ints);
}
[Test]
[PexGeneratedBy(typeof(ConcurrentListTests))]
public void Contains643()
{
    int[] ints = new int[43];
    this.Contains(1, ints);
}
  }
}