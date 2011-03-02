// <copyright file="ConcurrentListTTest.cs" company="Microsoft">Copyright © Microsoft 2011</copyright>

using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using NUnit.Framework;

namespace ConcurrentList.UnitTests
{
  [TestFixture]
  [PexClass(typeof(ConcurrentList<>))]
  [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
  [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
  public partial class ConcurrentListPexTests
  {
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public void Add<T>([PexAssumeUnderTest]ConcurrentList<T> target, T element)
    {
      target.Add(element);
      // TODO: add assertions to method ConcurrentListTTest.Add(ConcurrentList`1<!!0>, !!0)
    }

    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public void Clear<T>([PexAssumeUnderTest]ConcurrentList<T> target)
    {
      target.Clear();
      // TODO: add assertions to method ConcurrentListTTest.Clear(ConcurrentList`1<!!0>)
    }

    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public void ItemSet<T>(
        [PexAssumeUnderTest]ConcurrentList<T> target,
        int index,
        T value
    )
    {
      target[index] = value;
      // TODO: add assertions to method ConcurrentListTTest.ItemSet(ConcurrentList`1<!!0>, Int32, !!0)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public T ItemGet<T>([PexAssumeUnderTest]ConcurrentList<T> target, int index)
    {
      T result = target[index];
      return result;
      // TODO: add assertions to method ConcurrentListTTest.ItemGet(ConcurrentList`1<!!0>, Int32)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public int IndexOf<T>([PexAssumeUnderTest]ConcurrentList<T> target, T element)
    {
      int result = target.IndexOf(element);
      return result;
      // TODO: add assertions to method ConcurrentListTTest.IndexOf(ConcurrentList`1<!!0>, !!0)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public IEnumerator<T> GetEnumerator<T>([PexAssumeUnderTest]ConcurrentList<T> target)
    {
      IEnumerator<T> result = target.GetEnumerator();
      return result;
      // TODO: add assertions to method ConcurrentListTTest.GetEnumerator(ConcurrentList`1<!!0>)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public int CountGet<T>([PexAssumeUnderTest]ConcurrentList<T> target)
    {
      int result = target.Count;
      return result;
      // TODO: add assertions to method ConcurrentListTTest.CountGet(ConcurrentList`1<!!0>)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public void CopyTo<T>(
        [PexAssumeUnderTest]ConcurrentList<T> target,
        T[] array,
        int index
    )
    {
      target.CopyTo(array, index);
      // TODO: add assertions to method ConcurrentListTTest.CopyTo(ConcurrentList`1<!!0>, !!0[], Int32)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public bool Contains<T>([PexAssumeUnderTest]ConcurrentList<T> target, T element)
    {
      bool result = target.Contains(element);
      return result;
      // TODO: add assertions to method ConcurrentListTTest.Contains(ConcurrentList`1<!!0>, !!0)
    }
    [PexGenericArguments(typeof(int))]
    [PexMethod]
    public ConcurrentList<T> Constructor<T>()
    {
      ConcurrentList<T> target = new ConcurrentList<T>();
      return target;
      // TODO: add assertions to method ConcurrentListTTest.Constructor()
    }
  }
}
