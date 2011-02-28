using System;
using System.Runtime.InteropServices;
namespace ConcurrentList
{
  // From: http://www-graphics.stanford.edu/~seander/bithacks.html#IntegerLogIEEE64Float
  [StructLayout(LayoutKind.Explicit)]
  unsafe public struct LOG2Hack
  {
    //static readonly int _endianess;
    //static readonly bool BigEndianSystem;

    [FieldOffset(0)]
    private fixed int u[2];
    [FieldOffset(0)]
    private double d;

    public static int Log2 (int v)
    {
      var h = new LOG2Hack();
      //h.d = 0;
      //t.u[__FLOAT_WORD_ORDER==LITTLE_ENDIAN] = 0x43300000;
      //t.u[__FLOAT_WORD_ORDER!=LITTLE_ENDIAN] = v;
      h.u[1] = 0x43300000;
      h.u[0] = v;
      h.d -= 4503599627370496.0;
      var r = (h.u[1] >> 20) - 0x3FF;
      return r;      
    }
  }
}

