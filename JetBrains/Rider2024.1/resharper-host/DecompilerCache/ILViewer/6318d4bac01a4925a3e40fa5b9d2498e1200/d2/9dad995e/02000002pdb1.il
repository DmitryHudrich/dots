// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6318D4BA-C01A-4925-A3E4-0FA5B9D2498E
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
      [0] int32 i,
      [1] bool V_1
    )

    // [3 6 - 3 15]
    IL_0000: ldc.i4.0
    IL_0001: stloc.0      // i

    IL_0002: br.s         IL_0015
    // start of loop, entry point: IL_0015

      // [4 1 - 4 2]
      IL_0004: nop

      // [5 5 - 5 38]
      IL_0005: ldstr        "123123"
      IL_000a: call         void [System.Console]System.Console::WriteLine(string)
      IL_000f: nop

      // [6 1 - 6 2]
      IL_0010: nop

      // [3 25 - 3 28]
      IL_0011: ldloc.0      // i
      IL_0012: ldc.i4.1
      IL_0013: add
      IL_0014: stloc.0      // i

      // [3 17 - 3 23]
      IL_0015: ldloc.0      // i
      IL_0016: ldc.i4.s     10 // 0x0a
      IL_0018: clt
      IL_001a: stloc.1      // V_1

      IL_001b: ldloc.1      // V_1
      IL_001c: brtrue.s     IL_0004
    // end of loop
    IL_001e: ret

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
