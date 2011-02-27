using System;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentList.Threading
{
    class ThreadJoiner : IDisposable
    {
        IEnumerable<Thread> m_threads;

        public ThreadJoiner(IEnumerable<Thread> threads)
        {
            m_threads = threads;
        }

        public ThreadJoiner(params Thread[] threads)
            : this((IEnumerable<Thread>)threads)
        { }

        public void Dispose()
        {
            IEnumerable<Thread> threads = Interlocked.Exchange(ref m_threads, null);
            if (threads != null)
            {
                foreach (Thread t in threads)
                {
                    if (t != null)
                    {
                        t.Join();
                    }
                }
            }
        }
    }
}
