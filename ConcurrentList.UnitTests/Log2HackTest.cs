// <copyright file="LOG2HackTest.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>

using System;
using System.Diagnostics;
using Microsoft.Pex.Framework;
using NUnit.Framework;

namespace ConcurrentList.UnitTests
{
  [TestFixture]
  [PexClass]
  public partial class Log2HackTest
  {
    [PexMethod]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(64)]
    [TestCase(65)]
    [TestCase(2 * 1024 * 1024)]
    public void Log2(int v)
    {
      PexAssume.IsTrue(v > 0);
      var realLog = (int)Math.Log(v, 2);
      var result = LOG2Hack.Log2(v);
      
      Assert.That(realLog, Is.EqualTo(result), "Value was :" + v);
    }

    [TestCase(100000000)]
    public void Log2Perf(int loopCount)
    {
      var sw = Stopwatch.StartNew();
      var sum1 = 0;
      for (var i = 1; i <= loopCount; i++)
        sum1 += LOG2Hack.Log2(i);

      Console.WriteLine("Log2Hack took {0}ms", sw.ElapsedMilliseconds);
      sw = Stopwatch.StartNew();
      var sum2 = 0;
      for (var i = 1; i <= loopCount; i++)
        sum2 += (int) Math.Log(i, 2);

      Console.WriteLine("Math.Log2 took {0}ms", sw.ElapsedMilliseconds);

      Assert.That(sum1, Is.EqualTo(sum2));
    }
  }
}