// Decompiled with JetBrains decompiler
// Type: Program
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: E192D5EE-0DDD-46AC-BD79-31CC84D5E493
// Assembly location: /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Debug/net8.0/ConsoleApp2.dll
// Local variable names from /home/hudric/RiderProjects/ConsoleApp2/ConsoleApp2/bin/Debug/net8.0/ConsoleApp2.pdb
// Compiler-generated code is shown

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[CompilerGenerated]
internal class Program
{
  private static void <Main>$(string[] args)
  {
  }

  public Program()
  {
    base..ctor();
  }

  [NullableContext(1)]
  [AsyncStateMachine(typeof (Program.<<<Main>$>g__Bebra|0_0>d))]
  [DebuggerStepThrough]
  [CompilerGenerated]
  internal static Task <<Main>$>g__Bebra|0_0()
  {
    Program.<<<Main>$>g__Bebra|0_0>d stateMachine = new Program.<<<Main>$>g__Bebra|0_0>d();
    stateMachine.<>t__builder = AsyncTaskMethodBuilder.Create();
    stateMachine.<>1__state = -1;
    stateMachine.<>t__builder.Start<Program.<<<Main>$>g__Bebra|0_0>d>(ref stateMachine);
    return stateMachine.<>t__builder.Task;
  }

  [CompilerGenerated]
  private sealed class <<<Main>$>g__Bebra|0_0>d : IAsyncStateMachine
  {
    public int <>1__state;
    public AsyncTaskMethodBuilder <>t__builder;
    private TaskAwaiter <>u__1;

    public <<<Main>$>g__Bebra|0_0>d()
    {
      base..ctor();
    }

    void IAsyncStateMachine.MoveNext()
    {
      int num1 = this.<>1__state;
      try
      {
        TaskAwaiter awaiter1;
        int num2;
        TaskAwaiter awaiter2;
        if (num1 != 0)
        {
          if (num1 != 1)
          {
            awaiter1 = Task.Delay(5000).GetAwaiter();
            if (!awaiter1.IsCompleted)
            {
              this.<>1__state = num2 = 0;
              this.<>u__1 = awaiter1;
              Program.<<<Main>$>g__Bebra|0_0>d stateMachine = this;
              this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<<Main>$>g__Bebra|0_0>d>(ref awaiter1, ref stateMachine);
              return;
            }
          }
          else
          {
            awaiter2 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_9;
          }
        }
        else
        {
          awaiter1 = this.<>u__1;
          this.<>u__1 = new TaskAwaiter();
          this.<>1__state = num2 = -1;
        }
        awaiter1.GetResult();
        awaiter2 = Task.Delay(5000).GetAwaiter();
        if (!awaiter2.IsCompleted)
        {
          this.<>1__state = num2 = 1;
          this.<>u__1 = awaiter2;
          Program.<<<Main>$>g__Bebra|0_0>d stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<<Main>$>g__Bebra|0_0>d>(ref awaiter2, ref stateMachine);
          return;
        }
label_9:
        awaiter2.GetResult();
      }
      catch (Exception ex)
      {
        this.<>1__state = -2;
        this.<>t__builder.SetException(ex);
        return;
      }
      this.<>1__state = -2;
      this.<>t__builder.SetResult();
    }

    [DebuggerHidden]
    void IAsyncStateMachine.SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
    {
    }
  }
}
