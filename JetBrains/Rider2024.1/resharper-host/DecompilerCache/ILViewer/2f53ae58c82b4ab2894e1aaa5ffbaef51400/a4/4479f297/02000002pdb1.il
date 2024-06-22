// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F53AE58-C82B-4AB2-894E-1AAA5FFBAEF5
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
    .maxstack 1
    .locals init (
      [0] int64* 'array'
    )

    // [2 1 - 2 2]
    IL_0000: nop

    // [3 5 - 3 39]
    IL_0001: ldc.i4       800 // 0x00000320
    IL_0006: conv.u
    IL_0007: localloc
    IL_0009: stloc.0      // 'array'

    // [4 1 - 4 2]
    IL_000a: nop
    IL_000b: ret

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
