// Decompiled with JetBrains decompiler
// Type: System.Environment
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

#nullable enable
namespace System
{
  /// <summary>Provides information about, and means to manipulate, the current environment and platform. This class cannot be inherited.</summary>
  public static class Environment
  {
    private static volatile sbyte s_privilegedProcess;
    #nullable disable
    internal static string[] s_commandLineArgs;
    private static volatile int s_processId;
    private static volatile string s_processPath;
    private static volatile OperatingSystem s_osVersion;
    private static volatile int s_systemPageSize;

    /// <summary>Gets a unique identifier for the current managed thread.</summary>
    /// <returns>A unique identifier for this managed thread.</returns>
    public static extern int CurrentManagedThreadId { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    [LibraryImport("QCall", EntryPoint = "Environment_Exit")]
    [DoesNotReturn]
    [DllImport("QCall", EntryPoint = "Environment_Exit")]
    private static extern void _Exit(int exitCode);

    /// <summary>Terminates this process and returns an exit code to the operating system.</summary>
    /// <param name="exitCode">The exit code to return to the operating system. Use 0 (zero) to indicate that the process completed successfully.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have sufficient security permission to perform this function.</exception>
    [DoesNotReturn]
    public static void Exit(int exitCode) => Environment._Exit(exitCode);

    /// <summary>Gets or sets the exit code of the process.</summary>
    /// <returns>A 32-bit signed integer containing the exit code. The default value is 0 (zero), which indicates that the process completed successfully.</returns>
    public static extern int ExitCode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

    #nullable enable
    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FailFast(string? message);

    /// <summary>Immediately terminates a process after writing a message to the Windows Application event log, and then includes the message and exception information in error reporting to Microsoft.</summary>
    /// <param name="message">A message that explains why the process was terminated, or <see langword="null" /> if no explanation is provided.</param>
    /// <param name="exception">An exception that represents the error that caused the termination. This is typically the exception in a <see langword="catch" /> block.</param>
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void FailFast(string? message, Exception? exception);

    #nullable disable
    [DoesNotReturn]
    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void FailFast(string message, Exception exception, string errorMessage);

    private static unsafe string[] InitializeCommandLineArgs(char* exePath, int argc, char** argv)
    {
      string[] strArray1 = new string[argc + 1];
      string[] strArray2 = new string[argc];
      strArray1[0] = new string(exePath);
      for (int index = 0; index < strArray2.Length; ++index)
        strArray1[index + 1] = strArray2[index] = new string(argv[index]);
      Environment.s_commandLineArgs = strArray1;
      return strArray2;
    }

    [LibraryImport("QCall", EntryPoint = "Environment_GetProcessorCount")]
    [DllImport("QCall", EntryPoint = "Environment_GetProcessorCount")]
    private static extern int GetProcessorCount();

    internal static string GetResourceStringLocal(string key) => SR.GetResourceString(key);

    /// <summary>Gets the number of milliseconds elapsed since the system started.</summary>
    /// <returns>A 32-bit signed integer containing the amount of time in milliseconds that has passed since the last time the computer was started.</returns>
    public static extern int TickCount { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>Gets the number of milliseconds elapsed since the system started.</summary>
    /// <returns>The elapsed milliseconds since the system started.</returns>
    public static extern long TickCount64 { [MethodImpl(MethodImplOptions.InternalCall)] get; }

    /// <summary>Gets the number of processors available to the current process.</summary>
    /// <returns>The 32-bit signed integer that specifies the number of processors that are available.</returns>
    public static int ProcessorCount { get; } = Environment.GetProcessorCount();

    internal static bool IsSingleProcessor => Environment.ProcessorCount == 1;

    /// <summary>Gets a value that indicates whether the current process is authorized to perform security-relevant functions.</summary>
    public static bool IsPrivilegedProcess
    {
      get
      {
        sbyte num = Environment.s_privilegedProcess;
        if (num == (sbyte) 0)
          Environment.s_privilegedProcess = num = Environment.IsPrivilegedProcessCore() ? (sbyte) 1 : (sbyte) -1;
        return num > (sbyte) 0;
      }
    }

    /// <summary>Gets a value that indicates whether the current application domain is being unloaded or the common language runtime (CLR) is shutting down.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application domain is being unloaded or the CLR is shutting down; otherwise, <see langword="false" />.</returns>
    public static bool HasShutdownStarted => false;

    #nullable enable
    /// <summary>Retrieves the value of an environment variable from the current process.</summary>
    /// <param name="variable">The name of the environment variable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <returns>The value of the environment variable specified by <paramref name="variable" />, or <see langword="null" /> if the environment variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable)
    {
      ArgumentNullException.ThrowIfNull((object) variable, nameof (variable));
      return Environment.GetEnvironmentVariableCore(variable);
    }

    /// <summary>Retrieves the value of an environment variable from the current process or from the Windows operating system registry key for the current user or local machine.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values. Only <see cref="F:System.EnvironmentVariableTarget.Process" /> is supported on .NET running on Unix-based systems.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="target" /> is not a valid <see cref="T:System.EnvironmentVariableTarget" /> value.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <returns>The value of the environment variable specified by the <paramref name="variable" /> and <paramref name="target" /> parameters, or <see langword="null" /> if the environment variable is not found.</returns>
    public static string? GetEnvironmentVariable(string variable, EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Process)
        return Environment.GetEnvironmentVariable(variable);
      ArgumentNullException.ThrowIfNull((object) variable, nameof (variable));
      bool fromMachine = Environment.ValidateAndConvertRegistryTarget(target);
      return Environment.GetEnvironmentVariableFromRegistry(variable, fromMachine);
    }

    /// <summary>Retrieves all environment variable names and their values from the current process, or from the Windows operating system registry key for the current user or local machine.</summary>
    /// <param name="target">One of the <see cref="T:System.EnvironmentVariableTarget" /> values. Only <see cref="F:System.EnvironmentVariableTarget.Process" /> is supported on .NET running on Unix-based systems.</param>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation for the specified value of <paramref name="target" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="target" /> contains an illegal value.</exception>
    /// <returns>A dictionary that contains all environment variable names and their values from the source specified by the <paramref name="target" /> parameter; otherwise, an empty dictionary if no environment variables are found.</returns>
    public static IDictionary GetEnvironmentVariables(EnvironmentVariableTarget target)
    {
      return target == EnvironmentVariableTarget.Process ? Environment.GetEnvironmentVariables() : (IDictionary) Environment.GetEnvironmentVariablesFromRegistry(Environment.ValidateAndConvertRegistryTarget(target));
    }

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="variable" /> or <paramref name="value" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public static void SetEnvironmentVariable(string variable, string? value)
    {
      Environment.ValidateVariableAndValue(variable, ref value);
      Environment.SetEnvironmentVariableCore(variable, value);
    }

    /// <summary>Creates, modifies, or deletes an environment variable stored in the current process or in the Windows operating system registry key reserved for the current user or local machine.</summary>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="value">A value to assign to <paramref name="variable" />.</param>
    /// <param name="target">One of the enumeration values that specifies the location of the environment variable.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="variable" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="variable" /> contains a zero-length string, an initial hexadecimal zero character (0x00), or an equal sign ("=").
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="variable" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is not a member of the <see cref="T:System.EnvironmentVariableTarget" /> enumeration.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Machine" /> or <see cref="F:System.EnvironmentVariableTarget.User" />, and the length of <paramref name="variable" /> is greater than or equal to 255.
    /// 
    /// -or-
    /// 
    /// <paramref name="target" /> is <see cref="F:System.EnvironmentVariableTarget.Process" /> and the length of <paramref name="value" /> is greater than or equal to 32,767 characters.
    /// 
    /// -or-
    /// 
    /// An error occurred during the execution of this operation.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    public static void SetEnvironmentVariable(
      string variable,
      string? value,
      EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Process)
      {
        Environment.SetEnvironmentVariable(variable, value);
      }
      else
      {
        Environment.ValidateVariableAndValue(variable, ref value);
        Environment.ValidateAndConvertRegistryTarget(target);
      }
    }

