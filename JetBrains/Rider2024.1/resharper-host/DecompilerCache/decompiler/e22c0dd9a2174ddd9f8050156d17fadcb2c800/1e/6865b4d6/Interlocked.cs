// Decompiled with JetBrains decompiler
// Type: System.Threading.Interlocked
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Threading.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable
namespace System.Threading
{
  /// <summary>Provides atomic operations for variables that are shared by multiple threads.</summary>
  public static class Interlocked
  {
    /// <summary>Increments a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be incremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The incremented value.</returns>
    public static int Increment(ref int location) => Interlocked.Add(ref location, 1);

    /// <summary>Increments a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be incremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The incremented value.</returns>
    public static long Increment(ref long location) => Interlocked.Add(ref location, 1L);

    /// <summary>Decrements a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be decremented.</param>
    /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The decremented value.</returns>
    public static int Decrement(ref int location) => Interlocked.Add(ref location, -1);

    /// <summary>Decrements the specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be decremented.</param>
    /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The decremented value.</returns>
    public static long Decrement(ref long location) => Interlocked.Add(ref location, -1L);

    /// <summary>Sets a 32-bit signed integer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int Exchange(ref int location1, int value);

    /// <summary>Sets a 64-bit signed integer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern long Exchange(ref long location1, long value);

    /// <summary>Sets an object to a specified value and returns a reference to the original object, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.ArgumentNullException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: NotNullIfNotNull("location1")]
    public static extern object? Exchange([NotNullIfNotNull("value")] ref object? location1, object? value);

    /// <summary>Sets a variable of the specified type <paramref name="T" /> to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value. This is a reference parameter (<see langword="ref" /> in C#, <see langword="ByRef" /> in Visual Basic).</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <typeparam name="T">The type to be used for <paramref name="location1" /> and <paramref name="value" />. This type must be a reference type.</typeparam>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [return: NotNullIfNotNull("location1")]
    public static T Exchange<T>([NotNullIfNotNull("value")] ref T location1, T value) where T : class?
    {
      return Unsafe.As<T>(Interlocked.Exchange(ref Unsafe.As<T, object>(ref location1), (object) value));
    }

    /// <summary>Compares two 32-bit signed integers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern int CompareExchange(ref int location1, int value, int comparand);

    /// <summary>Compares two 64-bit signed integers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern long CompareExchange(ref long location1, long value, long comparand);

    /// <summary>Compares two objects for reference equality and, if they are equal, replaces the first object.</summary>
    /// <param name="location1">The destination object that is compared by reference with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The object that replaces the destination object if the reference comparison results in equality.</param>
    /// <param name="comparand">The object that is compared by reference to the object at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    [return: NotNullIfNotNull("location1")]
    public static extern object? CompareExchange(
      ref object? location1,
      object? value,
      object? comparand);

    /// <summary>Compares two instances of the specified reference type <paramref name="T" /> for reference equality and, if they are equal, replaces the first one.</summary>
    /// <param name="location1">The destination, whose value is compared by reference with <paramref name="comparand" /> and possibly replaced. This is a reference parameter (<see langword="ref" /> in C#, <see langword="ByRef" /> in Visual Basic).</param>
    /// <param name="value">The value that replaces the destination value if the comparison by reference results in equality.</param>
    /// <param name="comparand">The value that is compared by reference to the value at <paramref name="location1" />.</param>
    /// <typeparam name="T">The type to be used for <paramref name="location1" />, <paramref name="value" />, and <paramref name="comparand" />. This type must be a reference type.</typeparam>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [return: NotNullIfNotNull("location1")]
    public static T CompareExchange<T>(ref T location1, T value, T comparand) where T : class?
    {
      return Unsafe.As<T>(Interlocked.CompareExchange(ref Unsafe.As<T, object>(ref location1), (object) value, (object) comparand));
    }

