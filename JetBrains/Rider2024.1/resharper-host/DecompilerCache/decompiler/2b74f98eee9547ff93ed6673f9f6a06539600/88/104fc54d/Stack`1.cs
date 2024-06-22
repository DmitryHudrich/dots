// Decompiled with JetBrains decompiler
// Type: System.Collections.Generic.Stack`1
// Assembly: System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2B74F98E-EE95-47FF-93ED-6673F9F6A065
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.5/System.Collections.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.5/ref/net8.0/System.Collections.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#nullable enable
namespace System.Collections.Generic
{
  /// <summary>Represents a variable size last-in-first-out (LIFO) collection of instances of the same specified type.</summary>
  /// <typeparam name="T">Specifies the type of elements in the stack.</typeparam>
  [DebuggerTypeProxy(typeof (StackDebugView<>))]
  [DebuggerDisplay("Count = {Count}")]
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Stack<T> : IEnumerable<T>, IEnumerable, ICollection, IReadOnlyCollection<T>
  {
    #nullable disable
    private T[] _array;
    private int _size;
    private int _version;

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that is empty and has the default initial capacity.</summary>
    public Stack() => this._array = Array.Empty<T>();

    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that is empty and has the specified initial capacity or the default initial capacity, whichever is greater.</summary>
    /// <param name="capacity">The initial number of elements that the <see cref="T:System.Collections.Generic.Stack`1" /> can contain.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="capacity" /> is less than zero.</exception>
    public Stack(int capacity)
    {
      ArgumentOutOfRangeException.ThrowIfNegative<int>(capacity, nameof (capacity));
      this._array = new T[capacity];
    }

    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Collections.Generic.Stack`1" /> class that contains elements copied from the specified collection and has sufficient capacity to accommodate the number of elements copied.</summary>
    /// <param name="collection">The collection to copy elements from.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="collection" /> is <see langword="null" />.</exception>
    public Stack(IEnumerable<T> collection)
    {
      ArgumentNullException.ThrowIfNull((object) collection, nameof (collection));
      this._array = EnumerableHelpers.ToArray<T>(collection, out this._size);
    }

    /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
    public int Count => this._size;

    /// <summary>Gets a value indicating whether access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe).</summary>
    /// <returns>
    /// <see langword="true" /> if access to the <see cref="T:System.Collections.ICollection" /> is synchronized (thread safe); otherwise, <see langword="false" />.  In the default implementation of <see cref="T:System.Collections.Generic.Stack`1" />, this property always returns <see langword="false" />.</returns>
    bool ICollection.IsSynchronized => false;

    /// <summary>Gets an object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.</summary>
    /// <returns>An object that can be used to synchronize access to the <see cref="T:System.Collections.ICollection" />.  In the default implementation of <see cref="T:System.Collections.Generic.Stack`1" />, this property always returns the current instance.</returns>
    object ICollection.SyncRoot => (object) this;

    /// <summary>Removes all objects from the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    public void Clear()
    {
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        Array.Clear((Array) this._array, 0, this._size);
      this._size = 0;
      ++this._version;
    }

    /// <summary>Determines whether an element is in the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.Stack`1" />. The value can be <see langword="null" /> for reference types.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.Stack`1" />; otherwise, <see langword="false" />.</returns>
    public bool Contains(T item)
    {
      return this._size != 0 && Array.LastIndexOf<T>(this._array, item, this._size - 1) != -1;
    }

    /// <summary>Copies the <see cref="T:System.Collections.Generic.Stack`1" /> to an existing one-dimensional <see cref="T:System.Array" />, starting at the specified array index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.Stack`1" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.Stack`1" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.</exception>
    public void CopyTo(T[] array, int arrayIndex)
    {
      ArgumentNullException.ThrowIfNull((object) array, nameof (array));
      if (arrayIndex < 0 || arrayIndex > array.Length)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), (object) arrayIndex, SR.ArgumentOutOfRange_IndexMustBeLessOrEqual);
      if (array.Length - arrayIndex < this._size)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      int num1 = 0;
      int num2 = arrayIndex + this._size;
      while (num1 < this._size)
        array[--num2] = this._array[num1++];
    }

    #nullable disable
    /// <summary>Copies the elements of the <see cref="T:System.Collections.ICollection" /> to an <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.</summary>
    /// <param name="array">The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied from <see cref="T:System.Collections.ICollection" />. The <see cref="T:System.Array" /> must have zero-based indexing.</param>
    /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="arrayIndex" /> is less than zero.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="array" /> is multidimensional.
    /// 
    /// -or-
    /// 
    /// <paramref name="array" /> does not have zero-based indexing.
    /// 
    /// -or-
    /// 
    /// The number of elements in the source <see cref="T:System.Collections.ICollection" /> is greater than the available space from <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
    /// 
    /// -or-
    /// 
    /// The type of the source <see cref="T:System.Collections.ICollection" /> cannot be cast automatically to the type of the destination <paramref name="array" />.</exception>
    void ICollection.CopyTo(Array array, int arrayIndex)
    {
      ArgumentNullException.ThrowIfNull((object) array, nameof (array));
      if (array.Rank != 1)
        throw new ArgumentException(SR.Arg_RankMultiDimNotSupported, nameof (array));
      if (array.GetLowerBound(0) != 0)
        throw new ArgumentException(SR.Arg_NonZeroLowerBound, nameof (array));
      if (arrayIndex < 0 || arrayIndex > array.Length)
        throw new ArgumentOutOfRangeException(nameof (arrayIndex), (object) arrayIndex, SR.ArgumentOutOfRange_IndexMustBeLessOrEqual);
      if (array.Length - arrayIndex < this._size)
        throw new ArgumentException(SR.Argument_InvalidOffLen);
      try
      {
        Array.Copy((Array) this._array, 0, array, arrayIndex, this._size);
        Array.Reverse(array, arrayIndex, this._size);
      }
      catch (ArrayTypeMismatchException ex)
      {
        throw new ArgumentException(SR.Argument_IncompatibleArrayType, nameof (array));
      }
    }

    #nullable enable
    /// <summary>Returns an enumerator for the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.Stack`1.Enumerator" /> for the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
    public Stack<
    #nullable disable
    T>.Enumerator GetEnumerator() => new Stack<T>.Enumerator(this);

    /// <summary>Returns an enumerator that iterates through the collection.</summary>
    /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1" /> that can be used to iterate through the collection.</returns>
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
      return this.Count != 0 ? (IEnumerator<T>) this.GetEnumerator() : EnumerableHelpers.GetEmptyEnumerator<T>();
    }

    /// <summary>Returns an enumerator that iterates through a collection.</summary>
    /// <returns>An <see cref="T:System.Collections.IEnumerator" /> that can be used to iterate through the collection.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return (IEnumerator) ((IEnumerable<T>) this).GetEnumerator();
    }

    /// <summary>Sets the capacity to the actual number of elements in the <see cref="T:System.Collections.Generic.Stack`1" />, if that number is less than 90 percent of current capacity.</summary>
    public void TrimExcess()
    {
      if (this._size >= (int) ((double) this._array.Length * 0.9))
        return;
      Array.Resize<T>(ref this._array, this._size);
    }

    #nullable enable
    /// <summary>Returns the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" /> without removing it.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</exception>
    /// <returns>The object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
    public T Peek()
    {
      int index = this._size - 1;
      T[] array = this._array;
      if ((uint) index >= (uint) array.Length)
        this.ThrowForEmptyStack();
      return array[index];
    }

    /// <summary>Returns a value that indicates whether there is an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />, and if one is present, copies it to the <paramref name="result" /> parameter. The object is not removed from the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <param name="result">If present, the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />; otherwise, the default value of <typeparamref name="T" />.</param>
    /// <returns>
    /// <see langword="true" /> if there is an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />; <see langword="false" /> if the <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</returns>
    public bool TryPeek([MaybeNullWhen(false)] out T result)
    {
      int index = this._size - 1;
      T[] array = this._array;
      if ((uint) index >= (uint) array.Length)
      {
        result = default (T);
        return false;
      }
      result = array[index];
      return true;
    }

    /// <summary>Removes and returns the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</exception>
    /// <returns>The object removed from the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
    public T Pop()
    {
      int index = this._size - 1;
      T[] array = this._array;
      if ((uint) index >= (uint) array.Length)
        this.ThrowForEmptyStack();
      ++this._version;
      this._size = index;
      T obj = array[index];
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        array[index] = default (T);
      return obj;
    }

    /// <summary>Returns a value that indicates whether there is an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />, and if one is present, copies it to the <paramref name="result" /> parameter, and removes it from the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <param name="result">If present, the object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />; otherwise, the default value of <typeparamref name="T" />.</param>
    /// <returns>
    /// <see langword="true" /> if there is an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />; <see langword="false" /> if the <see cref="T:System.Collections.Generic.Stack`1" /> is empty.</returns>
    public bool TryPop([MaybeNullWhen(false)] out T result)
    {
      int index = this._size - 1;
      T[] array = this._array;
      if ((uint) index >= (uint) array.Length)
      {
        result = default (T);
        return false;
      }
      ++this._version;
      this._size = index;
      result = array[index];
      if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
        array[index] = default (T);
      return true;
    }

    /// <summary>Inserts an object at the top of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <param name="item">The object to push onto the <see cref="T:System.Collections.Generic.Stack`1" />. The value can be <see langword="null" /> for reference types.</param>
    public void Push(T item)
    {
      int size = this._size;
      T[] array = this._array;
      if ((uint) size < (uint) array.Length)
      {
        array[size] = item;
        ++this._version;
        this._size = size + 1;
      }
      else
        this.PushWithResize(item);
    }

    #nullable disable
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void PushWithResize(T item)
    {
      this.Grow(this._size + 1);
      this._array[this._size] = item;
      ++this._version;
      ++this._size;
    }

    /// <summary>Ensures that the capacity of this Stack is at least the specified <paramref name="capacity" />. If the current capacity is less than <paramref name="capacity" />, it is successively increased to twice the current capacity until it is at least the specified <paramref name="capacity" />.</summary>
    /// <param name="capacity">The minimum capacity to ensure.</param>
    /// <returns>The new capacity of this stack.</returns>
    public int EnsureCapacity(int capacity)
    {
      ArgumentOutOfRangeException.ThrowIfNegative<int>(capacity, nameof (capacity));
      if (this._array.Length < capacity)
        this.Grow(capacity);
      return this._array.Length;
    }

    private void Grow(int capacity)
    {
      int newSize = this._array.Length == 0 ? 4 : 2 * this._array.Length;
      if ((long) (uint) newSize > (long) Array.MaxLength)
        newSize = Array.MaxLength;
      if (newSize < capacity)
        newSize = capacity;
      Array.Resize<T>(ref this._array, newSize);
    }

    #nullable enable
    /// <summary>Copies the <see cref="T:System.Collections.Generic.Stack`1" /> to a new array.</summary>
    /// <returns>A new array containing copies of the elements of the <see cref="T:System.Collections.Generic.Stack`1" />.</returns>
    public T[] ToArray()
    {
      if (this._size == 0)
        return Array.Empty<T>();
      T[] array = new T[this._size];
      for (int index = 0; index < this._size; ++index)
        array[index] = this._array[this._size - index - 1];
      return array;
    }

    private void ThrowForEmptyStack()
    {
      throw new InvalidOperationException(SR.InvalidOperation_EmptyStack);
    }

    /// <summary>Enumerates the elements of a <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
    /// <typeparam name="T" />
    public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
    {
      #nullable disable
      private readonly Stack<T> _stack;
      private readonly int _version;
      private int _index;
      private T _currentElement;

      internal Enumerator(Stack<T> stack)
      {
        this._stack = stack;
        this._version = stack._version;
        this._index = -2;
        this._currentElement = default (T);
      }

      /// <summary>Releases all resources used by the <see cref="T:System.Collections.Generic.Stack`1.Enumerator" />.</summary>
      public void Dispose() => this._index = -1;

      /// <summary>Advances the enumerator to the next element of the <see cref="T:System.Collections.Generic.Stack`1" />.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      /// <returns>
      /// <see langword="true" /> if the enumerator was successfully advanced to the next element; <see langword="false" /> if the enumerator has passed the end of the collection.</returns>
      public bool MoveNext()
      {
        if (this._version != this._stack._version)
          throw new InvalidOperationException(SR.InvalidOperation_EnumFailedVersion);
        if (this._index == -2)
        {
          this._index = this._stack._size - 1;
          bool flag = this._index >= 0;
          if (flag)
            this._currentElement = this._stack._array[this._index];
          return flag;
        }
        if (this._index == -1)
          return false;
        bool flag1 = --this._index >= 0;
        this._currentElement = !flag1 ? default (T) : this._stack._array[this._index];
        return flag1;
      }

      #nullable enable
      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The element in the <see cref="T:System.Collections.Generic.Stack`1" /> at the current position of the enumerator.</returns>
      public T Current
      {
        get
        {
          if (this._index < 0)
            this.ThrowEnumerationNotStartedOrEnded();
          return this._currentElement;
        }
      }

      private void ThrowEnumerationNotStartedOrEnded()
      {
        throw new InvalidOperationException(this._index == -2 ? SR.InvalidOperation_EnumNotStarted : SR.InvalidOperation_EnumEnded);
      }

      /// <summary>Gets the element at the current position of the enumerator.</summary>
      /// <exception cref="T:System.InvalidOperationException">The enumerator is positioned before the first element of the collection or after the last element.</exception>
      /// <returns>The element in the collection at the current position of the enumerator.</returns>
      object? IEnumerator.Current => (object) this.Current;

      /// <summary>Sets the enumerator to its initial position, which is before the first element in the collection. This class cannot be inherited.</summary>
      /// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
      void IEnumerator.Reset()
      {
        if (this._version != this._stack._version)
          throw new InvalidOperationException(SR.InvalidOperation_EnumFailedVersion);
        this._index = -2;
        this._currentElement = default (T);
      }
    }
  }
}
