// Decompiled with JetBrains decompiler
// Type: System.Object
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

#nullable enable
namespace System
{
  /// <summary>Supports all classes in the .NET class hierarchy and provides low-level services to derived classes. This is the ultimate base class of all .NET classes; it is the root of the type hierarchy.</summary>
  [ClassInterface(ClassInterfaceType.AutoDispatch)]
  [ComVisible(true)]
  [TypeForwardedFrom("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Object
  {
    /// <summary>Gets the <see cref="T:System.Type" /> of the current instance.</summary>
    /// <returns>The exact runtime type of the current instance.</returns>
    [Intrinsic]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public extern Type GetType();

    /// <summary>Creates a shallow copy of the current <see cref="T:System.Object" />.</summary>
    /// <returns>A shallow copy of the current <see cref="T:System.Object" />.</returns>
    [Intrinsic]
    protected unsafe object MemberwiseClone()
    {
      object obj = RuntimeHelpers.AllocateUninitializedClone(this);
      UIntPtr rawObjectDataSize = RuntimeHelpers.GetRawObjectDataSize(obj);
      ref byte local1 = ref this.GetRawData();
      ref byte local2 = ref obj.GetRawData();
      if (RuntimeHelpers.GetMethodTable(obj)->ContainsGCPointers)
        Buffer.BulkMoveWithWriteBarrier(ref local2, ref local1, rawObjectDataSize);
      else
        Buffer.Memmove(ref local2, ref local1, rawObjectDataSize);
      return obj;
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
    [NonVersionable]
    public Object()
    {
    }

    /// <summary>Allows an object to try to free resources and perform other cleanup operations before it is reclaimed by garbage collection.</summary>
    [NonVersionable]
    ~Object()
    {
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public virtual string? ToString() => this.GetType().ToString();

    /// <summary>Determines whether the specified object is equal to the current object.</summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object  is equal to the current object; otherwise, <see langword="false" />.</returns>
    public virtual bool Equals(object? obj) => this == obj;

    /// <summary>Determines whether the specified object instances are considered equal.</summary>
    /// <param name="objA">The first object to compare.</param>
    /// <param name="objB">The second object to compare.</param>
    /// <returns>
    /// <see langword="true" /> if the objects are considered equal; otherwise, <see langword="false" />. If both <paramref name="objA" /> and <paramref name="objB" /> are null, the method returns <see langword="true" />.</returns>
    public static bool Equals(object? objA, object? objB)
    {
      if (objA == objB)
        return true;
      return objA != null && objB != null && objA.Equals(objB);
    }

    /// <summary>Determines whether the specified <see cref="T:System.Object" /> instances are the same instance.</summary>
    /// <param name="objA">The first object to compare.</param>
    /// <param name="objB">The second object  to compare.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="objA" /> is the same instance as <paramref name="objB" /> or if both are null; otherwise, <see langword="false" />.</returns>
    [NonVersionable]
    public static bool ReferenceEquals(object? objA, object? objB) => objA == objB;

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public virtual int GetHashCode() => RuntimeHelpers.GetHashCode(this);
  }
}
