// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EFADED41-7B92-4BE0-A542-235B97FE01C8
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
    .maxstack 3
    .locals init (
      [0] int32* 'array'
    )

    // [3 1 - 3 2]
    IL_0000: nop

    // [4 5 - 4 37]
    IL_0001: ldc.i4       400 // 0x00000190
    IL_0006: conv.u
    IL_0007: localloc
    IL_0009: stloc.0      // 'array'

    // [6 5 - 6 33]
    IL_000a: ldloc.0      // 'array'
    IL_000b: ldc.i4.5
    IL_000c: conv.i
    IL_000d: ldc.i4.4
    IL_000e: mul
    IL_000f: add
    IL_0010: ldind.i4
    IL_0011: call         void [System.Console]System.Console::WriteLine(int32)
    IL_0016: nop

    // [7 1 - 7 2]
    IL_0017: nop
    IL_0018: ret

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
