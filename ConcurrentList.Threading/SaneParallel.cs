using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentList.Threading
{
  public static class SaneParallel
  {
    private static int _threadCounter;
    private static int _maxThreads;
    public static int MaxConcurrentThreads { get { return _maxThreads; } }

    public static IDisposable Do(Action action, TimeSpan timeout)
    {
      DateTime startTime = DateTime.UtcNow;

      var loop = new Task(() => {
        while (DateTime.UtcNow - startTime < timeout) {
          action();
        }
      });

      loop.Start();


      return new TaskJoiner(loop);
    }

    public static IDisposable For(int fromInclusive, int toExclusive, Action<int> action)
    {
      _threadCounter = 0;
      _maxThreads = 0;
      var tasks = new List<Task>(toExclusive - fromInclusive);
      for (var i = fromInclusive; i < toExclusive; i++)
      {
        var local = i;
        tasks.Add(new Task(() => {
          Interlocked.Increment(ref _threadCounter);
          //Thread.Sleep(100);
          action(local);
          if (_threadCounter > _maxThreads)
            Interlocked.CompareExchange(ref _maxThreads, _threadCounter, _maxThreads);
          Interlocked.Decrement(ref _threadCounter);
        }));
      }

      foreach (var t in tasks)
        t.Start();

      return new TaskJoiner(tasks);
    }

    public static IDisposable ForEach<T>(IEnumerable<T> source, Action<T> action)
    {
      _threadCounter = 0;
      _maxThreads = 0;

      var tasks = source.Select(local => new Task(() => {
        Interlocked.Increment(ref _threadCounter);
        //Thread.Sleep(100);
        action(local);
        if (_threadCounter > _maxThreads)
          Interlocked.CompareExchange(ref _maxThreads, _threadCounter, _maxThreads);
        Interlocked.Decrement(ref _threadCounter);
      })).ToList();

      foreach (var t in tasks)
        t.Start();

      return new TaskJoiner(tasks);
    }
  }
}
