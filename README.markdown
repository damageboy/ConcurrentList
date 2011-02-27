ConcurrentList<T>
====================

This data structure aims to provide an intentionally partial, yet important, implementation of ConcurrentList<T>.

What is a ConcurrentList<T>
---------------------------

Quite simply, a lock-free append only vector. i.e. a vector that can only grow at its end (no remove/insert operations)

The [lock-free](http://en.wikipedia.org/wiki/Non-blocking_algorithm) part of the description refers to an implementation that does not use locking primitives such as mutexes etc.

Why use this data structure?
----------------------------

The main use-case for this data structure is for a single or more (but generally few) writers, or threads performing append operations to the vector, while having multiple (unbounded) set or readers. The readers remain un-hindered as they access the vector/list while the writers pay no additional penalty as opposed to using a locking primitive (mutex etc).

For more information about this data structure you can read a [paper describing it](http://www2.research.att.com/~bs/lock-free-vector.pdf)


Who did this
------------
[Daniel Tao](https://philosopherdeveloper.wordpress.com/2011/02/23/how-to-build-a-thread-safe-lock-free-resizable-array/)

What is this then?
------------------
A set of improvements over his implementation.
Mainly: 

* faster Log2 function
* minor performance improvements and optimizations
* more test coverage, courtesey of Pex
* better initial array sizes (i.e. start with 8 elements, then 16 and so forth)
