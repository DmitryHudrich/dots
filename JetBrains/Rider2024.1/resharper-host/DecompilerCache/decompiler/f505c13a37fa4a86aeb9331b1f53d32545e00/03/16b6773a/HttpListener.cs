// Decompiled with JetBrains decompiler
// Type: System.Net.HttpListener
// Assembly: System.Net.HttpListener, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: F505C13A-37FA-4A86-AEB9-331B1F53D325
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Net.HttpListener.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Net.HttpListener.xml

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Security;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace System.Net
{
  /// <summary>Provides a simple, programmatically controlled HTTP protocol listener. This class cannot be inherited.</summary>
  public sealed class HttpListener : IDisposable
  {
    #nullable disable
    private readonly object _internalLock;
    private volatile HttpListener.State _state;
    private readonly HttpListenerPrefixCollection _prefixes;
    internal Hashtable _uriPrefixes = new Hashtable();
    private bool _ignoreWriteExceptions;
    private readonly ServiceNameStore _defaultServiceNames;
    private readonly HttpListenerTimeoutManager _timeoutManager;
    private ExtendedProtectionPolicy _extendedProtectionPolicy;
    private AuthenticationSchemeSelector _authenticationDelegate;
    private AuthenticationSchemes _authenticationScheme = AuthenticationSchemes.Anonymous;
    private HttpListener.ExtendedProtectionSelector _extendedProtectionSelectorDelegate;
    private string _realm;
    private readonly Dictionary<HttpListenerContext, HttpListenerContext> _listenerContexts = new Dictionary<HttpListenerContext, HttpListenerContext>();
    private readonly List<HttpListenerContext> _contextQueue = new List<HttpListenerContext>();
    private readonly List<ListenerAsyncResult> _asyncWaitQueue = new List<ListenerAsyncResult>();
    private readonly Dictionary<HttpConnection, HttpConnection> _connections = new Dictionary<HttpConnection, HttpConnection>();
    private bool _unsafeConnectionNtlmAuthentication;

    #nullable enable
    internal ICollection PrefixCollection => this._uriPrefixes.Keys;

    /// <summary>Initializes a new instance of the <see cref="T:System.Net.HttpListener" /> class.</summary>
    /// <exception cref="T:System.PlatformNotSupportedException">This class cannot be used on the current operating system. Windows Server 2003 or Windows XP SP2 is required to use instances of this class.</exception>
    public HttpListener()
    {
      this._state = HttpListener.State.Stopped;
      this._internalLock = new object();
      this._defaultServiceNames = new ServiceNameStore();
      this._timeoutManager = new HttpListenerTimeoutManager(this);
      this._prefixes = new HttpListenerPrefixCollection(this);
      this._extendedProtectionPolicy = new ExtendedProtectionPolicy(PolicyEnforcement.Never);
    }

    /// <summary>Gets or sets the delegate called to determine the protocol used to authenticate clients.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>An <see cref="T:System.Net.AuthenticationSchemeSelector" /> delegate that invokes the method used to select an authentication protocol. The default value is <see langword="null" />.</returns>
    public AuthenticationSchemeSelector? AuthenticationSchemeSelectorDelegate
    {
      get => this._authenticationDelegate;
      set
      {
        this.CheckDisposed();
        this._authenticationDelegate = value;
      }
    }

    /// <summary>Gets or sets the delegate called to determine the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for each request.</summary>
    /// <exception cref="T:System.ArgumentException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property, but the <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.CustomChannelBinding" /> property must be <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property to <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property after the <see cref="M:System.Net.HttpListener.Start" /> method was already called.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionSelectorDelegate" /> property on a platform that does not support extended protection.</exception>
    /// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> that specifies the policy to use for extended protection.</returns>
    public HttpListener.ExtendedProtectionSelector? ExtendedProtectionSelectorDelegate
    {
      get => this._extendedProtectionSelectorDelegate;
      [param: DisallowNull] set
      {
        this.CheckDisposed();
        ArgumentNullException.ThrowIfNull((object) value, nameof (value));
        this._extendedProtectionSelectorDelegate = value;
      }
    }

    /// <summary>Gets or sets the scheme used to authenticate clients.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>A bitwise combination of <see cref="T:System.Net.AuthenticationSchemes" /> enumeration values that indicates how clients are to be authenticated. The default value is <see cref="F:System.Net.AuthenticationSchemes.Anonymous" />.</returns>
    public AuthenticationSchemes AuthenticationSchemes
    {
      get => this._authenticationScheme;
      set
      {
        this.CheckDisposed();
        this._authenticationScheme = value;
      }
    }

    /// <summary>Gets or sets the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for extended protection for a session.</summary>
    /// <exception cref="T:System.ArgumentException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property, but the <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.CustomChannelBinding" /> property was not <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentNullException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property to <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">An attempt was made to set the <see cref="P:System.Net.HttpListener.ExtendedProtectionPolicy" /> property after the <see cref="M:System.Net.HttpListener.Start" /> method was already called.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    /// <exception cref="T:System.PlatformNotSupportedException">The <see cref="P:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy.PolicyEnforcement" /> property was set to <see cref="F:System.Security.Authentication.ExtendedProtection.PolicyEnforcement.Always" /> on a platform that does not support extended protection.</exception>
    /// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> that specifies the policy to use for extended protection.</returns>
    public ExtendedProtectionPolicy ExtendedProtectionPolicy
    {
      get => this._extendedProtectionPolicy;
      set
      {
        this.CheckDisposed();
        ArgumentNullException.ThrowIfNull((object) value, nameof (value));
        this._extendedProtectionPolicy = value.CustomChannelBinding == null ? value : throw new ArgumentException(SR.net_listener_cannot_set_custom_cbt, nameof (value));
      }
    }

    /// <summary>Gets a default list of Service Provider Names (SPNs) as determined by registered prefixes.</summary>
    /// <returns>A <see cref="T:System.Security.Authentication.ExtendedProtection.ServiceNameCollection" /> that contains a list of SPNs.</returns>
    public ServiceNameCollection DefaultServiceNames => this._defaultServiceNames.ServiceNames;

    /// <summary>Gets the Uniform Resource Identifier (URI) prefixes handled by this <see cref="T:System.Net.HttpListener" /> object.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>An <see cref="T:System.Net.HttpListenerPrefixCollection" /> that contains the URI prefixes that this <see cref="T:System.Net.HttpListener" /> object is configured to handle.</returns>
    public HttpListenerPrefixCollection Prefixes
    {
      get
      {
        this.CheckDisposed();
        return this._prefixes;
      }
    }

    #nullable disable
    internal void AddPrefix(string uriPrefix)
    {
      ArgumentNullException.ThrowIfNull((object) uriPrefix, nameof (uriPrefix));
      try
      {
        this.CheckDisposed();
        int i;
        if (string.Compare(uriPrefix, 0, "http://", 0, 7, StringComparison.OrdinalIgnoreCase) == 0)
        {
          i = 7;
        }
        else
        {
          if (string.Compare(uriPrefix, 0, "https://", 0, 8, StringComparison.OrdinalIgnoreCase) != 0)
            throw new ArgumentException(SR.net_listener_scheme, nameof (uriPrefix));
          i = 8;
        }
        bool flag = false;
        int num;
        for (num = i; num < uriPrefix.Length && uriPrefix[num] != '/' && uriPrefix[num] != ':' | flag; ++num)
        {
          if (uriPrefix[num] == '[')
          {
            if (flag)
            {
              num = i;
              break;
            }
            flag = true;
          }
          if (flag && uriPrefix[num] == ']')
            flag = false;
        }
        if (i == num)
          throw new ArgumentException(SR.net_listener_host, nameof (uriPrefix));
        if (!uriPrefix.EndsWith('/'))
          throw new ArgumentException(SR.net_listener_slash, nameof (uriPrefix));
        string registeredPrefix = CreateRegisteredPrefix(uriPrefix, num, i);
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Info((object) this, FormattableStringFactory.Create("mapped uriPrefix: {0} to registeredPrefix: {1}", (object) uriPrefix, (object) registeredPrefix), nameof (AddPrefix));
        if (this._state == HttpListener.State.Started)
          this.AddPrefixCore(registeredPrefix);
        this._uriPrefixes[(object) uriPrefix] = (object) registeredPrefix;
        this._defaultServiceNames.Add(uriPrefix);
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) this, (object) ex, nameof (AddPrefix));
        throw;
      }

      static string CreateRegisteredPrefix(string uriPrefix, int j, int i)
      {
        int length1 = uriPrefix.Length;
        if (uriPrefix[j] != ':')
          length1 += i == 7 ? ":80".Length : ":443".Length;
        return string.Create<(string, int, int)>(length1, (uriPrefix, j, i), (SpanAction<char, (string, int, int)>) ((destination, state) =>
        {
          if (state.uriPrefix[state.j] == ':')
          {
            state.uriPrefix.CopyTo(destination);
          }
          else
          {
            int j = state.j;
            state.uriPrefix.AsSpan(0, j).CopyTo(destination);
            int start;
            if (state.i == 7)
            {
              ":80".CopyTo(destination.Slice(j));
              start = j + 3;
            }
            else
            {
              ":443".CopyTo(destination.Slice(j));
              start = j + 4;
            }
            state.uriPrefix.AsSpan(state.j).CopyTo(destination.Slice(start));
          }
          int length = destination.IndexOf<char>(':');
          if (length < 0)
            length = destination.Length;
          Ascii.ToLowerInPlace(destination.Slice(0, length), out int _);
        }));
      }
    }

    internal bool ContainsPrefix(string uriPrefix)
    {
      return this._uriPrefixes.Contains((object) uriPrefix);
    }

    internal bool RemovePrefix(string uriPrefix)
    {
      try
      {
        this.CheckDisposed();
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Info((object) this, FormattableStringFactory.Create("uriPrefix: {0}", (object) uriPrefix), nameof (RemovePrefix));
        ArgumentNullException.ThrowIfNull((object) uriPrefix, nameof (uriPrefix));
        if (!this._uriPrefixes.Contains((object) uriPrefix))
          return false;
        if (this._state == HttpListener.State.Started)
          this.RemovePrefixCore((string) this._uriPrefixes[(object) uriPrefix]);
        this._uriPrefixes.Remove((object) uriPrefix);
        this._defaultServiceNames.Remove(uriPrefix);
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) this, (object) ex, nameof (RemovePrefix));
        throw;
      }
      return true;
    }

    internal void RemoveAll(bool clear)
    {
      this.CheckDisposed();
      if (this._uriPrefixes.Count <= 0)
        return;
      if (this._state == HttpListener.State.Started)
      {
        foreach (string uriPrefix in (IEnumerable) this._uriPrefixes.Values)
          this.RemovePrefixCore(uriPrefix);
      }
      if (!clear)
        return;
      this._uriPrefixes.Clear();
      this._defaultServiceNames.Clear();
    }

    #nullable enable
    /// <summary>Gets or sets the realm, or resource partition, associated with this <see cref="T:System.Net.HttpListener" /> object.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>A <see cref="T:System.String" /> value that contains the name of the realm associated with the <see cref="T:System.Net.HttpListener" /> object.</returns>
    public string? Realm
    {
      get => this._realm;
      set
      {
        this.CheckDisposed();
        this._realm = value;
      }
    }

    /// <summary>Gets a value that indicates whether <see cref="T:System.Net.HttpListener" /> has been started.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Net.HttpListener" /> was started; otherwise, <see langword="false" />.</returns>
    public bool IsListening => this._state == HttpListener.State.Started;

    /// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that specifies whether your application receives exceptions that occur when an <see cref="T:System.Net.HttpListener" /> sends the response to the client.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>
    /// <see langword="true" /> if this <see cref="T:System.Net.HttpListener" /> should not return exceptions that occur when sending the response to the client; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
    public bool IgnoreWriteExceptions
    {
      get => this._ignoreWriteExceptions;
      set
      {
        this.CheckDisposed();
        this._ignoreWriteExceptions = value;
      }
    }

    /// <summary>Waits for an incoming request as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.HttpListenerContext" /> object that represents a client request.</returns>
    public Task<HttpListenerContext> GetContextAsync()
    {
      return Task.Factory.FromAsync<HttpListenerContext>((Func<AsyncCallback, object, IAsyncResult>) ((callback, state) => ((HttpListener) state).BeginGetContext(callback, state)), (Func<IAsyncResult, HttpListenerContext>) (iar => ((HttpListener) iar.AsyncState).EndGetContext(iar)), (object) this);
    }

    /// <summary>Shuts down the <see cref="T:System.Net.HttpListener" />.</summary>
    public void Close()
    {
      try
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Info((object) "HttpListenerRequest::Close()", (FormattableString) null, nameof (Close));
        ((IDisposable) this).Dispose();
      }
      catch (Exception ex)
      {
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Error((object) this, FormattableStringFactory.Create("Close {0}", (object) ex), nameof (Close));
        throw;
      }
    }

    internal void CheckDisposed()
    {
      ObjectDisposedException.ThrowIf(this._state == HttpListener.State.Closed, (object) this);
    }

    /// <summary>Releases the resources held by this <see cref="T:System.Net.HttpListener" /> object.</summary>
    void IDisposable.Dispose() => this.Dispose();

    /// <summary>Gets a value that indicates whether <see cref="T:System.Net.HttpListener" /> can be used with the current operating system.</summary>
    /// <returns>
    /// <see langword="true" /> on all platforms.</returns>
    public static bool IsSupported => true;

    /// <summary>The timeout manager for this <see cref="T:System.Net.HttpListener" /> instance.</summary>
    /// <returns>The timeout manager for this <see cref="T:System.Net.HttpListener" /> instance.</returns>
    public HttpListenerTimeoutManager TimeoutManager
    {
      get
      {
        this.CheckDisposed();
        return this._timeoutManager;
      }
    }

    #nullable disable
    private void AddPrefixCore(string uriPrefix) => HttpEndPointManager.AddPrefix(uriPrefix, this);

    private void RemovePrefixCore(string uriPrefix)
    {
      HttpEndPointManager.RemovePrefix(uriPrefix, this);
    }

    /// <summary>Allows this instance to receive incoming requests.</summary>
    /// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    public void Start()
    {
      lock (this._internalLock)
      {
        try
        {
          this.CheckDisposed();
          if (this._state == HttpListener.State.Started)
            return;
          HttpEndPointManager.AddListener(this);
          this._state = HttpListener.State.Started;
        }
        catch (Exception ex)
        {
          this._state = HttpListener.State.Closed;
          if (NetEventSource.Log.IsEnabled())
            NetEventSource.Error((object) this, FormattableStringFactory.Create("Start {0}", (object) ex), nameof (Start));
          throw;
        }
      }
    }

    /// <summary>Gets or sets a <see cref="T:System.Boolean" /> value that controls whether, when NTLM is used, additional requests using the same Transmission Control Protocol (TCP) connection are required to authenticate.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Security.Principal.IIdentity" /> of the first request will be used for subsequent requests on the same connection; otherwise, <see langword="false" />. The default value is <see langword="false" />.</returns>
    public bool UnsafeConnectionNtlmAuthentication
    {
      get => this._unsafeConnectionNtlmAuthentication;
      set
      {
        this.CheckDisposed();
        this._unsafeConnectionNtlmAuthentication = value;
      }
    }

    /// <summary>Causes this instance to stop receiving new incoming requests and terminates processing of all ongoing requests.</summary>
    /// <exception cref="T:System.ObjectDisposedException">This object has been closed.</exception>
    public void Stop()
    {
      lock (this._internalLock)
      {
        try
        {
          this.CheckDisposed();
          if (this._state == HttpListener.State.Stopped)
            return;
          this.Close(false);
        }
        catch (Exception ex)
        {
          if (NetEventSource.Log.IsEnabled())
            NetEventSource.Error((object) this, FormattableStringFactory.Create("Stop {0}", (object) ex), nameof (Stop));
          throw;
        }
        finally
        {
          this._state = HttpListener.State.Stopped;
        }
      }
    }

    /// <summary>Shuts down the <see cref="T:System.Net.HttpListener" /> object immediately, discarding all currently queued requests.</summary>
    public void Abort()
    {
      lock (this._internalLock)
      {
        try
        {
          if (this._state == HttpListener.State.Closed || this._state != HttpListener.State.Started)
            return;
          this.Close(true);
        }
        catch (Exception ex)
        {
          if (NetEventSource.Log.IsEnabled())
            NetEventSource.Error((object) this, FormattableStringFactory.Create("Abort {0}", (object) ex), nameof (Abort));
          throw;
        }
        finally
        {
          this._state = HttpListener.State.Closed;
        }
      }
    }

    private void Dispose()
    {
      lock (this._internalLock)
      {
        try
        {
          if (this._state == HttpListener.State.Closed)
            return;
          this.Close(true);
        }
        catch (Exception ex)
        {
          if (NetEventSource.Log.IsEnabled())
            NetEventSource.Error((object) this, FormattableStringFactory.Create("Dispose {0}", (object) ex), nameof (Dispose));
          throw;
        }
        finally
        {
          this._state = HttpListener.State.Closed;
        }
      }
    }

    private void Close(bool force)
    {
      this.CheckDisposed();
      HttpEndPointManager.RemoveListener(this);
      this.Cleanup(force);
    }

    internal void UnregisterContext(HttpListenerContext context)
    {
      lock (((ICollection) this._listenerContexts).SyncRoot)
        this._listenerContexts.Remove(context);
      lock (((ICollection) this._contextQueue).SyncRoot)
      {
        int index = this._contextQueue.IndexOf(context);
        if (index < 0)
          return;
        this._contextQueue.RemoveAt(index);
      }
    }

    internal void AddConnection(HttpConnection cnc)
    {
      lock (((ICollection) this._connections).SyncRoot)
        this._connections[cnc] = cnc;
    }

    internal void RemoveConnection(HttpConnection cnc)
    {
      lock (((ICollection) this._connections).SyncRoot)
        this._connections.Remove(cnc);
    }

    internal void RegisterContext(HttpListenerContext context)
    {
      lock (((ICollection) this._listenerContexts).SyncRoot)
        this._listenerContexts[context] = context;
      ListenerAsyncResult listenerAsyncResult = (ListenerAsyncResult) null;
      lock (((ICollection) this._asyncWaitQueue).SyncRoot)
      {
        if (this._asyncWaitQueue.Count == 0)
        {
          lock (((ICollection) this._contextQueue).SyncRoot)
            this._contextQueue.Add(context);
        }
        else
        {
          listenerAsyncResult = this._asyncWaitQueue[0];
          this._asyncWaitQueue.RemoveAt(0);
        }
      }
      listenerAsyncResult?.Complete(context);
    }

    private void Cleanup(bool close_existing)
    {
      lock (((ICollection) this._listenerContexts).SyncRoot)
      {
        if (close_existing)
        {
          Dictionary<HttpListenerContext, HttpListenerContext>.KeyCollection keys = this._listenerContexts.Keys;
          HttpListenerContext[] array = new HttpListenerContext[keys.Count];
          keys.CopyTo(array, 0);
          this._listenerContexts.Clear();
          for (int index = array.Length - 1; index >= 0; --index)
            array[index].Connection.Close(true);
        }
        lock (((ICollection) this._connections).SyncRoot)
        {
          Dictionary<HttpConnection, HttpConnection>.KeyCollection keys = this._connections.Keys;
          HttpConnection[] array = new HttpConnection[keys.Count];
          keys.CopyTo(array, 0);
          this._connections.Clear();
          for (int index = array.Length - 1; index >= 0; --index)
            array[index].Close(true);
        }
        lock (((ICollection) this._contextQueue).SyncRoot)
        {
          HttpListenerContext[] array = this._contextQueue.ToArray();
          this._contextQueue.Clear();
          for (int index = array.Length - 1; index >= 0; --index)
            array[index].Connection.Close(true);
        }
        lock (((ICollection) this._asyncWaitQueue).SyncRoot)
        {
          Exception exc = (Exception) new ObjectDisposedException("listener");
          foreach (ListenerAsyncResult asyncWait in this._asyncWaitQueue)
            asyncWait.Complete(exc);
          this._asyncWaitQueue.Clear();
        }
      }
    }

    private HttpListenerContext GetContextFromQueue()
    {
      lock (((ICollection) this._contextQueue).SyncRoot)
      {
        if (this._contextQueue.Count == 0)
          return (HttpListenerContext) null;
        HttpListenerContext context = this._contextQueue[0];
        this._contextQueue.RemoveAt(0);
        return context;
      }
    }

    #nullable enable
    /// <summary>Begins asynchronously retrieving an incoming request.</summary>
    /// <param name="callback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when a client request is available.</param>
    /// <param name="state">A user-defined object that contains information about the operation. This object is passed to the <paramref name="callback" /> delegate when the operation completes.</param>
    /// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
    /// <exception cref="T:System.InvalidOperationException">This object has not been started or is currently stopped.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    /// <returns>An <see cref="T:System.IAsyncResult" /> object that indicates the status of the asynchronous operation.</returns>
    public IAsyncResult BeginGetContext(AsyncCallback? callback, object? state)
    {
      this.CheckDisposed();
      if (this._state != HttpListener.State.Started)
        throw new InvalidOperationException(SR.Format(SR.net_listener_mustcall, (object) "Start()"));
      ListenerAsyncResult context = new ListenerAsyncResult(this, callback, state);
      lock (((ICollection) this._asyncWaitQueue).SyncRoot)
      {
        lock (((ICollection) this._contextQueue).SyncRoot)
        {
          HttpListenerContext contextFromQueue = this.GetContextFromQueue();
          if (contextFromQueue != null)
          {
            context.Complete(contextFromQueue, true);
            return (IAsyncResult) context;
          }
        }
        this._asyncWaitQueue.Add(context);
      }
      return (IAsyncResult) context;
    }

    /// <summary>Completes an asynchronous operation to retrieve an incoming client request.</summary>
    /// <param name="asyncResult">An <see cref="T:System.IAsyncResult" /> object that was obtained when the asynchronous operation was started.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> was not obtained by calling the <see cref="M:System.Net.HttpListener.BeginGetContext(System.AsyncCallback,System.Object)" /> method.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
    /// <exception cref="T:System.InvalidOperationException">The <see cref="M:System.Net.HttpListener.EndGetContext(System.IAsyncResult)" /> method was already called for the specified <paramref name="asyncResult" /> object.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    /// <returns>An <see cref="T:System.Net.HttpListenerContext" /> object that represents the client request.</returns>
    public HttpListenerContext EndGetContext(IAsyncResult asyncResult)
    {
      this.CheckDisposed();
      ArgumentNullException.ThrowIfNull((object) asyncResult, nameof (asyncResult));
      if (!(asyncResult is ListenerAsyncResult listenerAsyncResult) || this != listenerAsyncResult._parent)
        throw new ArgumentException(SR.net_io_invalidasyncresult, nameof (asyncResult));
      listenerAsyncResult._endCalled = !listenerAsyncResult._endCalled ? true : throw new InvalidOperationException(SR.Format(SR.net_io_invalidendcall, (object) nameof (EndGetContext)));
      if (!listenerAsyncResult.IsCompleted)
        listenerAsyncResult.AsyncWaitHandle.WaitOne();
      lock (((ICollection) this._asyncWaitQueue).SyncRoot)
      {
        int index = this._asyncWaitQueue.IndexOf(listenerAsyncResult);
        if (index >= 0)
          this._asyncWaitQueue.RemoveAt(index);
      }
      HttpListenerContext context = listenerAsyncResult.GetContext();
      context.ParseAuthentication(context.AuthenticationSchemes);
      return context;
    }

    #nullable disable
    internal AuthenticationSchemes SelectAuthenticationScheme(HttpListenerContext context)
    {
      return this.AuthenticationSchemeSelectorDelegate == null ? this._authenticationScheme : this.AuthenticationSchemeSelectorDelegate(context.Request);
    }

    #nullable enable
    /// <summary>Waits for an incoming request and returns when one is received.</summary>
    /// <exception cref="T:System.Net.HttpListenerException">A Win32 function call failed. Check the exception's <see cref="P:System.Net.HttpListenerException.ErrorCode" /> property to determine the cause of the exception.</exception>
    /// <exception cref="T:System.InvalidOperationException">This object has not been started or is currently stopped.
    /// 
    /// -or-
    /// 
    /// The <see cref="T:System.Net.HttpListener" /> does not have any Uniform Resource Identifier (URI) prefixes to respond to.</exception>
    /// <exception cref="T:System.ObjectDisposedException">This object is closed.</exception>
    /// <returns>An <see cref="T:System.Net.HttpListenerContext" /> object that represents a client request.</returns>
    public HttpListenerContext GetContext()
    {
      this.CheckDisposed();
      if (this._state == HttpListener.State.Stopped)
        throw new InvalidOperationException(SR.Format(SR.net_listener_mustcall, (object) "Start()"));
      if (this._prefixes.Count == 0)
        throw new InvalidOperationException(SR.Format(SR.net_listener_mustcall, (object) "AddPrefix()"));
      ListenerAsyncResult context = (ListenerAsyncResult) this.BeginGetContext((AsyncCallback) null, (object) null);
      context._inGet = true;
      return this.EndGetContext((IAsyncResult) context);
    }

    #nullable disable
    internal static SslStream CreateSslStream(
      Stream innerStream,
      bool ownsStream,
      RemoteCertificateValidationCallback callback)
    {
      return new SslStream(innerStream, ownsStream, callback);
    }

    internal static X509Certificate LoadCertificateAndKey(IPAddress addr, int port)
    {
      return (X509Certificate) null;
    }

    #nullable enable
    /// <summary>A delegate called to determine the <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> to use for each <see cref="T:System.Net.HttpListener" /> request.</summary>
    /// <param name="request">The <see cref="T:System.Net.HttpListenerRequest" /> to determine the extended protection policy that the <see cref="T:System.Net.HttpListener" /> instance will use to provide extended protection.</param>
    /// <returns>An <see cref="T:System.Security.Authentication.ExtendedProtection.ExtendedProtectionPolicy" /> object that specifies the extended protection policy to use for this request.</returns>
    public delegate ExtendedProtectionPolicy ExtendedProtectionSelector(HttpListenerRequest request);

    #nullable disable
    private enum State
    {
      Stopped,
      Started,
      Closed,
    }
  }
}