    /// <summary>Returns a string array containing the command-line arguments for the current process.</summary>
    /// <exception cref="T:System.NotSupportedException">The system does not support command-line arguments.</exception>
    /// <returns>An array of strings where each element contains a command-line argument. The first element is the executable file name, and the following zero or more elements contain the remaining command-line arguments.</returns>
    public static string[] GetCommandLineArgs()
    {
      return Environment.s_commandLineArgs == null ? Environment.GetCommandLineArgsNative() : (string[]) Environment.s_commandLineArgs.Clone();
    }

    /// <summary>Gets the command line for this process.</summary>
    /// <returns>A string containing command-line arguments.</returns>
    public static string CommandLine
    {
      get => PasteArguments.Paste((IEnumerable<string>) Environment.GetCommandLineArgs(), true);
    }

    /// <summary>Gets or sets the fully qualified path of the current working directory.</summary>
    /// <exception cref="T:System.ArgumentException">Attempted to set to an empty string ("").</exception>
    /// <exception cref="T:System.ArgumentNullException">Attempted to set to <see langword="null" />.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
    /// <exception cref="T:System.IO.DirectoryNotFoundException">Attempted to set a local path that cannot be found.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the appropriate permission.</exception>
    /// <returns>The directory path.</returns>
    public static string CurrentDirectory
    {
      get => Environment.CurrentDirectoryCore;
      set
      {
        ArgumentException.ThrowIfNullOrEmpty(value, nameof (value));
        Environment.CurrentDirectoryCore = value;
      }
    }

