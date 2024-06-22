// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55CB9DAB-C120-4E46-B83F-4E1124A4CCD9
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
      [0] int32 a,
      [1] native int* 'array'
    )

    // [3 1 - 3 2]
    IL_0000: nop

    // [5 1 - 5 11]
    IL_0001: ldc.i4.5
    IL_0002: stloc.0      // a

    // [7 1 - 7 34]
    IL_0003: ldc.i4.s     100 // 0x64
    IL_0005: conv.u
    IL_0006: sizeof       [System.Runtime]System.IntPtr
    IL_000c: mul.ovf.un
    IL_000d: localloc
    IL_000f: stloc.1      // 'array'

    // [8 1 - 8 2]
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
