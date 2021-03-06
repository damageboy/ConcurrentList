﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConcurrentList.Threading
{
  public static class CrazyParallel
  {
    private static int _threadCounter;
    private static int _maxThreads;
    public static int MaxConcurrentThreads { get { return _maxThreads; } }

    public static IDisposable Do(Action action, TimeSpan timeout)
    {
      DateTime startTime = DateTime.Now;

      ThreadStart loop = () =>
      {
        while (DateTime.Now - startTime < timeout)
        {
          action();
        }
      };

      var t = new Thread(loop);
      t.Start();

      return new ThreadJoiner(t);
    }

    public static IDisposable For(int fromInclusive, int toExclusive, Action<int> action)
    {
      _threadCounter = 0;
      _maxThreads = 0;
      var threads = new List<Thread>(toExclusive - fromInclusive);
      for (int i = fromInclusive; i < toExclusive; i++)
      {
        int local = i;
        threads.Add(new Thread(() => {
          Interlocked.Increment(ref _threadCounter);
          Thread.Sleep(100); 
          action(local);
          if (_threadCounter > _maxThreads)
            Interlocked.CompareExchange(ref _maxThreads, _threadCounter, _maxThreads);
          Interlocked.Decrement(ref _threadCounter);
        }));
      }

      foreach (var t in threads)
        t.Start();

      return new ThreadJoiner(threads);
    }

    public static IDisposable ForEach<T>(IEnumerable<T> source, Action<T> action)
    {
      _threadCounter = 0;
      _maxThreads = 0;
      var threads = source.Select(local => new Thread(() => {
        Interlocked.Increment(ref _threadCounter);
        Thread.Sleep(100);
        action(local);
        if (_threadCounter > _maxThreads)
          Interlocked.CompareExchange(ref _maxThreads, _threadCounter, _maxThreads);
        Interlocked.Decrement(ref _threadCounter);
      })).ToList();

      foreach (var t in threads)
        t.Start();

      return new ThreadJoiner(threads);
    }
  }
}