    /// <summary>Replaces the name of each environment variable embedded in the specified string with the string equivalent of the value of the variable, then returns the resulting string.</summary>
    /// <param name="name">A string containing the names of zero or more environment variables. Each environment variable is quoted with the percent sign character (%).</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="name" /> is <see langword="null" />.</exception>
    /// <returns>A string with each environment variable replaced by its value.</returns>
    public static string ExpandEnvironmentVariables(string name)
    {
      ArgumentNullException.ThrowIfNull((object) name, nameof (name));
      return name.Length == 0 ? name : Environment.ExpandEnvironmentVariablesCore(name);
    }

    /// <summary>Gets the path to the specified system special folder.</summary>
    /// <param name="folder">One of the enumeration values that identifies a system special folder.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").
    /// 
    /// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    public static string GetFolderPath(Environment.SpecialFolder folder)
    {
      return Environment.GetFolderPath(folder, Environment.SpecialFolderOption.None);
    }

    /// <summary>Gets the path to the specified system special folder using a specified option for accessing special folders.</summary>
    /// <param name="folder">One of the enumeration values that identifies a system special folder.</param>
    /// <param name="option">One of the enumeration values that specifies options to use for accessing a special folder.</param>
    /// <exception cref="T:System.ArgumentException">
    ///         <paramref name="folder" /> is not a member of <see cref="T:System.Environment.SpecialFolder" />.
    /// 
    /// -or-
    /// 
    /// <paramref name="options" /> is not a member of <see cref="T:System.Environment.SpecialFolderOption" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The current platform is not supported.</exception>
    /// <returns>The path to the specified system special folder, if that folder physically exists on your computer; otherwise, an empty string ("").
    /// 
    /// A folder will not physically exist if the operating system did not create it, the existing folder was deleted, or the folder is a virtual directory, such as My Computer, which does not correspond to a physical path.</returns>
    public static string GetFolderPath(
      Environment.SpecialFolder folder,
      Environment.SpecialFolderOption option)
    {
      if (!Enum.IsDefined<Environment.SpecialFolder>(folder))
        throw new ArgumentOutOfRangeException(nameof (folder), (object) folder, SR.Format(SR.Arg_EnumIllegalVal, (object) folder));
      return option == Environment.SpecialFolderOption.None || Enum.IsDefined<Environment.SpecialFolderOption>(option) ? Environment.GetFolderPathCore(folder, option) : throw new ArgumentOutOfRangeException(nameof (option), (object) option, SR.Format(SR.Arg_EnumIllegalVal, (object) option));
    }

    /// <summary>Gets the unique identifier for the current process.</summary>
    /// <returns>A number that represents the unique identifier for the current process.</returns>
    public static int ProcessId
    {
      get
      {
        int processId = Environment.s_processId;
        if (processId == 0)
          Environment.s_processId = processId = Environment.GetProcessId();
        return processId;
      }
    }

    /// <summary>Returns the path of the executable that started the currently executing process. Returns <see langword="null" /> when the path is not available.</summary>
    /// <returns>The path of the executable that started the currently executing process.</returns>
    public static string? ProcessPath
    {
      get
      {
        string processPath = Environment.s_processPath;
        if (processPath == null)
        {
          Interlocked.CompareExchange<string>(ref Environment.s_processPath, Environment.GetProcessPath() ?? "", (string) null);
          processPath = Environment.s_processPath;
        }
        return processPath.Length == 0 ? (string) null : processPath;
      }
    }

    /// <summary>Gets a value that indicates whether the current process is a 64-bit process.</summary>
    /// <returns>
    /// <see langword="true" /> if the process is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitProcess => true;

    /// <summary>Gets a value that indicates whether the current operating system is a 64-bit operating system.</summary>
    /// <returns>
    /// <see langword="true" /> if the operating system is 64-bit; otherwise, <see langword="false" />.</returns>
    public static bool Is64BitOperatingSystem
    {
      get
      {
        if (true)
          ;
        return true;
      }
    }

    /// <summary>Gets the newline string defined for this environment.</summary>
    /// <returns>
    /// <see langword="\r\n" /> for non-Unix platforms, or <see langword="\n" /> for Unix platforms.</returns>
    public static string NewLine => "\n";

    /// <summary>Gets the current platform identifier and version number.</summary>
    /// <exception cref="T:System.InvalidOperationException">This property was unable to obtain the system version.
    /// 
    /// -or-
    /// 
    /// The obtained platform identifier is not a member of <see cref="T:System.PlatformID" /></exception>
    /// <returns>The platform identifier and version number.</returns>
    public static OperatingSystem OSVersion
    {
      get
      {
        OperatingSystem osVersion = Environment.s_osVersion;
        if (osVersion == null)
        {
          Interlocked.CompareExchange<OperatingSystem>(ref Environment.s_osVersion, Environment.GetOSVersion(), (OperatingSystem) null);
          osVersion = Environment.s_osVersion;
        }
        return osVersion;
      }
    }

