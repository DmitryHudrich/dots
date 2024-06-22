// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 18F1E5BB-FAEC-446C-ADDF-CB9B4DBA9588
// Location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Debug/net8.0/ConsoleApp2.dll
// Sequence point data and variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Debug/net8.0/ConsoleApp2.pdb

.class private auto ansi beforefieldinit
  Program
    extends [System.Runtime]System.Object
{
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
    = (01 00 00 00 )

  .method private hidebysig static void
    '<Main>$'(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 2
    .locals init (
      [0] valuetype [System.Runtime]System.Span`1<valuetype [System.Runtime]System.DateTime> 'array',
      [1] int32 i,
      [2] valuetype [System.Runtime]System.Span`1<valuetype [System.Runtime]System.DateTime> V_2,
      [3] bool V_3
    )

    // [3 1 - 3 2]
    IL_0000: nop

    // [5 10 - 5 19]
    IL_0001: ldc.i4.0
    IL_0002: stloc.1      // i

    IL_0003: br.s         IL_0021
    // start of loop, entry point: IL_0021

      // [6 5 - 6 6]
      IL_0005: nop

      // [7 9 - 7 42]
      IL_0006: ldc.i4.s     100 // 0x64
      IL_0008: conv.u
      IL_0009: sizeof       [System.Runtime]System.DateTime
      IL_000f: mul.ovf.un
      IL_0010: localloc
      IL_0012: ldc.i4.s     100 // 0x64
      IL_0014: newobj       instance void valuetype [System.Runtime]System.Span`1<valuetype [System.Runtime]System.DateTime>::.ctor(void*, int32)
      IL_0019: stloc.2      // V_2
      IL_001a: ldloc.2      // V_2
      IL_001b: stloc.0      // 'array'

      // [8 5 - 8 6]
      IL_001c: nop

      // [5 34 - 5 37]
      IL_001d: ldloc.1      // i
      IL_001e: ldc.i4.1
      IL_001f: add
      IL_0020: stloc.1      // i

      // [5 21 - 5 32]
      IL_0021: ldloc.1      // i
      IL_0022: ldc.i4       1000000 // 0x000f4240
      IL_0027: clt
      IL_0029: stloc.3      // V_3

      IL_002a: ldloc.3      // V_3
      IL_002b: brtrue.s     IL_0005
    // end of loop

    // [9 1 - 9 2]
    IL_002d: nop
    IL_002e: ret

  } // end of method Program::'<Main>$'

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Runtime]System.Object::.ctor()
    IL_0006: nop
    IL_0007: ret

  } // end of method Program::.ctor
} // end of class Program
