// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6DA1C122-1099-4DE1-90FF-589BE65A5405
// Location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.dll
// Sequence point data and variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.pdb

.class private auto ansi beforefieldinit
  Program
    extends [System.Runtime]System.Object
{
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
    = (01 00 00 00 )

  .method private hidebysig static int32
    '<Main>$'(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 3
    .locals init (
      [0] int32 a,
      [1] int32 i
    )

    // [6 1 - 6 11]
    IL_0000: ldc.i4.0
    IL_0001: stloc.0      // a

    // [8 6 - 8 15]
    IL_0002: ldc.i4.0
    IL_0003: stloc.1      // i

    IL_0004: br.s         IL_0010
    // start of loop, entry point: IL_0010

      // [10 5 - 10 19]
      IL_0006: ldloc.0      // a
      IL_0007: ldloc.0      // a
      IL_0008: ldc.i4.2
      IL_0009: mul
      IL_000a: add
      IL_000b: stloc.0      // a

      // [8 25 - 8 28]
      IL_000c: ldloc.1      // i
      IL_000d: ldc.i4.1
      IL_000e: add
      IL_000f: stloc.1      // i

      // [8 17 - 8 23]
      IL_0010: ldloc.1      // i
      IL_0011: ldc.i4.s     10 // 0x0a
      IL_0013: blt.s        IL_0006
    // end of loop

    // [13 1 - 13 10]
    IL_0015: ldloc.0      // a
    IL_0016: ret

  } // end of method Program::'<Main>$'

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Runtime]System.Object::.ctor()
    IL_0006: ret

  } // end of method Program::.ctor
} // end of class Program
