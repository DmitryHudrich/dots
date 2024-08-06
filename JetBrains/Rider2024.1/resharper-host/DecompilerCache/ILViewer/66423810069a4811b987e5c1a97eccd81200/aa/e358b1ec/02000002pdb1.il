// Type: Program 
// Assembly: ConsoleApp9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 66423810-069A-4811-B987-E5C1A97ECCD8
// Location: /home/w1thluv/RiderProjects/ConsoleApp9/ConsoleApp9/bin/Release/net8.0/ConsoleApp9.dll
// Sequence point data and variable names from /home/w1thluv/RiderProjects/ConsoleApp9/ConsoleApp9/bin/Release/net8.0/ConsoleApp9.pdb

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

    // [4 1 - 6 46]
    IL_0000: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Empty<string>()
    IL_0005: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_000a: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Append<string>(class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/>, !!0/*string*/)
    IL_000f: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_0014: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Append<string>(class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/>, !!0/*string*/)

    // [8 1 - 8 31]
    IL_0019: call         void [System.Console]System.Console::WriteLine(object)
    IL_001e: ret

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
