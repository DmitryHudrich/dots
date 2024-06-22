// Type: Program 
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E4661B4C-435F-4CAF-B241-188D11A55D12
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

    // [3 1 - 3 43]
    IL_0000: ldc.i4.5
    IL_0001: newarr       [System.Runtime]System.Int32
    IL_0006: dup
    IL_0007: ldtoken      field valuetype '<PrivateImplementationDetails>'/'__StaticArrayInitTypeSize=20' '<PrivateImplementationDetails>'::'4F6ADDC9659D6FB90FE94B6688A79F2A1FA8D36EC43F8F3E1D9B6528C448A384'
    IL_000c: call         void [System.Runtime]System.Runtime.CompilerServices.RuntimeHelpers::InitializeArray(class [System.Runtime]System.Array, valuetype [System.Runtime]System.RuntimeFieldHandle)

    // [4 1 - 4 37]
    IL_0011: ldc.i4.1
    IL_0012: ldc.i4.3
    IL_0013: call         valuetype [System.Runtime]System.Span`1<!!0/*int32*/> [System.Memory]System.MemoryExtensions::AsSpan<int32>(!!0/*int32*/[], int32, int32)
    IL_0018: pop
    IL_0019: ret

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
