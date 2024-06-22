// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FEA48976-3639-4A02-BC8F-04ADBCBFA51E
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

    // [3 1 - 3 36]
    IL_0000: ldstr        "Hello, World!"
    IL_0005: call         void [System.Console]System.Console::WriteLine(string)
    IL_000a: nop

    // [5 6 - 5 15]
    IL_000b: ldc.i4.0
    IL_000c: stloc.0      // i

    IL_000d: br.s         IL_001d
    // start of loop, entry point: IL_001d

      // [6 1 - 6 2]
      IL_000f: nop

      // [7 5 - 7 27]
      IL_0010: ldc.i4.s     10 // 0x0a
      IL_0012: call         void [System.Console]System.Console::WriteLine(int32)
      IL_0017: nop

      // [8 1 - 8 2]
      IL_0018: nop

      // [5 25 - 5 28]
      IL_0019: ldloc.0      // i
      IL_001a: ldc.i4.1
      IL_001b: add
      IL_001c: stloc.0      // i

      // [5 17 - 5 23]
      IL_001d: ldloc.0      // i
      IL_001e: ldc.i4.s     10 // 0x0a
      IL_0020: clt
      IL_0022: stloc.1      // V_1

      IL_0023: ldloc.1      // V_1
      IL_0024: brtrue.s     IL_000f
    // end of loop
    IL_0026: ret

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
