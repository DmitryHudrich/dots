// Decompiled with JetBrains decompiler
// Type: System.Net.HttpListenerContext
// Assembly: System.Net.HttpListener, Version=8.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51
// MVID: F505C13A-37FA-4A86-AEB9-331B1F53D325
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Net.HttpListener.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Net.HttpListener.xml

using System.Diagnostics.CodeAnalysis;
using System.Net.WebSockets;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace System.Net
{
  /// <summary>Provides access to the request and response objects used by the <see cref="T:System.Net.HttpListener" /> class. This class cannot be inherited.</summary>
  public sealed class HttpListenerContext
  {
    #nullable disable
    internal HttpListener _listener;
    private HttpListenerResponse _response;
    private IPrincipal _user;
    private readonly HttpConnection _connection;

    #nullable enable
    /// <summary>Gets the <see cref="T:System.Net.HttpListenerRequest" /> that represents a client's request for a resource.</summary>
    /// <returns>An <see cref="T:System.Net.HttpListenerRequest" /> object that represents the client request.</returns>
    public HttpListenerRequest Request { get; }

    /// <summary>Gets an object used to obtain identity, authentication information, and security roles for the client whose request is represented by this <see cref="T:System.Net.HttpListenerContext" /> object.</summary>
    /// <returns>An <see cref="T:System.Security.Principal.IPrincipal" /> object that describes the client, or <see langword="null" /> if the <see cref="T:System.Net.HttpListener" /> that supplied this <see cref="T:System.Net.HttpListenerContext" /> does not require authentication.</returns>
    public IPrincipal? User => this._user;

    internal AuthenticationSchemes AuthenticationSchemes { get; set; }

    /// <summary>Gets the <see cref="T:System.Net.HttpListenerResponse" /> object that will be sent to the client in response to the client's request.</summary>
    /// <returns>An <see cref="T:System.Net.HttpListenerResponse" /> object used to send a response back to the client.</returns>
    public HttpListenerResponse Response
    {
      get => this._response ?? (this._response = new HttpListenerResponse(this));
    }

    /// <summary>Accept a WebSocket connection as an asynchronous operation.</summary>
    /// <param name="subProtocol">The supported WebSocket sub-protocol.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="subProtocol" /> is an empty string
    /// 
    /// -or-
    /// 
    /// <paramref name="subProtocol" /> contains illegal characters.</exception>
    /// <exception cref="T:System.Net.WebSockets.WebSocketException">An error occurred when sending the response to complete the WebSocket handshake.</exception>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.WebSockets.HttpListenerWebSocketContext" /> object.</returns>
    public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(string? subProtocol)
    {
      return this.AcceptWebSocketAsync(subProtocol, 16384, WebSocket.DefaultKeepAliveInterval);
    }

    /// <summary>Accept a WebSocket connection specifying the supported WebSocket sub-protocol  and WebSocket keep-alive interval as an asynchronous operation.</summary>
    /// <param name="subProtocol">The supported WebSocket sub-protocol.</param>
    /// <param name="keepAliveInterval">The WebSocket protocol keep-alive interval in milliseconds.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="subProtocol" /> is an empty string
    /// 
    /// -or-
    /// 
    /// <paramref name="subProtocol" /> contains illegal characters.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="keepAliveInterval" /> is too small.</exception>
    /// <exception cref="T:System.Net.WebSockets.WebSocketException">An error occurred when sending the response to complete the WebSocket handshake.</exception>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.WebSockets.HttpListenerWebSocketContext" /> object.</returns>
    public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(
      string? subProtocol,
      TimeSpan keepAliveInterval)
    {
      return this.AcceptWebSocketAsync(subProtocol, 16384, keepAliveInterval);
    }

    #nullable disable
    internal HttpListenerContext(HttpConnection connection)
    {
      this._connection = connection;
      this._response = new HttpListenerResponse(this);
      this.Request = new HttpListenerRequest(this);
      this.ErrorStatus = 400;
    }

    internal int ErrorStatus { get; set; }

    #nullable enable
    internal string? ErrorMessage { get; set; }

    internal bool HaveError => this.ErrorMessage != null;

    internal HttpConnection Connection => this._connection;

    internal void ParseAuthentication(AuthenticationSchemes expectedSchemes)
    {
      if (expectedSchemes == AuthenticationSchemes.Anonymous)
        return;
      string header = this.Request.Headers["Authorization"];
      if (string.IsNullOrEmpty(header) || !HttpListenerContext.IsBasicHeader(header))
        return;
      this._user = HttpListenerContext.ParseBasicAuthentication(header.Substring("Basic".Length + 1));
    }

    #nullable disable
    internal static IPrincipal ParseBasicAuthentication(string authData)
    {
      string username;
      string password;
      return !HttpListenerContext.TryParseBasicAuth(authData, out HttpStatusCode _, out username, out password) ? (IPrincipal) null : (IPrincipal) new GenericPrincipal((IIdentity) new HttpListenerBasicIdentity(username, password), Array.Empty<string>());
    }

    internal static bool IsBasicHeader(string header)
    {
      return header.Length >= 6 && header[5] == ' ' && string.Compare(header, 0, "Basic", 0, 5, StringComparison.OrdinalIgnoreCase) == 0;
    }

    internal static bool TryParseBasicAuth(
      string headerValue,
      out HttpStatusCode errorCode,
      [NotNullWhen(true)] out string username,
      [NotNullWhen(true)] out string password)
    {
      errorCode = HttpStatusCode.OK;
      username = password = (string) null;
      try
      {
        if (string.IsNullOrWhiteSpace(headerValue))
          return false;
        string str = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue));
        int length = str.IndexOf(':');
        if (length < 0)
        {
          errorCode = HttpStatusCode.BadRequest;
          return false;
        }
        username = str.Substring(0, length);
        password = str.Substring(length + 1);
        return true;
      }
      catch
      {
        errorCode = HttpStatusCode.InternalServerError;
        return false;
      }
    }

    #nullable enable
    /// <summary>Accept a WebSocket connection specifying the supported WebSocket sub-protocol, receive buffer size, and WebSocket keep-alive interval as an asynchronous operation.</summary>
    /// <param name="subProtocol">The supported WebSocket sub-protocol.</param>
    /// <param name="receiveBufferSize">The receive buffer size in bytes.</param>
    /// <param name="keepAliveInterval">The WebSocket protocol keep-alive interval in milliseconds.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="subProtocol" /> is an empty string
    /// 
    /// -or-
    /// 
    /// <paramref name="subProtocol" /> contains illegal characters.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="keepAliveInterval" /> is too small.
    /// 
    /// -or-
    /// 
    /// <paramref name="receiveBufferSize" /> is less than 16 bytes
    /// 
    /// -or-
    /// 
    /// <paramref name="receiveBufferSize" /> is greater than 64K bytes.</exception>
    /// <exception cref="T:System.Net.WebSockets.WebSocketException">An error occurred when sending the response to complete the WebSocket handshake.</exception>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.WebSockets.HttpListenerWebSocketContext" /> object.</returns>
    public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(
      string? subProtocol,
      int receiveBufferSize,
      TimeSpan keepAliveInterval)
    {
      return HttpWebSocket.AcceptWebSocketAsyncCore(this, subProtocol, receiveBufferSize, keepAliveInterval);
    }

    /// <summary>Accept a WebSocket connection specifying the supported WebSocket sub-protocol, receive buffer size, WebSocket keep-alive interval, and the internal buffer as an asynchronous operation.</summary>
    /// <param name="subProtocol">The supported WebSocket sub-protocol.</param>
    /// <param name="receiveBufferSize">The receive buffer size in bytes.</param>
    /// <param name="keepAliveInterval">The WebSocket protocol keep-alive interval in milliseconds.</param>
    /// <param name="internalBuffer">An internal buffer to use for this operation.</param>
    /// <exception cref="T:System.ArgumentException">
    ///        <paramref name="subProtocol" /> is an empty string
    /// 
    /// -or-
    /// 
    /// <paramref name="subProtocol" /> contains illegal characters.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    ///        <paramref name="keepAliveInterval" /> is too small.
    /// 
    /// -or-
    /// 
    /// <paramref name="receiveBufferSize" /> is less than 16 bytes
    /// 
    /// -or-
    /// 
    /// <paramref name="receiveBufferSize" /> is greater than 64K bytes.</exception>
    /// <exception cref="T:System.Net.WebSockets.WebSocketException">An error occurred when sending the response to complete the WebSocket handshake.</exception>
    /// <returns>The task object representing the asynchronous operation. The <see cref="P:System.Threading.Tasks.Task`1.Result" /> property on the task object returns an <see cref="T:System.Net.WebSockets.HttpListenerWebSocketContext" /> object.</returns>
    public Task<HttpListenerWebSocketContext> AcceptWebSocketAsync(
      string? subProtocol,
      int receiveBufferSize,
      TimeSpan keepAliveInterval,
      ArraySegment<byte> internalBuffer)
    {
      WebSocketValidate.ValidateArraySegment(internalBuffer, nameof (internalBuffer));
      HttpWebSocket.ValidateOptions(subProtocol, receiveBufferSize, 16, keepAliveInterval);
      return HttpWebSocket.AcceptWebSocketAsyncCore(this, subProtocol, receiveBufferSize, keepAliveInterval);
    }
  }
}
