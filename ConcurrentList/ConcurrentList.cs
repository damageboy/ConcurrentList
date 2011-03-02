using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


namespace ConcurrentList
{
  public class ConcurrentList<T> : IList<T>
  {
    private int _nextIndex;
    private int _count;
    
    static readonly int[] Sizes;
    static readonly int[] Counts;
    const int NUM_BUCKETS = 29;
    const int FIRST_SIZE = 8;


    static ConcurrentList()
    {
      Sizes = new int[NUM_BUCKETS];
      Counts = new int[NUM_BUCKETS];

      Sizes[0] = FIRST_SIZE;
      Counts[0] = FIRST_SIZE;

      for (int i = 1; i < NUM_BUCKETS; i++)
      {
        Sizes[i] = Sizes[i - 1] << 1;
        Counts[i] = Counts[i - 1] + Sizes[i];
      }
    }

    private readonly T[][] _array;

    public ConcurrentList()
    {
      _array = new T[NUM_BUCKETS][];
    }

    private const short SPIN_COUNT = 100;

    public T this[int index]
    {
      get {
        if (index < 0 || index >= _count)
          throw new ArgumentOutOfRangeException("index");

        var arrayIndex = GetArrayIndex(index);
        if (arrayIndex > 0)
          index -= Counts[arrayIndex - 1];

        return _array[arrayIndex][index];
      }
      set {
        if (index < 0 || index >= _count)
          throw new ArgumentOutOfRangeException("index");

        var arrayIndex = GetArrayIndex(index);
        if (arrayIndex > 0)
          index -= Counts[arrayIndex - 1];

        _array[arrayIndex][index] = value;
      }
    }



    public void Clear()
    {
      //long expectedLong;
      var spinCount = 0;

      var frozenNext = Interlocked.Exchange(ref _nextIndex, 0);
      //expectedLong = ((long) frozenNext << 32) | frozenNext;
      while (Interlocked.CompareExchange(ref _count, 0, frozenNext) != frozenNext)
        if (++spinCount == SPIN_COUNT)
        {
          // Someone is doing insane number of concurrent additions for us to be
          // stuck so far..., better go sleep for a while to let some other threads progress
          Thread.Sleep(0);
          spinCount = 0;
        }

    }

#if HELL_FROZE_OVER
    public void ClearX()
    {
      var frozenNext = Interlocked.Exchange(ref _nextIndex, 0);
    start_clear:      
      var spinCount = 0;
      while (_count != frozenNext)
        if (++spinCount == SPIN_COUNT) {
          // Someone is doing insane number of concurrent additions for us to be
          // stuck so far..., better go sleep for a while to let some other threads progress
          Thread.Sleep(0);
          spinCount = 0;
        }
      Thread.MemoryBarrier();
      //Interlocked.Exchange(ref _fuzzyCount, index + 1);
      if (Interlocked.CompareExchange(ref _count, 0, frozenNext) == frozenNext)
        return;

      goto start_clear;
    }
#endif

    public int Count
    {
      get { return _count; }
    }

    public void Add(T element)
    {
      var index = Interlocked.Increment(ref _nextIndex) - 1;
      var adjustedIndex = index;

      var arrayIndex = GetArrayIndex(adjustedIndex);

#if HELL_FROZE_OVER
      var prevCount = 0;
      if (arrayIndex > 0)
        prevCount = Counts[arrayIndex - 1];
      if (index < prevCount || index >= Counts[arrayIndex])
        throw new Exception("Booboo");
#endif

      if (arrayIndex > 0)
        adjustedIndex -= Counts[arrayIndex - 1];

      if (_array[arrayIndex] == null) {
        int arrayLength = Sizes[arrayIndex];
        Interlocked.CompareExchange(ref _array[arrayIndex], new T[arrayLength], null);
      }

      _array[arrayIndex][adjustedIndex] = element;

      // Do a "smart" spin loop, try spinning for a while waiting for the _fuzzyCount to hit the right value
      // After a while, try sleeping a bit,
      // once it hits the right value, do a compare and exchange, although it is somewhat superflous at this point
      // 
      var spinCount = 0;

#if HELL_FROZE_OVER      
      while (_count != index)
        if (++spinCount == SPIN_COUNT) {
          // Someone is doing insane number of concurrent additions for us to be
          // stuck so far..., better go sleep for a while to let some other threads progress
          Thread.Sleep(0);
          spinCount = 0;
        }

      if (Interlocked.CompareExchange(ref _count, index + 1, index) == index)
        return;

      throw new Exception("WTF");
#endif

      while (Interlocked.CompareExchange(ref _count, index + 1, index) != index) {
        if (++spinCount != SPIN_COUNT) continue;
        // Someone is doing insane number of concurrent additions for us to be
        // stuck so far..., better go sleep for a while to let some other threads progress
        Thread.Sleep(0);
        spinCount = 0;
      }


    }

    public bool Contains(T element)
    {
      return IndexOf(element) != -1;
    }

    public int IndexOf(T element)
    {
      IEqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

      int count = Count;
      for (int i = 0; i < count; i++)
      {
        if (equalityComparer.Equals(this[i], element))
        {
          return i;
        }
      }

      return -1;
    }

    public void CopyTo(T[] array, int index)
    {
      if (array == null)
      {
        throw new ArgumentNullException("array");
      }

      var count = Count;
      if (array.Length - index < count)
      {
        throw new ArgumentException("There is not enough available space in the destination array.");
      }

      var arrayIndex = 0;
      var elementsRemaining = count;
      while (elementsRemaining > 0)
      {
        var source = _array[arrayIndex++];
        var elementsToCopy = Math.Min(source.Length, elementsRemaining);
        var startIndex = count - elementsRemaining;

        Array.Copy(source, 0, array, startIndex, elementsToCopy);

        elementsRemaining -= elementsToCopy;
      }
    }

    public IEnumerator<T> GetEnumerator()
    {
      var count = Count;
      var ai = 0;
      while (count > 0)
      {
        var ar = _array[ai++];
        var lastElem = Math.Min(count, ar.Length);
        for (var j = 0; j < lastElem; j++)
          yield return ar[j];
        count -= ar.Length;
      }
    }

    private static int GetArrayIndex(int index)
    {
      return LOG2Hack.Log2(index / FIRST_SIZE + 1);
    }

    //private static int GetArrayIndex (int count)
    //{
    //  int arrayIndex = 0;

    //  if ((count & 0xFFFF0000) != 0) {
    //    count >>= 16;
    //    arrayIndex |= 16;
    //  }

    //  if ((count & 0xFF00) != 0) {
    //    count >>= 8;
    //    arrayIndex |= 8;
    //  }

    //  if ((count & 0xF0) != 0) {
    //    count >>= 4;
    //    arrayIndex |= 4;
    //  }

    //  if ((count & 0xC) != 0) {
    //    count >>= 2;
    //    arrayIndex |= 2;
    //  }

    //  if ((count & 0x2) != 0) {
    //    count >>= 1;
    //    arrayIndex |= 1;
    //  }

    //  return arrayIndex;
    //}

    void IList<T>.Insert(int index, T element)
    {
      throw new NotSupportedException();
    }

    void IList<T>.RemoveAt(int index)
    {
      throw new NotSupportedException();
    }

    bool ICollection<T>.IsReadOnly
    {
      get { return false; }
    }

    bool ICollection<T>.Remove(T element)
    {
      throw new NotSupportedException();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
