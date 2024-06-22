// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3E223815-AB34-4347-8C86-61A857207E14
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

    IL_0002: br.s         IL_0012
    // start of loop, entry point: IL_0012

      // [4 1 - 4 2]
      IL_0004: nop

      // [5 5 - 5 32]
      IL_0005: ldc.i4.s     24 // 0x18
      IL_0007: call         void [System.Console]System.Console::WriteLine(int32)
      IL_000c: nop

      // [6 1 - 6 2]
      IL_000d: nop

      // [3 25 - 3 28]
      IL_000e: ldloc.0      // i
      IL_000f: ldc.i4.1
      IL_0010: add
      IL_0011: stloc.0      // i

      // [3 17 - 3 23]
      IL_0012: ldloc.0      // i
      IL_0013: ldc.i4.s     10 // 0x0a
      IL_0015: clt
      IL_0017: stloc.1      // V_1

      IL_0018: ldloc.1      // V_1
      IL_0019: brtrue.s     IL_0004
    // end of loop
    IL_001b: ret

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
