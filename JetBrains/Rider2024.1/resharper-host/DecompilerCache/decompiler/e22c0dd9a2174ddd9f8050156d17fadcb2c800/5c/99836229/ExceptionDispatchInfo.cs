// Decompiled with JetBrains decompiler
// Type: System.Runtime.ExceptionServices.ExceptionDispatchInfo
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

#nullable enable
namespace System.Runtime.ExceptionServices
{
  /// <summary>Represents an exception whose state is captured at a certain point in code.</summary>
  public sealed class ExceptionDispatchInfo
  {
    #nullable disable
    private readonly Exception _exception;
    private readonly Exception.DispatchState _dispatchState;

    private ExceptionDispatchInfo(Exception exception)
    {
      this._exception = exception;
      this._dispatchState = exception.CaptureDispatchState();
    }

    #nullable enable
    /// <summary>Creates an <see cref="T:System.Runtime.ExceptionServices.ExceptionDispatchInfo" /> object that represents the specified exception at the current point in code.</summary>
    /// <param name="source">The exception whose state is captured, and which is represented by the returned object.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="source" /> is <see langword="null" />.</exception>
    /// <returns>An object that represents the specified exception at the current point in code.</returns>
    public static ExceptionDispatchInfo Capture(Exception source)
    {
      ArgumentNullException.ThrowIfNull((object) source, nameof (source));
      return new ExceptionDispatchInfo(source);
    }

    /// <summary>Gets the exception that's represented by the current instance.</summary>
    /// <returns>The exception that's represented by the current instance.</returns>
    public Exception SourceException => this._exception;

    /// <summary>Throws the exception that's represented by the current <see cref="T:System.Runtime.ExceptionServices.ExceptionDispatchInfo" /> object, after restoring the state that was saved when the exception was captured.</summary>
    [DoesNotReturn]
    [StackTraceHidden]
    public void Throw()
    {
      this._exception.RestoreDispatchState(in this._dispatchState);
      throw this._exception;
    }

    /// <summary>Throws the source exception, maintaining the original Watson information and augmenting rather than replacing the original stack trace.</summary>
    /// <param name="source">The exception whose state is captured, then rethrown.</param>
    [DoesNotReturn]
    [StackTraceHidden]
    public static void Throw(Exception source) => ExceptionDispatchInfo.Capture(source).Throw();

    /// <summary>Stores the current stack trace into the specified <see cref="T:System.Exception" /> instance.</summary>
    /// <param name="source">The unthrown exception.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="source" /> argument was previously thrown or previously had a stack trace stored into it.</exception>
    /// <returns>The <paramref name="source" /> exception instance with the stack trace included.</returns>
    [StackTraceHidden]
    public static Exception SetCurrentStackTrace(Exception source)
    {
      ArgumentNullException.ThrowIfNull((object) source, nameof (source));
      source.SetCurrentStackTrace();
      return source;
    }

    /// <summary>Stores the provided stack trace into the specified <see cref="T:System.Exception" /> instance.</summary>
    /// <param name="source">The unthrown exception.</param>
    /// <param name="stackTrace">The stack trace string to persist within <paramref name="source" />. This is normally acquired from the <see cref="P:System.Exception.StackTrace" /> property of the remote exception instance.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="source" /> or <paramref name="stackTrace" /> argument was <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="source" /> argument was previously thrown or previously had a stack trace stored into it.</exception>
    /// <returns>The <paramref name="source" /> exception instance.</returns>
    public static Exception SetRemoteStackTrace(Exception source, string stackTrace)
    {
      ArgumentNullException.ThrowIfNull((object) source, nameof (source));
      ArgumentNullException.ThrowIfNull((object) stackTrace, nameof (stackTrace));
      source.SetRemoteStackTrace(stackTrace);
      return source;
    }
  }
}
