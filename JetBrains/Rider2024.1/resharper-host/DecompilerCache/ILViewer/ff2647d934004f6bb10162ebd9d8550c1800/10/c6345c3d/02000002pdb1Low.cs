// Decompiled with JetBrains decompiler
// Type: Program
// Assembly: ConsoleApp2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FF2647D9-3400-4F6B-B101-62EBD9D8550C
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
  [AsyncStateMachine(typeof (Program.<<Main>$>d__0))]
  [DebuggerStepThrough]
  private static Task <Main>$(string[] args)
  {
    Program.<<Main>$>d__0 stateMachine = new Program.<<Main>$>d__0();
    stateMachine.<>t__builder = AsyncTaskMethodBuilder.Create();
    stateMachine.args = args;
    stateMachine.<>1__state = -1;
    stateMachine.<>t__builder.Start<Program.<<Main>$>d__0>(ref stateMachine);
    return stateMachine.<>t__builder.Task;
  }

  public Program()
  {
    base..ctor();
  }

  [DebuggerStepThrough]
  [SpecialName]
  private static void <Main>(string[] args)
  {
    Program.<Main>$(args).GetAwaiter().GetResult();
  }

  [CompilerGenerated]
  private sealed class <<Main>$>d__0 : IAsyncStateMachine
  {
    public int <>1__state;
    public AsyncTaskMethodBuilder <>t__builder;
    public string[] args;
    private TaskAwaiter <>u__1;

    public <<Main>$>d__0()
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
        TaskAwaiter awaiter3;
        TaskAwaiter awaiter4;
        TaskAwaiter awaiter5;
        TaskAwaiter awaiter6;
        switch (num1)
        {
          case 0:
            awaiter1 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            break;
          case 1:
            awaiter2 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_8;
          case 2:
            awaiter3 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_11;
          case 3:
            awaiter4 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_14;
          case 4:
            awaiter5 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_17;
          case 5:
            awaiter6 = this.<>u__1;
            this.<>u__1 = new TaskAwaiter();
            this.<>1__state = num2 = -1;
            goto label_20;
          default:
            awaiter1 = Task.Delay(5000).GetAwaiter();
            if (!awaiter1.IsCompleted)
            {
              this.<>1__state = num2 = 0;
              this.<>u__1 = awaiter1;
              Program.<<Main>$>d__0 stateMachine = this;
              this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter1, ref stateMachine);
              return;
            }
            break;
        }
        awaiter1.GetResult();
        awaiter2 = Task.Delay(5000).GetAwaiter();
        if (!awaiter2.IsCompleted)
        {
          this.<>1__state = num2 = 1;
          this.<>u__1 = awaiter2;
          Program.<<Main>$>d__0 stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter2, ref stateMachine);
          return;
        }
label_8:
        awaiter2.GetResult();
        awaiter3 = Task.Delay(5000).GetAwaiter();
        if (!awaiter3.IsCompleted)
        {
          this.<>1__state = num2 = 2;
          this.<>u__1 = awaiter3;
          Program.<<Main>$>d__0 stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter3, ref stateMachine);
          return;
        }
label_11:
        awaiter3.GetResult();
        awaiter4 = Task.Delay(5000).GetAwaiter();
        if (!awaiter4.IsCompleted)
        {
          this.<>1__state = num2 = 3;
          this.<>u__1 = awaiter4;
          Program.<<Main>$>d__0 stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter4, ref stateMachine);
          return;
        }
label_14:
        awaiter4.GetResult();
        awaiter5 = Task.Delay(5000).GetAwaiter();
        if (!awaiter5.IsCompleted)
        {
          this.<>1__state = num2 = 4;
          this.<>u__1 = awaiter5;
          Program.<<Main>$>d__0 stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter5, ref stateMachine);
          return;
        }
label_17:
        awaiter5.GetResult();
        awaiter6 = Task.Delay(5000).GetAwaiter();
        if (!awaiter6.IsCompleted)
        {
          this.<>1__state = num2 = 5;
          this.<>u__1 = awaiter6;
          Program.<<Main>$>d__0 stateMachine = this;
          this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Program.<<Main>$>d__0>(ref awaiter6, ref stateMachine);
          return;
        }
label_20:
        awaiter6.GetResult();
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
