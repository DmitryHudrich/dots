// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E5F31403-E5BA-438C-B910-43E3FA925652
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
      [0] int32 a,
      [1] int64* 'array',
      [2] int32 i,
      [3] bool V_3
    )

    // [2 1 - 2 2]
    IL_0000: nop

    // [3 5 - 3 15]
    IL_0001: ldc.i4.0
    IL_0002: stloc.0      // a

    // [4 10 - 4 19]
    IL_0003: ldc.i4.0
    IL_0004: stloc.2      // i

    IL_0005: br.s         IL_0011
    // start of loop, entry point: IL_0011

      // [5 5 - 5 6]
      IL_0007: nop

      // [6 9 - 6 16]
      IL_0008: ldloc.0      // a
      IL_0009: ldc.i4.1
      IL_000a: add
      IL_000b: stloc.0      // a

      // [7 5 - 7 6]
      IL_000c: nop

      // [4 29 - 4 32]
      IL_000d: ldloc.2      // i
      IL_000e: ldc.i4.1
      IL_000f: add
      IL_0010: stloc.2      // i

      // [4 21 - 4 27]
      IL_0011: ldloc.2      // i
      IL_0012: ldc.i4.s     10 // 0x0a
      IL_0014: clt
      IL_0016: stloc.3      // V_3

      IL_0017: ldloc.3      // V_3
      IL_0018: brtrue.s     IL_0007
    // end of loop

    // [8 5 - 8 39]
    IL_001a: ldc.i4       800 // 0x00000320
    IL_001f: conv.u
    IL_0020: localloc
    IL_0022: stloc.1      // 'array'

    // [9 1 - 9 2]
    IL_0023: nop
    IL_0024: ret

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
