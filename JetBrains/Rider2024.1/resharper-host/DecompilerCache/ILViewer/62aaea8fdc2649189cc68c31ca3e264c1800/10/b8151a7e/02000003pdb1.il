// Type: VectorAdditionBenchmark 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 62AAEA8F-DC26-4918-9CC6-8C31CA3E264C
// Location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.dll
// Sequence point data and variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.pdb

.class public auto ansi beforefieldinit
  VectorAdditionBenchmark
    extends [System.Runtime]System.Object
{
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableContextAttribute::.ctor(unsigned int8)
    = (01 00 01 00 00 ) // .....
    // unsigned int8(1) // 0x01
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.NullableAttribute::.ctor(unsigned int8)
    = (01 00 00 00 00 ) // .....
    // unsigned int8(0) // 0x00

  .field private float32[] array1

  .field private float32[] array2

  .field private float32[] result

  .field public int32 Size

  .method public hidebysig specialname instance float32[]
    get_Result() cil managed
  {
    .maxstack 8

    // [13 30 - 13 36]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        float32[] VectorAdditionBenchmark::result
    IL_0006: ret

  } // end of method VectorAdditionBenchmark::get_Result

  .method public hidebysig instance void
    Setup() cil managed
  {
    .maxstack 3
    .locals init (
      [0] class [System.Runtime]System.Random random,
      [1] int32 i
    )

    // [19 9 - 19 34]
    IL_0000: ldarg.0      // this
    IL_0001: ldarg.0      // this
    IL_0002: ldfld        int32 VectorAdditionBenchmark::Size
    IL_0007: newarr       [System.Runtime]System.Single
    IL_000c: stfld        float32[] VectorAdditionBenchmark::array1

    // [20 9 - 20 34]
    IL_0011: ldarg.0      // this
    IL_0012: ldarg.0      // this
    IL_0013: ldfld        int32 VectorAdditionBenchmark::Size
    IL_0018: newarr       [System.Runtime]System.Single
    IL_001d: stfld        float32[] VectorAdditionBenchmark::array2

    // [21 9 - 21 34]
    IL_0022: ldarg.0      // this
    IL_0023: ldarg.0      // this
    IL_0024: ldfld        int32 VectorAdditionBenchmark::Size
    IL_0029: newarr       [System.Runtime]System.Single
    IL_002e: stfld        float32[] VectorAdditionBenchmark::result

    // [23 9 - 23 35]
    IL_0033: newobj       instance void [System.Runtime]System.Random::.ctor()
    IL_0038: stloc.0      // random

    // [24 14 - 24 23]
    IL_0039: ldc.i4.0
    IL_003a: stloc.1      // i

    IL_003b: br.s         IL_005f
    // start of loop, entry point: IL_005f

      // [25 13 - 25 52]
      IL_003d: ldarg.0      // this
      IL_003e: ldfld        float32[] VectorAdditionBenchmark::array1
      IL_0043: ldloc.1      // i
      IL_0044: ldloc.0      // random
      IL_0045: callvirt     instance float64 [System.Runtime]System.Random::NextDouble()
      IL_004a: conv.r4
      IL_004b: stelem.r4

      // [26 13 - 26 52]
      IL_004c: ldarg.0      // this
      IL_004d: ldfld        float32[] VectorAdditionBenchmark::array2
      IL_0052: ldloc.1      // i
      IL_0053: ldloc.0      // random
      IL_0054: callvirt     instance float64 [System.Runtime]System.Random::NextDouble()
      IL_0059: conv.r4
      IL_005a: stelem.r4

      // [24 35 - 24 38]
      IL_005b: ldloc.1      // i
      IL_005c: ldc.i4.1
      IL_005d: add
      IL_005e: stloc.1      // i

      // [24 25 - 24 33]
      IL_005f: ldloc.1      // i
      IL_0060: ldarg.0      // this
      IL_0061: ldfld        int32 VectorAdditionBenchmark::Size
      IL_0066: blt.s        IL_003d
    // end of loop

    // [28 5 - 28 6]
    IL_0068: ret

  } // end of method VectorAdditionBenchmark::Setup

  .method public hidebysig instance void
    NormalArrayAddition() cil managed
  {
    .maxstack 5
    .locals init (
      [0] int32 i
    )

    // [31 14 - 31 23]
    IL_0000: ldc.i4.0
    IL_0001: stloc.0      // i

    IL_0002: br.s         IL_0021
    // start of loop, entry point: IL_0021

      // [32 13 - 32 47]
      IL_0004: ldarg.0      // this
      IL_0005: ldfld        float32[] VectorAdditionBenchmark::result
      IL_000a: ldloc.0      // i
      IL_000b: ldarg.0      // this
      IL_000c: ldfld        float32[] VectorAdditionBenchmark::array1
      IL_0011: ldloc.0      // i
      IL_0012: ldelem.r4
      IL_0013: ldarg.0      // this
      IL_0014: ldfld        float32[] VectorAdditionBenchmark::array2
      IL_0019: ldloc.0      // i
      IL_001a: ldelem.r4
      IL_001b: add
      IL_001c: stelem.r4

      // [31 35 - 31 38]
      IL_001d: ldloc.0      // i
      IL_001e: ldc.i4.1
      IL_001f: add
      IL_0020: stloc.0      // i

      // [31 25 - 31 33]
      IL_0021: ldloc.0      // i
      IL_0022: ldarg.0      // this
      IL_0023: ldfld        int32 VectorAdditionBenchmark::Size
      IL_0028: blt.s        IL_0004
    // end of loop

    // [34 5 - 34 6]
    IL_002a: ret

  } // end of method VectorAdditionBenchmark::NormalArrayAddition

  .method public hidebysig instance void
    SimdArrayAddition() cil managed
  {
    .maxstack 5
    .locals init (
      [0] float32* array2Ptr,
      [1] float32[] pinned V_1,
      [2] float32* array1Ptr,
      [3] float32[] pinned V_3,
      [4] float32* resultPtr,
      [5] float32[] pinned V_5,
      [6] int32 simdLength,
      [7] int32 i,
      [8] valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32> v2
    )

    // [37 16 - 37 41]
    IL_0000: ldarg.0      // this
    IL_0001: ldfld        float32[] VectorAdditionBenchmark::array2
    IL_0006: dup
    IL_0007: stloc.1      // V_1
    IL_0008: brfalse.s    IL_000f
    IL_000a: ldloc.1      // V_1
    IL_000b: ldlen
    IL_000c: conv.i4
    IL_000d: brtrue.s     IL_0014
    IL_000f: ldc.i4.0
    IL_0010: conv.u
    IL_0011: stloc.0      // array2Ptr
    IL_0012: br.s         IL_001d
    IL_0014: ldloc.1      // V_1
    IL_0015: ldc.i4.0
    IL_0016: ldelema      [System.Runtime]System.Single
    IL_001b: conv.u
    IL_001c: stloc.0      // array2Ptr

    // [38 20 - 38 45]
    IL_001d: ldarg.0      // this
    IL_001e: ldfld        float32[] VectorAdditionBenchmark::array1
    IL_0023: dup
    IL_0024: stloc.3      // V_3
    IL_0025: brfalse.s    IL_002c
    IL_0027: ldloc.3      // V_3
    IL_0028: ldlen
    IL_0029: conv.i4
    IL_002a: brtrue.s     IL_0031
    IL_002c: ldc.i4.0
    IL_002d: conv.u
    IL_002e: stloc.2      // array1Ptr
    IL_002f: br.s         IL_003a
    IL_0031: ldloc.3      // V_3
    IL_0032: ldc.i4.0
    IL_0033: ldelema      [System.Runtime]System.Single
    IL_0038: conv.u
    IL_0039: stloc.2      // array1Ptr

    // [39 24 - 39 49]
    IL_003a: ldarg.0      // this
    IL_003b: ldfld        float32[] VectorAdditionBenchmark::result
    IL_0040: dup
    IL_0041: stloc.s      V_5
    IL_0043: brfalse.s    IL_004b
    IL_0045: ldloc.s      V_5
    IL_0047: ldlen
    IL_0048: conv.i4
    IL_0049: brtrue.s     IL_0051
    IL_004b: ldc.i4.0
    IL_004c: conv.u
    IL_004d: stloc.s      resultPtr
    IL_004f: br.s         IL_005c
    IL_0051: ldloc.s      V_5
    IL_0053: ldc.i4.0
    IL_0054: ldelema      [System.Runtime]System.Single
    IL_0059: conv.u
    IL_005a: stloc.s      resultPtr

    // [40 21 - 40 41]
    IL_005c: call         bool [System.Runtime.Intrinsics]System.Runtime.Intrinsics.X86.Avx::get_IsSupported()
    IL_0061: brfalse      IL_00ec

    // [41 25 - 41 65]
    IL_0066: call         int32 valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32>::get_Count()
    IL_006b: stloc.s      simdLength

    // [42 25 - 42 35]
    IL_006d: ldc.i4.0
    IL_006e: stloc.s      i

    IL_0070: br.s         IL_00a1
    // start of loop, entry point: IL_00a1

      // [45 29 - 45 71]
      IL_0072: ldloc.2      // array1Ptr
      IL_0073: ldloc.s      i
      IL_0075: conv.i
      IL_0076: ldc.i4.4
      IL_0077: mul
      IL_0078: add
      IL_0079: call         valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32> [System.Runtime.Intrinsics]System.Runtime.Intrinsics.X86.Avx::LoadVector256(float32*)

      // [46 29 - 46 71]
      IL_007e: ldloc.0      // array2Ptr
      IL_007f: ldloc.s      i
      IL_0081: conv.i
      IL_0082: ldc.i4.4
      IL_0083: mul
      IL_0084: add
      IL_0085: call         valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32> [System.Runtime.Intrinsics]System.Runtime.Intrinsics.X86.Avx::LoadVector256(float32*)
      IL_008a: stloc.s      v2

      // [47 29 - 47 59]
      IL_008c: ldloc.s      v2
      IL_008e: call         valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32> [System.Runtime.Intrinsics]System.Runtime.Intrinsics.X86.Avx::Add(valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32>, valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<float32>)

      // [48 29 - 48 54]
      IL_0093: ldloc.s      resultPtr
      IL_0095: call         void [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256::Store<float32>(valuetype [System.Runtime.Intrinsics]System.Runtime.Intrinsics.Vector256`1<!!0/*float32*/>, !!0/*float32*/*)

      // [44 56 - 44 71]
      IL_009a: ldloc.s      i
      IL_009c: ldloc.s      simdLength
      IL_009e: add
      IL_009f: stloc.s      i

      // [44 32 - 44 54]
      IL_00a1: ldloc.s      i
      IL_00a3: ldarg.0      // this
      IL_00a4: ldfld        int32 VectorAdditionBenchmark::Size
      IL_00a9: ldloc.s      simdLength
      IL_00ab: sub
      IL_00ac: ble.s        IL_0072
    // end of loop

    IL_00ae: br.s         IL_00e0
    // start of loop, entry point: IL_00e0

      // [53 29 - 53 63]
      IL_00b0: ldarg.0      // this
      IL_00b1: ldfld        float32[] VectorAdditionBenchmark::result
      IL_00b6: ldloc.s      i
      IL_00b8: ldarg.0      // this
      IL_00b9: ldfld        float32[] VectorAdditionBenchmark::array1
      IL_00be: ldloc.s      i
      IL_00c0: ldelem.r4
      IL_00c1: ldarg.0      // this
      IL_00c2: ldfld        float32[] VectorAdditionBenchmark::array2
      IL_00c7: ldloc.s      i
      IL_00c9: ldelem.r4
      IL_00ca: add
      IL_00cb: stelem.r4

      // [54 29 - 54 58]
      IL_00cc: ldarg.0      // this
      IL_00cd: ldfld        float32[] VectorAdditionBenchmark::result
      IL_00d2: ldloc.s      i
      IL_00d4: ldelem.r4
      IL_00d5: call         void [System.Console]System.Console::WriteLine(float32)

      // [52 42 - 52 45]
      IL_00da: ldloc.s      i
      IL_00dc: ldc.i4.1
      IL_00dd: add
      IL_00de: stloc.s      i

      // [52 32 - 52 40]
      IL_00e0: ldloc.s      i
      IL_00e2: ldarg.0      // this
      IL_00e3: ldfld        int32 VectorAdditionBenchmark::Size
      IL_00e8: blt.s        IL_00b0
    // end of loop

    IL_00ea: br.s         IL_00f2

    // [58 25 - 58 67]
    IL_00ec: newobj       instance void [System.Runtime]System.PlatformNotSupportedException::.ctor()
    IL_00f1: throw

    IL_00f2: ldnull
    IL_00f3: stloc.s      V_5

    IL_00f5: ldnull
    IL_00f6: stloc.3      // V_3

    IL_00f7: ldnull
    IL_00f8: stloc.1      // V_1

    // [63 5 - 63 6]
    IL_00f9: ret

  } // end of method VectorAdditionBenchmark::SimdArrayAddition

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    // [16 5 - 16 31]
    IL_0000: ldarg.0      // this
    IL_0001: ldc.i4       1000000 // 0x000f4240
    IL_0006: stfld        int32 VectorAdditionBenchmark::Size
    IL_000b: ldarg.0      // this
    IL_000c: call         instance void [System.Runtime]System.Object::.ctor()
    IL_0011: ret

  } // end of method VectorAdditionBenchmark::.ctor

  .property instance float32[] Result()
  {
    .get instance float32[] VectorAdditionBenchmark::get_Result()
  } // end of property VectorAdditionBenchmark::Result
} // end of class VectorAdditionBenchmark