    /// <summary>Adds two 32-bit integers and replaces the first integer with the sum, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The new value that was stored at <paramref name="location1" /> by this operation.</returns>
    public static int Add(ref int location1, int value)
    {
      return Interlocked.ExchangeAdd(ref location1, value) + value;
    }

    /// <summary>Adds two 64-bit integers and replaces the first integer with the sum, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The new value that was stored at <paramref name="location1" /> by this operation.</returns>
    public static long Add(ref long location1, long value)
    {
      return Interlocked.ExchangeAdd(ref location1, value) + value;
    }

    #nullable disable
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern int ExchangeAdd(ref int location1, int value);

    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern long ExchangeAdd(ref long location1, long value);

    #nullable enable
    /// <summary>Returns a 64-bit value, loaded as an atomic operation.</summary>
    /// <param name="location">The 64-bit value to be loaded.</param>
    /// <returns>The loaded value.</returns>
    public static long Read([RequiresLocation, In] ref long location)
    {
      return Interlocked.CompareExchange(ref Unsafe.AsRef<long>(ref location), 0L, 0L);
    }

    [LibraryImport("QCall", EntryPoint = "Interlocked_MemoryBarrierProcessWide")]
    [DllImport("QCall", EntryPoint = "Interlocked_MemoryBarrierProcessWide")]
    private static extern void _MemoryBarrierProcessWide();

    /// <summary>Provides a process-wide memory barrier that ensures that reads and writes from any CPU cannot move across the barrier.</summary>
    public static void MemoryBarrierProcessWide() => Interlocked._MemoryBarrierProcessWide();

    /// <summary>Increments a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be incremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The incremented value.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Increment(ref uint location) => Interlocked.Add(ref location, 1U);

    /// <summary>Increments a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be incremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The incremented value.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Increment(ref ulong location) => Interlocked.Add(ref location, 1UL);

    /// <summary>Decrements a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be decremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The decremented value.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Decrement(ref uint location)
    {
      return (uint) Interlocked.Add(ref Unsafe.As<uint, int>(ref location), -1);
    }

    /// <summary>Decrements a specified variable and stores the result, as an atomic operation.</summary>
    /// <param name="location">The variable whose value is to be decremented.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The decremented value.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Decrement(ref ulong location)
    {
      return (ulong) Interlocked.Add(ref Unsafe.As<ulong, long>(ref location), -1L);
    }

    /// <summary>Sets a 32-bit unsigned integer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Exchange(ref uint location1, uint value)
    {
      return (uint) Interlocked.Exchange(ref Unsafe.As<uint, int>(ref location1), (int) value);
    }

    /// <summary>Sets a 64-bit unsigned integer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Exchange(ref ulong location1, ulong value)
    {
      return (ulong) Interlocked.Exchange(ref Unsafe.As<ulong, long>(ref location1), (long) value);
    }

    /// <summary>Sets a single-precision floating point number to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Exchange(ref float location1, float value)
    {
      return Unsafe.BitCast<int, float>(Interlocked.Exchange(ref Unsafe.As<float, int>(ref location1), Unsafe.BitCast<float, int>(value)));
    }

    /// <summary>Sets a double-precision floating point number to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Exchange(ref double location1, double value)
    {
      return Unsafe.BitCast<long, double>(Interlocked.Exchange(ref Unsafe.As<double, long>(ref location1), Unsafe.BitCast<double, long>(value)));
    }

    /// <summary>Sets a platform-specific handle or pointer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr Exchange(ref IntPtr location1, IntPtr value)
    {
      return (IntPtr) Interlocked.Exchange(ref Unsafe.As<IntPtr, long>(ref location1), (long) value);
    }

    /// <summary>Sets a platform-specific handle or pointer to a specified value and returns the original value, as an atomic operation.</summary>
    /// <param name="location1">The variable to set to the specified value.</param>
    /// <param name="value">The value to which the <paramref name="location1" /> parameter is set.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value of <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UIntPtr Exchange(ref UIntPtr location1, UIntPtr value)
    {
      return (UIntPtr) (ulong) Interlocked.Exchange(ref Unsafe.As<UIntPtr, long>(ref location1), (long) (ulong) value);
    }