    /// <summary>Gets a version consisting of the major, minor, build, and revision numbers of the common language runtime.</summary>
    /// <returns>The version of the common language runtime.</returns>
    public static Version Version
    {
      get
      {
        ReadOnlySpan<char> readOnlySpan = typeof (object).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.AsSpan();
        int length = readOnlySpan.IndexOfAny<char>('-', '+', ' ');
        if (length >= 0)
          readOnlySpan = readOnlySpan.Slice(0, length);
        Version result;
        return !Version.TryParse(readOnlySpan, out result) ? new Version() : result;
      }
    }

    /// <summary>Gets current stack trace information.</summary>
    /// <returns>A string containing stack trace information. This value can be <see cref="F:System.String.Empty" />.</returns>
    public static string StackTrace
    {
      [MethodImpl(MethodImplOptions.NoInlining)] get
      {
        return new System.Diagnostics.StackTrace(true).ToString(System.Diagnostics.StackTrace.TraceFormat.Normal);
      }
    }

    /// <summary>Gets the number of bytes in the operating system's memory page.</summary>
    /// <returns>The number of bytes in the system memory page.</returns>
    public static int SystemPageSize
    {
      get
      {
        int systemPageSize = Environment.s_systemPageSize;
        if (systemPageSize == 0)
          Environment.s_systemPageSize = systemPageSize = Environment.GetSystemPageSize();
        return systemPageSize;
      }
    }

    private static bool ValidateAndConvertRegistryTarget(EnvironmentVariableTarget target)
    {
      if (target == EnvironmentVariableTarget.Machine)
        return true;
      if (target == EnvironmentVariableTarget.User)
        return false;
      throw new ArgumentOutOfRangeException(nameof (target), (object) target, SR.Format(SR.Arg_EnumIllegalVal, (object) target));
    }

    #nullable disable
    private static void ValidateVariableAndValue(string variable, ref string value)
    {
      ArgumentException.ThrowIfNullOrEmpty(variable, nameof (variable));
      if (variable[0] == char.MinValue)
        throw new ArgumentException(SR.Argument_StringFirstCharIsZero, nameof (variable));
      if (variable.Contains('='))
        throw new ArgumentException(SR.Argument_IllegalEnvVarName, nameof (variable));
      if (!string.IsNullOrEmpty(value) && value[0] != char.MinValue)
        return;
      value = (string) null;
    }

    private static string GetEnvironmentVariableCore(string variable)
    {
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(stackalloc char[128]);
      uint environmentVariable;
      while ((long) (environmentVariable = Interop.Kernel32.GetEnvironmentVariable(variable, ref valueStringBuilder.GetPinnableReference(), (uint) valueStringBuilder.Capacity)) > (long) valueStringBuilder.Capacity)
        valueStringBuilder.EnsureCapacity((int) environmentVariable);
      if (environmentVariable == 0U && Marshal.GetLastPInvokeError() == 203)
      {
        valueStringBuilder.Dispose();
        return (string) null;
      }
      valueStringBuilder.Length = (int) environmentVariable;
      return valueStringBuilder.ToString();
    }

    internal static string GetEnvironmentVariableCore_NoArrayPool(string variable)
    {
      Span<char> span = stackalloc char[128];
      uint environmentVariable = Interop.Kernel32.GetEnvironmentVariable(variable, ref MemoryMarshal.GetReference<char>(span), (uint) span.Length);
      return environmentVariable <= 0U || (long) environmentVariable > (long) span.Length ? (string) null : span.Slice(0, (int) environmentVariable).ToString();
    }

    private static void SetEnvironmentVariableCore(string variable, string value)
    {
      if (Interop.Kernel32.SetEnvironmentVariable(variable, value))
        return;
      int lastPinvokeError = Marshal.GetLastPInvokeError();
      switch (lastPinvokeError)
      {
        case 8:
        case 1450:
          throw new OutOfMemoryException(Marshal.GetPInvokeErrorMessage(lastPinvokeError));
        case 203:
          break;
        case 206:
          throw new ArgumentException(SR.Argument_LongEnvVarValue);
        default:
          throw new ArgumentException(Marshal.GetPInvokeErrorMessage(lastPinvokeError));
      }
    }

