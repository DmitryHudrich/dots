// Type: Program 
// Assembly: ConsoleApp9, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E9C2992E-A4AC-40DC-89D2-2473991D58D0
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
    .maxstack 4
    .locals init (
      [0] class [System.Runtime]System.Collections.Generic.IEnumerable`1<string> collection1
    )

    // [4 1 - 6 46]
    IL_0000: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Empty<string>()
    IL_0005: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_000a: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Append<string>(class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/>, !!0/*string*/)
    IL_000f: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_0014: call         class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/> [System.Linq]System.Linq.Enumerable::Append<string>(class [System.Runtime]System.Collections.Generic.IEnumerable`1<!!0/*string*/>, !!0/*string*/)
    IL_0019: stloc.0      // collection1

    // [8 1 - 12 3]
    IL_001a: ldc.i4.2
    IL_001b: newarr       [System.Runtime]System.String
    IL_0020: dup
    IL_0021: ldc.i4.0
    IL_0022: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_0027: stelem.ref
    IL_0028: dup
    IL_0029: ldc.i4.1
    IL_002a: ldstr        "DHHSDLKFHKLSHDFKLHSLKDHFLKSDF"
    IL_002f: stelem.ref
    IL_0030: newobj       instance void class '<>z__ReadOnlyArray`1'<string>::.ctor(!0/*string*/[])

    // [16 1 - 16 32]
    IL_0035: ldloc.0      // collection1
    IL_0036: call         void [System.Console]System.Console::WriteLine(object)

    // [17 1 - 17 32]
    IL_003b: call         void [System.Console]System.Console::WriteLine(object)
    IL_0040: ret

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
