using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConcurrentList.UnitTests
{
  internal class SeriesLength
  {
    public static long Calc(int start, int count)
    {
      int middle;
      if (count % 2 == 0) {
        middle = count / 2;
        return ((long) middle)*(2*start + count - 1);
      }
      middle = (count - 1) / 2; 
      return ((long) count)*(start + middle);
    }
  }
}
