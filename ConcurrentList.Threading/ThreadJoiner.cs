using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConcurrentList.Threading
{
  class ThreadJoiner : IDisposable
  {
    IEnumerable<Thread> _threads;

    public ThreadJoiner(IEnumerable<Thread> threads)
    {
      _threads = threads;
    }

    public ThreadJoiner(params Thread[] threads)
      : this((IEnumerable<Thread>)threads)
    { }

    public void Dispose()
    {
      var threads = Interlocked.Exchange(ref _threads, null);
      if (threads == null) return;
      foreach (var t in threads.Where(t => t != null)) {
        t.Join();
      }
    }
  }
}
