using System;

namespace ConcurrentList
{
  // From: http://www-graphics.stanford.edu/~seander/bithacks.html#IntegerLogIEEE64Float  
  unsafe public class LOG2Hack
  {    
    public static int Log2(int v)
    {
      //t.u[__FLOAT_WORD_ORDER==LITTLE_ENDIAN] = 0x43300000;
      //t.u[__FLOAT_WORD_ORDER!=LITTLE_ENDIAN] = v;
      double xd;
      var x0p = ((int *) &xd);
      *x0p++ = v;
      *x0p = 0x43300000;

      xd -= 4503599627370496.0;
      var r = (*x0p >> 20) - 0x3FF;
      //Console.WriteLine("0x{0:X0},0x{1:X0}={2}", (ulong)&x0, (ulong)&x1, r);
      return r;
    }
  }
}