    #nullable enable
    /// <summary>Retrieves all environment variable names and their values from the current process.</summary>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission to perform this operation.</exception>
    /// <exception cref="T:System.OutOfMemoryException">The buffer is out of memory.</exception>
    /// <returns>A dictionary that contains all environment variable names and their values; otherwise, an empty dictionary if no environment variables are found.</returns>
    public static unsafe IDictionary GetEnvironmentVariables()
    {
      char* environmentStringsW = Interop.Kernel32.GetEnvironmentStringsW();
      if ((IntPtr) environmentStringsW == IntPtr.Zero)
        throw new OutOfMemoryException();
      try
      {
        Hashtable environmentVariables = new Hashtable();
        char* chPtr = environmentStringsW;
        while (true)
        {
          ReadOnlySpan<char> fromNullTerminated = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(chPtr);
          if (!fromNullTerminated.IsEmpty)
          {
            int length = fromNullTerminated.IndexOf<char>('=');
            if (length > 0)
            {
              string key = new string(fromNullTerminated.Slice(0, length));
              string str = new string(fromNullTerminated.Slice(length + 1));
              try
              {
                environmentVariables.Add((object) key, (object) str);
              }
              catch (ArgumentException ex)
              {
              }
            }
            chPtr += fromNullTerminated.Length + 1;
          }
          else
            break;
        }
        return (IDictionary) environmentVariables;
      }
      finally
      {
        Interop.Kernel32.FreeEnvironmentStringsW(environmentStringsW);
      }
    }

    #nullable disable
    private static string GetEnvironmentVariableFromRegistry(string variable, bool fromMachine)
    {
      return (string) null;
    }

    private static Hashtable GetEnvironmentVariablesFromRegistry(bool fromMachine)
    {
      return new Hashtable();
    }

    /// <summary>Gets a value indicating whether the current process is running in user interactive mode.</summary>
    /// <returns>
    /// <see langword="true" /> if the current process is running in user interactive mode; otherwise, <see langword="false" />.</returns>
    public static bool UserInteractive => true;

    #nullable enable
    private static string CurrentDirectoryCore
    {
      get => Interop.Sys.GetCwd();
      set => Interop.CheckIo(Interop.Sys.ChDir(value), value, true);
    }

    #nullable disable
    private static string ExpandEnvironmentVariablesCore(string name)
    {
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(stackalloc char[128]);
      int num1 = 0;
      int num2;
      while (num1 < name.Length && (num2 = name.IndexOf('%', num1 + 1)) >= 0)
      {
        if (name[num1] == '%')
        {
          string environmentVariable = Environment.GetEnvironmentVariable(name.Substring(num1 + 1, num2 - num1 - 1));
          if (environmentVariable != null)
          {
            valueStringBuilder.Append(environmentVariable);
            num1 = num2 + 1;
            continue;
          }
        }
        valueStringBuilder.Append(name.AsSpan(num1, num2 - num1));
        num1 = num2;
      }
      valueStringBuilder.Append(name.AsSpan(num1));
      return valueStringBuilder.ToString();
    }

    #nullable enable
    /// <summary>Gets the fully qualified path of the system directory.</summary>
    /// <returns>A string containing a directory path.</returns>
    public static string SystemDirectory
    {
      get
      {
        return Environment.GetFolderPathCore(Environment.SpecialFolder.System, Environment.SpecialFolderOption.None);
      }
    }

    private static int GetSystemPageSize()
    {
      return Environment.CheckedSysConf(Interop.Sys.SysConfName._SC_PAGESIZE);
    }

    /// <summary>Gets the network domain name associated with the current user.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">The operating system does not support retrieving the network domain name.</exception>
    /// <exception cref="T:System.InvalidOperationException">The network domain name cannot be retrieved.</exception>
    /// <returns>The network domain name associated with the current user.</returns>
    public static string UserDomainName => Environment.MachineName;

    private static int CheckedSysConf(Interop.Sys.SysConfName name)
    {
      long num = Interop.Sys.SysConf(name);
      if (num == -1L)
      {
        Interop.ErrorInfo lastErrorInfo = Interop.Sys.GetLastErrorInfo();
        throw lastErrorInfo.Error == Interop.Error.EINVAL ? (Exception) new ArgumentOutOfRangeException(nameof (name), (object) name, lastErrorInfo.GetErrorMessage()) : Interop.GetIOException(lastErrorInfo);
      }
      return (int) num;
    }

    #nullable disable
    private static string GetFolderPathCore(
      Environment.SpecialFolder folder,
      Environment.SpecialFolderOption option)
    {
      string path = Environment.GetFolderPathCoreWithoutValidation(folder) ?? string.Empty;
      if (path.Length == 0 || option == Environment.SpecialFolderOption.DoNotVerify || Interop.Sys.Access(path, Interop.Sys.AccessMode.R_OK) == 0)
        return path;
      if (option == Environment.SpecialFolderOption.None)
        return string.Empty;
      Directory.CreateDirectory(path);
      return path;
    }

