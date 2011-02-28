using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentList.Threading
{
  class TaskJoiner : IDisposable
  {
    IEnumerable<Task> _tasks;

    public TaskJoiner(IEnumerable<Task> tasks)
    {
      _tasks = tasks;
    }

    public TaskJoiner(params Task[] threads)
      : this((IEnumerable<Task>)threads)
    { }

    public void Dispose()
    {
      var tasks = Interlocked.Exchange(ref _tasks, null);
      if (tasks == null) return;
      foreach (var t in tasks.Where(t => t != null)) {
        t.Wait();
      }
    }
  }
}