// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BDBFF2B5-09D5-4992-8375-49CBAAC09FD7
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
      [0] unsigned int8* 'array'
    )

    // [2 1 - 2 2]
    IL_0000: nop

    // [3 5 - 3 38]
    IL_0001: ldc.i4.s     100 // 0x64
    IL_0003: conv.u
    IL_0004: localloc
    IL_0006: stloc.0      // 'array'

    // [4 1 - 4 2]
    IL_0007: nop
    IL_0008: ret

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
