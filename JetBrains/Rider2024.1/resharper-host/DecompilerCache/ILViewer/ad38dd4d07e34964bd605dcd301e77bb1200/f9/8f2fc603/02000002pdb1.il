// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AD38DD4D-07E3-4964-BD60-5DCD301E77BB
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
      [0] string bebra,
      [1] int32 i,
      [2] bool V_2
    )

    // [4 1 - 4 19]
    IL_0000: ldstr        "asd"
    IL_0005: stloc.0      // bebra

    // [6 6 - 6 15]
    IL_0006: ldc.i4.0
    IL_0007: stloc.1      // i

    IL_0008: br.s         IL_0017
    // start of loop, entry point: IL_0017

      // [7 1 - 7 2]
      IL_000a: nop

      // [8 5 - 8 30]
      IL_000b: ldloc.0      // bebra
      IL_000c: call         void [System.Console]System.Console::WriteLine(string)
      IL_0011: nop

      // [9 1 - 9 2]
      IL_0012: nop

      // [6 25 - 6 28]
      IL_0013: ldloc.1      // i
      IL_0014: ldc.i4.1
      IL_0015: add
      IL_0016: stloc.1      // i

      // [6 17 - 6 23]
      IL_0017: ldloc.1      // i
      IL_0018: ldc.i4.s     10 // 0x0a
      IL_001a: clt
      IL_001c: stloc.2      // V_2

      IL_001d: ldloc.2      // V_2
      IL_001e: brtrue.s     IL_000a
    // end of loop
    IL_0020: ret

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
