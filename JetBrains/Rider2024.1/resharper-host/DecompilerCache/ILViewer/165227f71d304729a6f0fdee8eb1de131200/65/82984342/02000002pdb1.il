// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 165227F7-1D30-4729-A6F0-FDEE8EB1DE13
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
      [0] int32[] 'array'
    )

    // [3 1 - 3 26]
    IL_0000: ldc.i4.s     100 // 0x64
    IL_0002: newarr       [System.Runtime]System.Int32
    IL_0007: stloc.0      // 'array'

    // [5 1 - 5 29]
    IL_0008: ldloc.0      // 'array'
    IL_0009: ldc.i4.5
    IL_000a: ldelem.i4
    IL_000b: call         void [System.Console]System.Console::WriteLine(int32)
    IL_0010: nop
    IL_0011: ret

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
