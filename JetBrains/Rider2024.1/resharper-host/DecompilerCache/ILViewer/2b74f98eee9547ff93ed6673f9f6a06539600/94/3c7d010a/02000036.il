// Type: System.Collections.Generic.Stack`1 
// Assembly: System.Collections, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 2B74F98E-EE95-47FF-93ED-6673F9F6A065
// Location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.5/System.Collections.dll
// Sequence point data and variable names from decompiler

.class public auto ansi serializable beforefieldinit
  System.Collections.Generic.Stack`1<T>
    extends [System.Private.CoreLib]System.Object
    implements
      class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!0/*T*/>,
      [System.Private.CoreLib]System.Collections.IEnumerable,
      [System.Private.CoreLib]System.Collections.ICollection,
      class [System.Private.CoreLib]System.Collections.Generic.IReadOnlyCollection`1<!0/*T*/>
{
  .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableContextAttribute::.ctor(unsigned int8)
    = (01 00 01 00 00 ) // .....
    // unsigned int8(1) // 0x01
  .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
    = (01 00 00 00 00 ) // .....
    // unsigned int8(0) // 0x00
  .custom instance void [System.Private.CoreLib]System.Diagnostics.DebuggerTypeProxyAttribute::.ctor(class [System.Private.CoreLib]System.Type)
    = (
      01 00 2b 53 79 73 74 65 6d 2e 43 6f 6c 6c 65 63 // ..+System.Collec
      74 69 6f 6e 73 2e 47 65 6e 65 72 69 63 2e 53 74 // tions.Generic.St
      61 63 6b 44 65 62 75 67 56 69 65 77 60 31 00 00 // ackDebugView`1..
    )
    // type(class System.Collections.Generic.StackDebugView`1)
  .custom instance void [System.Private.CoreLib]System.Diagnostics.DebuggerDisplayAttribute::.ctor(string)
    = (
      01 00 0f 43 6f 75 6e 74 20 3d 20 7b 43 6f 75 6e // ...Count = {Coun
      74 7d 00 00                                     // t}..
    )
    // string('Count = {Count}')
  .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.TypeForwardedFromAttribute::.ctor(string)
    = (
      01 00 49 53 79 73 74 65 6d 2c 20 56 65 72 73 69 // ..ISystem, Versi
      6f 6e 3d 34 2e 30 2e 30 2e 30 2c 20 43 75 6c 74 // on=4.0.0.0, Cult
      75 72 65 3d 6e 65 75 74 72 61 6c 2c 20 50 75 62 // ure=neutral, Pub
      6c 69 63 4b 65 79 54 6f 6b 65 6e 3d 62 37 37 61 // licKeyToken=b77a
      35 63 35 36 31 39 33 34 65 30 38 39 00 00       // 5c561934e089..
    )
    // string('System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089')
  .param type [1] /*T*/
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
      = (01 00 02 00 00 ) // .....
      // unsigned int8(2) // 0x02
  .interfaceimpl type class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!0/*T*/>
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8[])
      = (01 00 02 00 00 00 00 01 00 00 ) // ..........
      // unsigned int8[2]
        /*( unsigned int8(0) // 0x00
        unsigned int8(1) // 0x01
         )*/
  .interfaceimpl type [System.Private.CoreLib]System.Collections.IEnumerable
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
      = (01 00 00 00 00 ) // .....
      // unsigned int8(0) // 0x00
  .interfaceimpl type [System.Private.CoreLib]System.Collections.ICollection
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
      = (01 00 00 00 00 ) // .....
      // unsigned int8(0) // 0x00
  .interfaceimpl type class [System.Private.CoreLib]System.Collections.Generic.IReadOnlyCollection`1<!0/*T*/>
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8[])
      = (01 00 02 00 00 00 00 01 00 00 ) // ..........
      // unsigned int8[2]
        /*( unsigned int8(0) // 0x00
        unsigned int8(1) // 0x01
         )*/

  .class nested public sealed sequential ansi beforefieldinit
    Enumerator<T>
      extends [System.Private.CoreLib]System.ValueType
      implements
        class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/>,
        [System.Private.CoreLib]System.Collections.IEnumerator,
        [System.Private.CoreLib]System.IDisposable
  {
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableContextAttribute::.ctor(unsigned int8)
      = (01 00 00 00 00 ) // .....
      // unsigned int8(0) // 0x00
    .param type [1] /*T*/
      .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
        = (01 00 02 00 00 ) // .....
        // unsigned int8(2) // 0x02
    .interfaceimpl type class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/>
      .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8[])
        = (01 00 02 00 00 00 00 01 00 00 ) // ..........
        // unsigned int8[2]
          /*( unsigned int8(0) // 0x00
          unsigned int8(1) // 0x01
           )*/

    .field private initonly class System.Collections.Generic.Stack`1<!0/*T*/> _stack

    .field private initonly int32 _version

    .field private int32 _index

    .field private !0/*T*/ _currentElement

    .method assembly hidebysig specialname rtspecialname instance void
      .ctor(
        class System.Collections.Generic.Stack`1<!0/*T*/> stack
      ) cil managed
    {
      .maxstack 8

      // [322 9 - 322 28]
      IL_0000: ldarg.0      // this
      IL_0001: ldarg.1      // stack
      IL_0002: stfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack

      // [323 9 - 323 39]
      IL_0007: ldarg.0      // this
      IL_0008: ldarg.1      // stack
      IL_0009: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
      IL_000e: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_version

      // [324 9 - 324 25]
      IL_0013: ldarg.0      // this
      IL_0014: ldc.i4.s     -2 // 0xfe
      IL_0016: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index

      // [325 9 - 325 43]
      IL_001b: ldarg.0      // this
      IL_001c: ldflda       !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement
      IL_0021: initobj      !0/*T*/
      IL_0027: ret

    } // end of method Enumerator::.ctor

    .method public final hidebysig virtual newslot instance void
      Dispose() cil managed
    {
      .maxstack 8

      // [329 32 - 329 48]
      IL_0000: ldarg.0      // this
      IL_0001: ldc.i4.m1
      IL_0002: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0007: ret

    } // end of method Enumerator::Dispose

    .method public final hidebysig virtual newslot instance bool
      MoveNext() cil managed
    {
      .maxstack 3
      .locals (
        [0] bool 'flag[71-104], flag1[139-186]',
        [1] int32 V_1
      )

      // [337 9 - 337 51]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_version
      IL_0006: ldarg.0      // this
      IL_0007: ldfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack
      IL_000c: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
      IL_0011: beq.s        IL_001e

      // [338 11 - 338 86]
      IL_0013: call         string System.SR::get_InvalidOperation_EnumFailedVersion()
      IL_0018: newobj       instance void [System.Private.CoreLib]System.InvalidOperationException::.ctor(string)
      IL_001d: throw

      // [339 9 - 339 31]
      IL_001e: ldarg.0      // this
      IL_001f: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0024: ldc.i4.s     -2 // 0xfe
      IL_0026: bne.un.s     IL_0069

      // [341 11 - 341 46]
      IL_0028: ldarg.0      // this
      IL_0029: ldarg.0      // this
      IL_002a: ldfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack
      IL_002f: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_0034: ldc.i4.1
      IL_0035: sub
      IL_0036: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index

      // [342 11 - 342 39]
      IL_003b: ldarg.0      // this
      IL_003c: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0041: ldc.i4.0
      IL_0042: clt
      IL_0044: ldc.i4.0
      IL_0045: ceq
      IL_0047: stloc.0      // flag

      // [343 11 - 343 20]
      IL_0048: ldloc.0      // flag
      IL_0049: brfalse.s    IL_0067

      // [344 13 - 344 67]
      IL_004b: ldarg.0      // this
      IL_004c: ldarg.0      // this
      IL_004d: ldfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack
      IL_0052: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
      IL_0057: ldarg.0      // this
      IL_0058: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_005d: ldelem       !0/*T*/
      IL_0062: stfld        !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement

      // [345 11 - 345 23]
      IL_0067: ldloc.0      // flag
      IL_0068: ret

      // [347 9 - 347 31]
      IL_0069: ldarg.0      // this
      IL_006a: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_006f: ldc.i4.m1
      IL_0070: bne.un.s     IL_0074

      // [348 11 - 348 24]
      IL_0072: ldc.i4.0
      IL_0073: ret

      // [349 9 - 349 40]
      IL_0074: ldarg.0      // this
      IL_0075: ldarg.0      // this
      IL_0076: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_007b: ldc.i4.1
      IL_007c: sub
      IL_007d: stloc.1      // V_1
      IL_007e: ldloc.1      // V_1
      IL_007f: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0084: ldloc.1      // V_1
      IL_0085: ldc.i4.0
      IL_0086: clt
      IL_0088: ldc.i4.0
      IL_0089: ceq
      IL_008b: stloc.0      // flag1

      // [350 9 - 350 86]
      IL_008c: ldloc.0      // flag1
      IL_008d: brfalse.s    IL_00ad

      IL_008f: ldarg.0      // this
      IL_0090: ldarg.0      // this
      IL_0091: ldfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack
      IL_0096: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
      IL_009b: ldarg.0      // this
      IL_009c: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_00a1: ldelem       !0/*T*/
      IL_00a6: stfld        !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement
      IL_00ab: br.s         IL_00b9
      IL_00ad: ldarg.0      // this
      IL_00ae: ldflda       !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement
      IL_00b3: initobj      !0/*T*/

      // [351 9 - 351 22]
      IL_00b9: ldloc.0      // flag1
      IL_00ba: ret

    } // end of method Enumerator::MoveNext

    .method public final hidebysig virtual newslot specialname instance !0/*T*/
      get_Current() cil managed
    {
      .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableContextAttribute::.ctor(unsigned int8)
        = (01 00 01 00 00 ) // .....
        // unsigned int8(1) // 0x01
      .maxstack 8

      // [362 11 - 362 31]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0006: ldc.i4.0
      IL_0007: bge.s        IL_000f

      // [363 13 - 363 53]
      IL_0009: ldarg.0      // this
      IL_000a: call         instance void valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::ThrowEnumerationNotStartedOrEnded()

      // [364 11 - 364 39]
      IL_000f: ldarg.0      // this
      IL_0010: ldfld        !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement
      IL_0015: ret

    } // end of method Enumerator::get_Current

    .method private hidebysig instance void
      ThrowEnumerationNotStartedOrEnded() cil managed
    {
      .maxstack 8

      // [370 9 - 370 132]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index
      IL_0006: ldc.i4.s     -2 // 0xfe
      IL_0008: beq.s        IL_0011

      IL_000a: call         string System.SR::get_InvalidOperation_EnumEnded()
      IL_000f: br.s         IL_0016
      IL_0011: call         string System.SR::get_InvalidOperation_EnumNotStarted()
      IL_0016: newobj       instance void [System.Private.CoreLib]System.InvalidOperationException::.ctor(string)
      IL_001b: throw

    } // end of method Enumerator::ThrowEnumerationNotStartedOrEnded

    .method private final hidebysig virtual newslot specialname instance object
      System.Collections.IEnumerator.get_Current() cil managed
    {
      .override method instance object [System.Private.CoreLib]System.Collections.IEnumerator::get_Current()
      .maxstack 8

      // [376 38 - 376 59]
      IL_0000: ldarg.0      // this
      IL_0001: call         instance !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::get_Current()
      IL_0006: box          !0/*T*/
      IL_000b: ret

    } // end of method Enumerator::System.Collections.IEnumerator.get_Current

    .method private final hidebysig virtual newslot instance void
      System.Collections.IEnumerator.Reset() cil managed
    {
      .override method instance void [System.Private.CoreLib]System.Collections.IEnumerator::Reset()
      .maxstack 8

      // [382 9 - 382 51]
      IL_0000: ldarg.0      // this
      IL_0001: ldfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_version
      IL_0006: ldarg.0      // this
      IL_0007: ldfld        class System.Collections.Generic.Stack`1<!0/*T*/> valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_stack
      IL_000c: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
      IL_0011: beq.s        IL_001e

      // [383 11 - 383 86]
      IL_0013: call         string System.SR::get_InvalidOperation_EnumFailedVersion()
      IL_0018: newobj       instance void [System.Private.CoreLib]System.InvalidOperationException::.ctor(string)
      IL_001d: throw

      // [384 9 - 384 25]
      IL_001e: ldarg.0      // this
      IL_001f: ldc.i4.s     -2 // 0xfe
      IL_0021: stfld        int32 valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_index

      // [385 9 - 385 43]
      IL_0026: ldarg.0      // this
      IL_0027: ldflda       !0/*T*/ valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::_currentElement
      IL_002c: initobj      !0/*T*/
      IL_0032: ret

    } // end of method Enumerator::System.Collections.IEnumerator.Reset

    .property instance !0/*T*/ Current()
    {
      .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
        = (01 00 01 00 00 ) // .....
        // unsigned int8(1) // 0x01
      .get instance !0/*T*/ System.Collections.Generic.Stack`1/Enumerator::get_Current()
    } // end of property Enumerator::Current

    .property instance object 'System.Collections.IEnumerator.Current'()
    {
      .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
        = (01 00 02 00 00 ) // .....
        // unsigned int8(2) // 0x02
      .get instance object System.Collections.Generic.Stack`1/Enumerator::System.Collections.IEnumerator.get_Current()
    } // end of property Enumerator::System.Collections.IEnumerator.Current
  } // end of class Enumerator

  .field private !0/*T*/[] _array

  .field private int32 _size

  .field private int32 _version

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Private.CoreLib]System.Object::.ctor()

    // [29 23 - 29 53]
    IL_0006: ldarg.0      // this
    IL_0007: call         !!0/*T*/[] [System.Private.CoreLib]System.Array::Empty<!0/*T*/>()
    IL_000c: stfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0011: ret

  } // end of method Stack`1::.ctor

  .method public hidebysig specialname rtspecialname instance void
    .ctor(
      int32 capacity
    ) cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Private.CoreLib]System.Object::.ctor()

    // [37 7 - 37 84]
    IL_0006: ldarg.1      // capacity
    IL_0007: ldstr        "capacity"
    IL_000c: call         void [System.Private.CoreLib]System.ArgumentOutOfRangeException::ThrowIfNegative<int32>(!!0/*int32*/, string)

    // [38 7 - 38 36]
    IL_0011: ldarg.0      // this
    IL_0012: ldarg.1      // capacity
    IL_0013: newarr       !0/*T*/
    IL_0018: stfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_001d: ret

  } // end of method Stack`1::.ctor

  .method public hidebysig specialname rtspecialname instance void
    .ctor(
      class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!0/*T*/> collection
    ) cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Private.CoreLib]System.Object::.ctor()

    // [48 7 - 48 82]
    IL_0006: ldarg.1      // collection
    IL_0007: ldstr        "collection"
    IL_000c: call         void [System.Private.CoreLib]System.ArgumentNullException::ThrowIfNull(object, string)

    // [49 7 - 49 77]
    IL_0011: ldarg.0      // this
    IL_0012: ldarg.1      // collection
    IL_0013: ldarg.0      // this
    IL_0014: ldflda       int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0019: call         !!0/*T*/[] System.Collections.Generic.EnumerableHelpers::ToArray<!0/*T*/>(class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!!0/*T*/>, int32&)
    IL_001e: stfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0023: ret

  } // end of method Stack`1::.ctor

  .method public final hidebysig virtual newslot specialname instance int32
    get_Count() cil managed
  {
    .maxstack 8

    // [54 25 - 54 35]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: ret

  } // end of method Stack`1::get_Count

  .method private final hidebysig virtual newslot specialname instance bool
    System.Collections.ICollection.get_IsSynchronized() cil managed
  {
    .override method instance bool [System.Private.CoreLib]System.Collections.ICollection::get_IsSynchronized()
    .maxstack 8

    // [59 40 - 59 45]
    IL_0000: ldc.i4.0
    IL_0001: ret

  } // end of method Stack`1::System.Collections.ICollection.get_IsSynchronized

  .method private final hidebysig virtual newslot specialname instance object
    System.Collections.ICollection.get_SyncRoot() cil managed
  {
    .override method instance object [System.Private.CoreLib]System.Collections.ICollection::get_SyncRoot()
    .maxstack 8

    // [63 36 - 63 49]
    IL_0000: ldarg.0      // this
    IL_0001: ret

  } // end of method Stack`1::System.Collections.ICollection.get_SyncRoot

  .method public hidebysig instance void
    Clear() cil managed
  {
    .maxstack 8

    // [68 7 - 68 63]
    IL_0000: call         bool [System.Private.CoreLib]System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences<!0/*T*/>()
    IL_0005: brfalse.s    IL_0019

    // [69 9 - 69 56]
    IL_0007: ldarg.0      // this
    IL_0008: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000d: ldc.i4.0
    IL_000e: ldarg.0      // this
    IL_000f: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0014: call         void [System.Private.CoreLib]System.Array::Clear(class [System.Private.CoreLib]System.Array, int32, int32)

    // [70 7 - 70 21]
    IL_0019: ldarg.0      // this
    IL_001a: ldc.i4.0
    IL_001b: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size

    // [71 7 - 71 22]
    IL_0020: ldarg.0      // this
    IL_0021: ldarg.0      // this
    IL_0022: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_0027: ldc.i4.1
    IL_0028: add
    IL_0029: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_002e: ret

  } // end of method Stack`1::Clear

  .method public hidebysig instance bool
    Contains(
      !0/*T*/ item
    ) cil managed
  {
    .maxstack 8

    // [80 7 - 80 95]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: brfalse.s    IL_0023

    IL_0008: ldarg.0      // this
    IL_0009: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000e: ldarg.1      // item
    IL_000f: ldarg.0      // this
    IL_0010: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0015: ldc.i4.1
    IL_0016: sub
    IL_0017: call         int32 [System.Private.CoreLib]System.Array::LastIndexOf<!0/*T*/>(!!0/*T*/[], !!0/*T*/, int32)
    IL_001c: ldc.i4.m1
    IL_001d: ceq
    IL_001f: ldc.i4.0
    IL_0020: ceq
    IL_0022: ret
    IL_0023: ldc.i4.0
    IL_0024: ret

  } // end of method Stack`1::Contains

  .method public hidebysig instance void
    CopyTo(
      !0/*T*/[] 'array',
      int32 arrayIndex
    ) cil managed
  {
    .maxstack 6
    .locals (
      [0] int32 num1,
      [1] int32 num2
    )

    // [93 7 - 93 72]
    IL_0000: ldarg.1      // 'array'
    IL_0001: ldstr        "array"
    IL_0006: call         void [System.Private.CoreLib]System.ArgumentNullException::ThrowIfNull(object, string)

    // [94 7 - 94 55]
    IL_000b: ldarg.2      // arrayIndex
    IL_000c: ldc.i4.0
    IL_000d: blt.s        IL_0015

    IL_000f: ldarg.2      // arrayIndex
    IL_0010: ldarg.1      // 'array'
    IL_0011: ldlen
    IL_0012: conv.i4
    IL_0013: ble.s        IL_002b

    // [95 9 - 95 135]
    IL_0015: ldstr        "arrayIndex"
    IL_001a: ldarg.2      // arrayIndex
    IL_001b: box          [System.Private.CoreLib]System.Int32
    IL_0020: call         string System.SR::get_ArgumentOutOfRange_IndexMustBeLessOrEqual()
    IL_0025: newobj       instance void [System.Private.CoreLib]System.ArgumentOutOfRangeException::.ctor(string, object, string)
    IL_002a: throw

    // [96 7 - 96 50]
    IL_002b: ldarg.1      // 'array'
    IL_002c: ldlen
    IL_002d: conv.i4
    IL_002e: ldarg.2      // arrayIndex
    IL_002f: sub
    IL_0030: ldarg.0      // this
    IL_0031: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0036: bge.s        IL_0043

    // [97 9 - 97 64]
    IL_0038: call         string System.SR::get_Argument_InvalidOffLen()
    IL_003d: newobj       instance void [System.Private.CoreLib]System.ArgumentException::.ctor(string)
    IL_0042: throw

    // [98 7 - 98 19]
    IL_0043: ldc.i4.0
    IL_0044: stloc.0      // num1

    // [99 7 - 99 41]
    IL_0045: ldarg.2      // arrayIndex
    IL_0046: ldarg.0      // this
    IL_0047: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_004c: add
    IL_004d: stloc.1      // num2

    IL_004e: br.s         IL_006b
    // start of loop, entry point: IL_006b

      // [101 9 - 101 44]
      IL_0050: ldarg.1      // 'array'
      IL_0051: ldloc.1      // num2
      IL_0052: ldc.i4.1
      IL_0053: sub
      IL_0054: dup
      IL_0055: stloc.1      // num2
      IL_0056: ldarg.0      // this
      IL_0057: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
      IL_005c: ldloc.0      // num1
      IL_005d: dup
      IL_005e: ldc.i4.1
      IL_005f: add
      IL_0060: stloc.0      // num1
      IL_0061: ldelem       !0/*T*/
      IL_0066: stelem       !0/*T*/

      // [100 7 - 100 32]
      IL_006b: ldloc.0      // num1
      IL_006c: ldarg.0      // this
      IL_006d: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_0072: blt.s        IL_0050
    // end of loop
    IL_0074: ret

  } // end of method Stack`1::CopyTo

  .method private final hidebysig virtual newslot instance void
    System.Collections.ICollection.CopyTo(
      class [System.Private.CoreLib]System.Array 'array',
      int32 arrayIndex
    ) cil managed
  {
    .override method instance void [System.Private.CoreLib]System.Collections.ICollection::CopyTo(class [System.Private.CoreLib]System.Array, int32)
    .maxstack 5

    // [128 7 - 128 72]
    IL_0000: ldarg.1      // 'array'
    IL_0001: ldstr        "array"
    IL_0006: call         void [System.Private.CoreLib]System.ArgumentNullException::ThrowIfNull(object, string)

    // [129 7 - 129 27]
    IL_000b: ldarg.1      // 'array'
    IL_000c: callvirt     instance int32 [System.Private.CoreLib]System.Array::get_Rank()
    IL_0011: ldc.i4.1
    IL_0012: beq.s        IL_0024

    // [130 9 - 130 86]
    IL_0014: call         string System.SR::get_Arg_RankMultiDimNotSupported()
    IL_0019: ldstr        "array"
    IL_001e: newobj       instance void [System.Private.CoreLib]System.ArgumentException::.ctor(string, string)
    IL_0023: throw

    // [131 7 - 131 39]
    IL_0024: ldarg.1      // 'array'
    IL_0025: ldc.i4.0
    IL_0026: callvirt     instance int32 [System.Private.CoreLib]System.Array::GetLowerBound(int32)
    IL_002b: brfalse.s    IL_003d

    // [132 9 - 132 79]
    IL_002d: call         string System.SR::get_Arg_NonZeroLowerBound()
    IL_0032: ldstr        "array"
    IL_0037: newobj       instance void [System.Private.CoreLib]System.ArgumentException::.ctor(string, string)
    IL_003c: throw

    // [133 7 - 133 55]
    IL_003d: ldarg.2      // arrayIndex
    IL_003e: ldc.i4.0
    IL_003f: blt.s        IL_004a

    IL_0041: ldarg.2      // arrayIndex
    IL_0042: ldarg.1      // 'array'
    IL_0043: callvirt     instance int32 [System.Private.CoreLib]System.Array::get_Length()
    IL_0048: ble.s        IL_0060

    // [134 9 - 134 135]
    IL_004a: ldstr        "arrayIndex"
    IL_004f: ldarg.2      // arrayIndex
    IL_0050: box          [System.Private.CoreLib]System.Int32
    IL_0055: call         string System.SR::get_ArgumentOutOfRange_IndexMustBeLessOrEqual()
    IL_005a: newobj       instance void [System.Private.CoreLib]System.ArgumentOutOfRangeException::.ctor(string, object, string)
    IL_005f: throw

    // [135 7 - 135 50]
    IL_0060: ldarg.1      // 'array'
    IL_0061: callvirt     instance int32 [System.Private.CoreLib]System.Array::get_Length()
    IL_0066: ldarg.2      // arrayIndex
    IL_0067: sub
    IL_0068: ldarg.0      // this
    IL_0069: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_006e: bge.s        IL_007b

    // [136 9 - 136 64]
    IL_0070: call         string System.SR::get_Argument_InvalidOffLen()
    IL_0075: newobj       instance void [System.Private.CoreLib]System.ArgumentException::.ctor(string)
    IL_007a: throw
    IL_007b: nop
    .try
    {

      // [139 9 - 139 74]
      IL_007c: ldarg.0      // this
      IL_007d: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
      IL_0082: ldc.i4.0
      IL_0083: ldarg.1      // 'array'
      IL_0084: ldarg.2      // arrayIndex
      IL_0085: ldarg.0      // this
      IL_0086: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_008b: call         void [System.Private.CoreLib]System.Array::Copy(class [System.Private.CoreLib]System.Array, int32, class [System.Private.CoreLib]System.Array, int32, int32)

      // [140 9 - 140 53]
      IL_0090: ldarg.1      // 'array'
      IL_0091: ldarg.2      // arrayIndex
      IL_0092: ldarg.0      // this
      IL_0093: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_0098: call         void [System.Private.CoreLib]System.Array::Reverse(class [System.Private.CoreLib]System.Array, int32, int32)

      IL_009d: leave.s      IL_00b0
    } // end of .try
    catch [System.Private.CoreLib]System.ArrayTypeMismatchException
    {

      // [142 7 - 142 44]
      IL_009f: pop

      // [144 9 - 144 88]
      IL_00a0: call         string System.SR::get_Argument_IncompatibleArrayType()
      IL_00a5: ldstr        "array"
      IL_00aa: newobj       instance void [System.Private.CoreLib]System.ArgumentException::.ctor(string, string)
      IL_00af: throw
    } // end of catch

    IL_00b0: ret

  } // end of method Stack`1::System.Collections.ICollection.CopyTo

  .method public hidebysig instance valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>
    GetEnumerator() cil managed
  {
    .custom instance void [System.Private.CoreLib]System.Runtime.CompilerServices.NullableContextAttribute::.ctor(unsigned int8)
      = (01 00 00 00 00 ) // .....
      // unsigned int8(0) // 0x00
    .maxstack 8

    // [153 38 - 153 67]
    IL_0000: ldarg.0      // this
    IL_0001: newobj       instance void valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>::.ctor(class System.Collections.Generic.Stack`1<!0/*T*/>)
    IL_0006: ret

  } // end of method Stack`1::GetEnumerator

  .method private final hidebysig virtual newslot instance class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/>
    'System.Collections.Generic.IEnumerable<T>.GetEnumerator'() cil managed
  {
    .override method instance class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/> class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!0/*T*/>::GetEnumerator()
    .maxstack 1
    .locals (
      [0] class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/> V_0
    )

    // [159 7 - 159 114]
    IL_0000: ldarg.0      // this
    IL_0001: call         instance int32 class System.Collections.Generic.Stack`1<!0/*T*/>::get_Count()
    IL_0006: brfalse.s    IL_0016

    IL_0008: ldarg.0      // this
    IL_0009: call         instance valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/> class System.Collections.Generic.Stack`1<!0/*T*/>::GetEnumerator()
    IL_000e: box          valuetype System.Collections.Generic.Stack`1/Enumerator<!0/*T*/>
    IL_0013: stloc.0      // V_0
    IL_0014: ldloc.0      // V_0
    IL_0015: ret
    IL_0016: call         class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!!0/*T*/> System.Collections.Generic.EnumerableHelpers::GetEmptyEnumerator<!0/*T*/>()
    IL_001b: ret

  } // end of method Stack`1::'System.Collections.Generic.IEnumerable<T>.GetEnumerator'

  .method private final hidebysig virtual newslot instance class [System.Private.CoreLib]System.Collections.IEnumerator
    System.Collections.IEnumerable.GetEnumerator() cil managed
  {
    .override method instance class [System.Private.CoreLib]System.Collections.IEnumerator [System.Private.CoreLib]System.Collections.IEnumerable::GetEnumerator()
    .maxstack 8

    // [166 7 - 166 68]
    IL_0000: ldarg.0      // this
    IL_0001: callvirt     instance class [System.Private.CoreLib]System.Collections.Generic.IEnumerator`1<!0/*T*/> class [System.Private.CoreLib]System.Collections.Generic.IEnumerable`1<!0/*T*/>::GetEnumerator()
    IL_0006: ret

  } // end of method Stack`1::System.Collections.IEnumerable.GetEnumerator

  .method public hidebysig instance void
    TrimExcess() cil managed
  {
    .maxstack 2
    .locals (
      [0] int32 V_0
    )

    // [172 7 - 172 67]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0006: ldlen
    IL_0007: conv.i4
    IL_0008: conv.r8
    IL_0009: ldc.r8       0.9
    IL_0012: mul
    IL_0013: conv.i4
    IL_0014: stloc.0      // V_0

    IL_0015: ldarg.0      // this
    IL_0016: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_001b: ldloc.0      // V_0
    IL_001c: bge.s        IL_002f

    // [174 7 - 174 51]
    IL_001e: ldarg.0      // this
    IL_001f: ldflda       !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0024: ldarg.0      // this
    IL_0025: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_002a: call         void [System.Private.CoreLib]System.Array::Resize<!0/*T*/>(!!0/*T*/[]&, int32)

    IL_002f: ret

  } // end of method Stack`1::TrimExcess

  .method public hidebysig instance !0/*T*/
    Peek() cil managed
  {
    .maxstack 2
    .locals (
      [0] int32 index,
      [1] !0/*T*/[] 'array'
    )

    // [183 7 - 183 33]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: ldc.i4.1
    IL_0007: sub
    IL_0008: stloc.0      // index

    // [184 7 - 184 30]
    IL_0009: ldarg.0      // this
    IL_000a: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000f: stloc.1      // 'array'

    // [185 7 - 185 47]
    IL_0010: ldloc.0      // index
    IL_0011: ldloc.1      // 'array'
    IL_0012: ldlen
    IL_0013: conv.i4
    IL_0014: blt.un.s     IL_001c

    // [186 9 - 186 34]
    IL_0016: ldarg.0      // this
    IL_0017: call         instance void class System.Collections.Generic.Stack`1<!0/*T*/>::ThrowForEmptyStack()

    // [187 7 - 187 27]
    IL_001c: ldloc.1      // 'array'
    IL_001d: ldloc.0      // index
    IL_001e: ldelem       !0/*T*/
    IL_0023: ret

  } // end of method Stack`1::Peek

  .method public hidebysig instance bool
    TryPeek(
      [out] !0/*T*/& result
    ) cil managed
  {
    .param [1]
      .custom instance void [System.Private.CoreLib]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute::.ctor(bool)
        = (01 00 00 00 00 ) // .....
        // bool(false)
    .maxstack 3
    .locals (
      [0] int32 index,
      [1] !0/*T*/[] 'array'
    )

    // [196 7 - 196 33]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: ldc.i4.1
    IL_0007: sub
    IL_0008: stloc.0      // index

    // [197 7 - 197 30]
    IL_0009: ldarg.0      // this
    IL_000a: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000f: stloc.1      // 'array'

    // [198 7 - 198 47]
    IL_0010: ldloc.0      // index
    IL_0011: ldloc.1      // 'array'
    IL_0012: ldlen
    IL_0013: conv.i4
    IL_0014: blt.un.s     IL_001f

    // [200 9 - 200 29]
    IL_0016: ldarg.1      // result
    IL_0017: initobj      !0/*T*/

    // [201 9 - 201 22]
    IL_001d: ldc.i4.0
    IL_001e: ret

    // [203 7 - 203 28]
    IL_001f: ldarg.1      // result
    IL_0020: ldloc.1      // 'array'
    IL_0021: ldloc.0      // index
    IL_0022: ldelem       !0/*T*/
    IL_0027: stobj        !0/*T*/

    // [204 7 - 204 19]
    IL_002c: ldc.i4.1
    IL_002d: ret

  } // end of method Stack`1::TryPeek

  .method public hidebysig instance !0/*T*/
    Pop() cil managed
  {
    .maxstack 3
    .locals (
      [0] int32 index,
      [1] !0/*T*/[] 'array',
      [2] !0/*T*/ obj,
      [3] !0/*T*/ V_3
    )

    // [212 7 - 212 33]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: ldc.i4.1
    IL_0007: sub
    IL_0008: stloc.0      // index

    // [213 7 - 213 30]
    IL_0009: ldarg.0      // this
    IL_000a: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000f: stloc.1      // 'array'

    // [214 7 - 214 47]
    IL_0010: ldloc.0      // index
    IL_0011: ldloc.1      // 'array'
    IL_0012: ldlen
    IL_0013: conv.i4
    IL_0014: blt.un.s     IL_001c

    // [215 9 - 215 34]
    IL_0016: ldarg.0      // this
    IL_0017: call         instance void class System.Collections.Generic.Stack`1<!0/*T*/>::ThrowForEmptyStack()

    // [216 7 - 216 22]
    IL_001c: ldarg.0      // this
    IL_001d: ldarg.0      // this
    IL_001e: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_0023: ldc.i4.1
    IL_0024: add
    IL_0025: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version

    // [217 7 - 217 25]
    IL_002a: ldarg.0      // this
    IL_002b: ldloc.0      // index
    IL_002c: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size

    // [218 7 - 218 27]
    IL_0031: ldloc.1      // 'array'
    IL_0032: ldloc.0      // index
    IL_0033: ldelem       !0/*T*/
    IL_0038: stloc.2      // obj

    // [219 7 - 219 63]
    IL_0039: call         bool [System.Private.CoreLib]System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences<!0/*T*/>()
    IL_003e: brfalse.s    IL_0050

    // [220 9 - 220 35]
    IL_0040: ldloc.1      // 'array'
    IL_0041: ldloc.0      // index
    IL_0042: ldloca.s     V_3
    IL_0044: initobj      !0/*T*/
    IL_004a: ldloc.3      // V_3
    IL_004b: stelem       !0/*T*/

    // [221 7 - 221 18]
    IL_0050: ldloc.2      // obj
    IL_0051: ret

  } // end of method Stack`1::Pop

  .method public hidebysig instance bool
    TryPop(
      [out] !0/*T*/& result
    ) cil managed
  {
    .param [1]
      .custom instance void [System.Private.CoreLib]System.Diagnostics.CodeAnalysis.MaybeNullWhenAttribute::.ctor(bool)
        = (01 00 00 00 00 ) // .....
        // bool(false)
    .maxstack 3
    .locals (
      [0] int32 index,
      [1] !0/*T*/[] 'array',
      [2] !0/*T*/ V_2
    )

    // [230 7 - 230 33]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: ldc.i4.1
    IL_0007: sub
    IL_0008: stloc.0      // index

    // [231 7 - 231 30]
    IL_0009: ldarg.0      // this
    IL_000a: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000f: stloc.1      // 'array'

    // [232 7 - 232 47]
    IL_0010: ldloc.0      // index
    IL_0011: ldloc.1      // 'array'
    IL_0012: ldlen
    IL_0013: conv.i4
    IL_0014: blt.un.s     IL_001f

    // [234 9 - 234 29]
    IL_0016: ldarg.1      // result
    IL_0017: initobj      !0/*T*/

    // [235 9 - 235 22]
    IL_001d: ldc.i4.0
    IL_001e: ret

    // [237 7 - 237 22]
    IL_001f: ldarg.0      // this
    IL_0020: ldarg.0      // this
    IL_0021: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_0026: ldc.i4.1
    IL_0027: add
    IL_0028: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version

    // [238 7 - 238 25]
    IL_002d: ldarg.0      // this
    IL_002e: ldloc.0      // index
    IL_002f: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size

    // [239 7 - 239 28]
    IL_0034: ldarg.1      // result
    IL_0035: ldloc.1      // 'array'
    IL_0036: ldloc.0      // index
    IL_0037: ldelem       !0/*T*/
    IL_003c: stobj        !0/*T*/

    // [240 7 - 240 63]
    IL_0041: call         bool [System.Private.CoreLib]System.Runtime.CompilerServices.RuntimeHelpers::IsReferenceOrContainsReferences<!0/*T*/>()
    IL_0046: brfalse.s    IL_0058

    // [241 9 - 241 35]
    IL_0048: ldloc.1      // 'array'
    IL_0049: ldloc.0      // index
    IL_004a: ldloca.s     V_2
    IL_004c: initobj      !0/*T*/
    IL_0052: ldloc.2      // V_2
    IL_0053: stelem       !0/*T*/

    // [242 7 - 242 19]
    IL_0058: ldc.i4.1
    IL_0059: ret

  } // end of method Stack`1::TryPop

  .method public hidebysig instance void
    Push(
      !0/*T*/ item
    ) cil managed
  {
    .maxstack 3
    .locals (
      [0] int32 size,
      [1] !0/*T*/[] 'array'
    )

    // [249 7 - 249 28]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: stloc.0      // size

    // [250 7 - 250 30]
    IL_0007: ldarg.0      // this
    IL_0008: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_000d: stloc.1      // 'array'

    // [251 7 - 251 45]
    IL_000e: ldloc.0      // size
    IL_000f: ldloc.1      // 'array'
    IL_0010: ldlen
    IL_0011: conv.i4
    IL_0012: bge.un.s     IL_0034

    // [253 9 - 253 27]
    IL_0014: ldloc.1      // 'array'
    IL_0015: ldloc.0      // size
    IL_0016: ldarg.1      // item
    IL_0017: stelem       !0/*T*/

    // [254 9 - 254 24]
    IL_001c: ldarg.0      // this
    IL_001d: ldarg.0      // this
    IL_001e: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_0023: ldc.i4.1
    IL_0024: add
    IL_0025: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version

    // [255 9 - 255 30]
    IL_002a: ldarg.0      // this
    IL_002b: ldloc.0      // size
    IL_002c: ldc.i4.1
    IL_002d: add
    IL_002e: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0033: ret

    // [258 9 - 258 34]
    IL_0034: ldarg.0      // this
    IL_0035: ldarg.1      // item
    IL_0036: call         instance void class System.Collections.Generic.Stack`1<!0/*T*/>::PushWithResize(!0/*T*/)
    IL_003b: ret

  } // end of method Stack`1::Push

  .method private hidebysig instance void
    PushWithResize(
      !0/*T*/ item
    ) cil managed noinlining
  {
    .maxstack 8

    // [265 7 - 265 32]
    IL_0000: ldarg.0      // this
    IL_0001: ldarg.0      // this
    IL_0002: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0007: ldc.i4.1
    IL_0008: add
    IL_0009: call         instance void class System.Collections.Generic.Stack`1<!0/*T*/>::Grow(int32)

    // [266 7 - 266 37]
    IL_000e: ldarg.0      // this
    IL_000f: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0014: ldarg.0      // this
    IL_0015: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_001a: ldarg.1      // item
    IL_001b: stelem       !0/*T*/

    // [267 7 - 267 22]
    IL_0020: ldarg.0      // this
    IL_0021: ldarg.0      // this
    IL_0022: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version
    IL_0027: ldc.i4.1
    IL_0028: add
    IL_0029: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_version

    // [268 7 - 268 19]
    IL_002e: ldarg.0      // this
    IL_002f: ldarg.0      // this
    IL_0030: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0035: ldc.i4.1
    IL_0036: add
    IL_0037: stfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_003c: ret

  } // end of method Stack`1::PushWithResize

  .method public hidebysig instance int32
    EnsureCapacity(
      int32 capacity
    ) cil managed
  {
    .maxstack 8

    // [276 7 - 276 84]
    IL_0000: ldarg.1      // capacity
    IL_0001: ldstr        "capacity"
    IL_0006: call         void [System.Private.CoreLib]System.ArgumentOutOfRangeException::ThrowIfNegative<int32>(!!0/*int32*/, string)

    // [277 7 - 277 41]
    IL_000b: ldarg.0      // this
    IL_000c: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0011: ldlen
    IL_0012: conv.i4
    IL_0013: ldarg.1      // capacity
    IL_0014: bge.s        IL_001d

    // [278 9 - 278 28]
    IL_0016: ldarg.0      // this
    IL_0017: ldarg.1      // capacity
    IL_0018: call         instance void class System.Collections.Generic.Stack`1<!0/*T*/>::Grow(int32)

    // [279 7 - 279 33]
    IL_001d: ldarg.0      // this
    IL_001e: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0023: ldlen
    IL_0024: conv.i4
    IL_0025: ret

  } // end of method Stack`1::EnsureCapacity

  .method private hidebysig instance void
    Grow(
      int32 capacity
    ) cil managed
  {
    .maxstack 2
    .locals (
      [0] int32 newSize
    )

    // [284 7 - 284 73]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0006: ldlen
    IL_0007: brfalse.s    IL_0015

    IL_0009: ldc.i4.2
    IL_000a: ldarg.0      // this
    IL_000b: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0010: ldlen
    IL_0011: conv.i4
    IL_0012: mul
    IL_0013: br.s         IL_0016
    IL_0015: ldc.i4.4
    IL_0016: stloc.0      // newSize

    // [285 7 - 285 58]
    IL_0017: ldloc.0      // newSize
    IL_0018: conv.u8
    IL_0019: call         int32 [System.Private.CoreLib]System.Array::get_MaxLength()
    IL_001e: conv.i8
    IL_001f: ble.s        IL_0027

    // [286 9 - 286 34]
    IL_0021: call         int32 [System.Private.CoreLib]System.Array::get_MaxLength()
    IL_0026: stloc.0      // newSize

    // [287 7 - 287 30]
    IL_0027: ldloc.0      // newSize
    IL_0028: ldarg.1      // capacity
    IL_0029: bge.s        IL_002d

    // [288 9 - 288 27]
    IL_002b: ldarg.1      // capacity
    IL_002c: stloc.0      // newSize

    // [289 7 - 289 48]
    IL_002d: ldarg.0      // this
    IL_002e: ldflda       !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
    IL_0033: ldloc.0      // newSize
    IL_0034: call         void [System.Private.CoreLib]System.Array::Resize<!0/*T*/>(!!0/*T*/[]&, int32)
    IL_0039: ret

  } // end of method Stack`1::Grow

  .method public hidebysig instance !0/*T*/[]
    ToArray() cil managed
  {
    .maxstack 5
    .locals (
      [0] !0/*T*/[] 'array',
      [1] int32 index
    )

    // [297 7 - 297 27]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0006: brtrue.s     IL_000e

    // [298 9 - 298 33]
    IL_0008: call         !!0/*T*/[] [System.Private.CoreLib]System.Array::Empty<!0/*T*/>()
    IL_000d: ret

    // [299 7 - 299 36]
    IL_000e: ldarg.0      // this
    IL_000f: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
    IL_0014: newarr       !0/*T*/
    IL_0019: stloc.0      // 'array'

    // [300 12 - 300 25]
    IL_001a: ldc.i4.0
    IL_001b: stloc.1      // index

    IL_001c: br.s         IL_003e
    // start of loop, entry point: IL_003e

      // [301 9 - 301 59]
      IL_001e: ldloc.0      // 'array'
      IL_001f: ldloc.1      // index
      IL_0020: ldarg.0      // this
      IL_0021: ldfld        !0/*T*/[] class System.Collections.Generic.Stack`1<!0/*T*/>::_array
      IL_0026: ldarg.0      // this
      IL_0027: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_002c: ldloc.1      // index
      IL_002d: sub
      IL_002e: ldc.i4.1
      IL_002f: sub
      IL_0030: ldelem       !0/*T*/
      IL_0035: stelem       !0/*T*/

      // [300 47 - 300 54]
      IL_003a: ldloc.1      // index
      IL_003b: ldc.i4.1
      IL_003c: add
      IL_003d: stloc.1      // index

      // [300 27 - 300 45]
      IL_003e: ldloc.1      // index
      IL_003f: ldarg.0      // this
      IL_0040: ldfld        int32 class System.Collections.Generic.Stack`1<!0/*T*/>::_size
      IL_0045: blt.s        IL_001e
    // end of loop

    // [302 7 - 302 20]
    IL_0047: ldloc.0      // 'array'
    IL_0048: ret

  } // end of method Stack`1::ToArray

  .method private hidebysig instance void
    ThrowForEmptyStack() cil managed
  {
    .maxstack 8

    // [307 7 - 307 74]
    IL_0000: call         string System.SR::get_InvalidOperation_EmptyStack()
    IL_0005: newobj       instance void [System.Private.CoreLib]System.InvalidOperationException::.ctor(string)
    IL_000a: throw

  } // end of method Stack`1::ThrowForEmptyStack

  .property instance int32 Count()
  {
    .get instance int32 System.Collections.Generic.Stack`1::get_Count()
  } // end of property Stack`1::Count

  .property instance bool 'System.Collections.ICollection.IsSynchronized'()
  {
    .get instance bool System.Collections.Generic.Stack`1::System.Collections.ICollection.get_IsSynchronized()
  } // end of property Stack`1::System.Collections.ICollection.IsSynchronized

  .property instance object 'System.Collections.ICollection.SyncRoot'()
  {
    .get instance object System.Collections.Generic.Stack`1::System.Collections.ICollection.get_SyncRoot()
  } // end of property Stack`1::System.Collections.ICollection.SyncRoot
} // end of class System.Collections.Generic.Stack`1
