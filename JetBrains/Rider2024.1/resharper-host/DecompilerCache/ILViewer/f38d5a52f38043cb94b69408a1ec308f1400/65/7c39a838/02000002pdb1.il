// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F38D5A52-F380-43CB-94B6-9408A1EC308F
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
      [0] valuetype [System.Runtime]System.DateTime* 'array'
    )

    // [3 1 - 3 2]
    IL_0000: nop

    // [4 5 - 4 42]
    IL_0001: ldc.i4.s     100 // 0x64
    IL_0003: conv.u
    IL_0004: sizeof       [System.Runtime]System.DateTime
    IL_000a: mul.ovf.un
    IL_000b: localloc
    IL_000d: stloc.0      // 'array'

    // [5 1 - 5 2]
    IL_000e: nop
    IL_000f: ret

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
