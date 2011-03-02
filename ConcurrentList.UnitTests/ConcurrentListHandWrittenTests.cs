using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Pex.Framework;
using NUnit.Framework;

namespace ConcurrentList.UnitTests
{
  [TestFixture]
  [PexClass]
  public partial class ConcurrentListHandWrittenTests
  {
    IList<int> _list;

    [SetUp]
    public void Init()
    {
      _list = new ConcurrentList<int>();
    }

    [Test]
    [PexMethod]
    public void ConstructorShouldNotThrowException()
    {
      Assert.That(() => new ConcurrentList<int>(), Throws.Nothing);
    }    

    [Test]
    [PexMethod]
    public void ItemShouldThrowOnInvalidIndex()
    {
      _list.Add(5);

      Assert.That(() => _list[-1], Throws.InstanceOf<ArgumentOutOfRangeException>());
      Assert.That(() => _list[1], Throws.InstanceOf<ArgumentOutOfRangeException>());
      Assert.That(() => _list[-1] = 5, Throws.InstanceOf<ArgumentOutOfRangeException>());
      Assert.That(() => _list[1] = 5, Throws.InstanceOf<ArgumentOutOfRangeException>());
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(100)]
    [PexMethod]
    public void GetItem(int index)
    {
      PexAssume.IsTrue(index >= 0);
      const int element = 5;

      for (int i = 0; i < index; i++)
      {
        _list.Add(0);
      }

      _list.Add(element);

      Assert.That(_list[index], Is.EqualTo(element));
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(100)]
    [PexMethod]
    public void SetItem(int index)
    {
      PexAssume.IsTrue(index >= 0);
      const int element = 5;

      for (int i = 0; i <= index; i++)
      {
        _list.Add(0);
      }

      _list[index] = element;

      Assert.That(_list[index], Is.EqualTo(element));
    }


    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(100)]
    public void Clear(int count)
    {
      var random = new Random();
      var i = 0;
      var backupList = new List<int>();
      while (i++ < count)
      {
        var num = random.Next();
        backupList.Add(num);
        _list.Add(num);
      }


      _list.Clear();

      Assert.That(_list.Count, Is.EqualTo(0));
    }


    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(100)]
    [TestCase(10000)]
    [PexMethod(MaxBranches = 20000)]
    public void Add(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var random = new Random();
      var i = 0;
      var backupList = new List<int>();
      while (i++ < count) {
        var num = random.Next();
        backupList.Add(num);
        _list.Add(num);
      }

      Assert.That(_list.Count, Is.EqualTo(count));

      for (i = 0; i < count; i++)
        Assert.That(_list[i], Is.EqualTo(backupList[i]));
    }



    [TestCase(1, new int[] { 3, 1, 2 })]
    [TestCase(5, new int[] { 8, 9, 10 })]
    [TestCase(3, new int[] { })]
    [TestCase(4, new int[] { 4 })]
    [TestCase(2, new int[] { 6 })]
    [PexMethod]
    public void IndexOf(int needle, [PexAssumeNotNull] int[] haystack)
    {
      Array.ForEach(haystack, i => _list.Add(i));
      Assert.That(_list.IndexOf(needle), Is.EqualTo(Array.IndexOf(haystack, needle)));
    }

    [TestCase(1, new int[] { 3, 1, 2 })]
    [TestCase(5, new int[] { 8, 9, 10 })]
    [TestCase(3, new int[] { })]
    [TestCase(4, new int[] { 4 })]
    [TestCase(2, new int[] { 6 })]
    [PexMethod]
    public void Contains(int needle, [PexAssumeNotNull]int[] haystack)
    {
      Array.ForEach(haystack, i => _list.Add(i));
      Assert.That(_list.Contains(needle), Is.EqualTo(Array.IndexOf(haystack, needle) != -1));
    }

    [Test]
    [PexMethod]
    public void CopyToShouldThrowOnInvalidIndex()
    {
      var random = new Random();
      for (int i = 0; i < 1000; i++)
      {
        _list.Add(random.Next());
      }

      var array = new int[999];
      Assert.That(() => _list.CopyTo(array, 0), Throws.ArgumentException);

      array = new int[1000];
      Assert.That(() => _list.CopyTo(array, 5), Throws.ArgumentException);

      array = null;
      Assert.That(() => _list.CopyTo(array, 0), Throws.InstanceOf<ArgumentNullException>());
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(1000)]
    [PexMethod]
    public void CopyTo(int count)
    {
      PexAssume.IsTrue(count >= 0);
      var random = new Random();
      for (int i = 0; i < count; i++)
      {
        _list.Add(random.Next());
      }

      var array = new int[count];
      _list.CopyTo(array, 0);

      Assert.That(array.SequenceEqual(_list));
    }

    [Test]
    [PexMethod]
    public void IsReadOnlyReturnsFalse()
    {
      var collection = (ICollection<int>)_list;
      Assert.That(collection.IsReadOnly, Is.False);
    }

    [Test]
    [PexMethod]
    public void InsertIsNotSupported()
    {
      var list = (IList<int>)_list;
      Assert.That(() => list.Insert(0, 0), Throws.InstanceOf<NotSupportedException>());
    }

    [Test]
    [PexMethod]
    public void RemoveIsNotSupported()
    {
      var collection = (ICollection<int>)_list;
      collection.Add(0);

      Assert.That(() => collection.Remove(0), Throws.InstanceOf<NotSupportedException>());
    }

    [Test]
    [PexMethod]
    public void RemoveAtIsNotSupported()
    {
      var list = (IList<int>)_list;
      list.Add(0);

      Assert.That(() => list.RemoveAt(0), Throws.InstanceOf<NotSupportedException>());
    }
  }
}