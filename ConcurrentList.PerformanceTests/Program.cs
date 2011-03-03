using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using ConcurrentList.Threading;

namespace ConcurrentList.PerformanceTests
{
  public class Program
  {
    const int ThreadCount = 32;
    const int N = 1000000;

    public static void Main()
    {
      Console.Write("Press Enter to start running trials, then Escape at any time to quit.");
      Console.ReadLine();

      var xxx = new ConcurrentList<int>();
      xxx.Add(0x666);
      Debugger.Break();

      var stop = new ManualResetEvent(false);
      var finished = new ManualResetEvent(false);

      ThreadStart start = () =>
      {
        int trial = 1;
        while (!stop.WaitOne(0))
        {
          var list = new List<int>();
          var concurrentList = new ConcurrentList<int>();

          Debugger.Break();

          TimeSpan listPerf = Benchmark(() => { lock (list) list.Add(0); }, ThreadCount, N);
          TimeSpan concurrentListPerf = Benchmark(() => concurrentList.Add(0), ThreadCount, N);

          Console.WriteLine("Trial {0}", trial++);
          Console.WriteLine("List<T>:           {0} ms", listPerf.TotalMilliseconds, list.Count);
          Console.WriteLine("ConcurrentList<T>: {0} ms", concurrentListPerf.TotalMilliseconds, concurrentList.Count);
          Console.WriteLine();
        }

        finished.Set();
      };

      var thread = new Thread(start);

      thread.Start();

      while (Console.ReadKey(true).Key != ConsoleKey.Escape)
      {
        // Do nothing.
      }

      stop.Set();
      finished.WaitOne();

      Console.Write("Finished. Press Enter to quit.");
      Console.ReadLine();
    }

    static TimeSpan Benchmark(Action action, int threads, int repetitionsPerThread)
    {
      GC.Collect();

      Action<int> threadAction = x =>
      {
        for (int i = 0; i < repetitionsPerThread; i++)
        {
          action();
        }
      };

      Stopwatch stopwatch = Stopwatch.StartNew();
      CrazyParallel.For(0, threads, threadAction).Dispose();
      stopwatch.Stop();

      return stopwatch.Elapsed;
    }
  }
}