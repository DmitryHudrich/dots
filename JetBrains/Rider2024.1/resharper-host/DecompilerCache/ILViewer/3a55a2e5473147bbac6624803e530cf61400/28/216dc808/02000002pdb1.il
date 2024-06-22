// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3A55A2E5-4731-47BB-AC66-24803E530CF6
// Location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.dll
// Sequence point data and variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Release/net8.0/ConsoleApp2.pdb

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
    .maxstack 8

    // [3 1 - 3 28]
    IL_0000: ldsfld       int32 Bebra::a
    IL_0005: call         void [System.Console]System.Console::WriteLine(int32)

    // [5 1 - 5 20]
    IL_000a: call         string [System.Console]System.Console::ReadLine()
    IL_000f: pop
    IL_0010: ret

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
