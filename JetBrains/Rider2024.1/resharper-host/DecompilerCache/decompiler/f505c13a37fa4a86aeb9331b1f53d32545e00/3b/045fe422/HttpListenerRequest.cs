// Decompiled with JetBrains decompiler
// Type: System.Net.HttpListenerRequest
// Assembly: System.Net.HttpListener, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: F505C13A-37FA-4A86-AEB9-331B1F53D325
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Net.HttpListener.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Net.HttpListener.xml

using System.Buffers;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace System.Net
{
  /// <summary>Describes an incoming HTTP request to an <see cref="T:System.Net.HttpListener" /> object. This class cannot be inherited.</summary>
  public sealed class HttpListenerRequest
  {
    #nullable disable
    private CookieCollection _cookies;
    private bool? _keepAlive;
    private string _rawUrl;
    private Uri _requestUri;
    private Version _version;
    private long _contentLength;
    private bool _clSet;
    private readonly WebHeaderCollection _headers;
    private string _method;
    private Stream _inputStream;
    private readonly HttpListenerContext _context;
    private bool _isChunked;
    private static readonly SearchValues<char> s_validMethodChars = SearchValues.Create((ReadOnlySpan<char>) "!#$%&'*+-.0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ^_`abcdefghijklmnopqrstuvwxyz|~");
    private static readonly unsafe byte[] s_100continue = new ReadOnlySpan<byte>((void*) &\u003CPrivateImplementationDetails\u003E.BF21968F7B2082AC6035C5EB389928E45AD039B9005681B933DC7467C435808D, 25).ToArray();

    #nullable enable
    /// <summary>Gets the MIME types accepted by the client.</summary>
    /// <returns>A <see cref="T:System.String" /> array that contains the type names specified in the request's <see langword="Accept" /> header or <see langword="null" /> if the client request did not include an <see langword="Accept" /> header.</returns>
    public string[]? AcceptTypes
    {
      get => HttpListenerRequest.Helpers.ParseMultivalueHeader(this.Headers["Accept"]);
    }

    /// <summary>Gets the natural languages that are preferred for the response.</summary>
    /// <returns>A <see cref="T:System.String" /> array that contains the languages specified in the request's <see cref="F:System.Net.HttpRequestHeader.AcceptLanguage" /> header or <see langword="null" /> if the client request did not include an <see cref="F:System.Net.HttpRequestHeader.AcceptLanguage" /> header.</returns>
    public string[]? UserLanguages
    {
      get => HttpListenerRequest.Helpers.ParseMultivalueHeader(this.Headers["Accept-Language"]);
    }

    #nullable disable
    private CookieCollection ParseCookies(Uri uri, string setCookieHeader)
    {
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Info((object) this, (object) ("uri:" + uri?.ToString() + " setCookieHeader:" + setCookieHeader), nameof (ParseCookies));
      CookieCollection cookieCollection = new CookieCollection();
      CookieParser cookieParser = new CookieParser(setCookieHeader);
      while (true)
      {
        Cookie server;
        do
        {
          server = cookieParser.GetServer();
          if (server != null)
          {
            if (NetEventSource.Log.IsEnabled())
              NetEventSource.Info((object) this, (object) ("CookieParser returned cookie: " + server.ToString()), nameof (ParseCookies));
          }
          else
            goto label_8;
        }
        while (server.Name.Length == 0);
        cookieCollection.InternalAdd(server, true);
      }
label_8:
      return cookieCollection;
    }

    #nullable enable
    /// <summary>Gets the cookies sent with the request.</summary>
    /// <returns>A <see cref="T:System.Net.CookieCollection" /> that contains cookies that accompany the request. This property returns an empty collection if the request does not contain cookies.</returns>
    public CookieCollection Cookies
    {
      get
      {
        if (this._cookies == null)
        {
          string header = this.Headers["Cookie"];
          if (!string.IsNullOrEmpty(header))
            this._cookies = this.ParseCookies(this.RequestUri, header);
          if (this._cookies == null)
            this._cookies = new CookieCollection();
        }
        return this._cookies;
      }
    }

    /// <summary>Gets the content encoding that can be used with data sent with the request.</summary>
    /// <returns>An <see cref="T:System.Text.Encoding" /> object suitable for use with the data in the <see cref="P:System.Net.HttpListenerRequest.InputStream" /> property.</returns>
    public Encoding ContentEncoding
    {
      get
      {
        if (this.UserAgent != null && CultureInfo.InvariantCulture.CompareInfo.IsPrefix(this.UserAgent, "UP"))
        {
          string header = this.Headers["x-up-devcap-post-charset"];
          if (header != null)
          {
            if (header.Length > 0)
            {
              try
              {
                return Encoding.GetEncoding(header);
              }
              catch (ArgumentException ex)
              {
              }
            }
          }
        }
        if (this.HasEntityBody && this.ContentType != null)
        {
          string setValueFromHeader = HttpListenerRequest.Helpers.GetCharSetValueFromHeader(this.ContentType);
          if (setValueFromHeader != null)
          {
            try
            {
              return Encoding.GetEncoding(setValueFromHeader);
            }
            catch (ArgumentException ex)
            {
            }
          }
        }
        return Encoding.Default;
      }
    }

    /// <summary>Gets the MIME type of the body data included in the request.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the text of the request's <see langword="Content-Type" /> header.</returns>
    public string? ContentType => this.Headers["Content-Type"];

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the request is sent from the local computer.</summary>
    /// <returns>
    /// <see langword="true" /> if the request originated on the same computer as the <see cref="T:System.Net.HttpListener" /> object that provided the request; otherwise, <see langword="false" />.</returns>
    public bool IsLocal => this.LocalEndPoint.Address.Equals((object) this.RemoteEndPoint.Address);

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the TCP connection was  a WebSocket request.</summary>
    /// <returns>Returns <see cref="T:System.Boolean" />.
    /// 
    /// <see langword="true" /> if the TCP connection is a WebSocket request; otherwise, <see langword="false" />.</returns>
    public bool IsWebSocketRequest
    {
      get
      {
        if (true)
          ;
        bool flag = false;
        if (string.IsNullOrEmpty(this.Headers["Connection"]) || string.IsNullOrEmpty(this.Headers["Upgrade"]))
          return false;
        foreach (string a in this.Headers.GetValues("Connection"))
        {
          if (string.Equals(a, "Upgrade", StringComparison.OrdinalIgnoreCase))
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          return false;
        foreach (string a in this.Headers.GetValues("Upgrade"))
        {
          if (string.Equals(a, "websocket", StringComparison.OrdinalIgnoreCase))
            return true;
        }
        return false;
      }
    }

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the client requests a persistent connection.</summary>
    /// <returns>
    /// <see langword="true" /> if the connection should be kept open; otherwise, <see langword="false" />.</returns>
    public bool KeepAlive
    {
      get
      {
        if (!this._keepAlive.HasValue)
        {
          string header = this.Headers["Proxy-Connection"];
          if (string.IsNullOrEmpty(header))
            header = this.Headers["Connection"];
          if (string.IsNullOrEmpty(header))
          {
            this._keepAlive = !(this.ProtocolVersion >= HttpVersion.Version11) ? new bool?(!string.IsNullOrEmpty(this.Headers["Keep-Alive"])) : new bool?(true);
          }
          else
          {
            string lowerInvariant = header.ToLowerInvariant();
            this._keepAlive = new bool?(lowerInvariant.IndexOf("close", StringComparison.OrdinalIgnoreCase) < 0 || lowerInvariant.Contains("keep-alive", StringComparison.OrdinalIgnoreCase));
          }
        }
        if (NetEventSource.Log.IsEnabled())
          NetEventSource.Info((object) this, (object) ("_keepAlive=" + this._keepAlive.ToString()), nameof (KeepAlive));
        return this._keepAlive.Value;
      }
    }

    /// <summary>Gets the query string included in the request.</summary>
    /// <returns>A <see cref="T:System.Collections.Specialized.NameValueCollection" /> object that contains the query data included in the request <see cref="P:System.Net.HttpListenerRequest.Url" />.</returns>
    public NameValueCollection QueryString
    {
      get
      {
        NameValueCollection nvc = new NameValueCollection();
        HttpListenerRequest.Helpers.FillFromString(nvc, this.Url.Query, true, this.ContentEncoding);
        return nvc;
      }
    }

    /// <summary>Gets the URL information (without the host and port) requested by the client.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the raw URL for this request.</returns>
    public string? RawUrl => this._rawUrl;

    private string RequestScheme => !this.IsSecureConnection ? "http" : "https";

    /// <summary>Gets the user agent presented by the client.</summary>
    /// <returns>A <see cref="T:System.String" /> object that contains the text of the request's <see langword="User-Agent" /> header.</returns>
    public string UserAgent => this.Headers["User-Agent"];

    /// <summary>Gets the server IP address and port number to which the request is directed.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the host address information.</returns>
    public string UserHostAddress => this.LocalEndPoint.ToString();

    /// <summary>Gets the DNS name and, if provided, the port number specified by the client.</summary>
    /// <returns>A <see cref="T:System.String" /> value that contains the text of the request's <see langword="Host" /> header.</returns>
    public string UserHostName => this.Headers["Host"];

    /// <summary>Gets the Uniform Resource Identifier (URI) of the resource that referred the client to the server.</summary>
    /// <returns>A <see cref="T:System.Uri" /> object that contains the text of the request's <see cref="F:System.Net.HttpRequestHeader.Referer" /> header, or <see langword="null" /> if the header was not included in the request.</returns>
    public Uri? UrlReferrer
    {
      get
      {
        string header = this.Headers["Referer"];
        if (header == null)
          return (Uri) null;
        Uri result;
        return !Uri.TryCreate(header, UriKind.RelativeOrAbsolute, out result) ? (Uri) null : result;
      }
    }

    /// <summary>Gets the <see cref="T:System.Uri" /> object requested by the client.</summary>
    /// <returns>A <see cref="T:System.Uri" /> object that identifies the resource requested by the client.</returns>
    public Uri? Url => this.RequestUri;

    /// <summary>Gets the HTTP version used by the requesting client.</summary>
    /// <returns>A <see cref="T:System.Version" /> that identifies the client's version of HTTP.</returns>
    public Version ProtocolVersion => this._version;

    /// <summary>Retrieves the client's X.509 v.3 certificate.</summary>
    /// <exception cref="T:System.InvalidOperationException">A call to this method to retrieve the client's X.509 v.3 certificate is in progress and therefore another call to this method cannot be made.</exception>
    /// <returns>A <see cref="N:System.Security.Cryptography.X509Certificates" /> object that contains the client's X.509 v.3 certificate.</returns>
    public X509Certificate2? GetClientCertificate()
    {
      this.ClientCertState = this.ClientCertState != ListenerClientCertState.InProgress ? ListenerClientCertState.InProgress : throw new InvalidOperationException(SR.Format(SR.net_listener_callinprogress, (object) "GetClientCertificate()/BeginGetClientCertificate()"));
      this.GetClientCertificateCore();
      this.ClientCertState = ListenerClientCertState.Completed;
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Info((object) this, FormattableStringFactory.Create("_clientCertificate:{0}", (object) this.ClientCertificate), nameof (GetClientCertificate));
      return this.ClientCertificate;
    }

    /// <summary>Begins an asynchronous request for the client's X.509 v.3 certificate.</summary>
    /// <param name="requestCallback">An <see cref="T:System.AsyncCallback" /> delegate that references the method to invoke when the operation is complete.</param>
    /// <param name="state">A user-defined object that contains information about the operation. This object is passed to the callback delegate when the operation completes.</param>
    /// <returns>An <see cref="T:System.IAsyncResult" /> that indicates the status of the operation.</returns>
    public IAsyncResult BeginGetClientCertificate(AsyncCallback? requestCallback, object? state)
    {
      if (NetEventSource.Log.IsEnabled())
        NetEventSource.Info((object) this, (FormattableString) null, nameof (BeginGetClientCertificate));
      this.ClientCertState = this.ClientCertState != ListenerClientCertState.InProgress ? ListenerClientCertState.InProgress : throw new InvalidOperationException(SR.Format(SR.net_listener_callinprogress, (object) "GetClientCertificate()/BeginGetClientCertificate()"));
      return (IAsyncResult) this.BeginGetClientCertificateCore(requestCallback, state);
    }

    /// <summary>Retrieves the client's X.509 v.3 certificate as an asynchronous operation.</summary>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns a <see cref="N:System.Security.Cryptography.X509Certificates" /> object that contains the client's X.509 v.3 certificate.</returns>
    public Task<X509Certificate2?> GetClientCertificateAsync()
    {
      return Task.Factory.FromAsync<X509Certificate2>((Func<AsyncCallback, object, IAsyncResult>) ((callback, state) => ((HttpListenerRequest) state).BeginGetClientCertificate(callback, state)), (Func<IAsyncResult, X509Certificate2>) (iar => ((HttpListenerRequest) iar.AsyncState).EndGetClientCertificate(iar)), (object) this);
    }

    internal ListenerClientCertState ClientCertState { get; set; }

    internal X509Certificate2? ClientCertificate { get; set; }

    /// <summary>Gets an error code that identifies a problem with the <see cref="T:System.Security.Cryptography.X509Certificates.X509Certificate" /> provided by the client.</summary>
    /// <exception cref="T:System.InvalidOperationException">The client certificate has not been initialized yet by a call to the <see cref="M:System.Net.HttpListenerRequest.BeginGetClientCertificate(System.AsyncCallback,System.Object)" /> or <see cref="M:System.Net.HttpListenerRequest.GetClientCertificate" /> methods
    /// 
    /// -or -
    /// 
    /// The operation is still in progress.</exception>
    /// <returns>An <see cref="T:System.Int32" /> value that contains a Windows error code.</returns>
    public int ClientCertificateError
    {
      get
      {
        if (this.ClientCertState == ListenerClientCertState.NotInitialized)
          throw new InvalidOperationException(SR.Format(SR.net_listener_mustcall, (object) "GetClientCertificate()/BeginGetClientCertificate()"));
        if (this.ClientCertState == ListenerClientCertState.InProgress)
          throw new InvalidOperationException(SR.Format(SR.net_listener_mustcompletecall, (object) "GetClientCertificate()/BeginGetClientCertificate()"));
        return this.GetClientCertificateErrorCore();
      }
    }

    #nullable disable
    internal HttpListenerRequest(HttpListenerContext context)
    {
      this._context = context;
      this._headers = new WebHeaderCollection();
      this._version = HttpVersion.Version10;
    }

    internal void SetRequestLine(string req)
    {
      // ISSUE: unable to decompile the method.
    }

    private static bool MaybeUri(string s)
    {
      int length = s.IndexOf(':');
      bool flag1 = (uint) length < 10U;
      if (flag1)
      {
        ReadOnlySpan<char> span = s.AsSpan(0, length);
        bool flag2;
        switch (span.Length)
        {
          case 3:
            if (span.SequenceEqual<char>("ftp".AsSpan()))
              break;
            goto default;
          case 4:
            switch (span[1])
            {
              case 'e':
                if (span.SequenceEqual<char>("news".AsSpan()))
                  break;
                goto label_15;
              case 'i':
                if (span.SequenceEqual<char>("file".AsSpan()))
                  break;
                goto label_15;
              case 'n':
                if (span.SequenceEqual<char>("nntp".AsSpan()))
                  break;
                goto label_15;
              case 't':
                if (span.SequenceEqual<char>("http".AsSpan()))
                  break;
                goto label_15;
              default:
                goto label_15;
            }
            break;
          case 5:
            if (span.SequenceEqual<char>("https".AsSpan()))
              break;
            goto default;
          case 6:
            switch (span[0])
            {
              case 'g':
                if (span.SequenceEqual<char>("gopher".AsSpan()))
                  break;
                goto label_15;
              case 'm':
                if (!span.SequenceEqual<char>("mailto".AsSpan()))
                  goto label_15;
                else
                  break;
              default:
                goto label_15;
            }
            break;
          case 7:
            if (span.SequenceEqual<char>("net.tcp".AsSpan()))
              break;
            goto default;
          case 8:
            if (span.SequenceEqual<char>("net.pipe".AsSpan()))
              break;
            goto default;
          default:
label_15:
            flag2 = false;
            goto label_16;
        }
        flag2 = true;
label_16:
        flag1 = flag2;
      }
      return flag1;
    }

    internal void FinishInitialization()
    {
      ReadOnlySpan<char> span = (ReadOnlySpan<char>) this.UserHostName;
      if (this._version > HttpVersion.Version10 && span.IsEmpty)
      {
        this._context.ErrorMessage = "Invalid host name";
      }
      else
      {
        Uri result = (Uri) null;
        string str = !HttpListenerRequest.MaybeUri(this._rawUrl.ToLowerInvariant()) || !Uri.TryCreate(this._rawUrl, UriKind.Absolute, out result) ? this._rawUrl : result.PathAndQuery;
        if (span.IsEmpty)
          span = (ReadOnlySpan<char>) this.UserHostAddress;
        if (result != (Uri) null)
          span = (ReadOnlySpan<char>) result.Host;
        int length = span.IndexOf<char>(':');
        if (length >= 0)
          span = span.Slice(0, length);
        DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(4, 3);
        interpolatedStringHandler.AppendFormatted(this.RequestScheme);
        interpolatedStringHandler.AppendLiteral("://");
        interpolatedStringHandler.AppendFormatted(span);
        interpolatedStringHandler.AppendLiteral(":");
        interpolatedStringHandler.AppendFormatted<int>(this.LocalEndPoint.Port);
        string stringAndClear = interpolatedStringHandler.ToStringAndClear();
        if (!Uri.TryCreate(stringAndClear + str, UriKind.Absolute, out this._requestUri))
        {
          this._context.ErrorMessage = WebUtility.HtmlEncode("Invalid url: " + stringAndClear + str);
        }
        else
        {
          this._requestUri = HttpListenerRequestUriBuilder.GetRequestUri(this._rawUrl, this._requestUri.Scheme, this._requestUri.Authority, this._requestUri.LocalPath, this._requestUri.Query);
          if (this._version >= HttpVersion.Version11)
          {
            string header = this.Headers["Transfer-Encoding"];
            this._isChunked = header != null && string.Equals(header, "chunked", StringComparison.OrdinalIgnoreCase);
            if (header != null && !this._isChunked)
            {
              this._context.Connection.SendError((string) null, 501);
              return;
            }
          }
          if (!this._isChunked && !this._clSet && (string.Equals(this._method, "POST", StringComparison.OrdinalIgnoreCase) || string.Equals(this._method, "PUT", StringComparison.OrdinalIgnoreCase)))
          {
            this._context.Connection.SendError((string) null, 411);
          }
          else
          {
            if (!string.Equals(this.Headers["Expect"], "100-continue", StringComparison.OrdinalIgnoreCase))
              return;
            this._context.Connection.GetResponseStream().InternalWrite(HttpListenerRequest.s_100continue, 0, HttpListenerRequest.s_100continue.Length);
          }
        }
      }
    }

    internal void AddHeader(string header)
    {
      int length = header.IndexOf(':');
      switch (length)
      {
        case -1:
        case 0:
          this._context.ErrorMessage = HttpStatusDescription.Get(400);
          this._context.ErrorStatus = 400;
          break;
        default:
          ReadOnlySpan<char> readOnlySpan = header.AsSpan(0, length).Trim();
          string name = readOnlySpan.ToString();
          readOnlySpan = header.AsSpan(length + 1).Trim();
          string s = readOnlySpan.ToString();
          if (name.Equals("content-length", StringComparison.OrdinalIgnoreCase))
          {
            ulong result;
            long num = ulong.TryParse(s, out result) ? (result <= (ulong) long.MaxValue ? (long) result : 0L) : long.Parse(s);
            if (num < 0L || this._clSet && num != this._contentLength)
            {
              this._context.ErrorMessage = "Invalid Content-Length.";
            }
            else
            {
              this._contentLength = num;
              this._clSet = true;
            }
          }
          else if (name.Equals("transfer-encoding", StringComparison.OrdinalIgnoreCase) && this.Headers["Transfer-Encoding"] != null)
          {
            this._context.ErrorStatus = 501;
            this._context.ErrorMessage = HttpStatusDescription.Get(HttpStatusCode.NotImplemented);
          }
          if (this._context.ErrorMessage != null)
            break;
          this._headers.Set(name, s);
          break;
      }
    }

    internal bool FlushInput()
    {
      if (!this.HasEntityBody)
        return true;
      int length = 2048;
      if (this._contentLength > 0L)
        length = (int) Math.Min(this._contentLength, (long) length);
      byte[] buffer = new byte[length];
label_5:
      try
      {
        IAsyncResult asyncResult = this.InputStream.BeginRead(buffer, 0, length, (AsyncCallback) null, (object) null);
        if (!asyncResult.IsCompleted && !asyncResult.AsyncWaitHandle.WaitOne(1000))
          return false;
        if (this.InputStream.EndRead(asyncResult) <= 0)
          return true;
        goto label_5;
      }
      catch (ObjectDisposedException ex)
      {
        this._inputStream = (Stream) null;
        return true;
      }
      catch
      {
        return false;
      }
    }

    private X509Certificate2 GetClientCertificateCore()
    {
      return this.ClientCertificate = this._context.Connection.ClientCertificate;
    }

    private int GetClientCertificateErrorCore()
    {
      HttpConnection connection = this._context.Connection;
      if (connection.ClientCertificate == null)
        return 0;
      int[] certificateErrors = connection.ClientCertificateErrors;
      return certificateErrors != null && certificateErrors.Length != 0 ? certificateErrors[0] : 0;
    }

    /// <summary>Gets the length of the body data included in the request.</summary>
    /// <returns>The value from the request's <see langword="Content-Length" /> header. This value is -1 if the content length is not known.</returns>
    public long ContentLength64
    {
      get
      {
        if (this._isChunked)
          this._contentLength = -1L;
        return this._contentLength;
      }
    }

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the request has associated body data.</summary>
    /// <returns>
    /// <see langword="true" /> if the request has associated body data; otherwise, <see langword="false" />.</returns>
    public bool HasEntityBody => this._contentLength > 0L || this._isChunked;

    #nullable enable
    /// <summary>Gets the collection of header name/value pairs sent in the request.</summary>
    /// <returns>A <see cref="T:System.Net.WebHeaderCollection" /> that contains the HTTP headers included in the request.</returns>
    public NameValueCollection Headers => (NameValueCollection) this._headers;

    /// <summary>Gets the HTTP method specified by the client.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the method used in the request.</returns>
    public string? HttpMethod => this._method;

    /// <summary>Gets a stream that contains the body data sent by the client.</summary>
    /// <returns>A readable <see cref="T:System.IO.Stream" /> object that contains the bytes sent by the client in the body of the request. This property returns <see cref="F:System.IO.Stream.Null" /> if no data is sent with the request.</returns>
    public Stream InputStream
    {
      get
      {
        if (this._inputStream == null)
          this._inputStream = this._isChunked || this._contentLength > 0L ? (Stream) this._context.Connection.GetRequestStream(this._isChunked, this._contentLength) : Stream.Null;
        return this._inputStream;
      }
    }

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the client sending this request is authenticated.</summary>
    /// <returns>
    /// <see langword="true" /> if the client was authenticated; otherwise, <see langword="false" />.</returns>
    public bool IsAuthenticated => false;

    /// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether the TCP connection used to send the request is using the Secure Sockets Layer (SSL) protocol.</summary>
    /// <returns>
    /// <see langword="true" /> if the TCP connection is using SSL; otherwise, <see langword="false" />.</returns>
    public bool IsSecureConnection => this._context.Connection.IsSecure;

    /// <summary>Gets the server IP address and port number to which the request is directed.</summary>
    /// <returns>An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address that the request is sent to.</returns>
    public IPEndPoint? LocalEndPoint => this._context.Connection.LocalEndPoint;

    /// <summary>Gets the client IP address and port number from which the request originated.</summary>
    /// <returns>An <see cref="T:System.Net.IPEndPoint" /> that represents the IP address and port number from which the request originated.</returns>
    public IPEndPoint? RemoteEndPoint => this._context.Connection.RemoteEndPoint;

    /// <summary>Gets the request identifier of the incoming HTTP request.</summary>
    /// <returns>A <see cref="T:System.Guid" /> object that contains the identifier of the HTTP request.</returns>
    public Guid RequestTraceIdentifier { get; } = Guid.NewGuid();

    #nullable disable
    private HttpListenerRequest.GetClientCertificateAsyncResult BeginGetClientCertificateCore(
      AsyncCallback requestCallback,
      object state)
    {
      HttpListenerRequest.GetClientCertificateAsyncResult clientCertificateCore = new HttpListenerRequest.GetClientCertificateAsyncResult((object) this, state, requestCallback);
      this.ClientCertState = ListenerClientCertState.Completed;
      clientCertificateCore.InvokeCallback((object) this.GetClientCertificateCore());
      return clientCertificateCore;
    }

    #nullable enable
    /// <summary>Ends an asynchronous request for the client's X.509 v.3 certificate.</summary>
    /// <param name="asyncResult">The pending request for the certificate.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="asyncResult" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="asyncResult" /> was not obtained by calling <see cref="M:System.Net.HttpListenerRequest.BeginGetClientCertificate(System.AsyncCallback,System.Object)" /><paramref name="e" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">This method was already called for the operation identified by <paramref name="asyncResult" />.</exception>
    /// <returns>The <see cref="T:System.IAsyncResult" /> object that is returned when the operation started.</returns>
    public X509Certificate2? EndGetClientCertificate(IAsyncResult asyncResult)
    {
      ArgumentNullException.ThrowIfNull((object) asyncResult, nameof (asyncResult));
      if (!(asyncResult is HttpListenerRequest.GetClientCertificateAsyncResult certificateAsyncResult) || certificateAsyncResult.AsyncObject != this)
        throw new ArgumentException(SR.net_io_invalidasyncresult, nameof (asyncResult));
      certificateAsyncResult.EndCalled = !certificateAsyncResult.EndCalled ? true : throw new InvalidOperationException(SR.Format(SR.net_io_invalidendcall, (object) nameof (EndGetClientCertificate)));
      return (X509Certificate2) certificateAsyncResult.Result;
    }

    /// <summary>Gets the Service Provider Name (SPN) that the client sent on the request.</summary>
    /// <returns>A <see cref="T:System.String" /> that contains the SPN the client sent on the request.</returns>
    public string? ServiceName => (string) null;

    /// <summary>Gets the <see cref="T:System.Net.TransportContext" /> for the client request.</summary>
    /// <returns>A <see cref="T:System.Net.TransportContext" /> object for the client request.</returns>
    public TransportContext TransportContext
    {
      get => (TransportContext) new HttpListenerRequest.Context();
    }

    private Uri? RequestUri => this._requestUri;

    #nullable disable
    private static class Helpers
    {
      internal static string GetCharSetValueFromHeader(string headerValue)
      {
        if (headerValue == null)
          return (string) null;
        int length1 = headerValue.Length;
        int length2 = "charset".Length;
        int startIndex;
        for (startIndex = 1; startIndex < length1; startIndex += length2)
        {
          startIndex = CultureInfo.InvariantCulture.CompareInfo.IndexOf(headerValue, "charset", startIndex, CompareOptions.IgnoreCase);
          if (startIndex >= 0 && startIndex + length2 < length1)
          {
            char c1 = headerValue[startIndex - 1];
            char c2 = headerValue[startIndex + length2];
            if ((c1 == ';' || c1 == ',' || char.IsWhiteSpace(c1)) && (c2 == '=' || char.IsWhiteSpace(c2)))
              break;
          }
          else
            break;
        }
        if (startIndex < 0 || startIndex >= length1)
          return (string) null;
        int index1 = startIndex + length2;
        while (index1 < length1 && char.IsWhiteSpace(headerValue[index1]))
          ++index1;
        if (index1 >= length1 || headerValue[index1] != '=')
          return (string) null;
        int num1 = index1 + 1;
        while (num1 < length1 && char.IsWhiteSpace(headerValue[num1]))
          ++num1;
        if (num1 >= length1)
          return (string) null;
        string setValueFromHeader;
        if (num1 < length1 && headerValue[num1] == '"')
        {
          if (num1 == length1 - 1)
            return (string) null;
          int num2 = headerValue.IndexOf('"', num1 + 1);
          if (num2 < 0 || num2 == num1 + 1)
            return (string) null;
          setValueFromHeader = headerValue.AsSpan(num1 + 1, num2 - num1 - 1).Trim().ToString();
        }
        else
        {
          int index2 = num1;
          while (index2 < length1 && headerValue[index2] != ';')
            ++index2;
          if (index2 == num1)
            return (string) null;
          setValueFromHeader = headerValue.AsSpan(num1, index2 - num1).Trim().ToString();
        }
        return setValueFromHeader;
      }

      internal static string[] ParseMultivalueHeader(string s)
      {
        if (s == null)
          return (string[]) null;
        int length = s.Length;
        List<string> stringList = new List<string>();
        int num1 = 0;
        while (num1 < length)
        {
          int num2 = s.IndexOf(',', num1);
          if (num2 < 0)
            num2 = length;
          stringList.Add(s.Substring(num1, num2 - num1));
          num1 = num2 + 1;
          if (num1 < length && s[num1] == ' ')
            ++num1;
        }
        int count = stringList.Count;
        string[] array;
        if (count == 0)
        {
          array = new string[1]{ string.Empty };
        }
        else
        {
          array = new string[count];
          stringList.CopyTo(0, array, 0, count);
        }
        return array;
      }

      private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
      {
        int length = s.Length;
        HttpListenerRequest.Helpers.UrlDecoder urlDecoder = new HttpListenerRequest.Helpers.UrlDecoder(length, e);
        for (int index = 0; index < length; ++index)
        {
          char ch1 = s[index];
          switch (ch1)
          {
            case '%':
              if (index < length - 2)
              {
                if (s[index + 1] == 'u' && index < length - 5)
                {
                  int num1 = HexConverter.FromChar((int) s[index + 2]);
                  int num2 = HexConverter.FromChar((int) s[index + 3]);
                  int num3 = HexConverter.FromChar((int) s[index + 4]);
                  int num4 = HexConverter.FromChar((int) s[index + 5]);
                  if ((num1 | num2 | num3 | num4) != (int) byte.MaxValue)
                  {
                    char ch2 = (char) (num1 << 12 | num2 << 8 | num3 << 4 | num4);
                    index += 5;
                    urlDecoder.AddChar(ch2);
                    break;
                  }
                  goto default;
                }
                else
                {
                  int num5 = HexConverter.FromChar((int) s[index + 1]);
                  int num6 = HexConverter.FromChar((int) s[index + 2]);
                  if ((num5 | num6) != (int) byte.MaxValue)
                  {
                    byte b = (byte) (num5 << 4 | num6);
                    index += 2;
                    urlDecoder.AddByte(b);
                    break;
                  }
                  goto default;
                }
              }
              else
                goto default;
            case '+':
              ch1 = ' ';
              goto default;
            default:
              if (((int) ch1 & 65408) == 0)
              {
                urlDecoder.AddByte((byte) ch1);
                break;
              }
              urlDecoder.AddChar(ch1);
              break;
          }
        }
        return urlDecoder.GetString();
      }

      internal static void FillFromString(
        NameValueCollection nvc,
        string s,
        bool urlencoded,
        Encoding encoding)
      {
        int index = s.StartsWith('?') ? 1 : 0;
        for (int length = s.Length; index < length; ++index)
        {
          int startIndex = index;
          int num = -1;
          for (; index < length; ++index)
          {
            switch (s[index])
            {
              case '&':
                goto label_7;
              case '=':
                if (num < 0)
                {
                  num = index;
                  break;
                }
                break;
            }
          }
label_7:
          string str = (string) null;
          string s1;
          if (num >= 0)
          {
            str = s.Substring(startIndex, num - startIndex);
            s1 = s.Substring(num + 1, index - num - 1);
          }
          else
            s1 = s.Substring(startIndex, index - startIndex);
          if (urlencoded)
            nvc.Add(str == null ? (string) null : HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(str, encoding), HttpListenerRequest.Helpers.UrlDecodeStringFromStringInternal(s1, encoding));
          else
            nvc.Add(str, s1);
          if (index == length - 1 && s[index] == '&')
            nvc.Add((string) null, "");
        }
      }

      private sealed class UrlDecoder
      {
        private readonly int _bufferSize;
        private int _numChars;
        private readonly char[] _charBuffer;
        private int _numBytes;
        private byte[] _byteBuffer;
        private readonly Encoding _encoding;

        private void FlushBytes()
        {
          if (this._numBytes <= 0)
            return;
          this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
          this._numBytes = 0;
        }

        internal UrlDecoder(int bufferSize, Encoding encoding)
        {
          this._bufferSize = bufferSize;
          this._encoding = encoding;
          this._charBuffer = new char[bufferSize];
        }

        internal void AddChar(char ch)
        {
          if (this._numBytes > 0)
            this.FlushBytes();
          this._charBuffer[this._numChars++] = ch;
        }

        internal void AddByte(byte b)
        {
          if (this._byteBuffer == null)
            this._byteBuffer = new byte[this._bufferSize];
          this._byteBuffer[this._numBytes++] = b;
        }

        internal string GetString()
        {
          if (this._numBytes > 0)
            this.FlushBytes();
          return this._numChars > 0 ? new string(this._charBuffer, 0, this._numChars) : string.Empty;
        }
      }
    }

    private sealed class Context : TransportContext
    {
      public override ChannelBinding GetChannelBinding(ChannelBindingKind kind)
      {
        if (kind != ChannelBindingKind.Endpoint)
          throw new NotSupportedException(SR.Format(SR.net_listener_invalid_cbt_type, (object) kind.ToString()));
        return (ChannelBinding) null;
      }
    }

    private sealed class GetClientCertificateAsyncResult : LazyAsyncResult
    {
      public GetClientCertificateAsyncResult(
        object myObject,
        object myState,
        AsyncCallback myCallBack)
        : base(myObject, myState, myCallBack)
      {
      }
    }
  }
}
