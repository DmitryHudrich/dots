// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 738511E4-AE18-4299-8EC1-44314D4D70EA
// Location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.dll
// Sequence point data and variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.pdb

.class private auto ansi beforefieldinit
  Program
    extends [System.Runtime]System.Object
{
  .custom instance void [System.Runtime]System.Runtime.CompilerServices.CompilerGeneratedAttribute::.ctor()
    = (01 00 00 00 )

  .method private hidebysig static int32
    '<Main>$'(
      string[] args
    ) cil managed
  {
    .entrypoint
    .maxstack 3
    .locals init (
      [0] int32 num2
    )

    // [6 1 - 6 35]
    IL_0000: ldarg.0      // args
    IL_0001: ldc.i4.0
    IL_0002: ldelem.ref
    IL_0003: call         int32 [System.Runtime]System.Int32::Parse(string)

    // [7 1 - 7 35]
    IL_0008: ldarg.0      // args
    IL_0009: ldc.i4.1
    IL_000a: ldelem.ref
    IL_000b: call         int32 [System.Runtime]System.Int32::Parse(string)
    IL_0010: stloc.0      // num2

    // [9 1 - 9 20]
    IL_0011: ldloc.0      // num2
    IL_0012: add
    IL_0013: ret

  } // end of method Program::'<Main>$'

  .method public hidebysig specialname rtspecialname instance void
    .ctor() cil managed
  {
    .maxstack 8

    IL_0000: ldarg.0      // this
    IL_0001: call         instance void [System.Runtime]System.Object::.ctor()
    IL_0006: ret

  } // end of method Program::.ctor
} // end of class Program