    private static string GetFolderPathCoreWithoutValidation(Environment.SpecialFolder folder)
    {
      if (folder == Environment.SpecialFolder.CommonApplicationData)
        return "/usr/share";
      if (folder == Environment.SpecialFolder.CommonTemplates)
        return "/usr/share/templates";
      string withoutValidation1 = (string) null;
      try
      {
        withoutValidation1 = PersistedFiles.GetHomeDirectory();
      }
      catch (Exception ex)
      {
      }
      if (string.IsNullOrEmpty(withoutValidation1))
        withoutValidation1 = "/";
      switch (folder)
      {
        case Environment.SpecialFolder.Desktop:
        case Environment.SpecialFolder.DesktopDirectory:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_DESKTOP_DIR", "Desktop");
        case Environment.SpecialFolder.Personal:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_DOCUMENTS_DIR", "Documents");
        case Environment.SpecialFolder.MyMusic:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_MUSIC_DIR", "Music");
        case Environment.SpecialFolder.MyVideos:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_VIDEOS_DIR", "Videos");
        case Environment.SpecialFolder.Fonts:
          return Path.Combine(withoutValidation1, ".fonts");
        case Environment.SpecialFolder.Templates:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_TEMPLATES_DIR", "Templates");
        case Environment.SpecialFolder.ApplicationData:
          return Environment.GetXdgConfig(withoutValidation1);
        case Environment.SpecialFolder.LocalApplicationData:
          string withoutValidation2 = Environment.GetEnvironmentVariable("XDG_DATA_HOME");
          if (withoutValidation2 == null || !withoutValidation2.StartsWith('/'))
            withoutValidation2 = Path.Combine(withoutValidation1, ".local", "share");
          return withoutValidation2;
        case Environment.SpecialFolder.MyPictures:
          return Environment.ReadXdgDirectory(withoutValidation1, "XDG_PICTURES_DIR", "Pictures");
        case Environment.SpecialFolder.UserProfile:
          return withoutValidation1;
        default:
          return string.Empty;
      }
    }

    private static string GetXdgConfig(string home)
    {
      string xdgConfig = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
      if (xdgConfig == null || !xdgConfig.StartsWith('/'))
        xdgConfig = Path.Combine(home, ".config");
      return xdgConfig;
    }

    private static string ReadXdgDirectory(string homeDir, string key, string fallback)
    {
      string environmentVariable = Environment.GetEnvironmentVariable(key);
      if (environmentVariable != null && environmentVariable.StartsWith('/'))
        return environmentVariable;
      string path = Path.Combine(Environment.GetXdgConfig(homeDir), "user-dirs.dirs");
      if (Interop.Sys.Access(path, Interop.Sys.AccessMode.R_OK) == 0)
      {
        try
        {
          using (StreamReader streamReader = new StreamReader(path))
          {
            string str;
            while ((str = streamReader.ReadLine()) != null)
            {
              int pos1 = 0;
              Environment.SkipWhitespace(str, ref pos1);
              if (pos1 < str.Length && string.CompareOrdinal(str, pos1, key, 0, key.Length) == 0)
              {
                int pos2 = pos1 + key.Length;
                Environment.SkipWhitespace(str, ref pos2);
                if (pos2 < str.Length - 4 && str[pos2] == '=')
                {
                  int pos3 = pos2 + 1;
                  Environment.SkipWhitespace(str, ref pos3);
                  if (pos3 < str.Length - 3 && str[pos3] == '"')
                  {
                    int num1 = pos3 + 1;
                    bool flag = false;
                    if (string.CompareOrdinal(str, num1, "$HOME/", 0, "$HOME/".Length) == 0)
                    {
                      flag = true;
                      num1 += "$HOME/".Length;
                    }
                    else if (str[num1] != '/')
                      continue;
                    int num2 = str.IndexOf('"', num1);
                    if (num2 > num1)
                    {
                      string path2 = str.Substring(num1, num2 - num1);
                      return flag ? Path.Combine(homeDir, path2) : path2;
                    }
                  }
                }
              }
            }
          }
        }
        catch (Exception ex)
        {
        }
      }
      return Path.Combine(homeDir, fallback);
    }

    private static void SkipWhitespace(string line, ref int pos)
    {
      while (pos < line.Length && char.IsWhiteSpace(line[pos]))
        ++pos;
    }

    #nullable enable
    /// <summary>Returns an array of string containing the names of the logical drives on the current computer.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permissions.</exception>
    /// <returns>An array of strings where each element contains the name of a logical drive. For example, if the computer's hard drive is the first logical drive, the first element returned is "C:\".</returns>
    public static string[] GetLogicalDrives() => Interop.Sys.GetAllMountPoints();

    /// <summary>Gets the NetBIOS name of this local computer.</summary>
    /// <exception cref="T:System.InvalidOperationException">The name of this computer cannot be obtained.</exception>
    /// <returns>The name of this computer.</returns>
    public static string MachineName
    {
      get
      {
        string hostName = Interop.Sys.GetHostName();
        int length = hostName.IndexOf('.');
        return length >= 0 ? hostName.Substring(0, length) : hostName;
      }
    }

    /// <summary>Gets the user name of the person who is associated with the current thread.</summary>
    /// <returns>The user name of the person who is associated with the current thread.</returns>
    public static string UserName => Interop.Sys.GetUserNameFromPasswd(Interop.Sys.GetEUid());

    private static bool IsPrivilegedProcessCore() => Interop.Sys.GetEUid() == 0U;

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int GetProcessId() => Interop.Sys.GetPid();

