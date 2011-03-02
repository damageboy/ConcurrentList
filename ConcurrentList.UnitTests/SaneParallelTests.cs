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
  public partial class SaneParallelTests
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
      var numCpus = Environment.ProcessorCount;
      SaneParallel.For(0, numCpus, x =>
      {
        for (var i = 0; i < count; i++)
          _list.Add(i);
      }).Dispose();

      Assert.That(_list.Count, Is.EqualTo(count * numCpus));
    }

    [TestCase(1000000)]
    [PexMethod(MaxBranches = 160000)]
    public void CountAlwaysPointsToAnExistingValue(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var random = new Random();
      var integers = Enumerable.Range(0, 10000).Select(i => random.Next(1, int.MaxValue)).ToList();
      var numCpus = Environment.ProcessorCount;
      using (SaneParallel.For(0, numCpus, x =>
      {
        for (var i = 1; i < count + 1; i++)
          _list.Add(i);
      }))
      {
        while (_list.Count < integers.Count)
        {
          if (_list.Count <= 0) continue;
          var lastElement = _list[_list.Count - 1];
          Assert.That(lastElement, Is.Not.EqualTo(0), "Reported Count: {0}", _list.Count);
        }
      }
    }

    [TestCase(20000000)]
    [PexMethod(MaxBranches = 20000)]
    public void Clear(int count)
    {
      var numClears = 100;
      var results = new List<int>(numClears * 2);

      var numCpus = Environment.ProcessorCount;
      var sw = Stopwatch.StartNew();
      using (SaneParallel.For(0, numCpus, x =>
      {
        for (var i = 0; i < count; i++)
          _list.Add(i);
      }))
      {
        for (var i = 0; i < numClears; i++)
        {
          Thread.Sleep(100);
          results.Add(_list.Count);
          _list.Clear();
          results.Add(_list.Count);
        }
      }
      sw.Stop();
      for (var i = 0; i < numClears; i++)
        Console.WriteLine("Before/After Clear #{0}: {1}/{2}", i, results[i << 1], results[(i << 1) + 1]);
      Console.WriteLine("ClearParallelSane took {0}ms", sw.ElapsedMilliseconds);

      _list.Clear();

      Assert.That(_list.Count, Is.EqualTo(0));
    }

    [TestCase(10000000)]
    [PexMethod(MaxBranches = 20000)]
    public void Add(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var numCpus = Environment.ProcessorCount;
      var sw = Stopwatch.StartNew();
      SaneParallel.For(0, numCpus, x =>
      {
        for (var i = 1; i <= count; i++)
          _list.Add(i);
      }).Dispose();

      sw.Stop();
      Console.WriteLine("AddParallelSane took {0}ms", sw.ElapsedMilliseconds);

      Assert.That(_list.Count, Is.EqualTo(count * numCpus));

      var sum = SeriesLength.Calc(1, count) * numCpus;
      var listSum = _list.Sum(x => (long)x);

      Assert.That(sum, Is.EqualTo(listSum));
    }
  }
}