// Decompiled with JetBrains decompiler
// Type: System.Diagnostics.Process
// Assembly: System.Diagnostics.Process, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: DFD8654E-01CB-439A-AE20-9D5E7800E079
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Diagnostics.Process.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Diagnostics.Process.xml

using Microsoft.Win32.SafeHandles;
using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Pipes;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace System.Diagnostics
{
  /// <summary>Provides access to local and remote processes and enables you to start and stop local system processes.</summary>
  [Designer("System.Diagnostics.Design.ProcessDesigner, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
  public class Process : Component, IDisposable
  {
    private bool _haveProcessId;
    private int _processId;
    private bool _haveProcessHandle;
    #nullable disable
    private SafeProcessHandle _processHandle;
    private bool _isRemoteMachine;
    private string _machineName;
    private ProcessInfo _processInfo;
    private ProcessThreadCollection _threads;
    private ProcessModuleCollection _modules;
    private bool _haveWorkingSetLimits;
    private IntPtr _minWorkingSet;
    private IntPtr _maxWorkingSet;
    private bool _haveProcessorAffinity;
    private IntPtr _processorAffinity;
    private bool _havePriorityClass;
    private ProcessPriorityClass _priorityClass;
    private ProcessStartInfo _startInfo;
    private bool _watchForExit;
    private bool _watchingForExit;
    private EventHandler _onExited;
    private bool _exited;
    private int _exitCode;
    private DateTime? _startTime;
    private DateTime _exitTime;
    private bool _haveExitTime;
    private bool _priorityBoostEnabled;
    private bool _havePriorityBoostEnabled;
    private bool _raisedOnExited;
    private RegisteredWaitHandle _registeredWaitHandle;
    private WaitHandle _waitHandle;
    private StreamReader _standardOutput;
    private StreamWriter _standardInput;
    private StreamReader _standardError;
    private bool _disposed;
    private bool _standardInputAccessed;
    private Process.StreamReadMode _outputStreamReadMode;
    private Process.StreamReadMode _errorStreamReadMode;
    internal AsyncStreamReader _output;
    internal AsyncStreamReader _error;
    internal bool _pendingOutputRead;
    internal bool _pendingErrorRead;
    private static int s_cachedSerializationSwitch;
    private static volatile bool s_initialized;
    private static readonly object s_initializedGate = new object();
    private static readonly ReaderWriterLockSlim s_processStartLock = new ReaderWriterLockSlim();
    private ProcessWaitState.Holder _waitStateHolder;
    private static long s_ticksPerSecond;
    private static int s_childrenUsingTerminalCount;
    private static long s_bootTimeTicks;

    #nullable enable
    /// <summary>Occurs each time an application writes a line to its redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream.</summary>
    public event DataReceivedEventHandler? OutputDataReceived;

    /// <summary>Occurs when an application writes to its redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream.</summary>
    public event DataReceivedEventHandler? ErrorDataReceived;

    /// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.Process" /> class.</summary>
    public Process()
    {
      if (this.GetType() == typeof (Process))
        GC.SuppressFinalize((object) this);
      this._machineName = ".";
      this._outputStreamReadMode = Process.StreamReadMode.Undefined;
      this._errorStreamReadMode = Process.StreamReadMode.Undefined;
    }

    #nullable disable
    private Process(
      string machineName,
      bool isRemoteMachine,
      int processId,
      ProcessInfo processInfo)
    {
      GC.SuppressFinalize((object) this);
      this._processInfo = processInfo;
      this._machineName = machineName;
      this._isRemoteMachine = isRemoteMachine;
      this._processId = processId;
      this._haveProcessId = true;
      this._outputStreamReadMode = Process.StreamReadMode.Undefined;
      this._errorStreamReadMode = Process.StreamReadMode.Undefined;
    }

    #nullable enable
    /// <summary>Gets the native handle to this process.</summary>
    /// <returns>The native handle to this process.</returns>
    public SafeProcessHandle SafeHandle
    {
      get
      {
        this.EnsureState(Process.State.Associated);
        return this.GetOrOpenProcessHandle();
      }
    }

    /// <summary>Gets the native handle of the associated process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process has not been started or has exited. The <see cref="P:System.Diagnostics.Process.Handle" /> property cannot be read because there is no process associated with this <see cref="T:System.Diagnostics.Process" /> instance.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Diagnostics.Process" /> instance has been attached to a running process but you do not have the necessary permissions to get a handle with full access rights.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.Handle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>The handle that the operating system assigned to the associated process when the process was started. The system uses this handle to keep track of process attributes.</returns>
    public IntPtr Handle => this.SafeHandle.DangerousGetHandle();

    private bool Associated => this._haveProcessId || this._haveProcessHandle;

    /// <summary>Gets the base priority of the associated process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process has exited.
    /// 
    /// -or-
    /// 
    /// The process has not started, so there is no process ID.</exception>
    /// <returns>The base priority, which is computed from the <see cref="P:System.Diagnostics.Process.PriorityClass" /> of the associated process.</returns>
    public int BasePriority
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.BasePriority;
      }
    }

    /// <summary>Gets the value that the associated process specified when it terminated.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process has not exited.
    /// 
    /// -or-
    /// 
    /// The process <see cref="P:System.Diagnostics.Process.Handle" /> is not valid.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.ExitCode" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>The code that the associated process specified when it terminated.</returns>
    public int ExitCode
    {
      get
      {
        this.EnsureState(Process.State.Exited);
        return this._exitCode;
      }
    }

    /// <summary>Gets a value indicating whether the associated process has been terminated.</summary>
    /// <exception cref="T:System.InvalidOperationException">There is no process associated with the object.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">The exit code for the process could not be retrieved.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.HasExited" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>
    /// <see langword="true" /> if the operating system process referenced by the <see cref="T:System.Diagnostics.Process" /> component has terminated; otherwise, <see langword="false" />.</returns>
    public bool HasExited
    {
      get
      {
        if (!this._exited)
        {
          this.EnsureState(Process.State.Associated);
          this.UpdateHasExited();
          if (this._exited)
            this.RaiseOnExited();
        }
        return this._exited;
      }
    }

    /// <summary>Gets the time that the associated process was started.</summary>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.StartTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process has exited.
    /// 
    /// -or-
    /// 
    /// The process has not been started.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred in the call to the Windows function.</exception>
    /// <returns>An object  that indicates when the process started. An exception is thrown if the process is not running.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public DateTime StartTime
    {
      get
      {
        if (!this._startTime.HasValue)
          this._startTime = new DateTime?(this.StartTimeCore);
        return this._startTime.Value;
      }
    }

    /// <summary>Gets the time that the associated process exited.</summary>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.ExitTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>A <see cref="T:System.DateTime" /> that indicates when the associated process was terminated.</returns>
    public DateTime ExitTime
    {
      get
      {
        if (!this._haveExitTime)
        {
          this.EnsureState(Process.State.Exited);
          this._exitTime = this.ExitTimeCore;
          this._haveExitTime = true;
        }
        return this._exitTime;
      }
    }

    /// <summary>Gets the unique identifier for the associated process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process's <see cref="P:System.Diagnostics.Process.Id" /> property has not been set.
    /// 
    /// -or-
    /// 
    /// There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>The system-generated unique identifier of the process that is referenced by this <see cref="T:System.Diagnostics.Process" /> instance.</returns>
    public int Id
    {
      get
      {
        this.EnsureState(Process.State.HaveId);
        return this._processId;
      }
    }

    /// <summary>Gets the name of the computer the associated process is running on.</summary>
    /// <exception cref="T:System.InvalidOperationException">There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>The name of the computer that the associated process is running on.</returns>
    public string MachineName
    {
      get
      {
        this.EnsureState(Process.State.Associated);
        return this._machineName;
      }
    }

    /// <summary>Gets or sets the maximum allowable working set size, in bytes, for the associated process.</summary>
    /// <exception cref="T:System.ArgumentException">The maximum working set size is invalid. It must be greater than or equal to the minimum working set size.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">Working set information cannot be retrieved from the associated process resource.
    /// 
    /// -or-
    /// 
    /// The process identifier or process handle is zero because the process has not been started.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MaxWorkingSet" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.
    /// 
    /// -or-
    /// 
    /// The process has exited.</exception>
    /// <returns>The maximum working set size that is allowed in memory for the process, in bytes.</returns>
    public IntPtr MaxWorkingSet
    {
      [UnsupportedOSPlatform("ios"), UnsupportedOSPlatform("tvos"), SupportedOSPlatform("maccatalyst")] get
      {
        this.EnsureWorkingSetLimits();
        return this._maxWorkingSet;
      }
      [SupportedOSPlatform("freebsd"), SupportedOSPlatform("macos"), SupportedOSPlatform("maccatalyst"), SupportedOSPlatform("windows")] set
      {
        this.SetWorkingSetLimits(new IntPtr?(), new IntPtr?(value));
      }
    }

    /// <summary>Gets or sets the minimum allowable working set size, in bytes, for the associated process.</summary>
    /// <exception cref="T:System.ArgumentException">The minimum working set size is invalid. It must be less than or equal to the maximum working set size.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">Working set information cannot be retrieved from the associated process resource.
    /// 
    /// -or-
    /// 
    /// The process identifier or process handle is zero because the process has not been started.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MinWorkingSet" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.
    /// 
    /// -or-
    /// 
    /// The process has exited.</exception>
    /// <returns>The minimum working set size that is required in memory for the process, in bytes.</returns>
    public IntPtr MinWorkingSet
    {
      [UnsupportedOSPlatform("ios"), UnsupportedOSPlatform("tvos"), SupportedOSPlatform("maccatalyst")] get
      {
        this.EnsureWorkingSetLimits();
        return this._minWorkingSet;
      }
      [SupportedOSPlatform("freebsd"), SupportedOSPlatform("macos"), SupportedOSPlatform("maccatalyst"), SupportedOSPlatform("windows")] set
      {
        this.SetWorkingSetLimits(new IntPtr?(value), new IntPtr?());
      }
    }

    /// <summary>Gets the modules that have been loaded by the associated process.</summary>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.Modules" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">You are attempting to access the <see cref="P:System.Diagnostics.Process.Modules" /> property for either the system process or the idle process. These processes do not have modules.</exception>
    /// <returns>An array of type <see cref="T:System.Diagnostics.ProcessModule" /> that represents the modules that have been loaded by the associated process.</returns>
    public ProcessModuleCollection Modules
    {
      get
      {
        if (this._modules == null)
        {
          this.EnsureState(Process.State.HaveNonExitedId | Process.State.IsLocal);
          this._modules = ProcessManager.GetModules(this._processId);
        }
        return this._modules;
      }
    }

    /// <summary>Gets the amount of nonpaged system memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of system memory, in bytes, allocated for the associated process that cannot be written to the virtual memory paging file.</returns>
    public long NonpagedSystemMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.PoolNonPagedBytes;
      }
    }

    /// <summary>Gets the amount of nonpaged system memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of memory, in bytes, the system has allocated for the associated process that cannot be written to the virtual memory paging file.</returns>
    [Obsolete("Process.NonpagedSystemMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.NonpagedSystemMemorySize64 instead.")]
    public int NonpagedSystemMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.PoolNonPagedBytes;
      }
    }

    /// <summary>Gets the amount of paged memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of memory, in bytes, allocated in the virtual memory paging file for the associated process.</returns>
    public long PagedMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.PageFileBytes;
      }
    }

    /// <summary>Gets the amount of paged memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of memory, in bytes, allocated by the associated process that can be written to the virtual memory paging file.</returns>
    [Obsolete("Process.PagedMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PagedMemorySize64 instead.")]
    public int PagedMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.PageFileBytes;
      }
    }

    /// <summary>Gets the amount of pageable system memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of system memory, in bytes, allocated for the associated process that can be written to the virtual memory paging file.</returns>
    public long PagedSystemMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.PoolPagedBytes;
      }
    }

    /// <summary>Gets the amount of pageable system memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of memory, in bytes, the system has allocated for the associated process that can be written to the virtual memory paging file.</returns>
    [Obsolete("Process.PagedSystemMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PagedSystemMemorySize64 instead.")]
    public int PagedSystemMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.PoolPagedBytes;
      }
    }

    /// <summary>Gets the maximum amount of memory in the virtual memory paging file, in bytes, used by the associated process.</summary>
    /// <returns>The maximum amount of memory, in bytes, allocated in the virtual memory paging file for the associated process since it was started.</returns>
    public long PeakPagedMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.PageFileBytesPeak;
      }
    }

    /// <summary>Gets the maximum amount of memory in the virtual memory paging file, in bytes, used by the associated process.</summary>
    /// <returns>The maximum amount of memory, in bytes, allocated by the associated process that could be written to the virtual memory paging file.</returns>
    [Obsolete("Process.PeakPagedMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakPagedMemorySize64 instead.")]
    public int PeakPagedMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.PageFileBytesPeak;
      }
    }

    /// <summary>Gets the maximum amount of physical memory, in bytes, used by the associated process.</summary>
    /// <returns>The maximum amount of physical memory, in bytes, allocated for the associated process since it was started.</returns>
    public long PeakWorkingSet64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.WorkingSetPeak;
      }
    }

    /// <summary>Gets the peak working set size for the associated process, in bytes.</summary>
    /// <returns>The maximum amount of physical memory that the associated process has required all at once, in bytes.</returns>
    [Obsolete("Process.PeakWorkingSet has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakWorkingSet64 instead.")]
    public int PeakWorkingSet
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.WorkingSetPeak;
      }
    }

    /// <summary>Gets the maximum amount of virtual memory, in bytes, used by the associated process.</summary>
    /// <returns>The maximum amount of virtual memory, in bytes, allocated for the associated process since it was started.</returns>
    public long PeakVirtualMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.VirtualBytesPeak;
      }
    }

    /// <summary>Gets the maximum amount of virtual memory, in bytes, used by the associated process.</summary>
    /// <returns>The maximum amount of virtual memory, in bytes, that the associated process has requested.</returns>
    [Obsolete("Process.PeakVirtualMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PeakVirtualMemorySize64 instead.")]
    public int PeakVirtualMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.VirtualBytesPeak;
      }
    }

    /// <summary>Gets or sets a value indicating whether the associated process priority should temporarily be boosted by the operating system when the main window has the focus.</summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception">Priority boost information could not be retrieved from the associated process resource.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The process identifier or process handle is zero. (The process has not been started.)</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PriorityBoostEnabled" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
    /// <returns>
    /// <see langword="true" /> if dynamic boosting of the process priority should take place for a process when it is taken out of the wait state; otherwise, <see langword="false" />. The default is <see langword="false" />.</returns>
    public bool PriorityBoostEnabled
    {
      get
      {
        if (!this._havePriorityBoostEnabled)
        {
          this._priorityBoostEnabled = false;
          this._havePriorityBoostEnabled = true;
        }
        return this._priorityBoostEnabled;
      }
      set
      {
        Process.PriorityBoostEnabledCore = value;
        this._priorityBoostEnabled = value;
        this._havePriorityBoostEnabled = true;
      }
    }

    /// <summary>Gets or sets the overall priority category for the associated process.</summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception">Process priority information could not be set or retrieved from the associated process resource.
    /// 
    /// -or-
    /// 
    /// The process identifier or process handle is zero. (The process has not been started.)</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PriorityClass" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.</exception>
    /// <exception cref="T:System.ComponentModel.InvalidEnumArgumentException">Priority class cannot be set because it does not use a valid value, as defined in the <see cref="T:System.Diagnostics.ProcessPriorityClass" /> enumeration.</exception>
    /// <returns>The priority category for the associated process, from which the <see cref="P:System.Diagnostics.Process.BasePriority" /> of the process is calculated.</returns>
    public ProcessPriorityClass PriorityClass
    {
      get
      {
        if (!this._havePriorityClass)
        {
          this._priorityClass = this.PriorityClassCore;
          this._havePriorityClass = true;
        }
        return this._priorityClass;
      }
      set
      {
        this.PriorityClassCore = Enum.IsDefined<ProcessPriorityClass>(value) ? value : throw new InvalidEnumArgumentException(nameof (value), (int) value, typeof (ProcessPriorityClass));
        this._priorityClass = value;
        this._havePriorityClass = true;
      }
    }

    /// <summary>Gets the amount of private memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of memory, in bytes, allocated for the associated process that cannot be shared with other processes.</returns>
    public long PrivateMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.PrivateBytes;
      }
    }

    /// <summary>Gets the amount of private memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The number of bytes allocated by the associated process that cannot be shared with other processes.</returns>
    [Obsolete("Process.PrivateMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.PrivateMemorySize64 instead.")]
    public int PrivateMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.PrivateBytes;
      }
    }

    /// <summary>Gets or sets the processors on which the threads in this process can be scheduled to run.</summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception">
    ///        <see cref="P:System.Diagnostics.Process.ProcessorAffinity" /> information could not be set or retrieved from the associated process resource.
    /// 
    /// -or-
    /// 
    /// The process identifier or process handle is zero. (The process has not been started.)</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.ProcessorAffinity" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> was not available.
    /// 
    /// -or-
    /// 
    /// The process has exited.</exception>
    /// <returns>A bitmask representing the processors that the threads in the associated process can run on. The default depends on the number of processors on the computer. The default value is 2 n -1, where n is the number of processors.</returns>
    [SupportedOSPlatform("windows")]
    [SupportedOSPlatform("linux")]
    public IntPtr ProcessorAffinity
    {
      get
      {
        if (!this._haveProcessorAffinity)
        {
          this._processorAffinity = this.ProcessorAffinityCore;
          this._haveProcessorAffinity = true;
        }
        return this._processorAffinity;
      }
      set
      {
        this.ProcessorAffinityCore = value;
        this._processorAffinity = value;
        this._haveProcessorAffinity = true;
      }
    }

    /// <summary>Gets the Terminal Services session identifier for the associated process.</summary>
    /// <exception cref="T:System.NullReferenceException">There is no session associated with this process.</exception>
    /// <exception cref="T:System.InvalidOperationException">There is no process associated with this session identifier.
    /// 
    /// -or-
    /// 
    /// The associated process is not on this machine.</exception>
    /// <returns>The Terminal Services session identifier for the associated process.</returns>
    public int SessionId
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.SessionId;
      }
    }

    /// <summary>Gets or sets the properties to pass to the <see cref="M:System.Diagnostics.Process.Start" /> method of the <see cref="T:System.Diagnostics.Process" />.</summary>
    /// <exception cref="T:System.ArgumentNullException">The value that specifies the <see cref="P:System.Diagnostics.Process.StartInfo" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">.NET Core and .NET 5+ only: The <see cref="M:System.Diagnostics.Process.Start" /> method was not used to start the process.</exception>
    /// <returns>The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that represents the data with which to start the process. These arguments include the name of the executable file or document used to start the process.</returns>
    public ProcessStartInfo StartInfo
    {
      get
      {
        if (this._startInfo == null)
        {
          if (this.Associated)
            throw new InvalidOperationException(SR.CantGetProcessStartInfo);
          this._startInfo = new ProcessStartInfo();
        }
        return this._startInfo;
      }
      set
      {
        ArgumentNullException.ThrowIfNull((object) value, nameof (value));
        if (this.Associated)
          throw new InvalidOperationException(SR.CantSetProcessStartInfo);
        this._startInfo = value;
      }
    }

    /// <summary>Gets the set of threads that are running in the associated process.</summary>
    /// <exception cref="T:System.SystemException">The process does not have an <see cref="P:System.Diagnostics.Process.Id" />, or no process is associated with the <see cref="T:System.Diagnostics.Process" /> instance.
    /// 
    /// -or-
    /// 
    /// The associated process has exited.</exception>
    /// <returns>An array of type <see cref="T:System.Diagnostics.ProcessThread" /> representing the operating system threads currently running in the associated process.</returns>
    public ProcessThreadCollection Threads
    {
      get
      {
        if (this._threads == null)
        {
          this.EnsureState(Process.State.HaveProcessInfo);
          int count = this._processInfo._threadInfoList.Count;
          ProcessThread[] processThreads = new ProcessThread[count];
          for (int index = 0; index < count; ++index)
            processThreads[index] = new ProcessThread(this._isRemoteMachine, this._processId, this._processInfo._threadInfoList[index]);
          this._threads = new ProcessThreadCollection(processThreads);
        }
        return this._threads;
      }
    }

    /// <summary>Gets the number of handles opened by the process.</summary>
    /// <returns>The number of operating system handles the process has opened.</returns>
    public int HandleCount
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        this.EnsureHandleCountPopulated();
        return this._processInfo.HandleCount;
      }
    }

    private void EnsureHandleCountPopulated()
    {
      if (this._processInfo.HandleCount > 0 || !this._haveProcessId || this.GetHasExited(false))
        return;
      string directoryPathForProcess = Interop.procfs.GetFileDescriptorDirectoryPathForProcess(this._processId);
      if (!Directory.Exists(directoryPathForProcess))
        return;
      try
      {
        this._processInfo.HandleCount = Directory.GetFiles(directoryPathForProcess, "*", SearchOption.TopDirectoryOnly).Length;
      }
      catch (DirectoryNotFoundException ex)
      {
      }
    }

    /// <summary>Gets the amount of the virtual memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of virtual memory, in bytes, allocated for the associated process.</returns>
    public long VirtualMemorySize64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.VirtualBytes;
      }
    }

    /// <summary>Gets the size of the process's virtual memory, in bytes.</summary>
    /// <returns>The amount of virtual memory, in bytes, that the associated process has requested.</returns>
    [Obsolete("Process.VirtualMemorySize has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.VirtualMemorySize64 instead.")]
    public int VirtualMemorySize
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.VirtualBytes;
      }
    }

    /// <summary>Gets or sets whether the <see cref="E:System.Diagnostics.Process.Exited" /> event should be raised when the process terminates.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="E:System.Diagnostics.Process.Exited" /> event should be raised when the associated process is terminated (through either an exit or a call to <see cref="M:System.Diagnostics.Process.Kill" />); otherwise, <see langword="false" />. The default is <see langword="false" />. Note that even if the value of <see cref="P:System.Diagnostics.Process.EnableRaisingEvents" /> is <see langword="false" />, the <see cref="E:System.Diagnostics.Process.Exited" /> event will be raised by the <see cref="P:System.Diagnostics.Process.HasExited" /> property accessor, if it determines that the process has exited.</returns>
    public bool EnableRaisingEvents
    {
      get => this._watchForExit;
      set
      {
        if (value == this._watchForExit)
          return;
        if (this.Associated)
        {
          if (value)
            this.EnsureWatchingForExit();
          else
            this.StopWatchingForExit();
        }
        this._watchForExit = value;
      }
    }

    /// <summary>Gets a stream used to write the input of the application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardInput" /> stream has not been defined because <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" /> is set to <see langword="false" />.</exception>
    /// <returns>A <see cref="T:System.IO.StreamWriter" /> that can be used to write the standard input stream of the application.</returns>
    public StreamWriter StandardInput
    {
      get
      {
        this.CheckDisposed();
        if (this._standardInput == null)
          throw new InvalidOperationException(SR.CantGetStandardIn);
        this._standardInputAccessed = true;
        return this._standardInput;
      }
    }

    /// <summary>Gets a stream used to read the textual output of the application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has not been defined for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" /> is set to <see langword="true" /> and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> is set to <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has been opened for asynchronous read operations with <see cref="M:System.Diagnostics.Process.BeginOutputReadLine" />.</exception>
    /// <returns>A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard output stream of the application.</returns>
    public StreamReader StandardOutput
    {
      get
      {
        this.CheckDisposed();
        if (this._standardOutput == null)
          throw new InvalidOperationException(SR.CantGetStandardOut);
        if (this._outputStreamReadMode == Process.StreamReadMode.Undefined)
          this._outputStreamReadMode = Process.StreamReadMode.SyncMode;
        else if (this._outputStreamReadMode != Process.StreamReadMode.SyncMode)
          throw new InvalidOperationException(SR.CantMixSyncAsyncOperation);
        return this._standardOutput;
      }
    }

    /// <summary>Gets a stream used to read the error output of the application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has not been defined for redirection; ensure <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is set to <see langword="true" /> and <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> is set to <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has been opened for asynchronous read operations with <see cref="M:System.Diagnostics.Process.BeginErrorReadLine" />.</exception>
    /// <returns>A <see cref="T:System.IO.StreamReader" /> that can be used to read the standard error stream of the application.</returns>
    public StreamReader StandardError
    {
      get
      {
        this.CheckDisposed();
        if (this._standardError == null)
          throw new InvalidOperationException(SR.CantGetStandardError);
        if (this._errorStreamReadMode == Process.StreamReadMode.Undefined)
          this._errorStreamReadMode = Process.StreamReadMode.SyncMode;
        else if (this._errorStreamReadMode != Process.StreamReadMode.SyncMode)
          throw new InvalidOperationException(SR.CantMixSyncAsyncOperation);
        return this._standardError;
      }
    }

    /// <summary>Gets the amount of physical memory, in bytes, allocated for the associated process.</summary>
    /// <returns>The amount of physical memory, in bytes, allocated for the associated process.</returns>
    public long WorkingSet64
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.WorkingSet;
      }
    }

    /// <summary>Gets the associated process's physical memory usage, in bytes.</summary>
    /// <returns>The total amount of physical memory the associated process is using, in bytes.</returns>
    [Obsolete("Process.WorkingSet has been deprecated because the type of the property can't represent all valid results. Use System.Diagnostics.Process.WorkingSet64 instead.")]
    public int WorkingSet
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return (int) this._processInfo.WorkingSet;
      }
    }

    /// <summary>Occurs when a process exits.</summary>
    public event EventHandler Exited
    {
      add => this._onExited += value;
      remove => this._onExited -= value;
    }

    #nullable disable
    private void CompletionCallback(object waitHandleContext, bool wasSignaled)
    {
      lock (this)
      {
        if (waitHandleContext != this._waitHandle)
          return;
        this.StopWatchingForExit();
        this.RaiseOnExited();
      }
    }

    /// <summary>Release all resources used by this process.</summary>
    /// <param name="disposing">
    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
      if (this._disposed)
        return;
      if (disposing)
        this.Close();
      this._disposed = true;
    }

    /// <summary>Closes a process that has a user interface by sending a close message to its main window.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process has already exited.
    /// 
    /// -or-
    /// 
    /// No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>
    /// <see langword="true" /> if the close message was successfully sent; <see langword="false" /> if the associated process does not have a main window or if the main window is disabled (for example if a modal dialog is being shown).</returns>
    public bool CloseMainWindow() => false;

    /// <summary>Causes the <see cref="T:System.Diagnostics.Process" /> component to wait indefinitely for the associated process to enter an idle state. This overload applies only to processes with a user interface and, therefore, a message loop.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process does not have a graphical interface.
    /// 
    /// -or-
    /// 
    /// An unknown error occurred. The process failed to enter an idle state.
    /// 
    /// -or-
    /// 
    /// The process has already exited.
    /// 
    /// -or-
    /// 
    /// No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>
    /// <see langword="true" /> if the associated process has reached an idle state.</returns>
    public bool WaitForInputIdle() => this.WaitForInputIdle(int.MaxValue);

    /// <summary>Causes the <see cref="T:System.Diagnostics.Process" /> component to wait the specified number of milliseconds for the associated process to enter an idle state. This overload applies only to processes with a user interface and, therefore, a message loop.</summary>
    /// <param name="milliseconds">A value of 1 to <see cref="F:System.Int32.MaxValue">Int32.MaxValue</see> that specifies the amount of time, in milliseconds, to wait for the associated process to become idle. A value of 0 specifies an immediate return, and a value of -1 specifies an infinite wait.</param>
    /// <exception cref="T:System.InvalidOperationException">The process does not have a graphical interface.
    /// 
    /// -or-
    /// 
    /// An unknown error occurred. The process failed to enter an idle state.
    /// 
    /// -or-
    /// 
    /// The process has already exited.
    /// 
    /// -or-
    /// 
    /// No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>
    /// <see langword="true" /> if the associated process has reached an idle state; otherwise, <see langword="false" />.</returns>
    public bool WaitForInputIdle(int milliseconds) => Process.WaitForInputIdleCore(milliseconds);

    /// <summary>Causes the <see cref="T:System.Diagnostics.Process" /> component to wait the specified <paramref name="timeout" /> for the associated process to enter an idle state.
    /// This overload applies only to processes with a user interface and, therefore, a message loop.</summary>
    /// <param name="timeout">The amount of time, in milliseconds, to wait for the associated process to become idle.</param>
    /// <exception cref="T:System.InvalidOperationException">The process does not have a graphical interface.
    /// 
    /// -or-
    /// 
    /// An unknown error occurred. The process failed to enter an idle state.
    /// 
    /// -or-
    /// 
    /// The process has already exited.
    /// 
    /// -or-
    /// 
    /// No process is associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <returns>
    /// <see langword="true" /> if the associated process has reached an idle state; otherwise, <see langword="false" />.</returns>
    public bool WaitForInputIdle(TimeSpan timeout)
    {
      return this.WaitForInputIdle(Process.ToTimeoutMilliseconds(timeout));
    }

    #nullable enable
    /// <summary>Gets or sets the object used to marshal the event handler calls that are issued as a result of a process exit event.</summary>
    /// <returns>The <see cref="T:System.ComponentModel.ISynchronizeInvoke" /> used to marshal event handler calls that are issued as a result of an <see cref="E:System.Diagnostics.Process.Exited" /> event on the process.</returns>
    public ISynchronizeInvoke? SynchronizingObject { get; set; }

    /// <summary>Frees all the resources that are associated with this component.</summary>
    public void Close()
    {
      if (!this.Associated)
        return;
      lock (this)
        this.StopWatchingForExit();
      if (this._haveProcessHandle)
      {
        this._processHandle.Dispose();
        this._processHandle = (SafeProcessHandle) null;
        this._haveProcessHandle = false;
      }
      this._haveProcessId = false;
      this._isRemoteMachine = false;
      this._machineName = ".";
      this._raisedOnExited = false;
      try
      {
        if (this._standardOutput != null && (this._outputStreamReadMode == Process.StreamReadMode.AsyncMode || this._outputStreamReadMode == Process.StreamReadMode.Undefined))
        {
          if (this._outputStreamReadMode == Process.StreamReadMode.AsyncMode)
          {
            this._output?.CancelOperation();
            this._output?.Dispose();
          }
          this._standardOutput.Close();
        }
        if (this._standardError != null && (this._errorStreamReadMode == Process.StreamReadMode.AsyncMode || this._errorStreamReadMode == Process.StreamReadMode.Undefined))
        {
          if (this._errorStreamReadMode == Process.StreamReadMode.AsyncMode)
          {
            this._error?.CancelOperation();
            this._error?.Dispose();
          }
          this._standardError.Close();
        }
        if (this._standardInput == null || this._standardInputAccessed)
          return;
        this._standardInput.Close();
      }
      finally
      {
        this._standardOutput = (StreamReader) null;
        this._standardInput = (StreamWriter) null;
        this._standardError = (StreamReader) null;
        this._output = (AsyncStreamReader) null;
        this._error = (AsyncStreamReader) null;
        this.CloseCore();
        this.Refresh();
      }
    }

    private void ThrowIfExited(bool refresh)
    {
      if ((this._waitStateHolder != null || refresh) && this.GetHasExited(refresh))
        throw new InvalidOperationException(SR.Format(SR.ProcessHasExited, (object) this._processId.ToString()));
    }

    private void EnsureState(Process.State state)
    {
      if ((state & Process.State.Associated) != (Process.State) 0 && !this.Associated)
        throw new InvalidOperationException(SR.NoAssociatedProcess);
      if ((state & Process.State.HaveId) != (Process.State) 0)
      {
        if (!this._haveProcessId)
        {
          if (this._haveProcessHandle)
          {
            this.SetProcessId(ProcessManager.GetProcessIdFromHandle(this._processHandle));
          }
          else
          {
            this.EnsureState(Process.State.Associated);
            throw new InvalidOperationException(SR.ProcessIdRequired);
          }
        }
        if ((state & Process.State.HaveNonExitedId) == Process.State.HaveNonExitedId)
          this.ThrowIfExited(false);
      }
      if ((state & Process.State.IsLocal) != (Process.State) 0 && this._isRemoteMachine)
        throw new NotSupportedException(SR.NotSupportedRemote);
      if ((state & Process.State.HaveProcessInfo) != (Process.State) 0 && this._processInfo == null)
      {
        if ((state & Process.State.HaveNonExitedId) != Process.State.HaveNonExitedId)
          this.EnsureState(Process.State.HaveNonExitedId);
        this._processInfo = ProcessManager.GetProcessInfo(this._processId, this._machineName);
        if (this._processInfo == null)
          throw new InvalidOperationException(SR.NoProcessInfo);
      }
      if ((state & Process.State.Exited) == (Process.State) 0)
        return;
      if (!this.HasExited)
        throw new InvalidOperationException(SR.WaitTillExit);
      if (!this._haveProcessHandle)
        throw new InvalidOperationException(SR.NoProcessHandle);
    }

    private void EnsureWorkingSetLimits()
    {
      if (this._haveWorkingSetLimits)
        return;
      this.GetWorkingSetLimits(out this._minWorkingSet, out this._maxWorkingSet);
      this._haveWorkingSetLimits = true;
    }

    #nullable disable
    private void SetWorkingSetLimits(IntPtr? min, IntPtr? max)
    {
      Process.SetWorkingSetLimitsCore(min, max, out this._minWorkingSet, out this._maxWorkingSet);
      this._haveWorkingSetLimits = true;
    }

    #nullable enable
    /// <summary>Returns a new <see cref="T:System.Diagnostics.Process" /> component, given a process identifier and the name of a computer on the network.</summary>
    /// <param name="processId">The system-unique identifier of a process resource.</param>
    /// <param name="machineName">The name of a computer on the network.</param>
    /// <exception cref="T:System.ArgumentException">The process specified by the <paramref name="processId" /> parameter is not running. The identifier might be expired.
    /// 
    /// -or-
    /// 
    /// The <paramref name="machineName" /> parameter syntax is invalid. The name might have length zero (0).</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is <see langword="null" />.</exception>
    /// <returns>A <see cref="T:System.Diagnostics.Process" /> component that is associated with a remote process resource identified by the <paramref name="processId" /> parameter.</returns>
    public static Process GetProcessById(int processId, string machineName)
    {
      return ProcessManager.IsProcessRunning(processId, machineName) ? new Process(machineName, ProcessManager.IsRemoteMachine(machineName), processId, (ProcessInfo) null) : throw new ArgumentException(SR.Format(SR.MissingProcess, (object) processId.ToString()));
    }

    /// <summary>Returns a new <see cref="T:System.Diagnostics.Process" /> component, given the identifier of a process on the local computer.</summary>
    /// <param name="processId">The system-unique identifier of a process resource.</param>
    /// <exception cref="T:System.ArgumentException">The process specified by the <paramref name="processId" /> parameter is not running. The identifier might be expired.</exception>
    /// <returns>A <see cref="T:System.Diagnostics.Process" /> component that is associated with the local process resource identified by the <paramref name="processId" /> parameter.</returns>
    public static Process GetProcessById(int processId) => Process.GetProcessById(processId, ".");

    /// <summary>Creates an array of new <see cref="T:System.Diagnostics.Process" /> components and associates them with all the process resources on the local computer that share the specified process name.</summary>
    /// <param name="processName">The friendly name of the process.</param>
    /// <exception cref="T:System.InvalidOperationException">There are problems accessing the performance counter APIs used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP.</exception>
    /// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents the process resources running the specified application or file.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process[] GetProcessesByName(string? processName)
    {
      return Process.GetProcessesByName(processName, ".");
    }

    /// <summary>Creates a new <see cref="T:System.Diagnostics.Process" /> component for each process resource on the local computer.</summary>
    /// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents all the process resources running on the local computer.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process[] GetProcesses() => Process.GetProcesses(".");

    /// <summary>Creates a new <see cref="T:System.Diagnostics.Process" /> component for each process resource on the specified computer.</summary>
    /// <param name="machineName">The computer from which to read the list of processes.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="machineName" /> parameter syntax is invalid. It might have length zero (0).</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The operating system platform does not support this operation on remote computers.</exception>
    /// <exception cref="T:System.InvalidOperationException">There are problems accessing the performance counter APIs used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">A problem occurred accessing an underlying system API.</exception>
    /// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents all the process resources running on the specified computer.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process[] GetProcesses(string machineName)
    {
      bool isRemoteMachine = ProcessManager.IsRemoteMachine(machineName);
      ProcessInfo[] processInfos = ProcessManager.GetProcessInfos((string) null, machineName);
      Process[] processes = new Process[processInfos.Length];
      for (int index = 0; index < processInfos.Length; ++index)
      {
        ProcessInfo processInfo = processInfos[index];
        processes[index] = new Process(machineName, isRemoteMachine, processInfo.ProcessId, processInfo);
      }
      return processes;
    }

    /// <summary>Gets a new <see cref="T:System.Diagnostics.Process" /> component and associates it with the currently active process.</summary>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> component associated with the process resource that is running the calling application.</returns>
    public static Process GetCurrentProcess()
    {
      return new Process(".", false, Environment.ProcessId, (ProcessInfo) null);
    }

    /// <summary>Raises the <see cref="E:System.Diagnostics.Process.Exited" /> event.</summary>
    protected void OnExited()
    {
      EventHandler onExited = this._onExited;
      if (onExited == null)
        return;
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.BeginInvoke((Delegate) onExited, new object[2]
        {
          (object) this,
          (object) EventArgs.Empty
        });
      else
        onExited((object) this, EventArgs.Empty);
    }

    private void RaiseOnExited()
    {
      if (this._raisedOnExited)
        return;
      lock (this)
      {
        if (this._raisedOnExited)
          return;
        this._raisedOnExited = true;
        this.OnExited();
      }
    }

    /// <summary>Discards any information about the associated process that has been cached inside the process component.</summary>
    public void Refresh()
    {
      this._processInfo = (ProcessInfo) null;
      this._threads?.Dispose();
      this._threads = (ProcessThreadCollection) null;
      this._modules?.Dispose();
      this._modules = (ProcessModuleCollection) null;
      this._exited = false;
      this._haveWorkingSetLimits = false;
      this._haveProcessorAffinity = false;
      this._havePriorityClass = false;
      this._haveExitTime = false;
      this._havePriorityBoostEnabled = false;
    }

    #nullable disable
    private SafeProcessHandle GetOrOpenProcessHandle()
    {
      if (!this._haveProcessHandle)
      {
        this.CheckDisposed();
        this.SetProcessHandle(this.GetProcessHandle());
      }
      return this._processHandle;
    }

    private void SetProcessHandle(SafeProcessHandle processHandle)
    {
      this._processHandle = processHandle;
      this._haveProcessHandle = true;
      if (!this._watchForExit)
        return;
      this.EnsureWatchingForExit();
    }

    private void SetProcessId(int processId)
    {
      this._processId = processId;
      this._haveProcessId = true;
      this.ConfigureAfterProcessIdSet();
    }

    private void ConfigureAfterProcessIdSet() => this.GetWaitState();

    /// <summary>Starts (or reuses) the process resource that is specified by the <see cref="P:System.Diagnostics.Process.StartInfo" /> property of this <see cref="T:System.Diagnostics.Process" /> component and associates it with the component.</summary>
    /// <exception cref="T:System.InvalidOperationException">No file name was specified in the <see cref="T:System.Diagnostics.Process" /> component's <see cref="P:System.Diagnostics.Process.StartInfo" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> member of the <see cref="P:System.Diagnostics.Process.StartInfo" /> property is <see langword="true" /> while <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> is <see langword="true" />.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">Method not supported on operating systems without shell support such as Nano Server (.NET Core only).</exception>
    /// <returns>
    /// <see langword="true" /> if a process resource is started; <see langword="false" /> if no new process resource is started (for example, if an existing process is reused).</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public bool Start()
    {
      this.Close();
      ProcessStartInfo startInfo = this.StartInfo;
      if (startInfo.FileName.Length == 0)
        throw new InvalidOperationException(SR.FileNameMissing);
      if (startInfo.StandardInputEncoding != null && !startInfo.RedirectStandardInput)
        throw new InvalidOperationException(SR.StandardInputEncodingNotAllowed);
      if (startInfo.StandardOutputEncoding != null && !startInfo.RedirectStandardOutput)
        throw new InvalidOperationException(SR.StandardOutputEncodingNotAllowed);
      if (startInfo.StandardErrorEncoding != null && !startInfo.RedirectStandardError)
        throw new InvalidOperationException(SR.StandardErrorEncodingNotAllowed);
      if (!string.IsNullOrEmpty(startInfo.Arguments) && startInfo.HasArgumentList)
        throw new InvalidOperationException(SR.ArgumentAndArgumentListInitialized);
      if (startInfo.HasArgumentList)
      {
        int count = startInfo.ArgumentList.Count;
        for (int index = 0; index < count; ++index)
        {
          if (startInfo.ArgumentList[index] == null)
            throw new ArgumentNullException("item", SR.ArgumentListMayNotContainNull);
        }
      }
      this.CheckDisposed();
      SerializationGuard.ThrowIfDeserializationInProgress("AllowProcessCreation", ref Process.s_cachedSerializationSwitch);
      return this.StartCore(startInfo);
    }

    #nullable enable
    /// <summary>Starts a process resource by specifying the name of a document or application file and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
    /// <param name="fileName">The name of a document or application file to run in the process.</param>
    /// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file.
    /// 
    /// -or-
    /// 
    /// The file specified in the <paramref name="fileName" /> could not be found.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The PATH environment variable has a string containing quotes.</exception>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started. Note that a new process that's started alongside already running instances of the same process will be independent from the others. In addition, Start may return a non-null Process with its <see cref="P:System.Diagnostics.Process.HasExited" /> property already set to <see langword="true" />. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process Start(string fileName) => Process.Start(new ProcessStartInfo(fileName));

    /// <summary>Starts a process resource by specifying the name of an application and a set of command-line arguments, and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
    /// <param name="fileName">The name of an application file to run in the process.</param>
    /// <param name="arguments">Command-line arguments to pass when starting the process.</param>
    /// <exception cref="T:System.InvalidOperationException">The <paramref name="fileName" /> or <paramref name="arguments" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file.
    /// 
    /// -or-
    /// 
    /// The file specified in the <paramref name="fileName" /> could not be found.
    /// 
    ///  -or-
    /// 
    ///  The sum of the length of the arguments and the length of the full path to the process exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.IO.FileNotFoundException">The PATH environment variable has a string containing quotes.</exception>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started. Note that a new process that's started alongside already running instances of the same process will be independent from the others. In addition, Start may return a non-null Process with its <see cref="P:System.Diagnostics.Process.HasExited" /> property already set to <see langword="true" />. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process Start(string fileName, string arguments)
    {
      return Process.Start(new ProcessStartInfo(fileName, arguments));
    }

    /// <summary>Starts a process resource by specifying the name of an application and a set of command line arguments.</summary>
    /// <param name="fileName">The name of a document or application file to run in the process.</param>
    /// <param name="arguments">The command-line arguments to pass when starting the process.</param>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process Start(string fileName, IEnumerable<string> arguments)
    {
      return Process.Start(new ProcessStartInfo(fileName, arguments));
    }

    /// <summary>Starts the process resource that is specified by the parameter containing process start information (for example, the file name of the process to start) and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
    /// <param name="startInfo">The <see cref="T:System.Diagnostics.ProcessStartInfo" /> that contains the information that is used to start the process, including the file name and any command-line arguments.</param>
    /// <exception cref="T:System.InvalidOperationException">No file name was specified in the <paramref name="startInfo" /> parameter's <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is <see langword="true" /> and the <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardInput" />, <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" />, or <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> property is also <see langword="true" />.
    /// 
    /// -or-
    /// 
    /// The <see cref="P:System.Diagnostics.ProcessStartInfo.UseShellExecute" /> property of the <paramref name="startInfo" /> parameter is <see langword="true" /> and the <see cref="P:System.Diagnostics.ProcessStartInfo.UserName" /> property is not <see langword="null" /> or empty or the <see cref="P:System.Diagnostics.ProcessStartInfo.Password" /> property is not <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="startInfo" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file.
    /// 
    /// -or-
    /// 
    /// The file specified in the <paramref name="startInfo" /> parameter's <see cref="P:System.Diagnostics.ProcessStartInfo.FileName" /> property could not be found.
    /// 
    /// -or-
    /// 
    /// The sum of the length of the arguments and the length of the full path to the process exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">Method not supported on operating systems without shell support such as Nano Server (.NET Core only).</exception>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started. Note that a new process that's started alongside already running instances of the same process will be independent from the others. In addition, Start may return a non-null Process with its <see cref="P:System.Diagnostics.Process.HasExited" /> property already set to <see langword="true" />. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process? Start(ProcessStartInfo startInfo)
    {
      ArgumentNullException.ThrowIfNull((object) startInfo, nameof (startInfo));
      Process process = new Process();
      process.StartInfo = startInfo;
      return !process.Start() ? (Process) null : process;
    }

    private void StopWatchingForExit()
    {
      if (!this._watchingForExit)
        return;
      RegisteredWaitHandle registeredWaitHandle = (RegisteredWaitHandle) null;
      WaitHandle waitHandle = (WaitHandle) null;
      lock (this)
      {
        if (this._watchingForExit)
        {
          this._watchingForExit = false;
          waitHandle = this._waitHandle;
          this._waitHandle = (WaitHandle) null;
          registeredWaitHandle = this._registeredWaitHandle;
          this._registeredWaitHandle = (RegisteredWaitHandle) null;
        }
      }
      registeredWaitHandle?.Unregister((WaitHandle) null);
      waitHandle?.Dispose();
    }

    /// <summary>Formats the process's name as a string, combined with the parent component type, if applicable.</summary>
    /// <returns>The <see cref="P:System.Diagnostics.Process.ProcessName" />, combined with the base component's <see cref="M:System.Object.ToString" /> return value.</returns>
    public override string ToString()
    {
      string str = base.ToString();
      try
      {
        if (this.Associated)
        {
          if (this._processInfo == null)
            this._processInfo = ProcessManager.GetProcessInfo(this._processId, this._machineName);
          if (this._processInfo != null)
          {
            string processName = this._processInfo.ProcessName;
            if (processName.Length != 0)
              str = str + " (" + processName + ")";
          }
        }
      }
      catch
      {
      }
      return str;
    }

    /// <summary>Instructs the <see cref="T:System.Diagnostics.Process" /> component to wait indefinitely for the associated process to exit.</summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception">The wait setting could not be accessed.</exception>
    /// <exception cref="T:System.SystemException">No process <see cref="P:System.Diagnostics.Process.Id" /> has been set, and a <see cref="P:System.Diagnostics.Process.Handle" /> from which the <see cref="P:System.Diagnostics.Process.Id" /> property can be determined does not exist.
    /// 
    /// -or-
    /// 
    /// There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
    /// 
    /// -or-
    /// 
    /// You are attempting to call <see cref="M:System.Diagnostics.Process.WaitForExit" /> for a process that is running on a remote computer. This method is available only for processes that are running on the local computer.</exception>
    public void WaitForExit() => this.WaitForExit(-1);

    /// <summary>Instructs the <see cref="T:System.Diagnostics.Process" /> component to wait the specified number of milliseconds for the associated process to exit.</summary>
    /// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated process to exit. A value of 0 specifies an immediate return, and a value of -1 specifies an infinite wait.</param>
    /// <exception cref="T:System.ComponentModel.Win32Exception">The wait setting could not be accessed.</exception>
    /// <exception cref="T:System.SystemException">No process <see cref="P:System.Diagnostics.Process.Id" /> has been set, and a <see cref="P:System.Diagnostics.Process.Handle" /> from which the <see cref="P:System.Diagnostics.Process.Id" /> property can be determined does not exist.
    /// 
    /// -or-
    /// 
    /// There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
    /// 
    /// -or-
    /// 
    /// You are attempting to call <see cref="M:System.Diagnostics.Process.WaitForExit(System.Int32)" /> for a process that is running on a remote computer. This method is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="milliseconds" /> is a negative number other than -1, which represents an infinite time-out.</exception>
    /// <returns>
    /// <see langword="true" /> if the associated process has exited; otherwise, <see langword="false" />.</returns>
    public bool WaitForExit(int milliseconds)
    {
      bool flag = this.WaitForExitCore(milliseconds);
      if (flag && this._watchForExit)
        this.RaiseOnExited();
      return flag;
    }

    /// <summary>Instructs the Process component to wait the specified amount of time for the associated process to exit.</summary>
    /// <param name="timeout">The amount of time to wait for the associated process to exit.</param>
    /// <returns>
    /// <see langword="true" /> if the associated process has exited; otherwise, <see langword="false" />.</returns>
    public bool WaitForExit(TimeSpan timeout)
    {
      return this.WaitForExit(Process.ToTimeoutMilliseconds(timeout));
    }

    private static int ToTimeoutMilliseconds(TimeSpan timeout)
    {
      long totalMilliseconds = (long) timeout.TotalMilliseconds;
      ArgumentOutOfRangeException.ThrowIfLessThan<long>(totalMilliseconds, -1L, nameof (timeout));
      ArgumentOutOfRangeException.ThrowIfGreaterThan<long>(totalMilliseconds, (long) int.MaxValue, nameof (timeout));
      return (int) totalMilliseconds;
    }

    /// <summary>Instructs the process component to wait for the associated process to exit, or for the <paramref name="cancellationToken" /> to be cancelled.</summary>
    /// <param name="cancellationToken">An optional token to cancel the asynchronous operation.</param>
    /// <returns>A task that will complete when the process has exited, cancellation has been requested, or an error occurs.</returns>
    public async Task WaitForExitAsync(CancellationToken cancellationToken1 = default (CancellationToken))
    {
      if (!this.Associated)
        throw new InvalidOperationException(SR.NoAssociatedProcess);
      if (!this.HasExited)
        cancellationToken1.ThrowIfCancellationRequested();
      EventHandler handler;
      try
      {
        this.EnableRaisingEvents = true;
      }
      catch (InvalidOperationException ex)
      {
        if (this.HasExited)
        {
          await WaitUntilOutputEOF(cancellationToken1).ConfigureAwait(false);
          handler = (EventHandler) null;
          return;
        }
        if (!((object) ex is Exception source))
          throw (object) ex;
        ExceptionDispatchInfo.Capture(source).Throw();
      }
      TaskCompletionSource tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
      handler = (EventHandler) ((_1, _2) => tcs.TrySetResult());
      this.Exited += handler;
      try
      {
        if (!this.HasExited)
        {
          CancellationTokenRegistration tokenRegistration = cancellationToken1.UnsafeRegister((Action<object, CancellationToken>) ((s, cancellationToken2) => ((TaskCompletionSource) s).TrySetCanceled(cancellationToken2)), (object) tcs);
          try
          {
            await tcs.Task.ConfigureAwait(false);
          }
          finally
          {
            tokenRegistration.Dispose();
          }
          tokenRegistration = new CancellationTokenRegistration();
        }
        await WaitUntilOutputEOF(cancellationToken1).ConfigureAwait(false);
        handler = (EventHandler) null;
      }
      finally
      {
        this.Exited -= handler;
      }

      #nullable disable
      async Task WaitUntilOutputEOF(CancellationToken cancellationToken)
      {
        ConfiguredTaskAwaitable configuredTaskAwaitable;
        if (this._output != null)
        {
          configuredTaskAwaitable = this._output.EOF.WaitAsync(cancellationToken).ConfigureAwait(false);
          await configuredTaskAwaitable;
        }
        if (this._error == null)
          return;
        configuredTaskAwaitable = this._error.EOF.WaitAsync(cancellationToken).ConfigureAwait(false);
        await configuredTaskAwaitable;
      }
    }

    /// <summary>Begins asynchronous read operations on the redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream of the application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardOutput" /> property is <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  An asynchronous read operation is already in progress on the <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream.
    /// 
    /// -or-
    /// 
    ///  The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream has been used by a synchronous read operation.</exception>
    public void BeginOutputReadLine()
    {
      if (this._outputStreamReadMode == Process.StreamReadMode.Undefined)
        this._outputStreamReadMode = Process.StreamReadMode.AsyncMode;
      else if (this._outputStreamReadMode != Process.StreamReadMode.AsyncMode)
        throw new InvalidOperationException(SR.CantMixSyncAsyncOperation);
      this._pendingOutputRead = !this._pendingOutputRead ? true : throw new InvalidOperationException(SR.PendingAsyncOperation);
      if (this._output == null)
      {
        if (this._standardOutput == null)
          throw new InvalidOperationException(SR.CantGetStandardOut);
        this._output = new AsyncStreamReader(this._standardOutput.BaseStream, new Action<string>(this.OutputReadNotifyUser), this._standardOutput.CurrentEncoding);
      }
      this._output.BeginReadLine();
    }

    /// <summary>Begins asynchronous read operations on the redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream of the application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.ProcessStartInfo.RedirectStandardError" /> property is <see langword="false" />.
    /// 
    /// -or-
    /// 
    ///  An asynchronous read operation is already in progress on the <see cref="P:System.Diagnostics.Process.StandardError" /> stream.
    /// 
    /// -or-
    /// 
    ///  The <see cref="P:System.Diagnostics.Process.StandardError" /> stream has been used by a synchronous read operation.</exception>
    public void BeginErrorReadLine()
    {
      if (this._errorStreamReadMode == Process.StreamReadMode.Undefined)
        this._errorStreamReadMode = Process.StreamReadMode.AsyncMode;
      else if (this._errorStreamReadMode != Process.StreamReadMode.AsyncMode)
        throw new InvalidOperationException(SR.CantMixSyncAsyncOperation);
      this._pendingErrorRead = !this._pendingErrorRead ? true : throw new InvalidOperationException(SR.PendingAsyncOperation);
      if (this._error == null)
      {
        if (this._standardError == null)
          throw new InvalidOperationException(SR.CantGetStandardError);
        this._error = new AsyncStreamReader(this._standardError.BaseStream, new Action<string>(this.ErrorReadNotifyUser), this._standardError.CurrentEncoding);
      }
      this._error.BeginReadLine();
    }

    /// <summary>Cancels the asynchronous read operation on the redirected <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream of an application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardOutput" /> stream is not enabled for asynchronous read operations.</exception>
    public void CancelOutputRead()
    {
      this.CheckDisposed();
      if (this._output == null)
        throw new InvalidOperationException(SR.NoAsyncOperation);
      this._output.CancelOperation();
      this._pendingOutputRead = false;
    }

    /// <summary>Cancels the asynchronous read operation on the redirected <see cref="P:System.Diagnostics.Process.StandardError" /> stream of an application.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.StandardError" /> stream is not enabled for asynchronous read operations.</exception>
    public void CancelErrorRead()
    {
      this.CheckDisposed();
      if (this._error == null)
        throw new InvalidOperationException(SR.NoAsyncOperation);
      this._error.CancelOperation();
      this._pendingErrorRead = false;
    }

    internal void OutputReadNotifyUser(string data)
    {
      DataReceivedEventHandler outputDataReceived = this.OutputDataReceived;
      if (outputDataReceived == null)
        return;
      DataReceivedEventArgs e = new DataReceivedEventArgs(data);
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.Invoke((Delegate) outputDataReceived, new object[2]
        {
          (object) this,
          (object) e
        });
      else
        outputDataReceived((object) this, e);
    }

    internal void ErrorReadNotifyUser(string data)
    {
      DataReceivedEventHandler errorDataReceived = this.ErrorDataReceived;
      if (errorDataReceived == null)
        return;
      DataReceivedEventArgs e = new DataReceivedEventArgs(data);
      ISynchronizeInvoke synchronizingObject = this.SynchronizingObject;
      if (synchronizingObject != null && synchronizingObject.InvokeRequired)
        synchronizingObject.Invoke((Delegate) errorDataReceived, new object[2]
        {
          (object) this,
          (object) e
        });
      else
        errorDataReceived((object) this, e);
    }

    private void CheckDisposed() => ObjectDisposedException.ThrowIf(this._disposed, (object) this);

    private static Win32Exception CreateExceptionForErrorStartingProcess(
      string errorMessage,
      int errorCode,
      string fileName,
      string workingDirectory)
    {
      string p2 = string.IsNullOrEmpty(workingDirectory) ? Directory.GetCurrentDirectory() : workingDirectory;
      string message = SR.Format(SR.ErrorStartingProcess, (object) fileName, (object) p2, (object) errorMessage);
      return new Win32Exception(errorCode, message);
    }

    /// <summary>Puts a <see cref="T:System.Diagnostics.Process" /> component in state to interact with operating system processes that run in a special mode by enabling the native property <see langword="SeDebugPrivilege" /> on the current thread.</summary>
    public static void EnterDebugMode()
    {
    }

    /// <summary>Takes a <see cref="T:System.Diagnostics.Process" /> component out of the state that lets it interact with operating system processes that run in a special mode.</summary>
    public static void LeaveDebugMode()
    {
    }

    #nullable enable
    /// <summary>Starts a process resource by specifying the name of an application, a user name, a password, and a domain and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
    /// <param name="fileName">The name of an application file to run in the process.</param>
    /// <param name="userName">The user name to use when starting the process.</param>
    /// <param name="password">A <see cref="T:System.Security.SecureString" /> that contains the password to use when starting the process.</param>
    /// <param name="domain">The domain to use when starting the process.</param>
    /// <exception cref="T:System.InvalidOperationException">No file name was specified.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">There was an error in opening the associated file.
    /// 
    /// -or-
    /// 
    /// The file specified in the <paramref name="fileName" /> could not be found.</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">This member is not supported on Linux or macOS (.NET Core only).</exception>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started. Note that a new process that's started alongside already running instances of the same process will be independent from the others. In addition, Start may return a non-null Process with its <see cref="P:System.Diagnostics.Process.HasExited" /> property already set to <see langword="true" />. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
    [CLSCompliant(false)]
    [SupportedOSPlatform("windows")]
    public static Process Start(
      string fileName,
      string userName,
      SecureString password,
      string domain)
    {
      throw new PlatformNotSupportedException(SR.ProcessStartWithPasswordAndDomainNotSupported);
    }

    /// <summary>Starts a process resource by specifying the name of an application, a set of command-line arguments, a user name, a password, and a domain and associates the resource with a new <see cref="T:System.Diagnostics.Process" /> component.</summary>
    /// <param name="fileName">The name of an application file to run in the process.</param>
    /// <param name="arguments">Command-line arguments to pass when starting the process.</param>
    /// <param name="userName">The user name to use when starting the process.</param>
    /// <param name="password">A <see cref="T:System.Security.SecureString" /> that contains the password to use when starting the process.</param>
    /// <param name="domain">The domain to use when starting the process.</param>
    /// <exception cref="T:System.InvalidOperationException">No file name was specified.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">An error occurred when opening the associated file.
    /// 
    /// -or-
    /// 
    /// The file specified in the <paramref name="fileName" /> could not be found.
    /// 
    ///  -or-
    /// 
    ///  The sum of the length of the arguments and the length of the full path to the associated file exceeds 2080. The error message associated with this exception can be one of the following: "The data area passed to a system call is too small." or "Access is denied."</exception>
    /// <exception cref="T:System.ObjectDisposedException">The process object has already been disposed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">This member is not supported on Linux or macOS (.NET Core only).</exception>
    /// <returns>A new <see cref="T:System.Diagnostics.Process" /> that is associated with the process resource, or <see langword="null" /> if no process resource is started. Note that a new process that's started alongside already running instances of the same process will be independent from the others. In addition, Start may return a non-null Process with its <see cref="P:System.Diagnostics.Process.HasExited" /> property already set to <see langword="true" />. In this case, the started process may have activated an existing instance of itself and then exited.</returns>
    [CLSCompliant(false)]
    [SupportedOSPlatform("windows")]
    public static Process Start(
      string fileName,
      string arguments,
      string userName,
      SecureString password,
      string domain)
    {
      throw new PlatformNotSupportedException(SR.ProcessStartWithPasswordAndDomainNotSupported);
    }

    /// <summary>Immediately stops the associated process.</summary>
    /// <exception cref="T:System.ComponentModel.Win32Exception">The associated process could not be terminated.</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to call <see cref="M:System.Diagnostics.Process.Kill" /> for a process that is running on a remote computer. The method is available only for processes running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public void Kill()
    {
      if (Process.PlatformDoesNotSupportProcessStartAndKill)
        throw new PlatformNotSupportedException();
      this.EnsureState(Process.State.HaveId);
      if (!this.GetHasExited(false) && Interop.Sys.Kill(this._processId, Interop.Sys.Signals.SIGKILL) != 0 && Interop.Sys.GetLastError() != Interop.Error.ESRCH)
        throw new Win32Exception();
    }

    private bool GetHasExited(bool refresh) => this.GetWaitState().GetExited(out int? _, refresh);

    #nullable disable
    private List<Exception> KillTree()
    {
      List<Exception> exceptions = (List<Exception>) null;
      this.KillTree(ref exceptions);
      return exceptions;
    }

    private void KillTree(ref List<Exception> exceptions)
    {
      if (this.GetHasExited(false))
        return;
      if (Interop.Sys.Kill(this._processId, Interop.Sys.Signals.SIGSTOP) != 0)
      {
        if (Interop.Sys.GetLastError() == Interop.Error.ESRCH)
          return;
        (exceptions ?? (exceptions = new List<Exception>())).Add((Exception) new Win32Exception());
      }
      else
      {
        List<Process> childProcesses = this.GetChildProcesses();
        if (Interop.Sys.Kill(this._processId, Interop.Sys.Signals.SIGKILL) != 0 && Interop.Sys.GetLastError() != Interop.Error.ESRCH)
          (exceptions ?? (exceptions = new List<Exception>())).Add((Exception) new Win32Exception());
        foreach (Process process in childProcesses)
        {
          process.KillTree(ref exceptions);
          process.Dispose();
        }
      }
    }

    private void CloseCore()
    {
      if (this._waitStateHolder == null)
        return;
      this._waitStateHolder.Dispose();
      this._waitStateHolder = (ProcessWaitState.Holder) null;
    }

    private void EnsureWatchingForExit()
    {
      if (this._watchingForExit)
        return;
      lock (this)
      {
        if (this._watchingForExit)
          return;
        this._watchingForExit = true;
        try
        {
          this._waitHandle = (WaitHandle) new ProcessWaitHandle(this.GetWaitState());
          this._registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(this._waitHandle, new WaitOrTimerCallback(this.CompletionCallback), (object) this._waitHandle, -1, true);
        }
        catch
        {
          this._waitHandle?.Dispose();
          this._waitHandle = (WaitHandle) null;
          this._watchingForExit = false;
          throw;
        }
      }
    }

    private bool WaitForExitCore(int milliseconds)
    {
      bool flag = this.GetWaitState().WaitForExit(milliseconds);
      if (flag && milliseconds == -1)
      {
        this._output?.EOF.GetAwaiter().GetResult();
        this._error?.EOF.GetAwaiter().GetResult();
      }
      return flag;
    }

    #nullable enable
    /// <summary>Gets the main module for the associated process.</summary>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainModule" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">A 32-bit process is trying to access the modules of a 64-bit process.</exception>
    /// <exception cref="T:System.InvalidOperationException">The process <see cref="P:System.Diagnostics.Process.Id" /> is not available.
    /// 
    /// -or-
    /// 
    /// The process has exited.</exception>
    /// <returns>The <see cref="T:System.Diagnostics.ProcessModule" /> that was used to start the process.</returns>
    public ProcessModule? MainModule
    {
      get
      {
        ProcessModuleCollection modules = this.Modules;
        return modules.Count <= 0 ? (ProcessModule) null : modules[0];
      }
    }

    private void UpdateHasExited()
    {
      int? exitCode;
      this._exited = this.GetWaitState().GetExited(out exitCode, true);
      if (!this._exited || !exitCode.HasValue)
        return;
      this._exitCode = exitCode.Value;
    }

    private DateTime ExitTimeCore => this.GetWaitState().ExitTime;

    private static bool PriorityBoostEnabledCore
    {
      set
      {
      }
    }

    private ProcessPriorityClass PriorityClassCore
    {
      get
      {
        this.EnsureState(Process.State.HaveNonExitedId);
        int priority1;
        int priority2 = Interop.Sys.GetPriority(Interop.Sys.PriorityWhich.PRIO_PROCESS, this._processId, out priority1);
        if (priority2 != 0)
          throw new Win32Exception(priority2);
        if (priority1 < -15)
          return ProcessPriorityClass.RealTime;
        if (priority1 < -10)
          return ProcessPriorityClass.High;
        if (priority1 < -5)
          return ProcessPriorityClass.AboveNormal;
        if (priority1 == 0)
          return ProcessPriorityClass.Normal;
        return priority1 > 10 ? ProcessPriorityClass.Idle : ProcessPriorityClass.BelowNormal;
      }
      set
      {
        this.EnsureState(Process.State.HaveNonExitedId);
        int nice = 0;
        switch (value)
        {
          case ProcessPriorityClass.Idle:
            nice = 19;
            break;
          case ProcessPriorityClass.High:
            nice = -11;
            break;
          case ProcessPriorityClass.RealTime:
            nice = -19;
            break;
          case ProcessPriorityClass.BelowNormal:
            nice = 10;
            break;
          case ProcessPriorityClass.AboveNormal:
            nice = -6;
            break;
        }
        if (Interop.Sys.SetPriority(Interop.Sys.PriorityWhich.PRIO_PROCESS, this._processId, nice) == -1)
          throw new Win32Exception();
      }
    }

    #nullable disable
    private bool IsParentOf(Process possibleChildProcess)
    {
      try
      {
        return this.Id == possibleChildProcess.ParentProcessId;
      }
      catch (Exception ex) when (Process.IsProcessInvalidException(ex))
      {
        return false;
      }
    }

    private bool Equals(Process process)
    {
      try
      {
        return this.Id == process.Id;
      }
      catch (Exception ex) when (Process.IsProcessInvalidException(ex))
      {
        return false;
      }
    }

    private SafeProcessHandle GetProcessHandle()
    {
      if (this._haveProcessHandle)
      {
        this.ThrowIfExited(true);
        return this._processHandle;
      }
      this.EnsureState(Process.State.HaveNonExitedId | Process.State.IsLocal);
      return new SafeProcessHandle(this._processId, this.GetSafeWaitHandle());
    }

    private bool StartCore(ProcessStartInfo startInfo)
    {
      if (Process.PlatformDoesNotSupportProcessStartAndKill)
        throw new PlatformNotSupportedException();
      Process.EnsureInitialized();
      if (startInfo.UseShellExecute && (startInfo.RedirectStandardInput || startInfo.RedirectStandardOutput || startInfo.RedirectStandardError))
        throw new InvalidOperationException(SR.CantRedirectStreams);
      int stdinFd = -1;
      int stdoutFd = -1;
      int stderrFd = -1;
      string[] envp = Process.CreateEnvp(startInfo);
      string workingDirectory = !string.IsNullOrWhiteSpace(startInfo.WorkingDirectory) ? startInfo.WorkingDirectory : (string) null;
      bool setCredentials = !string.IsNullOrEmpty(startInfo.UserName);
      uint userId = 0;
      uint groupId = 0;
      uint[] groups = (uint[]) null;
      if (setCredentials)
        (userId, groupId, groups) = Process.GetUserAndGroupIds(startInfo);
      bool usesTerminal = !startInfo.RedirectStandardInput || !startInfo.RedirectStandardOutput || !startInfo.RedirectStandardError;
      if (startInfo.UseShellExecute)
      {
        string verb = startInfo.Verb;
        if (verb != string.Empty && !string.Equals(verb, "open", StringComparison.OrdinalIgnoreCase))
          throw new Win32Exception(1155);
        bool flag = false;
        string resolvedFilename = Process.ResolveExecutableForShellExecute(startInfo.FileName, workingDirectory);
        if (resolvedFilename != null)
        {
          string[] argv = Process.ParseArgv(startInfo);
          flag = this.ForkAndExecProcess(startInfo, resolvedFilename, argv, envp, workingDirectory, setCredentials, userId, groupId, groups, out stdinFd, out stdoutFd, out stderrFd, usesTerminal, false);
        }
        if (!flag)
        {
          string pathToOpenFile = Process.GetPathToOpenFile();
          string[] argv = Process.ParseArgv(startInfo, pathToOpenFile, true);
          this.ForkAndExecProcess(startInfo, pathToOpenFile, argv, envp, workingDirectory, setCredentials, userId, groupId, groups, out stdinFd, out stdoutFd, out stderrFd, usesTerminal);
        }
      }
      else
      {
        string str = Process.ResolvePath(startInfo.FileName);
        string[] argv = Process.ParseArgv(startInfo);
        if (Directory.Exists(str))
          throw new Win32Exception(SR.DirectoryNotValidAsInput);
        this.ForkAndExecProcess(startInfo, str, argv, envp, workingDirectory, setCredentials, userId, groupId, groups, out stdinFd, out stdoutFd, out stderrFd, usesTerminal);
      }
      if (startInfo.RedirectStandardInput)
        this._standardInput = new StreamWriter((Stream) Process.OpenStream(stdinFd, PipeDirection.Out), startInfo.StandardInputEncoding ?? Encoding.Default, 4096)
        {
          AutoFlush = true
        };
      if (startInfo.RedirectStandardOutput)
        this._standardOutput = new StreamReader((Stream) Process.OpenStream(stdoutFd, PipeDirection.In), startInfo.StandardOutputEncoding ?? Encoding.Default, true, 4096);
      if (startInfo.RedirectStandardError)
        this._standardError = new StreamReader((Stream) Process.OpenStream(stderrFd, PipeDirection.In), startInfo.StandardErrorEncoding ?? Encoding.Default, true, 4096);
      return true;
    }

    private bool ForkAndExecProcess(
      ProcessStartInfo startInfo,
      string resolvedFilename,
      string[] argv,
      string[] envp,
      string cwd,
      bool setCredentials,
      uint userId,
      uint groupId,
      uint[] groups,
      out int stdinFd,
      out int stdoutFd,
      out int stderrFd,
      bool usesTerminal,
      bool throwOnNoExec = true)
    {
      if (string.IsNullOrEmpty(resolvedFilename))
      {
        Interop.ErrorInfo errorInfo = Interop.Error.ENOENT.Info();
        throw Process.CreateExceptionForErrorStartingProcess(errorInfo.GetErrorMessage(), errorInfo.RawErrno, startInfo.FileName, cwd);
      }
      Process.s_processStartLock.EnterReadLock();
      try
      {
        if (usesTerminal)
          Process.ConfigureTerminalForChildProcesses(1);
        int lpChildPid;
        int num = Interop.Sys.ForkAndExecProcess(resolvedFilename, argv, envp, cwd, startInfo.RedirectStandardInput, startInfo.RedirectStandardOutput, startInfo.RedirectStandardError, setCredentials, userId, groupId, groups, out lpChildPid, out stdinFd, out stdoutFd, out stderrFd);
        if (num == 0)
        {
          this._waitStateHolder = new ProcessWaitState.Holder(lpChildPid, true, usesTerminal);
          this.SetProcessId(lpChildPid);
          this.SetProcessHandle(new SafeProcessHandle(this._processId, this.GetSafeWaitHandle()));
          return true;
        }
        if (!throwOnNoExec && new Interop.ErrorInfo(num).Error == Interop.Error.ENOEXEC)
          return false;
        throw Process.CreateExceptionForErrorStartingProcess(new Interop.ErrorInfo(num).GetErrorMessage(), num, resolvedFilename, cwd);
      }
      finally
      {
        Process.s_processStartLock.ExitReadLock();
        if (this._waitStateHolder == null & usesTerminal)
        {
          Process.s_processStartLock.EnterWriteLock();
          Process.ConfigureTerminalForChildProcesses(-1);
          Process.s_processStartLock.ExitWriteLock();
        }
      }
    }

    private static string[] ParseArgv(
      ProcessStartInfo psi,
      string resolvedExe = null,
      bool ignoreArguments = false)
    {
      if (string.IsNullOrEmpty(resolvedExe) && (ignoreArguments || string.IsNullOrEmpty(psi.Arguments) && !psi.HasArgumentList))
        return new string[1]{ psi.FileName };
      List<string> results = new List<string>();
      if (!string.IsNullOrEmpty(resolvedExe))
      {
        results.Add(resolvedExe);
        if (resolvedExe.Contains("kfmclient"))
          results.Add("openURL");
      }
      results.Add(psi.FileName);
      if (!ignoreArguments)
      {
        if (!string.IsNullOrEmpty(psi.Arguments))
          Process.ParseArgumentsIntoList(psi.Arguments, results);
        else if (psi.HasArgumentList)
          results.AddRange((IEnumerable<string>) psi.ArgumentList);
      }
      return results.ToArray();
    }

    private static string[] CreateEnvp(ProcessStartInfo psi)
    {
      string[] envp = new string[psi.Environment.Count];
      int num = 0;
      foreach (KeyValuePair<string, string> keyValuePair in (IEnumerable<KeyValuePair<string, string>>) psi.Environment)
        envp[num++] = keyValuePair.Key + "=" + keyValuePair.Value;
      return envp;
    }

    private static string ResolveExecutableForShellExecute(string filename, string workingDirectory)
    {
      string path1 = (string) null;
      if (Path.IsPathRooted(filename))
      {
        if (File.Exists(filename))
          path1 = filename;
      }
      else
      {
        Uri result;
        if (Uri.TryCreate(filename, UriKind.Absolute, out result))
        {
          if (result.IsFile && result.Host == "" && File.Exists(result.LocalPath))
            path1 = result.LocalPath;
        }
        else
        {
          workingDirectory = workingDirectory != null ? Path.GetFullPath(workingDirectory) : Directory.GetCurrentDirectory();
          string path2 = Path.Combine(workingDirectory, filename);
          path1 = !File.Exists(path2) ? Process.FindProgramInPath(filename) : path2;
        }
      }
      if (path1 == null)
        return (string) null;
      return Interop.Sys.Access(path1, Interop.Sys.AccessMode.X_OK) == 0 ? path1 : (string) null;
    }

    private static string ResolvePath(string filename)
    {
      if (Path.IsPathRooted(filename))
        return filename;
      string processPath = Environment.ProcessPath;
      if (processPath != null)
      {
        try
        {
          string path = Path.Combine(Path.GetDirectoryName(processPath), filename);
          if (File.Exists(path))
            return path;
        }
        catch (ArgumentException ex)
        {
        }
      }
      string path1 = Path.Combine(Directory.GetCurrentDirectory(), filename);
      return File.Exists(path1) ? path1 : Process.FindProgramInPath(filename);
    }

    private static string FindProgramInPath(string program)
    {
      string environmentVariable = Environment.GetEnvironmentVariable("PATH");
      if (environmentVariable != null)
      {
        StringParser stringParser = new StringParser(environmentVariable, ':', true);
        while (stringParser.MoveNext())
        {
          string fullPath = Path.Combine(stringParser.ExtractCurrent(), program);
          if (Process.IsExecutable(fullPath))
            return fullPath;
        }
      }
      return (string) null;
    }

    private static bool IsExecutable(string fullPath)
    {
      Interop.Sys.FileStatus output;
      if (Interop.Sys.Stat(fullPath, out output) < 0 || (output.Mode & 61440) == 16384)
        return false;
      UnixFileMode unixFileMode = (UnixFileMode) (output.Mode & 73);
      switch (unixFileMode)
      {
        case UnixFileMode.None:
          return false;
        case UnixFileMode.OtherExecute | UnixFileMode.GroupExecute | UnixFileMode.UserExecute:
          return true;
        default:
          uint euid = Interop.Sys.GetEUid();
          if (euid == 0U)
            return true;
          if ((int) euid == (int) output.Uid)
            return (unixFileMode & UnixFileMode.UserExecute) != 0;
          bool flag1 = (unixFileMode & UnixFileMode.GroupExecute) != 0;
          bool flag2 = (unixFileMode & UnixFileMode.OtherExecute) != 0;
          return flag1 == flag2 || Interop.Sys.IsMemberOfGroup(output.Gid) ? flag1 : flag2;
      }
    }

    internal static TimeSpan TicksToTimeSpan(double ticks)
    {
      long num = Volatile.Read(ref Process.s_ticksPerSecond);
      if (num == 0L)
      {
        num = Interop.Sys.SysConf(Interop.Sys.SysConfName._SC_CLK_TCK);
        if (num <= 0L)
          throw new Win32Exception();
        Volatile.Write(ref Process.s_ticksPerSecond, num);
      }
      return TimeSpan.FromSeconds(ticks / (double) num);
    }

    private static AnonymousPipeClientStream OpenStream(int fd, PipeDirection direction)
    {
      return new AnonymousPipeClientStream(direction, new SafePipeHandle((IntPtr) fd, true));
    }

    private static void ParseArgumentsIntoList(string arguments, List<string> results)
    {
      for (int i = 0; i < arguments.Length; ++i)
      {
        while (i < arguments.Length && (arguments[i] == ' ' || arguments[i] == '\t'))
          ++i;
        if (i == arguments.Length)
          break;
        results.Add(Process.GetNextArgument(arguments, ref i));
      }
    }

    private static string GetNextArgument(string arguments, ref int i)
    {
      ValueStringBuilder valueStringBuilder = new ValueStringBuilder(stackalloc char[256]);
      bool flag = false;
      while (i < arguments.Length)
      {
        int count = 0;
        while (i < arguments.Length && arguments[i] == '\\')
        {
          ++i;
          ++count;
        }
        if (count > 0)
        {
          if (i >= arguments.Length || arguments[i] != '"')
          {
            valueStringBuilder.Append('\\', count);
          }
          else
          {
            valueStringBuilder.Append('\\', count / 2);
            if (count % 2 != 0)
            {
              valueStringBuilder.Append('"');
              ++i;
            }
          }
        }
        else
        {
          char c = arguments[i];
          switch (c)
          {
            case '\t':
            case ' ':
              if (!flag)
                goto label_17;
              else
                break;
            case '"':
              if (flag && i < arguments.Length - 1 && arguments[i + 1] == '"')
              {
                valueStringBuilder.Append('"');
                ++i;
              }
              else
                flag = !flag;
              ++i;
              continue;
          }
          valueStringBuilder.Append(c);
          ++i;
        }
      }
label_17:
      return valueStringBuilder.ToString();
    }

    private ProcessWaitState GetWaitState()
    {
      if (this._waitStateHolder == null)
      {
        this.EnsureState(Process.State.HaveId);
        this._waitStateHolder = new ProcessWaitState.Holder(this._processId);
      }
      return this._waitStateHolder._state;
    }

    private SafeWaitHandle GetSafeWaitHandle()
    {
      return this.GetWaitState().EnsureExitedEvent().GetSafeWaitHandle();
    }

    private static (uint userId, uint groupId, uint[] groups) GetUserAndGroupIds(
      ProcessStartInfo startInfo)
    {
      (uint? userId, uint? groupId) = Process.GetUserAndGroupIds(startInfo.UserName);
      if (!userId.HasValue)
        throw new Win32Exception(SR.Format(SR.UserDoesNotExist, (object) startInfo.UserName));
      return (userId.Value, groupId.Value, Interop.Sys.GetGroupList(startInfo.UserName, groupId.Value) ?? throw new Win32Exception(SR.Format(SR.UserGroupsCannotBeDetermined, (object) startInfo.UserName)));
    }

    private static unsafe (uint? userId, uint? groupId) GetUserAndGroupIds(string userName)
    {
      byte* buf1 = stackalloc byte[256];
      Interop.Sys.Passwd? passwd;
      if (Process.TryGetPasswd(userName, buf1, 256, out passwd))
        return !passwd.HasValue ? (new uint?(), new uint?()) : (new uint?(passwd.Value.UserId), new uint?(passwd.Value.GroupId));
      int length = 256;
      while (true)
      {
        length *= 2;
        byte[] numArray = new byte[length];
        fixed (byte* buf2 = &numArray[0])
        {
          if (!Process.TryGetPasswd(userName, buf2, numArray.Length, out passwd))
          {
            // ISSUE: __unpin statement
            __unpin(buf2);
          }
          else
            break;
        }
      }
      return !passwd.HasValue ? (new uint?(), new uint?()) : (new uint?(passwd.Value.UserId), new uint?(passwd.Value.GroupId));
    }

    private static unsafe bool TryGetPasswd(
      string name,
      byte* buf,
      int bufLen,
      out Interop.Sys.Passwd? passwd)
    {
      Interop.Sys.Passwd pwd;
      int pwNamR = Interop.Sys.GetPwNamR(name, out pwd, buf, bufLen);
      switch (pwNamR)
      {
        case -1:
          passwd = new Interop.Sys.Passwd?();
          return true;
        case 0:
          passwd = new Interop.Sys.Passwd?(pwd);
          return true;
        default:
          Interop.ErrorInfo errorInfo = new Interop.ErrorInfo(pwNamR);
          if (errorInfo.Error != Interop.Error.ERANGE)
            throw new Win32Exception(errorInfo.RawErrno, errorInfo.GetErrorMessage());
          passwd = new Interop.Sys.Passwd?();
          return false;
      }
    }

    /// <summary>Gets the window handle of the main window of the associated process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.MainWindowHandle" /> is not defined because the process has exited.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainWindowHandle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>The system-generated window handle of the main window of the associated process.</returns>
    public IntPtr MainWindowHandle => IntPtr.Zero;

    #nullable enable
    /// <summary>Gets the caption of the main window of the process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Diagnostics.Process.MainWindowTitle" /> property is not defined because the process has exited.</exception>
    /// <exception cref="T:System.NotSupportedException">You are trying to access the <see cref="P:System.Diagnostics.Process.MainWindowTitle" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>The main window title of the process.</returns>
    public string MainWindowTitle => string.Empty;

    /// <summary>Gets a value indicating whether the user interface of the process is responding.</summary>
    /// <exception cref="T:System.InvalidOperationException">There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.Responding" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>
    /// <see langword="true" /> if the user interface of the associated process is responding to the system; otherwise, <see langword="false" />.</returns>
    public bool Responding => true;

    private static bool WaitForInputIdleCore(int _)
    {
      throw new InvalidOperationException(SR.InputIdleUnknownError);
    }

    private static void EnsureInitialized()
    {
      if (Process.s_initialized)
        return;
      lock (Process.s_initializedGate)
      {
        if (Process.s_initialized)
          return;
        if (!Interop.Sys.InitializeTerminalAndSignalHandling())
          throw new Win32Exception();
        // ISSUE: method pointer
        // ISSUE: cast to a function pointer type
        Interop.Sys.RegisterForSigChld((__FnPtr<int (int, int)>) __methodptr(OnSigChild));
        Process.SetDelayedSigChildConsoleConfigurationHandler();
        Process.s_initialized = true;
      }
    }

    [UnmanagedCallersOnly]
    private static int OnSigChild(int reapAll, int configureConsole)
    {
      Process.s_processStartLock.EnterWriteLock();
      try
      {
        bool childrenUsingTerminal1 = Process.AreChildrenUsingTerminal;
        ProcessWaitState.CheckChildren(reapAll != 0, configureConsole != 0);
        bool childrenUsingTerminal2 = Process.AreChildrenUsingTerminal;
        return !childrenUsingTerminal1 || childrenUsingTerminal2 || configureConsole != 0 ? 0 : 1;
      }
      finally
      {
        Process.s_processStartLock.ExitWriteLock();
      }
    }

    /// <summary>Gets the name of the process.</summary>
    /// <exception cref="T:System.InvalidOperationException">The process does not have an identifier, or no process is associated with the <see cref="T:System.Diagnostics.Process" />.
    /// 
    /// -or-
    /// 
    /// The associated process has exited.</exception>
    /// <exception cref="T:System.NotSupportedException">The process is not on this computer.</exception>
    /// <returns>The name that the system uses to identify the process to the user.</returns>
    public string ProcessName
    {
      get
      {
        this.EnsureState(Process.State.HaveProcessInfo);
        return this._processInfo.ProcessName;
      }
    }

    private static bool PlatformDoesNotSupportProcessStartAndKill
    {
      get
      {
        return OperatingSystem.IsIOS() && !OperatingSystem.IsMacCatalyst() || OperatingSystem.IsTvOS();
      }
    }

    internal static void ConfigureTerminalForChildProcesses(int increment, bool configureConsole = true)
    {
      int num = Interlocked.Add(ref Process.s_childrenUsingTerminalCount, increment);
      if (increment > 0)
      {
        Interop.Sys.ConfigureTerminalForChildProcess(true);
      }
      else
      {
        if (!(num == 0 & configureConsole))
          return;
        Interop.Sys.ConfigureTerminalForChildProcess(false);
      }
    }

    private static void SetDelayedSigChildConsoleConfigurationHandler()
    {
      // ISSUE: method pointer
      // ISSUE: cast to a function pointer type
      Interop.Sys.SetDelayedSigChildConsoleConfigurationHandler((__FnPtr<void ()>) __methodptr(DelayedSigChildConsoleConfiguration));
    }

    [UnmanagedCallersOnly]
    private static void DelayedSigChildConsoleConfiguration()
    {
      Process.s_processStartLock.EnterWriteLock();
      try
      {
        if (Process.s_childrenUsingTerminalCount != 0)
          return;
        Interop.Sys.ConfigureTerminalForChildProcess(false);
      }
      finally
      {
        Process.s_processStartLock.ExitWriteLock();
      }
    }

    private static bool AreChildrenUsingTerminal => Process.s_childrenUsingTerminalCount > 0;

    /// <summary>Creates an array of new <see cref="T:System.Diagnostics.Process" /> components and associates them with all the process resources on a remote computer that share the specified process name.</summary>
    /// <param name="processName">The friendly name of the process.</param>
    /// <param name="machineName">The name of a computer on the network.</param>
    /// <exception cref="T:System.ArgumentException">The <paramref name="machineName" /> parameter syntax is invalid. It might have length zero (0).</exception>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="machineName" /> parameter is <see langword="null" />.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The operating system platform does not support this operation on remote computers.</exception>
    /// <exception cref="T:System.InvalidOperationException">The attempt to connect to <paramref name="machineName" /> has failed.
    /// 
    ///  -or-
    /// 
    /// There are problems accessing the performance counter APIs used to get process information. This exception is specific to Windows NT, Windows 2000, and Windows XP.</exception>
    /// <exception cref="T:System.ComponentModel.Win32Exception">A problem occurred accessing an underlying system API.</exception>
    /// <returns>An array of type <see cref="T:System.Diagnostics.Process" /> that represents the process resources running the specified application or file.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public static Process[] GetProcessesByName(string? processName, string machineName)
    {
      ProcessManager.ThrowIfRemoteMachine(machineName);
      if (processName == null)
        processName = "";
      ArrayBuilder<Process> arrayBuilder = new ArrayBuilder<Process>();
      foreach (int enumerateProcessId in ProcessManager.EnumerateProcessIds())
      {
        Interop.procfs.ParsedStat result1;
        if (Interop.procfs.TryReadStatFile(enumerateProcessId, out result1))
        {
          string untruncatedProcessName = Process.GetUntruncatedProcessName(ref result1);
          Interop.procfs.ParsedStatus result2;
          if ((processName == "" || string.Equals(processName, untruncatedProcessName, StringComparison.OrdinalIgnoreCase)) && Interop.procfs.TryReadStatusFile(enumerateProcessId, out result2))
          {
            ProcessInfo processInfo = ProcessManager.CreateProcessInfo(ref result1, ref result2, untruncatedProcessName);
            arrayBuilder.Add(new Process(machineName, false, enumerateProcessId, processInfo));
          }
        }
      }
      return arrayBuilder.ToArray();
    }

    /// <summary>Gets the privileged processor time for this process.</summary>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.PrivilegedProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the process has spent running code inside the operating system core.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public TimeSpan PrivilegedProcessorTime
    {
      get => Process.TicksToTimeSpan((double) this.GetStat().stime);
    }

    internal DateTime StartTimeCore
    {
      get => Process.BootTimeToDateTime(Process.TicksToTimeSpan((double) this.GetStat().starttime));
    }

    internal static DateTime BootTimeToDateTime(TimeSpan timespanAfterBoot)
    {
      return (Process.BootTime + timespanAfterBoot).ToLocalTime();
    }

    private static DateTime BootTime
    {
      get
      {
        long ticks = Interlocked.Read(ref Process.s_bootTimeTicks);
        if (ticks == 0L)
        {
          ticks = Interop.Sys.GetBootTimeTicks();
          long num = Interlocked.CompareExchange(ref Process.s_bootTimeTicks, ticks, 0L);
          if (num != 0L)
            ticks = num;
        }
        return new DateTime(ticks);
      }
    }

    private int ParentProcessId => this.GetStat().ppid;

    #nullable disable
    private static string GetPathToOpenFile()
    {
      string[] strArray = new string[3]
      {
        "xdg-open",
        "gnome-open",
        "kfmclient"
      };
      foreach (string program in strArray)
      {
        string programInPath = Process.FindProgramInPath(program);
        if (!string.IsNullOrEmpty(programInPath))
          return programInPath;
      }
      return (string) null;
    }

    /// <summary>Gets the total processor time for this process.</summary>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.TotalProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the associated process has spent utilizing the CPU. This value is the sum of the <see cref="P:System.Diagnostics.Process.UserProcessorTime" /> and the <see cref="P:System.Diagnostics.Process.PrivilegedProcessorTime" />.</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public TimeSpan TotalProcessorTime
    {
      get
      {
        Interop.procfs.ParsedStat stat = this.GetStat();
        return Process.TicksToTimeSpan((double) (stat.utime + stat.stime));
      }
    }

    /// <summary>Gets the user processor time for this process.</summary>
    /// <exception cref="T:System.NotSupportedException">You are attempting to access the <see cref="P:System.Diagnostics.Process.UserProcessorTime" /> property for a process that is running on a remote computer. This property is available only for processes that are running on the local computer.</exception>
    /// <returns>A <see cref="T:System.TimeSpan" /> that indicates the amount of time that the associated process has spent running code inside the application portion of the process (not inside the operating system core).</returns>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public TimeSpan UserProcessorTime => Process.TicksToTimeSpan((double) this.GetStat().utime);

    private IntPtr ProcessorAffinityCore
    {
      get
      {
        this.EnsureState(Process.State.HaveNonExitedId);
        IntPtr mask;
        if (Interop.Sys.SchedGetAffinity(this._processId, out mask) != 0)
          throw new Win32Exception();
        return mask;
      }
      set
      {
        this.EnsureState(Process.State.HaveNonExitedId);
        if (Interop.Sys.SchedSetAffinity(this._processId, ref value) != 0)
          throw new Win32Exception();
      }
    }

    private void GetWorkingSetLimits(out IntPtr minWorkingSet, out IntPtr maxWorkingSet)
    {
      minWorkingSet = IntPtr.Zero;
      this.EnsureState(Process.State.HaveNonExitedId);
      ulong limit;
      if (!Interop.cgroups.TryGetMemoryLimit(out limit))
        limit = this.GetStat().rsslim;
      switch (IntPtr.Size)
      {
        case 4:
          if (limit > (ulong) int.MaxValue)
          {
            limit = (ulong) int.MaxValue;
            break;
          }
          break;
        case 8:
          if (limit > (ulong) long.MaxValue)
          {
            limit = (ulong) long.MaxValue;
            break;
          }
          break;
      }
      maxWorkingSet = (IntPtr) (long) limit;
    }

    private static void SetWorkingSetLimitsCore(
      IntPtr? newMin,
      IntPtr? newMax,
      out IntPtr resultingMin,
      out IntPtr resultingMax)
    {
      throw new PlatformNotSupportedException(SR.MinimumWorkingSetNotSupported);
    }

    internal static string GetExePath(int processId = -1)
    {
      return processId != -1 ? Interop.Sys.ReadLink((ReadOnlySpan<char>) Interop.procfs.GetExeFilePathForProcess(processId)) : Environment.ProcessPath;
    }

    internal static string GetUntruncatedProcessName(ref Interop.procfs.ParsedStat stat)
    {
      string linePathForProcess = Interop.procfs.GetCmdLinePathForProcess(stat.pid);
      byte[] array1 = (byte[]) null;
      try
      {
        using (FileStream fileStream = new FileStream(linePathForProcess, FileMode.Open, FileAccess.Read, FileShare.Read, 1, false))
        {
          Span<byte> span1 = stackalloc byte[512];
          int num1 = 0;
          int num2;
          do
          {
            if (num1 == span1.Length)
            {
              byte[] destination = ArrayPool<byte>.Shared.Rent(span1.Length * 2);
              span1.CopyTo((Span<byte>) destination);
              byte[] array2 = array1;
              span1 = (Span<byte>) (array1 = destination);
              if (array2 != null)
                ArrayPool<byte>.Shared.Return(array2);
            }
            num2 = fileStream.Read(span1.Slice(num1));
            num1 += num2;
            Span<byte> span2 = span1.Slice(0, num1);
            int length1 = span2.IndexOf<byte>((byte) 0);
            if (length1 != -1)
            {
              string untruncatedNameFromArg = GetUntruncatedNameFromArg(span2.Slice(0, length1), stat.comm);
              if (untruncatedNameFromArg != null)
                return untruncatedNameFromArg;
              Span<byte> span3 = span2.Slice(length1 + 1);
              int length2 = span3.IndexOf<byte>((byte) 0);
              if (length2 != -1)
                return GetUntruncatedNameFromArg(span3.Slice(0, length2), stat.comm) ?? stat.comm;
            }
          }
          while (num2 != 0);
          return stat.comm;
        }
      }
      catch (IOException ex)
      {
        return stat.comm;
      }
      finally
      {
        if (array1 != null)
          ArrayPool<byte>.Shared.Return(array1);
      }

      static string GetUntruncatedNameFromArg(Span<byte> arg, string prefix)
      {
        int start = arg.LastIndexOf<byte>((byte) 47) + 1;
        string str = Encoding.UTF8.GetString((ReadOnlySpan<byte>) arg.Slice(start));
        return str.StartsWith(prefix, StringComparison.OrdinalIgnoreCase) ? str : (string) null;
      }
    }

    private Interop.procfs.ParsedStat GetStat()
    {
      this.EnsureState(Process.State.HaveNonExitedId);
      Interop.procfs.ParsedStat result;
      if (!Interop.procfs.TryReadStatFile(this._processId, out result))
        throw new Win32Exception(SR.ProcessInformationUnavailable);
      return result;
    }

    /// <summary>Immediately stops the associated process, and optionally its child/descendent processes.</summary>
    /// <param name="entireProcessTree">
    /// <see langword="true" /> to kill the associated process and its descendants; <see langword="false" /> to kill only the associated process.</param>
    /// <exception cref="T:System.ComponentModel.Win32Exception">The associated process could not be terminated.
    /// 
    /// -or-
    /// 
    /// The process is terminating.</exception>
    /// <exception cref="T:System.NotSupportedException">You are attempting to call <see cref="M:System.Diagnostics.Process.Kill" /> for a process that is running on a remote computer. The method is available only for processes running on the local computer.</exception>
    /// <exception cref="T:System.InvalidOperationException">.NET Framework and .NET Core 3.0 and earlier versions only: The process has already exited.
    /// 
    /// -or-
    /// 
    /// There is no process associated with this <see cref="T:System.Diagnostics.Process" /> object.
    /// 
    /// -or-
    /// 
    /// The calling process is a member of the associated process's descendant tree.</exception>
    /// <exception cref="T:System.AggregateException">Not all processes in the associated process's descendant tree could be terminated.</exception>
    [UnsupportedOSPlatform("ios")]
    [UnsupportedOSPlatform("tvos")]
    [SupportedOSPlatform("maccatalyst")]
    public void Kill(bool entireProcessTree)
    {
      if (!entireProcessTree)
      {
        this.Kill();
      }
      else
      {
        this.EnsureState(Process.State.IsLocal | Process.State.Associated);
        if (this.IsSelfOrDescendantOf(Process.GetCurrentProcess()))
          throw new InvalidOperationException(SR.KillEntireProcessTree_DisallowedBecauseTreeContainsCallingProcess);
        List<Exception> innerExceptions = this.KillTree();
        if (innerExceptions != null && innerExceptions.Count != 0)
          throw new AggregateException(SR.KillEntireProcessTree_TerminationIncomplete, (IEnumerable<Exception>) innerExceptions);
      }
    }

    private bool IsSelfOrDescendantOf(Process processOfInterest)
    {
      if (this.Equals(processOfInterest))
        return true;
      Process[] processes = Process.GetProcesses();
      try
      {
        Queue<Process> processQueue = new Queue<Process>();
        Process result = this;
        do
        {
          foreach (Process childProcess in result.GetChildProcesses(processes))
          {
            if (processOfInterest.Equals(childProcess))
              return true;
            processQueue.Enqueue(childProcess);
          }
        }
        while (processQueue.TryDequeue(out result));
      }
      finally
      {
        foreach (Component component in processes)
          component.Dispose();
      }
      return false;
    }

    private List<Process> GetChildProcesses(Process[] processes = null)
    {
      bool flag1 = processes == null;
      if (processes == null)
        processes = Process.GetProcesses();
      List<Process> childProcesses = new List<Process>();
      foreach (Process process in processes)
      {
        bool flag2 = flag1;
        try
        {
          if (this.IsParentOf(process))
          {
            childProcesses.Add(process);
            flag2 = false;
          }
        }
        finally
        {
          if (flag2)
            process.Dispose();
        }
      }
      return childProcesses;
    }

    private static bool IsProcessInvalidException(Exception e)
    {
      return e is InvalidOperationException || e is Win32Exception;
    }

    private enum StreamReadMode
    {
      Undefined,
      SyncMode,
      AsyncMode,
    }

    private enum State
    {
      HaveId = 1,
      IsLocal = 2,
      HaveNonExitedId = 5,
      HaveProcessInfo = 8,
      Exited = 16, // 0x00000010
      Associated = 32, // 0x00000020
    }
  }
}