    #nullable disable
    [MethodImpl(MethodImplOptions.NoInlining)]
    private static string GetProcessPath() => Interop.Sys.GetProcessPath();

    private static string[] GetCommandLineArgsNative() => Array.Empty<string>();

    /// <summary>Gets the amount of physical memory mapped to the process context.</summary>
    /// <returns>A 64-bit signed integer containing the number of bytes of physical memory mapped to the process context.</returns>
    public static long WorkingSet
    {
      get
      {
        Interop.procfs.ParsedStatus result;
        return !Interop.procfs.TryReadStatusFile(Environment.ProcessId, out result) ? 0L : (long) result.VmRSS;
      }
    }

    private static OperatingSystem GetOSVersion()
    {
      return Environment.GetOperatingSystem(Interop.Sys.GetUnixRelease());
    }

    private static OperatingSystem GetOperatingSystem(string release)
    {
      int major = 0;
      int minor = 0;
      int build = 0;
      int revision = 0;
      if (release != null)
      {
        int pos = 0;
        major = Environment.FindAndParseNextNumber(release, ref pos);
        minor = Environment.FindAndParseNextNumber(release, ref pos);
        build = Environment.FindAndParseNextNumber(release, ref pos);
        revision = Environment.FindAndParseNextNumber(release, ref pos);
      }
      return new OperatingSystem(PlatformID.Unix, new Version(major, minor, build, revision));
    }

    private static int FindAndParseNextNumber(string text, ref int pos)
    {
      int num = text.AsSpan(pos).IndexOfAnyInRange<char>('0', '9');
      pos = num >= 0 ? pos + num : text.Length;
      int andParseNextNumber = 0;
      while ((uint) pos < (uint) text.Length)
      {
        char c = text[pos];
        if (char.IsAsciiDigit(c))
        {
          try
          {
            andParseNextNumber = checked (andParseNextNumber * 10 + (int) c - 48);
          }
          catch (OverflowException ex)
          {
            return int.MaxValue;
          }
          ++pos;
        }
        else
          break;
      }
      return andParseNextNumber;
    }