    /// <summary>Compares two 32-bit unsigned integers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint CompareExchange(ref uint location1, uint value, uint comparand)
    {
      return (uint) Interlocked.CompareExchange(ref Unsafe.As<uint, int>(ref location1), (int) value, (int) comparand);
    }

    /// <summary>Compares two 64-bit unsigned integers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong CompareExchange(ref ulong location1, ulong value, ulong comparand)
    {
      return (ulong) Interlocked.CompareExchange(ref Unsafe.As<ulong, long>(ref location1), (long) value, (long) comparand);
    }

    /// <summary>Compares two single-precision floating point numbers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float CompareExchange(ref float location1, float value, float comparand)
    {
      return Unsafe.BitCast<int, float>(Interlocked.CompareExchange(ref Unsafe.As<float, int>(ref location1), Unsafe.BitCast<float, int>(value), Unsafe.BitCast<float, int>(comparand)));
    }

    /// <summary>Compares two double-precision floating point numbers for equality and, if they are equal, replaces the first value.</summary>
    /// <param name="location1">The destination, whose value is compared with <paramref name="comparand" /> and possibly replaced.</param>
    /// <param name="value">The value that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The value that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double CompareExchange(ref double location1, double value, double comparand)
    {
      return Unsafe.BitCast<long, double>(Interlocked.CompareExchange(ref Unsafe.As<double, long>(ref location1), Unsafe.BitCast<double, long>(value), Unsafe.BitCast<double, long>(comparand)));
    }

    /// <summary>Compares two platform-specific handles or pointers for equality and, if they are equal, replaces the first one.</summary>
    /// <param name="location1">The destination <see cref="T:System.IntPtr" />, whose value is compared with the value of <paramref name="comparand" /> and possibly replaced by <paramref name="value" />.</param>
    /// <param name="value">The <see cref="T:System.IntPtr" /> that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The <see cref="T:System.IntPtr" /> that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a null pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IntPtr CompareExchange(ref IntPtr location1, IntPtr value, IntPtr comparand)
    {
      return (IntPtr) Interlocked.CompareExchange(ref Unsafe.As<IntPtr, long>(ref location1), (long) value, (long) comparand);
    }

    /// <summary>Compares two platform-specific handles or pointers for equality and, if they are equal, replaces the first one.</summary>
    /// <param name="location1">The destination <see cref="T:System.UIntPtr" />, whose value is compared with the value of <paramref name="comparand" /> and possibly replaced by <paramref name="value" />.</param>
    /// <param name="value">The <see cref="T:System.UIntPtr" /> that replaces the destination value if the comparison results in equality.</param>
    /// <param name="comparand">The <see cref="T:System.UIntPtr" /> that is compared to the value at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static UIntPtr CompareExchange(ref UIntPtr location1, UIntPtr value, UIntPtr comparand)
    {
      return (UIntPtr) (ulong) Interlocked.CompareExchange(ref Unsafe.As<UIntPtr, long>(ref location1), (long) (ulong) value, (long) (ulong) comparand);
    }

    /// <summary>Adds two 32-bit unsigned integers and replaces the first integer with the sum, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The new value that was stored at <paramref name="location1" /> by this operation.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Add(ref uint location1, uint value)
    {
      return (uint) Interlocked.Add(ref Unsafe.As<uint, int>(ref location1), (int) value);
    }

    /// <summary>Adds two 64-bit unsigned integers and replaces the first integer with the sum, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be added. The sum of the two values is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be added to the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The new value that was stored at <paramref name="location1" /> by this operation.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Add(ref ulong location1, ulong value)
    {
      return (ulong) Interlocked.Add(ref Unsafe.As<ulong, long>(ref location1), (long) value);
    }

