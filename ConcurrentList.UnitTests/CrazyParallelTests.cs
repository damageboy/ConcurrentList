using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using ConcurrentList.Threading;
using Microsoft.Pex.Framework;
using NUnit.Framework;

namespace ConcurrentList.UnitTests
{
  [TestFixture]
  [PexClass]
  public partial class CrazyParallelTests
  {
    IList<int> _list;

    [SetUp]
    public void Init()
    {
      _list = new ConcurrentList<int>();
    }


    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(1000)]
    [PexMethod]
    public void Count(int count)
    {
      PexAssume.IsTrue(count >= 0);
      CrazyParallel.For(0, count, i => _list.Add(i)).Dispose();
      Assert.That(_list.Count, Is.EqualTo(count));
    }

    [TestCase(10000)]
    [PexMethod(MaxBranches = 20000)]
    public void Clear(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var numClears = 100;
      var results = new List<int>(numClears * 2);
      var sw = Stopwatch.StartNew();

      using (CrazyParallel.For(0, count, x => _list.Add(x)))
      {
        for (var i = 0; i < numClears; i++)
        {
          Thread.Sleep(0);
          results.Add(_list.Count);
          _list.Clear();
          results.Add(_list.Count);
        }
      }
      sw.Stop();
      for (var i = 0; i < numClears; i++)
        Console.WriteLine("Before/After Clear #{0}: {1}/{2}", i, results[i << 1], results[(i << 1) + 1]);

      Console.WriteLine("ClearParallelCrazy took {0}ms", sw.ElapsedMilliseconds);
      Console.WriteLine("ClearParallelCrazy MaxThreads {0}", CrazyParallel.MaxConcurrentThreads);
      _list.Clear();

      Assert.That(_list.Count, Is.EqualTo(0));
    }


    [TestCase(10000)]
    [PexMethod(MaxBranches = 20000)]
    public void Add(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var sw = Stopwatch.StartNew();
      CrazyParallel.For(0, count, x => _list.Add(x)).Dispose();
      sw.Stop();
      Console.WriteLine("AddParallelCrazy took {0}ms", sw.ElapsedMilliseconds);
      Console.WriteLine("AddParallelCrazy MaxThreads {0}", CrazyParallel.MaxConcurrentThreads);

      Assert.That(_list.Count, Is.EqualTo(count));

      var sum = SeriesLength.Calc(1, count);
      var listSum = _list.Sum(x => (long)x);

      Assert.That(sum, Is.EqualTo(listSum));
    }
  }
}