    #nullable enable
    /// <summary>Specifies enumerated constants used to retrieve directory paths to system special folders.</summary>
    public enum SpecialFolder
    {
      /// <summary>The logical Desktop rather than the physical file system location.</summary>
      Desktop = 0,
      /// <summary>The directory that contains the user's program groups.</summary>
      Programs = 2,
      /// <summary>The My Documents folder. This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.Personal" />.</summary>
      MyDocuments = 5,
      /// <summary>The directory that serves as a common repository for documents.  This member is equivalent to  <see cref="F:System.Environment.SpecialFolder.MyDocuments" />.</summary>
      Personal = 5,
      /// <summary>The directory that serves as a common repository for the user's favorite items.</summary>
      Favorites = 6,
      /// <summary>The directory that corresponds to the user's Startup program group. The system starts these programs whenever a user logs on or starts Windows.</summary>
      Startup = 7,
      /// <summary>The directory that contains the user's most recently used documents.</summary>
      Recent = 8,
      /// <summary>The directory that contains the Send To menu items.</summary>
      SendTo = 9,
      /// <summary>The directory that contains the Start menu items.</summary>
      StartMenu = 11, // 0x0000000B
      /// <summary>The My Music folder.</summary>
      MyMusic = 13, // 0x0000000D
      /// <summary>The file system directory that serves as a repository for videos that belong to a user.</summary>
      MyVideos = 14, // 0x0000000E
      /// <summary>The directory used to physically store file objects on the desktop. Do not confuse this directory with the desktop folder itself, which is a virtual folder.</summary>
      DesktopDirectory = 16, // 0x00000010
      /// <summary>The My Computer folder. When passed to the <see langword="Environment.GetFolderPath" /> method, the <see langword="MyComputer" /> enumeration member always yields the empty string ("") because no path is defined for the My Computer folder.</summary>
      MyComputer = 17, // 0x00000011
      /// <summary>A file system directory that contains the link objects that may exist in the My Network Places virtual folder.</summary>
      NetworkShortcuts = 19, // 0x00000013
      /// <summary>A virtual folder that contains fonts.</summary>
      Fonts = 20, // 0x00000014
      /// <summary>The directory that serves as a common repository for document templates.</summary>
      Templates = 21, // 0x00000015
      /// <summary>The file system directory that contains the programs and folders that appear on the Start menu for all users.</summary>
      CommonStartMenu = 22, // 0x00000016
      /// <summary>A folder for components that are shared across applications.</summary>
      CommonPrograms = 23, // 0x00000017
      /// <summary>The file system directory that contains the programs that appear in the Startup folder for all users.</summary>
      CommonStartup = 24, // 0x00000018
      /// <summary>The file system directory that contains files and folders that appear on the desktop for all users.</summary>
      CommonDesktopDirectory = 25, // 0x00000019
      /// <summary>The directory that serves as a common repository for application-specific data for the current roaming user. A roaming user works on more than one computer on a network. A roaming user's profile is kept on a server on the network and is loaded onto a system when the user logs on.</summary>
      ApplicationData = 26, // 0x0000001A
      /// <summary>The file system directory that contains the link objects that can exist in the Printers virtual folder.</summary>
      PrinterShortcuts = 27, // 0x0000001B
      /// <summary>The directory that serves as a common repository for application-specific data that is used by the current, non-roaming user.</summary>
      LocalApplicationData = 28, // 0x0000001C
      /// <summary>The directory that serves as a common repository for temporary Internet files.</summary>
      InternetCache = 32, // 0x00000020
      /// <summary>The directory that serves as a common repository for Internet cookies.</summary>
      Cookies = 33, // 0x00000021
      /// <summary>The directory that serves as a common repository for Internet history items.</summary>
      History = 34, // 0x00000022
      /// <summary>The directory that serves as a common repository for application-specific data that is used by all users.</summary>
      CommonApplicationData = 35, // 0x00000023
      /// <summary>The Windows directory or SYSROOT. This corresponds to the %windir% or %SYSTEMROOT% environment variables.</summary>
      Windows = 36, // 0x00000024
      /// <summary>The System directory.</summary>
      System = 37, // 0x00000025
      /// <summary>The program files directory.
      /// 
      /// In a non-x86 process, passing <see cref="F:System.Environment.SpecialFolder.ProgramFiles" /> to the <see cref="M:System.Environment.GetFolderPath(System.Environment.SpecialFolder)" /> method returns the path for non-x86 programs. To get the x86 program files directory in a non-x86 process, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
      ProgramFiles = 38, // 0x00000026
      /// <summary>The My Pictures folder.</summary>
      MyPictures = 39, // 0x00000027
      /// <summary>The user's profile folder. Applications should not create files or folders at this level; they should put their data under the locations referred to by <see cref="F:System.Environment.SpecialFolder.ApplicationData" />.</summary>
      UserProfile = 40, // 0x00000028
      /// <summary>The Windows System folder.</summary>
      SystemX86 = 41, // 0x00000029
      /// <summary>The x86 Program Files folder.</summary>
      ProgramFilesX86 = 42, // 0x0000002A
      /// <summary>The directory for components that are shared across applications.
      /// 
      /// To get the x86 common program files directory in a non-x86 process, use the <see cref="F:System.Environment.SpecialFolder.ProgramFilesX86" /> member.</summary>
      CommonProgramFiles = 43, // 0x0000002B
      /// <summary>The Program Files folder.</summary>
      CommonProgramFilesX86 = 44, // 0x0000002C
      /// <summary>The file system directory that contains the templates that are available to all users.</summary>
      CommonTemplates = 45, // 0x0000002D
      /// <summary>The file system directory that contains documents that are common to all users.</summary>
      CommonDocuments = 46, // 0x0000002E
      /// <summary>The file system directory that contains administrative tools for all users of the computer.</summary>
      CommonAdminTools = 47, // 0x0000002F
      /// <summary>The file system directory that is used to store administrative tools for an individual user. The Microsoft Management Console (MMC) will save customized consoles to this directory, and it will roam with the user.</summary>
      AdminTools = 48, // 0x00000030
      /// <summary>The file system directory that serves as a repository for music files common to all users.</summary>
      CommonMusic = 53, // 0x00000035
      /// <summary>The file system directory that serves as a repository for image files common to all users.</summary>
      CommonPictures = 54, // 0x00000036
      /// <summary>The file system directory that serves as a repository for video files common to all users.</summary>
      CommonVideos = 55, // 0x00000037
      /// <summary>The file system directory that contains resource data.</summary>
      Resources = 56, // 0x00000038
      /// <summary>The file system directory that contains localized resource data.</summary>
      LocalizedResources = 57, // 0x00000039
      /// <summary>This value is recognized in Windows Vista for backward compatibility, but the special folder itself is no longer used.</summary>
      CommonOemLinks = 58, // 0x0000003A
      /// <summary>The file system directory that acts as a staging area for files waiting to be written to a CD.</summary>
      CDBurning = 59, // 0x0000003B
    }

    /// <summary>Specifies options to use for getting the path to a special folder.</summary>
    public enum SpecialFolderOption
    {
      /// <summary>The path to the folder is verified. If the folder exists, the path is returned. If the folder does not exist, an empty string is returned. This is the default behavior.</summary>
      None = 0,
      /// <summary>The path to the folder is returned without verifying whether the path exists. If the folder is located on a network, specifying this option can reduce lag time.</summary>
      DoNotVerify = 16384, // 0x00004000
      /// <summary>The path to the folder is created if it does not already exist.</summary>
      Create = 32768, // 0x00008000
    }
  }
}