    /// <summary>Returns a 64-bit unsigned value, loaded as an atomic operation.</summary>
    /// <param name="location">The 64-bit value to be loaded.</param>
    /// <returns>The loaded value.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Read([RequiresLocation, In] ref ulong location)
    {
      return Interlocked.CompareExchange(ref Unsafe.AsRef<ulong>(ref location), 0UL, 0UL);
    }

    /// <summary>Bitwise "ands" two 32-bit signed integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int And(ref int location1, int value)
    {
      int comparand = location1;
      int num1;
      while (true)
      {
        int num2 = comparand & value;
        num1 = Interlocked.CompareExchange(ref location1, num2, comparand);
        if (num1 != comparand)
          comparand = num1;
        else
          break;
      }
      return num1;
    }

    /// <summary>Bitwise "ands" two 32-bit unsigned integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint And(ref uint location1, uint value)
    {
      return (uint) Interlocked.And(ref Unsafe.As<uint, int>(ref location1), (int) value);
    }

    /// <summary>Bitwise "ands" two 64-bit signed integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long And(ref long location1, long value)
    {
      long comparand = location1;
      long num1;
      while (true)
      {
        long num2 = comparand & value;
        num1 = Interlocked.CompareExchange(ref location1, num2, comparand);
        if (num1 != comparand)
          comparand = num1;
        else
          break;
      }
      return num1;
    }

    /// <summary>Bitwise "ands" two 64-bit unsigned integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong And(ref ulong location1, ulong value)
    {
      return (ulong) Interlocked.And(ref Unsafe.As<ulong, long>(ref location1), (long) value);
    }

    /// <summary>Bitwise "ors" two 32-bit signed integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Or(ref int location1, int value)
    {
      int comparand = location1;
      int num1;
      while (true)
      {
        int num2 = comparand | value;
        num1 = Interlocked.CompareExchange(ref location1, num2, comparand);
        if (num1 != comparand)
          comparand = num1;
        else
          break;
      }
      return num1;
    }

    /// <summary>Bitwise "ors" two 32-bit unsigned integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Or(ref uint location1, uint value)
    {
      return (uint) Interlocked.Or(ref Unsafe.As<uint, int>(ref location1), (int) value);
    }

    /// <summary>Bitwise "ors" two 64-bit signed integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Or(ref long location1, long value)
    {
      long comparand = location1;
      long num1;
      while (true)
      {
        long num2 = comparand | value;
        num1 = Interlocked.CompareExchange(ref location1, num2, comparand);
        if (num1 != comparand)
          comparand = num1;
        else
          break;
      }
      return num1;
    }

    /// <summary>Bitwise "ors" two 64-bit unsigned integers and replaces the first integer with the result, as an atomic operation.</summary>
    /// <param name="location1">A variable containing the first value to be combined. The result is stored in <paramref name="location1" />.</param>
    /// <param name="value">The value to be combined with the integer at <paramref name="location1" />.</param>
    /// <exception cref="T:System.NullReferenceException">The address of <paramref name="location1" /> is a <see langword="null" /> pointer.</exception>
    /// <returns>The original value in <paramref name="location1" />.</returns>
    [CLSCompliant(false)]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Or(ref ulong location1, ulong value)
    {
      return (ulong) Interlocked.Or(ref Unsafe.As<ulong, long>(ref location1), (long) value);
    }

    /// <summary>Synchronizes memory access as follows: The processor that executes the current thread cannot reorder instructions in such a way that memory accesses before the call to <see cref="M:System.Threading.Interlocked.MemoryBarrier" /> execute after memory accesses that follow the call to <see cref="M:System.Threading.Interlocked.MemoryBarrier" />.</summary>
    [Intrinsic]
    public static void MemoryBarrier() => Interlocked.MemoryBarrier();

    [Intrinsic]
    internal static void ReadMemoryBarrier() => Interlocked.ReadMemoryBarrier();
  }
}
