// Decompiled with JetBrains decompiler
// Type: System.Uri
// Assembly: System.Private.Uri, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: B99B4CC8-2C73-4135-8B27-807D501446BA
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.Uri.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.Buffers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

#nullable enable
namespace System
{
  /// <summary>Provides an object representation of a uniform resource identifier (URI) and easy access to the parts of the URI.</summary>
  [TypeForwardedFrom("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089")]
  [Serializable]
  public class Uri : ISpanFormattable, IFormattable, ISerializable
  {
    /// <summary>Specifies that the URI is a pointer to a file. This field is read-only.</summary>
    public static readonly string UriSchemeFile = UriParser.FileUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the File Transfer Protocol (FTP). This field is read-only.</summary>
    public static readonly string UriSchemeFtp = UriParser.FtpUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the SSH File Transfer Protocol (SFTP). This field is read-only.</summary>
    public static readonly string UriSchemeSftp = "sftp";
    /// <summary>Specifies that the URI is accessed through the File Transfer Protocol Secure (FTPS). This field is read-only.</summary>
    public static readonly string UriSchemeFtps = "ftps";
    /// <summary>Specifies that the URI is accessed through the Gopher protocol. This field is read-only.</summary>
    public static readonly string UriSchemeGopher = UriParser.GopherUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the Hypertext Transfer Protocol (HTTP). This field is read-only.</summary>
    public static readonly string UriSchemeHttp = UriParser.HttpUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the Secure Hypertext Transfer Protocol (HTTPS). This field is read-only.</summary>
    public static readonly string UriSchemeHttps = UriParser.HttpsUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the WebSocket protocol (WS). This field is read-only.</summary>
    public static readonly string UriSchemeWs = UriParser.WsUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the WebSocket Secure protocol (WSS). This field is read-only.</summary>
    public static readonly string UriSchemeWss = UriParser.WssUri.SchemeName;
    /// <summary>Specifies that the URI is an email address and is accessed through the Simple Mail Transport Protocol (SMTP). This field is read-only.</summary>
    public static readonly string UriSchemeMailto = UriParser.MailToUri.SchemeName;
    /// <summary>Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP). This field is read-only.</summary>
    public static readonly string UriSchemeNews = UriParser.NewsUri.SchemeName;
    /// <summary>Specifies that the URI is an Internet news group and is accessed through the Network News Transport Protocol (NNTP). This field is read-only.</summary>
    public static readonly string UriSchemeNntp = UriParser.NntpUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the Secure Socket Shell protocol (SSH). This field is read-only.</summary>
    public static readonly string UriSchemeSsh = "ssh";
    /// <summary>Specifies that the URI is accessed through the Telnet protocol. This field is read-only.</summary>
    public static readonly string UriSchemeTelnet = UriParser.TelnetUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the NetTcp scheme used by Windows Communication Foundation (WCF). This field is read-only.</summary>
    public static readonly string UriSchemeNetTcp = UriParser.NetTcpUri.SchemeName;
    /// <summary>Specifies that the URI is accessed through the NetPipe scheme used by Windows Communication Foundation (WCF). This field is read-only.</summary>
    public static readonly string UriSchemeNetPipe = UriParser.NetPipeUri.SchemeName;
    /// <summary>Specifies the characters that separate the communication protocol scheme from the address portion of the URI. This field is read-only.</summary>
    public static readonly string SchemeDelimiter = "://";
    #nullable disable
    private string _string;
    private string _originalUnicodeString;
    internal UriParser _syntax;
    internal Uri.Flags _flags;
    private Uri.UriInfo _info;
    private static readonly SearchValues<char> s_schemeChars = SearchValues.Create((ReadOnlySpan<char>) "+-.0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz");
    private static readonly SearchValues<char> s_segmentSeparatorChars = SearchValues.Create((ReadOnlySpan<char>) ":\\/?#");

    private void InterlockedSetFlags(Uri.Flags flags)
    {
      if (this._syntax.IsSimple)
      {
        long num = (long) Interlocked.Or(ref Unsafe.As<Uri.Flags, ulong>(ref this._flags), (ulong) flags);
      }
      else
      {
        lock (this._info)
          this._flags |= flags;
      }
    }

    private bool IsImplicitFile => (this._flags & Uri.Flags.ImplicitFile) > Uri.Flags.Zero;

    private bool IsUncOrDosPath
    {
      get => (this._flags & (Uri.Flags.DosPath | Uri.Flags.UncPath)) > Uri.Flags.Zero;
    }

    private bool IsDosPath => (this._flags & Uri.Flags.DosPath) > Uri.Flags.Zero;

    private bool IsUncPath => (this._flags & Uri.Flags.UncPath) > Uri.Flags.Zero;

    private bool IsUnixPath => (this._flags & Uri.Flags.UnixPath) > Uri.Flags.Zero;

    private Uri.Flags HostType => this._flags & Uri.Flags.HostTypeMask;

    #nullable enable
    private UriParser Syntax => this._syntax;

    private bool IsNotAbsoluteUri => this._syntax == null;

    private bool IriParsing => Uri.IriParsingStatic(this._syntax);

    #nullable disable
    internal static bool IriParsingStatic(UriParser syntax)
    {
      return syntax == null || syntax.InFact(UriSyntaxFlags.AllowIriParsing);
    }

    internal bool DisablePathAndQueryCanonicalization
    {
      get => (this._flags & Uri.Flags.DisablePathAndQueryCanonicalization) > Uri.Flags.Zero;
    }

    internal bool UserDrivenParsing => (this._flags & Uri.Flags.UserDrivenParsing) > Uri.Flags.Zero;

    private int SecuredPathIndex
    {
      get
      {
        if (!this.IsDosPath)
          return 0;
        switch (this._string[(int) this._info.Offset.Path])
        {
          case '/':
          case '\\':
            return 3;
          default:
            return 2;
        }
      }
    }

    private bool NotAny(Uri.Flags flags) => (this._flags & flags) == Uri.Flags.Zero;

    private bool InFact(Uri.Flags flags) => (this._flags & flags) > Uri.Flags.Zero;

    private static bool StaticNotAny(Uri.Flags allFlags, Uri.Flags checkFlags)
    {
      return (allFlags & checkFlags) == Uri.Flags.Zero;
    }

    private static bool StaticInFact(Uri.Flags allFlags, Uri.Flags checkFlags)
    {
      return (allFlags & checkFlags) > Uri.Flags.Zero;
    }

    [MemberNotNull("_info")]
    private Uri.UriInfo EnsureUriInfo()
    {
      Uri.Flags flags = this._flags;
      if ((flags & Uri.Flags.MinimalUriInfoSet) == Uri.Flags.Zero)
        this.CreateUriInfo(flags);
      return this._info;
    }

    private void EnsureParseRemaining()
    {
      if ((this._flags & Uri.Flags.AllUriInfoSet) != Uri.Flags.Zero)
        return;
      this.ParseRemaining();
    }

    [MemberNotNull("_info")]
    private void EnsureHostString(bool allowDnsOptimization)
    {
      if (this.EnsureUriInfo().Host != null || allowDnsOptimization && this.InFact(Uri.Flags.CanonicalDnsHost))
        return;
      this.CreateHostString();
    }

    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI.</summary>
    /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="T:System.Uri" /> instance. Note that an IPv6 address in string form must be enclosed within brackets. For example, "http://[2607:f8b0:400d:c06::69]".</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uriString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
    /// 
    /// <paramref name="uriString" /> is empty.
    /// 
    ///  -or-
    /// 
    ///  The scheme specified in <paramref name="uriString" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="uriString" /> contains too many slashes.
    /// 
    ///  -or-
    /// 
    ///  The password specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The file name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The user name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.
    /// 
    ///  -or-
    /// 
    ///  The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.
    /// 
    ///  -or-
    /// 
    ///  The length of <paramref name="uriString" /> exceeds 65519 characters.
    /// 
    ///  -or-
    /// 
    ///  The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.
    /// 
    ///  -or-
    /// 
    ///  There is an invalid character sequence in <paramref name="uriString" />.
    /// 
    ///  -or-
    /// 
    ///  The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    public Uri([StringSyntax("Uri")] string uriString)
    {
      ArgumentNullException.ThrowIfNull((object) uriString, nameof (uriString));
      this.CreateThis(uriString, false, UriKind.Absolute);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI, with explicit control of character escaping.</summary>
    /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="T:System.Uri" /> instance. Note that an IPv6 address in string form must be enclosed within brackets. For example, "http://[2607:f8b0:400d:c06::69]".</param>
    /// <param name="dontEscape">
    /// <see langword="true" /> if <paramref name="uriString" /> is completely escaped; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uriString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">
    ///        <paramref name="uriString" /> is empty or contains only spaces.
    /// 
    /// -or-
    /// 
    /// The scheme specified in <paramref name="uriString" /> is not valid.
    /// 
    /// -or-
    /// 
    /// <paramref name="uriString" /> contains too many slashes.
    /// 
    /// -or-
    /// 
    /// The password specified in <paramref name="uriString" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host name specified in <paramref name="uriString" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The file name specified in <paramref name="uriString" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The user name specified in <paramref name="uriString" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.
    /// 
    /// -or-
    /// 
    /// The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.
    /// 
    /// -or-
    /// 
    /// The length of <paramref name="uriString" /> exceeds 65519 characters.
    /// 
    /// -or-
    /// 
    /// The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.
    /// 
    /// -or-
    /// 
    /// There is an invalid character sequence in <paramref name="uriString" />.
    /// 
    /// -or-
    /// 
    /// The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    [Obsolete("This constructor has been deprecated; the dontEscape parameter is always false. Use Uri(string) instead.")]
    public Uri([StringSyntax("Uri")] string uriString, bool dontEscape)
    {
      ArgumentNullException.ThrowIfNull((object) uriString, nameof (uriString));
      this.CreateThis(uriString, dontEscape, UriKind.Absolute);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the specified base and relative URIs, with explicit control of character escaping.</summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="relativeUri">The relative URI to add to the base URI.</param>
    /// <param name="dontEscape">
    /// <see langword="true" /> if <paramref name="uriString" /> is completely escaped; otherwise, <see langword="false" />.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="baseUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance.</exception>
    /// <exception cref="T:System.UriFormatException">The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.
    /// 
    /// -or-
    /// 
    /// The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.
    /// 
    /// -or-
    /// 
    /// The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.
    /// 
    /// -or-
    /// 
    /// The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.
    /// 
    /// -or-
    /// 
    /// The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.
    /// 
    /// -or-
    /// 
    /// The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.
    /// 
    /// -or-
    /// 
    /// There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.
    /// 
    /// -or-
    /// 
    /// The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    [Obsolete("This constructor has been deprecated; the dontEscape parameter is always false. Use Uri(Uri, string) instead.")]
    public Uri(Uri baseUri, string? relativeUri, bool dontEscape)
    {
      ArgumentNullException.ThrowIfNull((object) baseUri, nameof (baseUri));
      if (!baseUri.IsAbsoluteUri)
        throw new ArgumentOutOfRangeException(nameof (baseUri));
      this.CreateUri(baseUri, relativeUri, dontEscape);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI. This constructor allows you to specify if the URI string is a relative URI, absolute URI, or is indeterminate.</summary>
    /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="T:System.Uri" /> instance. Note that an IPv6 address in string form must be enclosed within brackets. For example, "http://[2607:f8b0:400d:c06::69]".</param>
    /// <param name="uriKind">Specifies whether the URI string is a relative URI, absolute URI, or is indeterminate.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="uriKind" /> is invalid.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uriString" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
    /// 
    /// <paramref name="uriString" /> contains a relative URI and <paramref name="uriKind" /> is <see cref="F:System.UriKind.Absolute" />.
    /// 
    ///  or
    /// 
    ///  <paramref name="uriString" /> contains an absolute URI and <paramref name="uriKind" /> is <see cref="F:System.UriKind.Relative" />.
    /// 
    ///  or
    /// 
    ///  <paramref name="uriString" /> is empty.
    /// 
    ///  -or-
    /// 
    ///  The scheme specified in <paramref name="uriString" /> is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.
    /// 
    ///  -or-
    /// 
    ///  <paramref name="uriString" /> contains too many slashes.
    /// 
    ///  -or-
    /// 
    ///  The password specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The file name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The user name specified in <paramref name="uriString" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host or authority name specified in <paramref name="uriString" /> cannot be terminated by backslashes.
    /// 
    ///  -or-
    /// 
    ///  The port number specified in <paramref name="uriString" /> is not valid or cannot be parsed.
    /// 
    ///  -or-
    /// 
    ///  The length of <paramref name="uriString" /> exceeds 65519 characters.
    /// 
    ///  -or-
    /// 
    ///  The length of the scheme specified in <paramref name="uriString" /> exceeds 1023 characters.
    /// 
    ///  -or-
    /// 
    ///  There is an invalid character sequence in <paramref name="uriString" />.
    /// 
    ///  -or-
    /// 
    ///  The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    public Uri([StringSyntax("Uri", new object[] {"uriKind"})] string uriString, UriKind uriKind)
    {
      ArgumentNullException.ThrowIfNull((object) uriString, nameof (uriString));
      this.CreateThis(uriString, false, uriKind);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class with the specified URI and additional <see cref="T:System.UriCreationOptions" />.</summary>
    /// <param name="uriString">A string that identifies the resource to be represented by the <see cref="T:System.Uri" /> instance.</param>
    /// <param name="creationOptions">Options that control how the <see cref="T:System.Uri" /> is created and behaves.</param>
    public Uri([StringSyntax("Uri")] string uriString, in UriCreationOptions creationOptions)
    {
      ArgumentNullException.ThrowIfNull((object) uriString, nameof (uriString));
      this.CreateThis(uriString, false, UriKind.Absolute, in creationOptions);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the specified base URI and relative URI string.</summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="relativeUri">The relative URI to add to the base URI.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="baseUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance.</exception>
    /// <exception cref="T:System.UriFormatException">Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
    /// 
    /// The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.
    /// 
    /// -or-
    /// 
    /// The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.
    /// 
    /// -or-
    /// 
    /// The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    /// -or-
    /// 
    /// The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.
    /// 
    /// -or-
    /// 
    /// The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.
    /// 
    /// -or-
    /// 
    /// The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.
    /// 
    /// -or-
    /// 
    /// The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.
    /// 
    /// -or-
    /// 
    /// There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.
    /// 
    /// -or-
    /// 
    /// The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    public Uri(Uri baseUri, string? relativeUri)
    {
      ArgumentNullException.ThrowIfNull((object) baseUri, nameof (baseUri));
      if (!baseUri.IsAbsoluteUri)
        throw new ArgumentOutOfRangeException(nameof (baseUri));
      this.CreateUri(baseUri, relativeUri, false);
    }

    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
    /// <param name="serializationInfo">An instance of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class containing the information required to serialize the new <see cref="T:System.Uri" /> instance.</param>
    /// <param name="streamingContext">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class containing the source of the serialized stream associated with the new <see cref="T:System.Uri" /> instance.</param>
    /// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationInfo" /> parameter contains a <see langword="null" /> URI.</exception>
    /// <exception cref="T:System.UriFormatException">The <paramref name="serializationInfo" /> parameter contains a URI that is empty.
    /// 
    /// -or-
    /// 
    /// The scheme specified is not correctly formed. See <see cref="M:System.Uri.CheckSchemeName(System.String)" />.
    /// 
    /// -or-
    /// 
    /// The URI contains too many slashes.
    /// 
    /// -or-
    /// 
    /// The password specified in the URI is not valid.
    /// 
    /// -or-
    /// 
    /// The host name specified in URI is not valid.
    /// 
    /// -or-
    /// 
    /// The file name specified in the URI is not valid.
    /// 
    /// -or-
    /// 
    /// The user name specified in the URI is not valid.
    /// 
    /// -or-
    /// 
    /// The host or authority name specified in the URI cannot be terminated by backslashes.
    /// 
    /// -or-
    /// 
    /// The port number specified in the URI is not valid or cannot be parsed.
    /// 
    /// -or-
    /// 
    /// The length of URI exceeds 65519 characters.
    /// 
    /// -or-
    /// 
    /// The length of the scheme specified in the URI exceeds 1023 characters.
    /// 
    /// -or-
    /// 
    /// There is an invalid character sequence in the URI.
    /// 
    /// -or-
    /// 
    /// The MS-DOS path specified in the URI must start with c:\\.</exception>
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    protected Uri(SerializationInfo serializationInfo, StreamingContext streamingContext)
    {
      string str = serializationInfo.GetString(nameof (AbsoluteUri));
      UriCreationOptions uriCreationOptions;
      if (str.Length != 0)
      {
        string uri = str;
        uriCreationOptions = new UriCreationOptions();
        ref UriCreationOptions local = ref uriCreationOptions;
        this.CreateThis(uri, false, UriKind.Absolute, in local);
      }
      else
      {
        string uri = serializationInfo.GetString("RelativeUri") ?? throw new ArgumentException(SR.Format(SR.InvalidNullArgument, (object) "RelativeUri"), nameof (serializationInfo));
        uriCreationOptions = new UriCreationOptions();
        ref UriCreationOptions local = ref uriCreationOptions;
        this.CreateThis(uri, false, UriKind.Relative, in local);
      }
    }

    #nullable disable
    /// <summary>Returns the data needed to serialize the current instance.</summary>
    /// <param name="serializationInfo">The information required to serialize the <see cref="T:System.Uri" />.</param>
    /// <param name="streamingContext">An object containing the source and destination of the serialized stream associated with the <see cref="T:System.Uri" />.</param>
    void ISerializable.GetObjectData(
      SerializationInfo serializationInfo,
      StreamingContext streamingContext)
    {
      this.GetObjectData(serializationInfo, streamingContext);
    }

    #nullable enable
    /// <summary>Returns the data needed to serialize the current instance.</summary>
    /// <param name="serializationInfo">The information required to serialize the <see cref="T:System.Uri" />.</param>
    /// <param name="streamingContext">An object that contains the source and destination of the serialized stream associated with the <see cref="T:System.Uri" />.</param>
    protected void GetObjectData(
      SerializationInfo serializationInfo,
      StreamingContext streamingContext)
    {
      if (this.IsAbsoluteUri)
      {
        serializationInfo.AddValue("AbsoluteUri", (object) this.GetParts(UriComponents.SerializationInfoString, UriFormat.UriEscaped));
      }
      else
      {
        serializationInfo.AddValue("AbsoluteUri", (object) string.Empty);
        serializationInfo.AddValue("RelativeUri", (object) this.GetParts(UriComponents.SerializationInfoString, UriFormat.UriEscaped));
      }
    }

    #nullable disable
    [MemberNotNull("_string")]
    private void CreateUri(Uri baseUri, string relativeUri, bool dontEscape)
    {
      string uri1 = relativeUri;
      int num1 = dontEscape ? 1 : 0;
      UriCreationOptions uriCreationOptions = new UriCreationOptions();
      ref UriCreationOptions local1 = ref uriCreationOptions;
      this.CreateThis(uri1, num1 != 0, UriKind.RelativeOrAbsolute, in local1);
      if (baseUri.Syntax.IsSimple)
      {
        Uri otherUri = Uri.ResolveHelper(baseUri, this, ref relativeUri, ref dontEscape);
        if (otherUri != (Uri) null)
        {
          if ((object) this == (object) otherUri)
            return;
          this.CreateThisFromUri(otherUri);
          return;
        }
      }
      else
      {
        dontEscape = false;
        UriFormatException parsingError;
        relativeUri = baseUri.Syntax.InternalResolve(baseUri, this, out parsingError);
        if (parsingError != null)
          throw parsingError;
      }
      this._flags = Uri.Flags.Zero;
      this._info = (Uri.UriInfo) null;
      this._syntax = (UriParser) null;
      this._originalUnicodeString = (string) null;
      string uri2 = relativeUri;
      int num2 = dontEscape ? 1 : 0;
      uriCreationOptions = new UriCreationOptions();
      ref UriCreationOptions local2 = ref uriCreationOptions;
      this.CreateThis(uri2, num2 != 0, UriKind.Absolute, in local2);
    }

    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.Uri" /> class based on the combination of a specified base <see cref="T:System.Uri" /> instance and a relative <see cref="T:System.Uri" /> instance.</summary>
    /// <param name="baseUri">An absolute <see cref="T:System.Uri" /> that is the base for the new <see cref="T:System.Uri" /> instance.</param>
    /// <param name="relativeUri">A relative <see cref="T:System.Uri" /> instance that is combined with <paramref name="baseUri" />.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance.</exception>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="baseUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="baseUri" /> is not an absolute <see cref="T:System.Uri" /> instance.</exception>
    /// <exception cref="T:System.UriFormatException">Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
    /// 
    /// The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is empty or contains only spaces.
    /// 
    ///  -or-
    /// 
    ///  The scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> contains too many slashes.
    /// 
    ///  -or-
    /// 
    ///  The password specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The file name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The user name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid.
    /// 
    ///  -or-
    /// 
    ///  The host or authority name specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> cannot be terminated by backslashes.
    /// 
    ///  -or-
    /// 
    ///  The port number specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> is not valid or cannot be parsed.
    /// 
    ///  -or-
    /// 
    ///  The length of the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 65519 characters.
    /// 
    ///  -or-
    /// 
    ///  The length of the scheme specified in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" /> exceeds 1023 characters.
    /// 
    ///  -or-
    /// 
    ///  There is an invalid character sequence in the URI formed by combining <paramref name="baseUri" /> and <paramref name="relativeUri" />.
    /// 
    ///  -or-
    /// 
    ///  The MS-DOS path specified in <paramref name="uriString" /> must start with c:\\.</exception>
    public Uri(Uri baseUri, Uri relativeUri)
    {
      ArgumentNullException.ThrowIfNull((object) baseUri, nameof (baseUri));
      if (!baseUri.IsAbsoluteUri)
        throw new ArgumentOutOfRangeException(nameof (baseUri));
      this.CreateThisFromUri(relativeUri);
      string newUriString = (string) null;
      bool userEscaped;
      if (baseUri.Syntax.IsSimple)
      {
        userEscaped = this.InFact(Uri.Flags.UserEscaped);
        Uri otherUri = Uri.ResolveHelper(baseUri, this, ref newUriString, ref userEscaped);
        if (otherUri != (Uri) null)
        {
          if ((object) this == (object) otherUri)
            return;
          this.CreateThisFromUri(otherUri);
          return;
        }
      }
      else
      {
        userEscaped = false;
        UriFormatException parsingError;
        newUriString = baseUri.Syntax.InternalResolve(baseUri, this, out parsingError);
        if (parsingError != null)
          throw parsingError;
      }
      this._flags = Uri.Flags.Zero;
      this._info = (Uri.UriInfo) null;
      this._syntax = (UriParser) null;
      this._originalUnicodeString = (string) null;
      this.CreateThis(newUriString, userEscaped, UriKind.Absolute);
    }

    #nullable disable
    private static void GetCombinedString(
      Uri baseUri,
      string relativeStr,
      bool dontEscape,
      ref string result)
    {
      for (int index = 0; index < relativeStr.Length && relativeStr[index] != '/' && relativeStr[index] != '\\' && relativeStr[index] != '?' && relativeStr[index] != '#'; ++index)
      {
        if (relativeStr[index] == ':')
        {
          if (index >= 2)
          {
            ParsingError error = ParsingError.None;
            UriParser uriParser = Uri.CheckSchemeSyntax(relativeStr.AsSpan(0, index), ref error);
            if (error == ParsingError.None)
            {
              if (baseUri.Syntax == uriParser)
              {
                relativeStr = index + 1 >= relativeStr.Length ? string.Empty : relativeStr.Substring(index + 1);
                break;
              }
              result = relativeStr;
              return;
            }
            break;
          }
          break;
        }
      }
      if (relativeStr.Length == 0)
        result = baseUri.OriginalString;
      else
        result = Uri.CombineUri(baseUri, relativeStr, dontEscape ? UriFormat.UriEscaped : UriFormat.SafeUnescaped);
    }

    private static UriFormatException GetException(ParsingError err)
    {
      switch (err)
      {
        case ParsingError.None:
          return (UriFormatException) null;
        case ParsingError.BadFormat:
          return new UriFormatException(SR.net_uri_BadFormat);
        case ParsingError.BadScheme:
          return new UriFormatException(SR.net_uri_BadScheme);
        case ParsingError.BadAuthority:
          return new UriFormatException(SR.net_uri_BadAuthority);
        case ParsingError.EmptyUriString:
          return new UriFormatException(SR.net_uri_EmptyUri);
        case ParsingError.SchemeLimit:
          return new UriFormatException(SR.net_uri_SchemeLimit);
        case ParsingError.SizeLimit:
          return new UriFormatException(SR.net_uri_SizeLimit);
        case ParsingError.MustRootedPath:
          return new UriFormatException(SR.net_uri_MustRootedPath);
        case ParsingError.BadHostName:
          return new UriFormatException(SR.net_uri_BadHostName);
        case ParsingError.NonEmptyHost:
          return new UriFormatException(SR.net_uri_BadFormat);
        case ParsingError.BadPort:
          return new UriFormatException(SR.net_uri_BadPort);
        case ParsingError.BadAuthorityTerminator:
          return new UriFormatException(SR.net_uri_BadAuthorityTerminator);
        case ParsingError.CannotCreateRelative:
          return new UriFormatException(SR.net_uri_CannotCreateRelative);
        default:
          return new UriFormatException(SR.net_uri_BadFormat);
      }
    }

    #nullable enable
    /// <summary>Gets the absolute path of the URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The absolute path to the resource.</returns>
    public string AbsolutePath
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        string absolutePath = this.PrivateAbsolutePath;
        if (this.IsDosPath && absolutePath[0] == '/')
          absolutePath = absolutePath.Substring(1);
        return absolutePath;
      }
    }

    private string PrivateAbsolutePath
    {
      get
      {
        Uri.MoreInfo moreInfo = this.EnsureUriInfo().MoreInfo;
        return moreInfo.Path ?? (moreInfo.Path = this.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.UriEscaped));
      }
    }

    /// <summary>Gets the absolute URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The entire URI.</returns>
    public string AbsoluteUri
    {
      get
      {
        if (this._syntax == null)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        Uri.MoreInfo moreInfo = this.EnsureUriInfo().MoreInfo;
        return moreInfo.AbsoluteUri ?? (moreInfo.AbsoluteUri = this.GetParts(UriComponents.AbsoluteUri, UriFormat.UriEscaped));
      }
    }

    /// <summary>Gets a local operating-system representation of a file name.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The local operating-system representation of a file name.</returns>
    public string LocalPath
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this.GetLocalPath();
      }
    }

    /// <summary>Gets the Domain Name System (DNS) host name or IP address and the port number for a server.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The authority component of the URI represented by this instance.</returns>
    public string Authority
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this.GetParts(UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped);
      }
    }

    /// <summary>Gets the type of the host name specified in the URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>A member of the <see cref="T:System.UriHostNameType" /> enumeration.</returns>
    public UriHostNameType HostNameType
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        if (this._syntax.IsSimple)
          this.EnsureUriInfo();
        else
          this.EnsureHostString(false);
        switch (this.HostType)
        {
          case Uri.Flags.IPv6HostType:
            return UriHostNameType.IPv6;
          case Uri.Flags.IPv4HostType:
            return UriHostNameType.IPv4;
          case Uri.Flags.DnsHostType:
            return UriHostNameType.Dns;
          case Uri.Flags.UncHostType:
            return UriHostNameType.Basic;
          case Uri.Flags.BasicHostType:
            return UriHostNameType.Basic;
          case Uri.Flags.HostTypeMask:
            return UriHostNameType.Unknown;
          default:
            return UriHostNameType.Unknown;
        }
      }
    }

    /// <summary>Gets a value that indicates whether the port value of the URI is the default for this scheme.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>
    /// <see langword="true" /> if the value in the <see cref="P:System.Uri.Port" /> property is the default port for this scheme; otherwise, <see langword="false" />.</returns>
    public bool IsDefaultPort
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        if (this._syntax.IsSimple)
          this.EnsureUriInfo();
        else
          this.EnsureHostString(false);
        return this.NotAny(Uri.Flags.NotDefaultPort);
      }
    }

    /// <summary>Gets a value that indicates whether the specified <see cref="T:System.Uri" /> is a file URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> is a file URI; otherwise, <see langword="false" />.</returns>
    public bool IsFile
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return (object) this._syntax.SchemeName == (object) Uri.UriSchemeFile;
      }
    }

    /// <summary>Gets a value that indicates whether the specified <see cref="T:System.Uri" /> references the local host.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>
    /// <see langword="true" /> if this <see cref="T:System.Uri" /> references the local host; otherwise, <see langword="false" />.</returns>
    public bool IsLoopback
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        this.EnsureHostString(false);
        return this.InFact(Uri.Flags.LoopbackHost);
      }
    }

    /// <summary>Gets the <see cref="P:System.Uri.AbsolutePath" /> and <see cref="P:System.Uri.Query" /> properties separated by a question mark (?).</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The <see cref="P:System.Uri.AbsolutePath" /> and <see cref="P:System.Uri.Query" /> properties separated by a question mark (?).</returns>
    public string PathAndQuery
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        Uri.UriInfo uriInfo = this.EnsureUriInfo();
        if (uriInfo.PathAndQuery == null)
        {
          string str = this.GetParts(UriComponents.PathAndQuery, UriFormat.UriEscaped);
          if (this.IsDosPath && str[0] == '/')
            str = str.Substring(1);
          uriInfo.PathAndQuery = str;
        }
        return uriInfo.PathAndQuery;
      }
    }

    /// <summary>Gets an array containing the path segments that make up the specified URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The path segments that make up the specified URI.</returns>
    public string[] Segments
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        string privateAbsolutePath = this.PrivateAbsolutePath;
        string[] segments;
        if (privateAbsolutePath.Length == 0)
        {
          segments = Array.Empty<string>();
        }
        else
        {
          ArrayBuilder<string> arrayBuilder = new ArrayBuilder<string>();
          int num;
          for (int startIndex = 0; startIndex < privateAbsolutePath.Length; startIndex = num + 1)
          {
            num = privateAbsolutePath.IndexOf('/', startIndex);
            if (num == -1)
              num = privateAbsolutePath.Length - 1;
            arrayBuilder.Add(privateAbsolutePath.Substring(startIndex, num - startIndex + 1));
          }
          segments = arrayBuilder.ToArray();
        }
        return segments;
      }
    }

    /// <summary>Gets a value that indicates whether the specified <see cref="T:System.Uri" /> is a universal naming convention (UNC) path.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> is a UNC path; otherwise, <see langword="false" />.</returns>
    public bool IsUnc
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this.IsUncPath;
      }
    }

    /// <summary>Gets the host component of this instance.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The host name. This is usually the DNS host name or IP address of the server.</returns>
    public string Host
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this.GetParts(UriComponents.Host, UriFormat.UriEscaped);
      }
    }

    #nullable disable
    private static bool StaticIsFile(UriParser syntax) => syntax.InFact(UriSyntaxFlags.FileLikeUri);

    private string GetLocalPath()
    {
      this.EnsureParseRemaining();
      if (!this.IsUncOrDosPath)
        return this.GetUnescapedParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.Unescaped);
      this.EnsureHostString(false);
      if (this.NotAny(Uri.Flags.HostNotCanonical | Uri.Flags.PathNotCanonical | Uri.Flags.ShouldBeCompressed))
      {
        int num = this.IsUncPath ? (int) this._info.Offset.Host - 2 : (int) this._info.Offset.Path;
        string str = !this.IsImplicitFile || (int) this._info.Offset.Host != (this.IsDosPath ? 0 : 2) || (int) this._info.Offset.Query != (int) this._info.Offset.End ? (!this.IsDosPath || this._string[num] != '/' && this._string[num] != '\\' ? this._string.Substring(num, (int) this._info.Offset.Query - num) : this._string.Substring(num + 1, (int) this._info.Offset.Query - num - 1)) : this._string;
        if (this.IsDosPath && str[1] == '|')
          str = str.Remove(1, 1).Insert(1, ":");
        return str.Replace('/', '\\');
      }
      int length = 0;
      int path = (int) this._info.Offset.Path;
      string host = this._info.Host;
      char[] chArray = new char[host.Length + 3 + (int) this._info.Offset.Fragment - (int) this._info.Offset.Path];
      if (this.IsUncPath)
      {
        chArray[0] = '\\';
        chArray[1] = '\\';
        length = 2;
        UriHelper.UnescapeString(host, 0, host.Length, chArray, ref length, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.CopyOnly, this._syntax, false);
      }
      else if (this._string[path] == '/' || this._string[path] == '\\')
        ++path;
      ushort num1 = (ushort) length;
      UnescapeMode unescapeMode = !this.InFact(Uri.Flags.PathNotCanonical) || this.IsImplicitFile ? UnescapeMode.CopyOnly : UnescapeMode.Unescape | UnescapeMode.UnescapeAll;
      UriHelper.UnescapeString(this._string, path, (int) this._info.Offset.Query, chArray, ref length, char.MaxValue, char.MaxValue, char.MaxValue, unescapeMode, this._syntax, true);
      if (chArray[1] == '|')
        chArray[1] = ':';
      if (this.InFact(Uri.Flags.ShouldBeCompressed))
        Uri.Compress(chArray, this.IsDosPath ? (int) num1 + 2 : (int) num1, ref length, this._syntax);
      chArray.AsSpan<char>(0, length).Replace<char>('/', '\\');
      return new string(chArray, 0, length);
    }

    /// <summary>Gets the port number of this URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The port number for this URI.</returns>
    public int Port
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        if (this._syntax.IsSimple)
          this.EnsureUriInfo();
        else
          this.EnsureHostString(false);
        return this.InFact(Uri.Flags.NotDefaultPort) ? (int) this._info.Offset.PortValue : this._syntax.DefaultPort;
      }
    }

    #nullable enable
    /// <summary>Gets any query information included in the specified URI, including the leading '?' character if not empty.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>Any query information included in the specified URI.</returns>
    public string Query
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        Uri.MoreInfo moreInfo = this.EnsureUriInfo().MoreInfo;
        return moreInfo.Query ?? (moreInfo.Query = this.GetParts(UriComponents.Query | UriComponents.KeepDelimiter, UriFormat.UriEscaped));
      }
    }

    /// <summary>Gets the escaped URI fragment, including the leading '#' character if not empty.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>URI fragment information.</returns>
    public string Fragment
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        Uri.MoreInfo moreInfo = this.EnsureUriInfo().MoreInfo;
        return moreInfo.Fragment ?? (moreInfo.Fragment = this.GetParts(UriComponents.Fragment | UriComponents.KeepDelimiter, UriFormat.UriEscaped));
      }
    }

    /// <summary>Gets the scheme name for this URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The scheme for this URI, converted to lowercase.</returns>
    public string Scheme
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this._syntax.SchemeName;
      }
    }

    /// <summary>Gets the original URI string that was passed to the <see cref="T:System.Uri" /> constructor.</summary>
    /// <returns>The exact URI specified when this instance was constructed; otherwise, <see cref="F:System.String.Empty" />.</returns>
    public string OriginalString => this._originalUnicodeString ?? this._string;

    /// <summary>Gets a host name that, after being unescaped if necessary, is safe to use for DNS resolution.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The host part of the URI in a format suitable for DNS resolution; or the original host string, if it is already suitable for resolution.</returns>
    public string DnsSafeHost
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        this.EnsureHostString(false);
        switch (this.HostType)
        {
          case Uri.Flags.IPv6HostType:
            return this.IdnHost;
          case Uri.Flags.BasicHostType:
            if (!this.InFact(Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical))
              break;
            goto case Uri.Flags.IPv6HostType;
        }
        return this._info.Host;
      }
    }

    /// <summary>Gets the RFC 3490 compliant International Domain Name of the host, using Punycode as appropriate. This string, after being unescaped if necessary, is safe to use for DNS resolution.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The hostname, formatted with Punycode according to the IDN standard.</returns>
    public string IdnHost
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        if (this._info?.IdnHost == null)
        {
          this.EnsureHostString(false);
          string str = this._info.Host;
          switch (this.HostType)
          {
            case Uri.Flags.IPv6HostType:
              str = this._info.ScopeId != null ? str.AsSpan(1, str.Length - 2).ToString() + (ReadOnlySpan<char>) this._info.ScopeId : str.Substring(1, str.Length - 2);
              break;
            case Uri.Flags.DnsHostType:
              str = DomainNameHelper.IdnEquivalent(str);
              break;
            case Uri.Flags.BasicHostType:
              if (this.InFact(Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical))
              {
                ValueStringBuilder dest = new ValueStringBuilder(stackalloc char[512]);
                UriHelper.UnescapeString(str, 0, str.Length, ref dest, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.Unescape | UnescapeMode.UnescapeAll, this._syntax, false);
                str = dest.ToString();
                break;
              }
              break;
          }
          this._info.IdnHost = str;
        }
        return this._info.IdnHost;
      }
    }

    /// <summary>Gets a value that indicates whether the <see cref="T:System.Uri" /> instance is absolute.</summary>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> instance is absolute; otherwise, <see langword="false" />.</returns>
    public bool IsAbsoluteUri => this._syntax != null;

    /// <summary>Gets a value that indicates whether the URI string was completely escaped before the <see cref="T:System.Uri" /> instance was created.</summary>
    /// <returns>
    /// <see langword="true" /> if the <paramref name="dontEscape" /> parameter was set to <see langword="true" /> when the <see cref="T:System.Uri" /> instance was created; otherwise, <see langword="false" />.</returns>
    public bool UserEscaped => this.InFact(Uri.Flags.UserEscaped);

    /// <summary>Gets the user name, password, or other user-specific information associated with the specified URI.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>The user information associated with the URI. The returned value does not include the '@' character reserved for delimiting the user information part of the URI.</returns>
    public string UserInfo
    {
      get
      {
        if (this.IsNotAbsoluteUri)
          throw new InvalidOperationException(SR.net_uri_NotAbsolute);
        return this.GetParts(UriComponents.UserInfo, UriFormat.UriEscaped);
      }
    }

    /// <summary>Determines whether the specified host name is a valid DNS name.</summary>
    /// <param name="name">The host name to validate. This can be an IPv4 or IPv6 address or an Internet host name.</param>
    /// <returns>The type of the host name. If the type of the host name cannot be determined or if the host name is <see langword="null" /> or a zero-length string, this method returns <see cref="F:System.UriHostNameType.Unknown" />.</returns>
    public static unsafe UriHostNameType CheckHostName(string? name)
    {
      if (string.IsNullOrEmpty(name) || name.Length > (int) short.MaxValue)
        return UriHostNameType.Unknown;
      int end = name.Length;
      IntPtr num;
      if (name == null)
      {
        num = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &name.GetPinnableReference())
          num = (IntPtr) chPtr;
      }
      char* name1 = (char*) num;
      if (name.StartsWith('[') && name.EndsWith(']') && IPv6AddressHelper.IsValid(name1, 1, ref end) && end == name.Length)
        return UriHostNameType.IPv6;
      end = name.Length;
      if (IPv4AddressHelper.IsValid(name1, 0, ref end, false, false, false) && end == name.Length)
        return UriHostNameType.IPv4;
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(chPtr);
      int length;
      if (DomainNameHelper.IsValid((ReadOnlySpan<char>) name, false, false, out length) && length == name.Length || DomainNameHelper.IsValid((ReadOnlySpan<char>) name, true, false, out length) && length == name.Length)
        return UriHostNameType.Dns;
      end = name.Length + 2;
      name = "[" + name + "]";
      IntPtr name2;
      if (name == null)
      {
        name2 = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &name.GetPinnableReference())
          name2 = (IntPtr) chPtr;
      }
      if (IPv6AddressHelper.IsValid((char*) name2, 1, ref end) && end == name.Length)
        return UriHostNameType.IPv6;
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(chPtr);
      return UriHostNameType.Unknown;
    }

    /// <summary>Gets the specified portion of a <see cref="T:System.Uri" /> instance.</summary>
    /// <param name="part">One of the enumeration values that specifies the end of the URI portion to return.</param>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Uri" /> instance is not an absolute instance.</exception>
    /// <exception cref="T:System.ArgumentException">The specified <paramref name="part" /> is not valid.</exception>
    /// <returns>The specified portion of the <see cref="T:System.Uri" /> instance.</returns>
    public string GetLeftPart(UriPartial part)
    {
      if (this.IsNotAbsoluteUri)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      this.EnsureUriInfo();
      switch (part)
      {
        case UriPartial.Scheme:
          return this.GetParts(UriComponents.Scheme | UriComponents.KeepDelimiter, UriFormat.UriEscaped);
        case UriPartial.Authority:
          return this.NotAny(Uri.Flags.AuthorityFound) || this.IsDosPath ? string.Empty : this.GetParts(UriComponents.SchemeAndServer | UriComponents.UserInfo, UriFormat.UriEscaped);
        case UriPartial.Path:
          return this.GetParts(UriComponents.SchemeAndServer | UriComponents.UserInfo | UriComponents.Path, UriFormat.UriEscaped);
        case UriPartial.Query:
          return this.GetParts(UriComponents.HttpRequestUrl | UriComponents.UserInfo, UriFormat.UriEscaped);
        default:
          throw new ArgumentException(SR.Format(SR.Argument_InvalidUriSubcomponent, (object) part), nameof (part));
      }
    }

    /// <summary>Converts a specified character into its hexadecimal equivalent.</summary>
    /// <param name="character">The character to convert to hexadecimal representation.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="character" /> is greater than 255.</exception>
    /// <returns>The hexadecimal representation of the specified character.</returns>
    public static string HexEscape(char character)
    {
      ArgumentOutOfRangeException.ThrowIfGreaterThan<char>(character, 'ÿ', nameof (character));
      return string.Create<byte>(3, (byte) character, (SpanAction<char, byte>) ((chars, b) =>
      {
        chars[0] = '%';
        HexConverter.ToCharsBuffer(b, chars, 1);
      }));
    }

    /// <summary>Converts a specified hexadecimal representation of a character to the character.</summary>
    /// <param name="pattern">The hexadecimal representation of a character.</param>
    /// <param name="index">The location in <paramref name="pattern" /> where the hexadecimal representation of a character begins.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="index" /> is less than 0 or greater than or equal to the number of characters in <paramref name="pattern" />.</exception>
    /// <returns>The character represented by the hexadecimal encoding at position <paramref name="index" />. If the character at <paramref name="index" /> is not hexadecimal encoded, the character at <paramref name="index" /> is returned. The value of <paramref name="index" /> is incremented to point to the character following the one returned.</returns>
    public static char HexUnescape(string pattern, ref int index)
    {
      ArgumentOutOfRangeException.ThrowIfNegative<int>(index, nameof (index));
      ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual<int>(index, pattern.Length, nameof (index));
      if (pattern[index] == '%' && pattern.Length - index >= 3)
      {
        char ch = UriHelper.DecodeHexChars((int) pattern[index + 1], (int) pattern[index + 2]);
        if (ch != char.MaxValue)
        {
          index += 3;
          return ch;
        }
      }
      return pattern[index++];
    }

    /// <summary>Determines whether a character in a string is hexadecimal encoded.</summary>
    /// <param name="pattern">The string to check.</param>
    /// <param name="index">The location in <paramref name="pattern" /> to check for hexadecimal encoding.</param>
    /// <returns>
    /// <see langword="true" /> if <paramref name="pattern" /> is hexadecimal encoded at the specified location; otherwise, <see langword="false" />.</returns>
    public static bool IsHexEncoding(string pattern, int index)
    {
      return pattern.Length - index >= 3 && pattern[index] == '%' && char.IsAsciiHexDigit(pattern[index + 1]) && char.IsAsciiHexDigit(pattern[index + 2]);
    }

    /// <summary>Determines whether the specified scheme name is valid.</summary>
    /// <param name="schemeName">The scheme name to validate.</param>
    /// <returns>
    /// <see langword="true" /> if the scheme name is valid; otherwise, <see langword="false" />.</returns>
    public static bool CheckSchemeName([NotNullWhen(true)] string? schemeName)
    {
      return !string.IsNullOrEmpty(schemeName) && char.IsAsciiLetter(schemeName[0]) && !schemeName.AsSpan().ContainsAnyExcept<char>(Uri.s_schemeChars);
    }

    /// <summary>Determines whether a specified character is a valid hexadecimal digit.</summary>
    /// <param name="character">The character to validate.</param>
    /// <returns>
    /// <see langword="true" /> if the character is a valid hexadecimal digit; otherwise, <see langword="false" />.</returns>
    public static bool IsHexDigit(char character) => char.IsAsciiHexDigit(character);

    /// <summary>Gets the decimal value of a hexadecimal digit.</summary>
    /// <param name="digit">The hexadecimal digit (0-9, a-f, A-F) to convert.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="digit" /> is not a valid hexadecimal digit (0-9, a-f, A-F).</exception>
    /// <returns>A number from 0 to 15 that corresponds to the specified hexadecimal digit.</returns>
    public static int FromHex(char digit)
    {
      int num = HexConverter.FromChar((int) digit);
      return num != (int) byte.MaxValue ? num : throw new ArgumentException((string) null, nameof (digit));
    }

    /// <summary>Gets the hash code for the URI.</summary>
    /// <returns>The hash value generated for this URI.</returns>
    public override int GetHashCode()
    {
      if (this.IsNotAbsoluteUri)
        return this.OriginalString.GetHashCode();
      Uri.MoreInfo moreInfo1 = this.EnsureUriInfo().MoreInfo;
      UriComponents uriParts = UriComponents.HttpRequestUrl;
      if (this._syntax.InFact(UriSyntaxFlags.MailToLikeUri))
        uriParts |= UriComponents.UserInfo;
      Uri.MoreInfo moreInfo2 = moreInfo1;
      string str = moreInfo2.RemoteUrl ?? (moreInfo2.RemoteUrl = this.GetParts(uriParts, UriFormat.SafeUnescaped));
      return this.IsUncOrDosPath ? StringComparer.OrdinalIgnoreCase.GetHashCode(str) : str.GetHashCode();
    }

    /// <summary>Gets a canonical string representation for the specified <see cref="T:System.Uri" /> instance.</summary>
    /// <returns>The unescaped canonical representation of the <see cref="T:System.Uri" /> instance. All characters are unescaped except #, ?, and %.</returns>
    public override string ToString()
    {
      if (this._syntax == null)
        return this._string;
      this.EnsureUriInfo();
      Uri.UriInfo info = this._info;
      return info.String ?? (info.String = this._syntax.IsSimple ? this.GetComponentsHelper(UriComponents.AbsoluteUri, (UriFormat) 32767) : this.GetParts(UriComponents.AbsoluteUri, UriFormat.SafeUnescaped));
    }

    /// <summary>Attempts to format a canonical string representation for the <see cref="T:System.Uri" /> instance into the specified span.</summary>
    /// <param name="destination">The span into which to write this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters that were written in <paramref name="destination" />.</param>
    /// <returns>
    /// <see langword="true" /> if the formatting was successful; otherwise, <see langword="false" />.</returns>
    public bool TryFormat(Span<char> destination, out int charsWritten)
    {
      ReadOnlySpan<char> readOnlySpan;
      if (this._syntax == null)
      {
        readOnlySpan = (ReadOnlySpan<char>) this._string;
      }
      else
      {
        this.EnsureUriInfo();
        if (this._info.String != null)
        {
          readOnlySpan = (ReadOnlySpan<char>) this._info.String;
        }
        else
        {
          UriFormat formatAs = (UriFormat) 32767;
          if (!this._syntax.IsSimple)
          {
            if (this.IsNotAbsoluteUri)
              throw new InvalidOperationException(SR.net_uri_NotAbsolute);
            if (this.UserDrivenParsing)
              throw new InvalidOperationException(SR.Format(SR.net_uri_UserDrivenParsing, (object) this.GetType()));
            if (this.DisablePathAndQueryCanonicalization)
              throw new InvalidOperationException(SR.net_uri_GetComponentsCalledWhenCanonicalizationDisabled);
            formatAs = UriFormat.SafeUnescaped;
          }
          this.EnsureParseRemaining();
          this.EnsureHostString(true);
          ushort nonCanonical = (ushort) ((uint) (ushort) this._flags & (uint) sbyte.MaxValue);
          if ((this._flags & (Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath)) != Uri.Flags.Zero || this.IsDosPath && this._string[(int) this._info.Offset.Path + this.SecuredPathIndex - 1] == '|')
            nonCanonical |= (ushort) 16;
          if (((int) sbyte.MaxValue & (int) nonCanonical) != 0)
            return this.TryRecreateParts(destination, out charsWritten, UriComponents.AbsoluteUri, nonCanonical, formatAs);
          readOnlySpan = this._string.AsSpan((int) this._info.Offset.Scheme, (int) this._info.Offset.End - (int) this._info.Offset.Scheme);
        }
      }
      if (readOnlySpan.TryCopyTo(destination))
      {
        charsWritten = readOnlySpan.Length;
        return true;
      }
      charsWritten = 0;
      return false;
    }

    #nullable disable
    /// <summary>Tries to format the value of the current instance into the provided span of characters.</summary>
    /// <param name="destination">The span in which to write this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters that were written in <code data-dev-comment-type="paramref">destination</code>.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <code data-dev-comment-type="paramref">destination</code>.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <code data-dev-comment-type="paramref">destination</code>.</param>
    /// <returns>
    /// <code data-dev-comment-type="langword">true</code> if the formatting was successful; otherwise, <code data-dev-comment-type="langword">false</code>.</returns>
    bool ISpanFormattable.TryFormat(
      Span<char> destination,
      out int charsWritten,
      ReadOnlySpan<char> format,
      IFormatProvider provider)
    {
      return this.TryFormat(destination, out charsWritten);
    }

    /// <summary>Formats the value of the current instance using the specified format.</summary>
    /// <param name="format">The format to use.
    /// -or-
    /// A <code data-dev-comment-type="langword">null</code> reference (<code data-dev-comment-type="langword">Nothing</code> in Visual Basic) to use the default format defined for the type of the <xref data-throw-if-not-resolved="true" uid="System.IFormattable"></xref> implementation.</param>
    /// <param name="formatProvider">The provider to use to format the value.
    /// -or-
    /// A <code data-dev-comment-type="langword">null</code> reference (<code data-dev-comment-type="langword">Nothing</code> in Visual Basic) to obtain the numeric format information from the current locale setting of the operating system.</param>
    /// <returns>The value of the current instance in the specified format.</returns>
    string IFormattable.ToString(string format, IFormatProvider formatProvider) => this.ToString();

    #nullable enable
    /// <summary>Determines whether two <see cref="T:System.Uri" /> instances have the same value.</summary>
    /// <param name="uri1">A URI to compare with <paramref name="uri2" />.</param>
    /// <param name="uri2">A URI to compare with <paramref name="uri1" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> instances are equivalent; otherwise, <see langword="false" />.</returns>
    public static bool operator ==(Uri? uri1, Uri? uri2)
    {
      if ((object) uri1 == (object) uri2)
        return true;
      return (object) uri1 != null && (object) uri2 != null && uri1.Equals((object) uri2);
    }

    /// <summary>Determines whether two <see cref="T:System.Uri" /> instances do not have the same value.</summary>
    /// <param name="uri1">A URI to compare with <paramref name="uri2" />.</param>
    /// <param name="uri2">A URI to compare with <paramref name="uri1" />.</param>
    /// <returns>
    /// <see langword="true" /> if the two <see cref="T:System.Uri" /> instances are not equal; otherwise, <see langword="false" />. If either parameter is <see langword="null" />, this method returns <see langword="true" />.</returns>
    public static bool operator !=(Uri? uri1, Uri? uri2)
    {
      if ((object) uri1 == (object) uri2)
        return false;
      return (object) uri1 == null || (object) uri2 == null || !uri1.Equals((object) uri2);
    }

    /// <summary>Compares two <see cref="T:System.Uri" /> instances for equality.</summary>
    /// <param name="comparand">The URI or a URI identifier to compare with the current instance.</param>
    /// <returns>
    /// <see langword="true" /> if the two instances represent the same URI; otherwise, <see langword="false" />.</returns>
    public override bool Equals([NotNullWhen(true)] object? comparand)
    {
      if (comparand == null)
        return false;
      if ((object) this == comparand)
        return true;
      Uri result = comparand as Uri;
      if ((object) result == null)
      {
        if (this.DisablePathAndQueryCanonicalization || !(comparand is string uriString))
          return false;
        if ((object) uriString == (object) this.OriginalString)
          return true;
        if (!Uri.TryCreate(uriString, UriKind.RelativeOrAbsolute, out result))
          return false;
      }
      if (this.DisablePathAndQueryCanonicalization != result.DisablePathAndQueryCanonicalization)
        return false;
      if ((object) this.OriginalString == (object) result.OriginalString)
        return true;
      if (this.IsAbsoluteUri != result.IsAbsoluteUri)
        return false;
      if (this.IsNotAbsoluteUri)
        return this.OriginalString.Equals(result.OriginalString);
      if ((this.NotAny(Uri.Flags.AllUriInfoSet) || result.NotAny(Uri.Flags.AllUriInfoSet)) && string.Equals(this._string, result._string, this.IsUncOrDosPath ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
        return true;
      this.EnsureUriInfo();
      result.EnsureUriInfo();
      if (!this.UserDrivenParsing && !result.UserDrivenParsing && this.Syntax.IsSimple && result.Syntax.IsSimple)
      {
        if (this.InFact(Uri.Flags.CanonicalDnsHost) && result.InFact(Uri.Flags.CanonicalDnsHost))
        {
          int host1 = (int) this._info.Offset.Host;
          int num = (int) this._info.Offset.Path;
          int host2 = (int) result._info.Offset.Host;
          int path = (int) result._info.Offset.Path;
          string str = result._string;
          if (num - host1 > path - host2)
            num = host1 + path - host2;
          while (host1 < num)
          {
            if ((int) this._string[host1] != (int) str[host2])
              return false;
            if (str[host2] != ':')
            {
              ++host1;
              ++host2;
            }
            else
              break;
          }
          if (host1 < (int) this._info.Offset.Path && this._string[host1] != ':' || host2 < path && str[host2] != ':')
            return false;
        }
        else
        {
          this.EnsureHostString(false);
          result.EnsureHostString(false);
          if (!this._info.Host.Equals(result._info.Host))
            return false;
        }
        if (this.Port != result.Port)
          return false;
      }
      Uri.MoreInfo moreInfo1 = this._info.MoreInfo;
      Uri.MoreInfo moreInfo2 = result._info.MoreInfo;
      UriComponents uriParts = UriComponents.HttpRequestUrl;
      if (this._syntax.InFact(UriSyntaxFlags.MailToLikeUri))
      {
        if (!result._syntax.InFact(UriSyntaxFlags.MailToLikeUri))
          return false;
        uriParts |= UriComponents.UserInfo;
      }
      Uri.MoreInfo moreInfo3 = moreInfo1;
      string a = moreInfo3.RemoteUrl ?? (moreInfo3.RemoteUrl = this.GetParts(uriParts, UriFormat.SafeUnescaped));
      Uri.MoreInfo moreInfo4 = moreInfo2;
      string b = moreInfo4.RemoteUrl ?? (moreInfo4.RemoteUrl = result.GetParts(uriParts, UriFormat.SafeUnescaped));
      return string.Equals(a, b, this.IsUncOrDosPath ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
    }

    /// <summary>Determines the difference between two <see cref="T:System.Uri" /> instances.</summary>
    /// <param name="uri">The URI to compare to the current URI.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this property is valid only for absolute URIs.</exception>
    /// <returns>If the hostname and scheme of this URI instance and <paramref name="uri" /> are the same, then this method returns a relative <see cref="T:System.Uri" /> that, when appended to the current URI instance, yields <paramref name="uri" />.
    /// 
    /// If the hostname or scheme is different, then this method returns a <see cref="T:System.Uri" /> that represents the <paramref name="uri" /> parameter.</returns>
    public Uri MakeRelativeUri(Uri uri)
    {
      ArgumentNullException.ThrowIfNull((object) uri, nameof (uri));
      if (this.IsNotAbsoluteUri || uri.IsNotAbsoluteUri)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      if (!(this.Scheme == uri.Scheme) || !(this.Host == uri.Host) || this.Port != uri.Port)
        return uri;
      string absolutePath = uri.AbsolutePath;
      string uriString = Uri.PathDifference(this.AbsolutePath, absolutePath, !this.IsUncOrDosPath);
      if (Uri.CheckForColonInFirstPathSegment(uriString) && (!uri.IsDosPath || !absolutePath.Equals(uriString, StringComparison.Ordinal)))
        uriString = "./" + uriString;
      return new Uri(uriString + uri.GetParts(UriComponents.Query | UriComponents.Fragment, UriFormat.UriEscaped), UriKind.Relative);
    }

    #nullable disable
    private static bool CheckForColonInFirstPathSegment(string uriString)
    {
      int index = uriString.AsSpan().IndexOfAny<char>(Uri.s_segmentSeparatorChars);
      return (uint) index < (uint) uriString.Length && uriString[index] == ':';
    }

    internal static string InternalEscapeString(string rawString)
    {
      return rawString != null ? UriHelper.EscapeString(rawString, true, UriHelper.UnreservedReservedExceptQuestionMarkHash) : string.Empty;
    }

    private static ParsingError ParseScheme(
      string uriString,
      ref Uri.Flags flags,
      ref UriParser syntax)
    {
      int length = uriString.Length;
      if (length == 0)
        return ParsingError.EmptyUriString;
      if (length >= 65520)
        return ParsingError.SizeLimit;
      if (uriString.StartsWith("https:", StringComparison.OrdinalIgnoreCase))
      {
        syntax = UriParser.HttpsUri;
        flags |= Uri.Flags.UserNotCanonical | Uri.Flags.HostNotCanonical;
      }
      else if (uriString.StartsWith("http:", StringComparison.OrdinalIgnoreCase))
      {
        syntax = UriParser.HttpUri;
        flags |= Uri.Flags.SchemeNotCanonical | Uri.Flags.HostNotCanonical;
      }
      else
      {
        ParsingError err = ParsingError.None;
        int checkImplicitFile = Uri.ParseSchemeCheckImplicitFile(uriString, ref err, ref flags, ref syntax);
        if (err != ParsingError.None)
          return err;
        flags |= (Uri.Flags) checkImplicitFile;
      }
      return ParsingError.None;
    }

    internal UriFormatException ParseMinimal()
    {
      ParsingError minimal = this.PrivateParseMinimal();
      if (minimal == ParsingError.None)
        return (UriFormatException) null;
      this._flags |= Uri.Flags.ErrorOrParsingRecursion;
      return Uri.GetException(minimal);
    }

    private unsafe ParsingError PrivateParseMinimal()
    {
      int index1 = (int) (this._flags & Uri.Flags.IndexMask);
      int length = this._string.Length;
      string newHost = (string) null;
      this._flags &= ~(Uri.Flags.IndexMask | Uri.Flags.UserDrivenParsing);
      string str = (this._flags & Uri.Flags.HostUnicodeNormalized) == Uri.Flags.Zero ? this.OriginalString : this._string;
      IntPtr num1;
      if (str == null)
      {
        num1 = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &str.GetPinnableReference())
          num1 = (IntPtr) chPtr;
      }
      char* pString = (char*) num1;
      if (length > index1 && UriHelper.IsLWS(pString[length - 1]))
      {
        int num2 = length - 1;
        do
          ;
        while (num2 != index1 && UriHelper.IsLWS(pString[--num2]));
        length = num2 + 1;
      }
      if (!OperatingSystem.IsWindows() && this.InFact(Uri.Flags.UnixPath))
      {
        this._flags |= Uri.Flags.BasicHostType;
        this._flags |= (Uri.Flags) index1;
        return ParsingError.None;
      }
      if (this._syntax.IsAllSet(UriSyntaxFlags.AllowEmptyHost | UriSyntaxFlags.AllowDOSPath) && this.NotAny(Uri.Flags.ImplicitFile) && index1 + 1 < length)
      {
        int index2 = index1;
        char ch1;
        while (index2 < length && ((ch1 = pString[index2]) == '\\' || ch1 == '/'))
          ++index2;
        if (this._syntax.InFact(UriSyntaxFlags.FileLikeUri) || index2 - index1 <= 3)
        {
          if (index2 - index1 >= 2)
            this._flags |= Uri.Flags.AuthorityFound;
          char ch2;
          if (index2 + 1 < length && ((ch2 = pString[index2 + 1]) == ':' || ch2 == '|') && char.IsAsciiLetter(pString[index2]))
          {
            char ch3;
            if (index2 + 2 >= length || (ch3 = pString[index2 + 2]) != '\\' && ch3 != '/')
            {
              if (this._syntax.InFact(UriSyntaxFlags.FileLikeUri))
                return ParsingError.MustRootedPath;
            }
            else
            {
              this._flags |= Uri.Flags.DosPath;
              if (this._syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
                this._flags |= Uri.Flags.AuthorityFound;
              index1 = index2 == index1 || index2 - index1 == 2 ? index2 : index2 - 1;
            }
          }
          else if (this._syntax.InFact(UriSyntaxFlags.FileLikeUri) && index2 - index1 >= 2 && index2 - index1 != 3 && index2 < length && pString[index2] != '?' && pString[index2] != '#')
          {
            this._flags |= Uri.Flags.UncPath;
            index1 = index2;
          }
          else if (!OperatingSystem.IsWindows() && this._syntax.InFact(UriSyntaxFlags.FileLikeUri) && pString[index2 - 1] == '/' && index2 - index1 == 3)
          {
            this._syntax = UriParser.UnixFileUri;
            this._flags |= Uri.Flags.AuthorityFound | Uri.Flags.UnixPath;
            index1 += 2;
          }
        }
      }
      if ((this._flags & (Uri.Flags.DosPath | Uri.Flags.UncPath | Uri.Flags.UnixPath)) == Uri.Flags.Zero)
      {
        if (index1 + 2 <= length)
        {
          char ch4 = pString[index1];
          char ch5 = pString[index1 + 1];
          if (this._syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
          {
            if (ch4 != '/' && ch4 != '\\' || ch5 != '/' && ch5 != '\\')
              return ParsingError.BadAuthority;
            this._flags |= Uri.Flags.AuthorityFound;
            index1 += 2;
          }
          else if (this._syntax.InFact(UriSyntaxFlags.OptionalAuthority) && (this.InFact(Uri.Flags.AuthorityFound) || ch4 == '/' && ch5 == '/'))
          {
            this._flags |= Uri.Flags.AuthorityFound;
            index1 += 2;
          }
          else if (this._syntax.NotAny(UriSyntaxFlags.MailToLikeUri))
          {
            if ((this._flags & (Uri.Flags.HasUnicode | Uri.Flags.HostUnicodeNormalized)) == Uri.Flags.HasUnicode)
              this._string = this._string.Substring(0, index1);
            this._flags |= (Uri.Flags) ((long) index1 | 458752L);
            return ParsingError.None;
          }
        }
        else
        {
          if (this._syntax.InFact(UriSyntaxFlags.MustHaveAuthority))
            return ParsingError.BadAuthority;
          if (this._syntax.NotAny(UriSyntaxFlags.MailToLikeUri))
          {
            if ((this._flags & (Uri.Flags.HasUnicode | Uri.Flags.HostUnicodeNormalized)) == Uri.Flags.HasUnicode)
              this._string = this._string.Substring(0, index1);
            this._flags |= (Uri.Flags) ((long) index1 | 458752L);
            return ParsingError.None;
          }
        }
      }
      if (this.InFact(Uri.Flags.DosPath))
      {
        this._flags |= (this._flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero ? Uri.Flags.BasicHostType : Uri.Flags.HostTypeMask;
        this._flags |= (Uri.Flags) index1;
        return ParsingError.None;
      }
      ParsingError err = ParsingError.None;
      int index3 = this.CheckAuthorityHelper(pString, index1, length, ref err, ref this._flags, this._syntax, ref newHost);
      if (err != ParsingError.None)
        return err;
      if (index3 < length)
      {
        char ch = pString[index3];
        if (ch == '\\' && this.NotAny(Uri.Flags.ImplicitFile) && this._syntax.NotAny(UriSyntaxFlags.AllowDOSPath))
          return ParsingError.BadAuthorityTerminator;
        if (!OperatingSystem.IsWindows() && ch == '/' && this.NotAny(Uri.Flags.ImplicitFile) && this.InFact(Uri.Flags.UncPath) && this._syntax == UriParser.FileUri)
          this._syntax = UriParser.UnixFileUri;
      }
      this._flags |= (Uri.Flags) index3;
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(chPtr);
      if (this.IriParsing && newHost != null)
        this._string = newHost;
      return ParsingError.None;
    }

    private unsafe void CreateUriInfo(Uri.Flags cF)
    {
      Uri.UriInfo uriInfo = new Uri.UriInfo();
      uriInfo.Offset.End = (ushort) this._string.Length;
      if (!this.UserDrivenParsing)
      {
        bool flag1 = false;
        int index1;
        if ((cF & Uri.Flags.ImplicitFile) != Uri.Flags.Zero)
        {
          index1 = 0;
          while (UriHelper.IsLWS(this._string[index1]))
          {
            ++index1;
            ++uriInfo.Offset.Scheme;
          }
          if (Uri.StaticInFact(cF, Uri.Flags.UncPath))
          {
            index1 += 2;
            int num = (int) (cF & Uri.Flags.IndexMask);
            while (index1 < num && (this._string[index1] == '/' || this._string[index1] == '\\'))
              ++index1;
          }
        }
        else
        {
          index1 = this._syntax.SchemeName.Length;
          while (this._string[index1++] != ':')
            ++uriInfo.Offset.Scheme;
          if ((cF & Uri.Flags.AuthorityFound) != Uri.Flags.Zero)
          {
            if (this._string[index1] == '\\' || this._string[index1 + 1] == '\\')
              flag1 = true;
            index1 += 2;
            if ((cF & (Uri.Flags.DosPath | Uri.Flags.UncPath)) != Uri.Flags.Zero)
            {
              for (int index2 = (int) (cF & Uri.Flags.IndexMask); index1 < index2 && (this._string[index1] == '/' || this._string[index1] == '\\'); ++index1)
                flag1 = true;
            }
          }
        }
        if (this._syntax.DefaultPort != -1)
          uriInfo.Offset.PortValue = (ushort) this._syntax.DefaultPort;
        if ((cF & Uri.Flags.HostTypeMask) == Uri.Flags.HostTypeMask || Uri.StaticInFact(cF, Uri.Flags.DosPath))
        {
          uriInfo.Offset.User = (ushort) (cF & Uri.Flags.IndexMask);
          uriInfo.Offset.Host = uriInfo.Offset.User;
          uriInfo.Offset.Path = uriInfo.Offset.User;
          cF &= ~Uri.Flags.IndexMask;
          if (flag1)
            cF |= Uri.Flags.SchemeNotCanonical;
        }
        else
        {
          uriInfo.Offset.User = (ushort) index1;
          if (this.HostType == Uri.Flags.BasicHostType)
          {
            uriInfo.Offset.Host = (ushort) index1;
            uriInfo.Offset.Path = (ushort) (cF & Uri.Flags.IndexMask);
            cF &= ~Uri.Flags.IndexMask;
          }
          else
          {
            if ((cF & Uri.Flags.HasUserInfo) != Uri.Flags.Zero)
            {
              while (this._string[index1] != '@')
                ++index1;
              int num = index1 + 1;
              uriInfo.Offset.Host = (ushort) num;
            }
            else
              uriInfo.Offset.Host = (ushort) index1;
            int index3 = (int) (cF & Uri.Flags.IndexMask);
            cF &= ~Uri.Flags.IndexMask;
            if (flag1)
              cF |= Uri.Flags.SchemeNotCanonical;
            uriInfo.Offset.Path = (ushort) index3;
            bool flag2 = false;
            if ((cF & Uri.Flags.HasUnicode) != Uri.Flags.Zero)
              uriInfo.Offset.End = (ushort) this._originalUnicodeString.Length;
            if (index3 < (int) uriInfo.Offset.End)
            {
              string originalString = this.OriginalString;
              IntPtr num1;
              if (originalString == null)
              {
                num1 = IntPtr.Zero;
              }
              else
              {
                fixed (char* chPtr = &originalString.GetPinnableReference())
                  num1 = (IntPtr) chPtr;
              }
              char* chPtr1 = (char*) num1;
              if (chPtr1[index3] == ':')
              {
                int num2 = 0;
                int index4;
                if ((index4 = index3 + 1) < (int) uriInfo.Offset.End)
                {
                  num2 = (int) chPtr1[index4] - 48;
                  switch (num2)
                  {
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                      flag2 = true;
                      if (num2 == 0)
                        cF |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
                      for (++index4; index4 < (int) uriInfo.Offset.End; ++index4)
                      {
                        int num3 = (int) chPtr1[index4] - 48;
                        switch (num3)
                        {
                          case 0:
                          case 1:
                          case 2:
                          case 3:
                          case 4:
                          case 5:
                          case 6:
                          case 7:
                          case 8:
                          case 9:
                            num2 = num2 * 10 + num3;
                            continue;
                          default:
                            goto label_49;
                        }
                      }
                      break;
                  }
                }
label_49:
                if (flag2 && this._syntax.DefaultPort != num2)
                {
                  uriInfo.Offset.PortValue = (ushort) num2;
                  cF |= Uri.Flags.NotDefaultPort;
                }
                else
                  cF |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
                uriInfo.Offset.Path = (ushort) index4;
              }
              // ISSUE: fixed variable is out of scope
              // ISSUE: __unpin statement
              __unpin(chPtr);
            }
          }
        }
      }
      cF |= Uri.Flags.MinimalUriInfoSet;
      Interlocked.CompareExchange<Uri.UriInfo>(ref this._info, uriInfo, (Uri.UriInfo) null);
      ulong num4;
      for (Uri.Flags comparand = this._flags; (comparand & Uri.Flags.MinimalUriInfoSet) == Uri.Flags.Zero; comparand = (Uri.Flags) num4)
      {
        Uri.Flags flags = comparand & ~Uri.Flags.IndexMask | cF;
        num4 = Interlocked.CompareExchange(ref Unsafe.As<Uri.Flags, ulong>(ref this._flags), (ulong) flags, (ulong) comparand);
        if ((Uri.Flags) num4 == comparand)
          break;
      }
    }

    private unsafe void CreateHostString()
    {
      if (!this._syntax.IsSimple)
      {
        lock (this._info)
        {
          if (this.NotAny(Uri.Flags.ErrorOrParsingRecursion))
          {
            this._flags |= Uri.Flags.ErrorOrParsingRecursion;
            this.GetHostViaCustomSyntax();
            this._flags &= ~Uri.Flags.ErrorOrParsingRecursion;
            return;
          }
        }
      }
      Uri.Flags flags = this._flags;
      string stringToEscape = Uri.CreateHostStringHelper(this._string, (int) this._info.Offset.Host, (int) this._info.Offset.Path, ref flags, ref this._info.ScopeId);
      if (stringToEscape.Length != 0)
      {
        if (this.HostType == Uri.Flags.BasicHostType)
        {
          int idx = 0;
          IntPtr str;
          if (stringToEscape == null)
          {
            str = IntPtr.Zero;
          }
          else
          {
            fixed (char* chPtr = &stringToEscape.GetPinnableReference())
              str = (IntPtr) chPtr;
          }
          Uri.Check check = this.CheckCanonical((char*) str, ref idx, stringToEscape.Length, char.MaxValue);
          // ISSUE: fixed variable is out of scope
          // ISSUE: __unpin statement
          __unpin(chPtr);
          if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None && (this.NotAny(Uri.Flags.ImplicitFile) || (check & Uri.Check.ReservedFound) != Uri.Check.None))
            flags |= Uri.Flags.HostNotCanonical;
          if (this.InFact(Uri.Flags.ImplicitFile) && (check & (Uri.Check.EscapedCanonical | Uri.Check.ReservedFound)) != Uri.Check.None)
            check &= ~Uri.Check.EscapedCanonical;
          if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
          {
            flags |= Uri.Flags.E_HostNotCanonical;
            if (this.NotAny(Uri.Flags.UserEscaped))
              stringToEscape = UriHelper.EscapeString(stringToEscape, !this.IsImplicitFile, UriHelper.UnreservedReservedExceptQuestionMarkHash);
          }
        }
        else if (this.NotAny(Uri.Flags.CanonicalDnsHost))
        {
          if (this._info.ScopeId != null)
          {
            flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
          }
          else
          {
            for (int index = 0; index < stringToEscape.Length; ++index)
            {
              if ((int) this._info.Offset.Host + index >= (int) this._info.Offset.End || (int) stringToEscape[index] != (int) this._string[(int) this._info.Offset.Host + index])
              {
                flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
                break;
              }
            }
          }
        }
      }
      this._info.Host = stringToEscape;
      this.InterlockedSetFlags(flags);
    }

    private static string CreateHostStringHelper(
      string str,
      int idx,
      int end,
      ref Uri.Flags flags,
      ref string scopeId)
    {
      bool flag = false;
      string hostStringHelper;
      switch (flags & Uri.Flags.HostTypeMask)
      {
        case Uri.Flags.IPv6HostType:
          hostStringHelper = IPv6AddressHelper.ParseCanonicalName(str, idx, ref flag, ref scopeId);
          break;
        case Uri.Flags.IPv4HostType:
          hostStringHelper = IPv4AddressHelper.ParseCanonicalName(str, idx, end, ref flag);
          break;
        case Uri.Flags.DnsHostType:
          hostStringHelper = DomainNameHelper.ParseCanonicalName(str, idx, end, ref flag);
          break;
        case Uri.Flags.UncHostType:
          hostStringHelper = UncNameHelper.ParseCanonicalName(str, idx, end, ref flag);
          break;
        case Uri.Flags.BasicHostType:
          hostStringHelper = !Uri.StaticInFact(flags, Uri.Flags.DosPath) ? str.Substring(idx, end - idx) : string.Empty;
          if (hostStringHelper.Length == 0)
          {
            flag = true;
            break;
          }
          break;
        case Uri.Flags.HostTypeMask:
          hostStringHelper = string.Empty;
          break;
        default:
          throw Uri.GetException(ParsingError.BadHostName);
      }
      if (flag)
        flags |= Uri.Flags.LoopbackHost;
      return hostStringHelper;
    }

    private unsafe void GetHostViaCustomSyntax()
    {
      if (this._info.Host != null)
        return;
      string str = this._syntax.InternalGetComponents(this, UriComponents.Host, UriFormat.UriEscaped);
      if (this._info.Host == null)
      {
        if (str.Length >= 65520)
          throw Uri.GetException(ParsingError.SizeLimit);
        ParsingError err = ParsingError.None;
        Uri.Flags flags = this._flags & ~Uri.Flags.HostTypeMask;
        IntPtr num;
        if (str == null)
        {
          num = IntPtr.Zero;
        }
        else
        {
          fixed (char* chPtr = &str.GetPinnableReference())
            num = (IntPtr) chPtr;
        }
        char* pString = (char*) num;
        string newHost = (string) null;
        if (this.CheckAuthorityHelper(pString, 0, str.Length, ref err, ref flags, this._syntax, ref newHost) != str.Length)
          flags = flags & ~Uri.Flags.HostTypeMask | Uri.Flags.HostTypeMask;
        // ISSUE: fixed variable is out of scope
        // ISSUE: __unpin statement
        __unpin(chPtr);
        if (err != ParsingError.None || (flags & Uri.Flags.HostTypeMask) == Uri.Flags.HostTypeMask)
        {
          this._flags = this._flags & ~Uri.Flags.HostTypeMask | Uri.Flags.BasicHostType;
        }
        else
        {
          str = Uri.CreateHostStringHelper(str, 0, str.Length, ref flags, ref this._info.ScopeId);
          for (int index = 0; index < str.Length; ++index)
          {
            if ((int) this._info.Offset.Host + index >= (int) this._info.Offset.End || (int) str[index] != (int) this._string[(int) this._info.Offset.Host + index])
            {
              this._flags |= Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical;
              break;
            }
          }
          this._flags = this._flags & ~Uri.Flags.HostTypeMask | flags & Uri.Flags.HostTypeMask;
        }
      }
      string components = this._syntax.InternalGetComponents(this, UriComponents.StrongPort, UriFormat.UriEscaped);
      int num1 = 0;
      if (string.IsNullOrEmpty(components))
      {
        this._flags &= ~Uri.Flags.NotDefaultPort;
        this._flags |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
        this._info.Offset.PortValue = (ushort) 0;
      }
      else
      {
        for (int index = 0; index < components.Length; ++index)
        {
          int num2 = (int) components[index] - 48;
          switch (num2)
          {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
              if ((num1 = num1 * 10 + num2) <= (int) ushort.MaxValue)
                continue;
              break;
          }
          throw new UriFormatException(SR.Format(SR.net_uri_PortOutOfRange, (object) this._syntax.GetType(), (object) components));
        }
        if (num1 != (int) this._info.Offset.PortValue)
        {
          if (num1 == this._syntax.DefaultPort)
            this._flags &= ~Uri.Flags.NotDefaultPort;
          else
            this._flags |= Uri.Flags.NotDefaultPort;
          this._flags |= Uri.Flags.PortNotCanonical | Uri.Flags.E_PortNotCanonical;
          this._info.Offset.PortValue = (ushort) num1;
        }
      }
      this._info.Host = str;
    }

    internal string GetParts(UriComponents uriParts, UriFormat formatAs)
    {
      return this.InternalGetComponents(uriParts, formatAs);
    }

    private string GetEscapedParts(UriComponents uriParts)
    {
      ushort nonCanonical = (ushort) (((int) (ushort) this._flags & 16256) >> 6);
      if (this.InFact(Uri.Flags.SchemeNotCanonical))
        nonCanonical |= (ushort) 1;
      if ((uriParts & UriComponents.Path) != (UriComponents) 0)
      {
        if (this.InFact(Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath))
          nonCanonical |= (ushort) 16;
        else if (this.IsDosPath && this._string[(int) this._info.Offset.Path + this.SecuredPathIndex - 1] == '|')
          nonCanonical |= (ushort) 16;
      }
      if (((int) (ushort) uriParts & (int) nonCanonical) == 0)
      {
        string partsFromUserString = this.GetUriPartsFromUserString(uriParts);
        if (partsFromUserString != null)
          return partsFromUserString;
      }
      return this.RecreateParts(uriParts, nonCanonical, UriFormat.UriEscaped);
    }

    private string GetUnescapedParts(UriComponents uriParts, UriFormat formatAs)
    {
      ushort nonCanonical = (ushort) ((uint) (ushort) this._flags & (uint) sbyte.MaxValue);
      if ((uriParts & UriComponents.Path) != (UriComponents) 0)
      {
        if ((this._flags & (Uri.Flags.ShouldBeCompressed | Uri.Flags.FirstSlashAbsent | Uri.Flags.BackslashInPath)) != Uri.Flags.Zero)
          nonCanonical |= (ushort) 16;
        else if (this.IsDosPath && this._string[(int) this._info.Offset.Path + this.SecuredPathIndex - 1] == '|')
          nonCanonical |= (ushort) 16;
      }
      if (((int) (ushort) uriParts & (int) nonCanonical) == 0)
      {
        string partsFromUserString = this.GetUriPartsFromUserString(uriParts);
        if (partsFromUserString != null)
          return partsFromUserString;
      }
      return this.RecreateParts(uriParts, nonCanonical, formatAs);
    }

    private string RecreateParts(UriComponents parts, ushort nonCanonical, UriFormat formatAs)
    {
      this.EnsureHostString(false);
      string str1 = this._string;
      ValueStringBuilder dest = str1.Length > 512 ? new ValueStringBuilder(str1.Length) : new ValueStringBuilder(stackalloc char[512]);
      string str2 = this.RecreateParts(ref dest, str1, parts, nonCanonical, formatAs).ToString();
      dest.Dispose();
      return str2;
    }

    private bool TryRecreateParts(
      [ScopedRef] Span<char> span,
      out int charsWritten,
      UriComponents parts,
      ushort nonCanonical,
      UriFormat formatAs)
    {
      this.EnsureHostString(false);
      string str = this._string;
      ValueStringBuilder dest = str.Length > 512 ? new ValueStringBuilder(str.Length) : new ValueStringBuilder(stackalloc char[512]);
      ReadOnlySpan<char> readOnlySpan = this.RecreateParts(ref dest, str, parts, nonCanonical, formatAs);
      bool flag = readOnlySpan.TryCopyTo(span);
      charsWritten = flag ? readOnlySpan.Length : 0;
      dest.Dispose();
      return flag;
    }

    private ReadOnlySpan<char> RecreateParts(
      [ScopedRef] ref ValueStringBuilder dest,
      string str,
      UriComponents parts,
      ushort nonCanonical,
      UriFormat formatAs)
    {
      if ((parts & UriComponents.Scheme) != (UriComponents) 0)
      {
        dest.Append(this._syntax.SchemeName);
        if (parts != UriComponents.Scheme)
        {
          dest.Append(':');
          if (this.InFact(Uri.Flags.AuthorityFound))
          {
            dest.Append('/');
            dest.Append('/');
          }
        }
      }
      if ((parts & UriComponents.UserInfo) != (UriComponents) 0 && this.InFact(Uri.Flags.HasUserInfo))
      {
        ReadOnlySpan<char> readOnlySpan = str.AsSpan((int) this._info.Offset.User, (int) this._info.Offset.Host - (int) this._info.Offset.User);
        if (((int) nonCanonical & 2) != 0)
        {
          switch (formatAs)
          {
            case UriFormat.UriEscaped:
              if (this.NotAny(Uri.Flags.UserEscaped))
              {
                UriHelper.EscapeString(readOnlySpan, ref dest, true, UriHelper.UnreservedReservedExceptQuestionMarkHash);
                break;
              }
              dest.Append(readOnlySpan);
              break;
            case UriFormat.Unescaped:
              UriHelper.UnescapeString(readOnlySpan, ref dest, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.Unescape | UnescapeMode.UnescapeAll, this._syntax, false);
              break;
            case UriFormat.SafeUnescaped:
              ref ReadOnlySpan<char> local = ref readOnlySpan;
              UriHelper.UnescapeString(local.Slice(0, local.Length - 1), ref dest, '@', '/', '\\', this.InFact(Uri.Flags.UserEscaped) ? UnescapeMode.Unescape : UnescapeMode.EscapeUnescape, this._syntax, false);
              dest.Append('@');
              break;
            default:
              dest.Append(readOnlySpan);
              break;
          }
        }
        else
          dest.Append(readOnlySpan);
        if (parts == UriComponents.UserInfo)
          --dest.Length;
      }
      if ((parts & UriComponents.Host) != (UriComponents) 0)
      {
        string str1 = this._info.Host;
        if (str1.Length != 0)
        {
          UnescapeMode unescapeMode = formatAs == UriFormat.UriEscaped || this.HostType != Uri.Flags.BasicHostType || ((int) nonCanonical & 4) == 0 ? UnescapeMode.CopyOnly : (formatAs == UriFormat.Unescaped ? UnescapeMode.Unescape | UnescapeMode.UnescapeAll : (this.InFact(Uri.Flags.UserEscaped) ? UnescapeMode.Unescape : UnescapeMode.EscapeUnescape));
          ValueStringBuilder dest1 = new ValueStringBuilder(stackalloc char[512]);
          if ((parts & UriComponents.NormalizedHost) != (UriComponents) 0)
          {
            str1 = UriHelper.StripBidiControlCharacters((ReadOnlySpan<char>) str1, str1);
            if (!DomainNameHelper.TryGetUnicodeEquivalent(str1, ref dest1))
              dest1.Length = 0;
          }
          UriHelper.UnescapeString(dest1.Length == 0 ? (ReadOnlySpan<char>) str1 : dest1.AsSpan(), ref dest, '/', '?', '#', unescapeMode, this._syntax, false);
          dest1.Dispose();
          if ((parts & UriComponents.SerializationInfoString) != (UriComponents) 0 && this.HostType == Uri.Flags.IPv6HostType && this._info.ScopeId != null)
          {
            --dest.Length;
            dest.Append(this._info.ScopeId);
            dest.Append(']');
          }
        }
      }
      if ((parts & UriComponents.Port) != (UriComponents) 0 && (this.InFact(Uri.Flags.NotDefaultPort) || (parts & UriComponents.StrongPort) != (UriComponents) 0 && this._syntax.DefaultPort != -1))
      {
        dest.Append(':');
        int charsWritten;
        this._info.Offset.PortValue.TryFormat(dest.AppendSpan(5), out charsWritten);
        dest.Length -= 5 - charsWritten;
      }
      if ((parts & UriComponents.Path) != (UriComponents) 0)
      {
        this.GetCanonicalPath(ref dest, formatAs);
        if (parts == UriComponents.Path)
        {
          int start = !this.InFact(Uri.Flags.AuthorityFound) || dest.Length == 0 || dest[0] != '/' ? 0 : 1;
          return dest.AsSpan(start);
        }
      }
      if ((parts & UriComponents.Query) != (UriComponents) 0 && (int) this._info.Offset.Query < (int) this._info.Offset.Fragment)
      {
        int start = (int) this._info.Offset.Query + 1;
        if (parts != UriComponents.Query)
          dest.Append('?');
        UnescapeMode unescapeMode1 = UnescapeMode.CopyOnly;
        if (((int) nonCanonical & 32) != 0)
        {
          UnescapeMode unescapeMode2;
          switch (formatAs)
          {
            case UriFormat.UriEscaped:
              if (this.NotAny(Uri.Flags.UserEscaped))
              {
                UriHelper.EscapeString(str.AsSpan(start, (int) this._info.Offset.Fragment - start), ref dest, true, UriHelper.UnreservedReservedExceptHash);
                goto label_40;
              }
              else
                goto label_39;
            case UriFormat.Unescaped:
              unescapeMode2 = UnescapeMode.Unescape | UnescapeMode.UnescapeAll;
              break;
            case (UriFormat) 32767:
              unescapeMode2 = (UnescapeMode) ((this.InFact(Uri.Flags.UserEscaped) ? 2 : 3) | 4);
              break;
            default:
              unescapeMode2 = this.InFact(Uri.Flags.UserEscaped) ? UnescapeMode.Unescape : UnescapeMode.EscapeUnescape;
              break;
          }
          unescapeMode1 = unescapeMode2;
        }
label_39:
        UriHelper.UnescapeString(str, start, (int) this._info.Offset.Fragment, ref dest, '#', char.MaxValue, char.MaxValue, unescapeMode1, this._syntax, true);
      }
label_40:
      if ((parts & UriComponents.Fragment) != (UriComponents) 0 && (int) this._info.Offset.Fragment < (int) this._info.Offset.End)
      {
        int start = (int) this._info.Offset.Fragment + 1;
        if (parts != UriComponents.Fragment)
          dest.Append('#');
        UnescapeMode unescapeMode3 = UnescapeMode.CopyOnly;
        if (((int) nonCanonical & 64) != 0)
        {
          UnescapeMode unescapeMode4;
          switch (formatAs)
          {
            case UriFormat.UriEscaped:
              if (this.NotAny(Uri.Flags.UserEscaped))
              {
                UriHelper.EscapeString(str.AsSpan(start, (int) this._info.Offset.End - start), ref dest, true, UriHelper.UnreservedReserved);
                goto label_52;
              }
              else
                goto label_51;
            case UriFormat.Unescaped:
              unescapeMode4 = UnescapeMode.Unescape | UnescapeMode.UnescapeAll;
              break;
            case (UriFormat) 32767:
              unescapeMode4 = (UnescapeMode) ((this.InFact(Uri.Flags.UserEscaped) ? 2 : 3) | 4);
              break;
            default:
              unescapeMode4 = this.InFact(Uri.Flags.UserEscaped) ? UnescapeMode.Unescape : UnescapeMode.EscapeUnescape;
              break;
          }
          unescapeMode3 = unescapeMode4;
        }
label_51:
        UriHelper.UnescapeString(str, start, (int) this._info.Offset.End, ref dest, '#', char.MaxValue, char.MaxValue, unescapeMode3, this._syntax, false);
      }
label_52:
      return dest.AsSpan();
    }

    private string GetUriPartsFromUserString(UriComponents uriParts)
    {
      switch (uriParts & ~UriComponents.KeepDelimiter)
      {
        case UriComponents.Scheme:
          return uriParts != UriComponents.Scheme ? this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.User - (int) this._info.Offset.Scheme) : this._syntax.SchemeName;
        case UriComponents.UserInfo:
          if (this.NotAny(Uri.Flags.HasUserInfo))
            return string.Empty;
          int num = uriParts != UriComponents.UserInfo ? (int) this._info.Offset.Host : (int) this._info.Offset.Host - 1;
          return (int) this._info.Offset.User >= num ? string.Empty : this._string.Substring((int) this._info.Offset.User, num - (int) this._info.Offset.User);
        case UriComponents.Host:
          int path = (int) this._info.Offset.Path;
          if (this.InFact(Uri.Flags.PortNotCanonical | Uri.Flags.NotDefaultPort))
          {
            while (this._string[--path] != ':')
              ;
          }
          return path - (int) this._info.Offset.Host != 0 ? this._string.Substring((int) this._info.Offset.Host, path - (int) this._info.Offset.Host) : string.Empty;
        case UriComponents.SchemeAndServer:
          return !this.InFact(Uri.Flags.HasUserInfo) ? this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.Path - (int) this._info.Offset.Scheme) : this._string.AsSpan((int) this._info.Offset.Scheme, (int) this._info.Offset.User - (int) this._info.Offset.Scheme).ToString() + this._string.AsSpan((int) this._info.Offset.Host, (int) this._info.Offset.Path - (int) this._info.Offset.Host);
        case UriComponents.UserInfo | UriComponents.Host | UriComponents.Port:
          return (int) this._info.Offset.Path - (int) this._info.Offset.User != 0 ? this._string.Substring((int) this._info.Offset.User, (int) this._info.Offset.Path - (int) this._info.Offset.User) : string.Empty;
        case UriComponents.SchemeAndServer | UriComponents.UserInfo:
          return this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.Path - (int) this._info.Offset.Scheme);
        case UriComponents.Path:
          int startIndex1 = uriParts != UriComponents.Path || !this.InFact(Uri.Flags.AuthorityFound) || (int) this._info.Offset.End <= (int) this._info.Offset.Path || this._string[(int) this._info.Offset.Path] != '/' ? (int) this._info.Offset.Path : (int) this._info.Offset.Path + 1;
          return startIndex1 >= (int) this._info.Offset.Query ? string.Empty : this._string.Substring(startIndex1, (int) this._info.Offset.Query - startIndex1);
        case UriComponents.Query:
          int startIndex2 = uriParts != UriComponents.Query ? (int) this._info.Offset.Query : (int) this._info.Offset.Query + 1;
          return startIndex2 >= (int) this._info.Offset.Fragment ? string.Empty : this._string.Substring(startIndex2, (int) this._info.Offset.Fragment - startIndex2);
        case UriComponents.PathAndQuery:
          return this._string.Substring((int) this._info.Offset.Path, (int) this._info.Offset.Fragment - (int) this._info.Offset.Path);
        case UriComponents.HttpRequestUrl:
          if (this.InFact(Uri.Flags.HasUserInfo))
            return this._string.AsSpan((int) this._info.Offset.Scheme, (int) this._info.Offset.User - (int) this._info.Offset.Scheme).ToString() + this._string.AsSpan((int) this._info.Offset.Host, (int) this._info.Offset.Fragment - (int) this._info.Offset.Host);
          return this._info.Offset.Scheme == (ushort) 0 && (int) this._info.Offset.Fragment == this._string.Length ? this._string : this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.Fragment - (int) this._info.Offset.Scheme);
        case UriComponents.HttpRequestUrl | UriComponents.UserInfo:
          return this._info.Offset.Scheme == (ushort) 0 && (int) this._info.Offset.Fragment == this._string.Length ? this._string : this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.Fragment - (int) this._info.Offset.Scheme);
        case UriComponents.Fragment:
          int startIndex3 = uriParts != UriComponents.Fragment ? (int) this._info.Offset.Fragment : (int) this._info.Offset.Fragment + 1;
          return startIndex3 >= (int) this._info.Offset.End ? string.Empty : this._string.Substring(startIndex3, (int) this._info.Offset.End - startIndex3);
        case UriComponents.PathAndQuery | UriComponents.Fragment:
          return this._string.Substring((int) this._info.Offset.Path, (int) this._info.Offset.End - (int) this._info.Offset.Path);
        case UriComponents.HttpRequestUrl | UriComponents.Fragment:
          if (this.InFact(Uri.Flags.HasUserInfo))
            return this._string.AsSpan((int) this._info.Offset.Scheme, (int) this._info.Offset.User - (int) this._info.Offset.Scheme).ToString() + this._string.AsSpan((int) this._info.Offset.Host, (int) this._info.Offset.End - (int) this._info.Offset.Host);
          return this._info.Offset.Scheme == (ushort) 0 && (int) this._info.Offset.End == this._string.Length ? this._string : this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.End - (int) this._info.Offset.Scheme);
        case UriComponents.AbsoluteUri:
          return this._info.Offset.Scheme == (ushort) 0 && (int) this._info.Offset.End == this._string.Length ? this._string : this._string.Substring((int) this._info.Offset.Scheme, (int) this._info.Offset.End - (int) this._info.Offset.Scheme);
        case UriComponents.HostAndPort:
          if (this.InFact(Uri.Flags.HasUserInfo))
            return this.InFact(Uri.Flags.NotDefaultPort) || this._syntax.DefaultPort == -1 ? this._string.Substring((int) this._info.Offset.Host, (int) this._info.Offset.Path - (int) this._info.Offset.Host) : this._string.AsSpan((int) this._info.Offset.Host, (int) this._info.Offset.Path - (int) this._info.Offset.Host).ToString() + (ReadOnlySpan<char>) ":" + (ReadOnlySpan<char>) this._info.Offset.PortValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          goto case UriComponents.StrongAuthority;
        case UriComponents.StrongAuthority:
          if (!this.InFact(Uri.Flags.NotDefaultPort) && this._syntax.DefaultPort != -1)
            return this._string.AsSpan((int) this._info.Offset.User, (int) this._info.Offset.Path - (int) this._info.Offset.User).ToString() + (ReadOnlySpan<char>) ":" + (ReadOnlySpan<char>) this._info.Offset.PortValue.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          goto case UriComponents.UserInfo | UriComponents.Host | UriComponents.Port;
        default:
          return (string) null;
      }
    }

    private static void GetLengthWithoutTrailingSpaces(string str, ref int length, int idx)
    {
      int num = length;
      while (num > idx && UriHelper.IsLWS(str[num - 1]))
        --num;
      length = num;
    }

    private unsafe void ParseRemaining()
    {
      this.EnsureUriInfo();
      Uri.Flags flags1 = Uri.Flags.Zero;
      if (!this.UserDrivenParsing)
      {
        bool flag1 = (this._flags & (Uri.Flags.HasUnicode | Uri.Flags.RestUnicodeNormalized)) == Uri.Flags.HasUnicode;
        int scheme = (int) this._info.Offset.Scheme;
        int length1 = this._string.Length;
        UriSyntaxFlags flags2 = this._syntax.Flags;
        string str1 = this._string;
        IntPtr num1;
        if (str1 == null)
        {
          num1 = IntPtr.Zero;
        }
        else
        {
          fixed (char* chPtr = &str1.GetPinnableReference())
            num1 = (IntPtr) chPtr;
        }
        char* str2 = (char*) num1;
        Uri.GetLengthWithoutTrailingSpaces(this._string, ref length1, scheme);
        if (this.IsImplicitFile)
        {
          flags1 |= Uri.Flags.SchemeNotCanonical;
        }
        else
        {
          string schemeName = this._syntax.SchemeName;
          int index;
          for (index = 0; index < schemeName.Length; ++index)
          {
            if ((int) schemeName[index] != (int) str2[scheme + index])
              flags1 |= Uri.Flags.SchemeNotCanonical;
          }
          if ((this._flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && (scheme + index + 3 >= length1 || str2[scheme + index + 1] != '/' || str2[scheme + index + 2] != '/'))
            flags1 |= Uri.Flags.SchemeNotCanonical;
        }
        if ((this._flags & Uri.Flags.HasUserInfo) != Uri.Flags.Zero)
        {
          int user = (int) this._info.Offset.User;
          Uri.Check check = this.CheckCanonical(str2, ref user, (int) this._info.Offset.Host, '@');
          if ((check & Uri.Check.DisplayCanonical) == Uri.Check.None)
            flags1 |= Uri.Flags.UserNotCanonical;
          if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
            flags1 |= Uri.Flags.E_UserNotCanonical;
          if (this.IriParsing && (check & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
            flags1 |= Uri.Flags.UserIriCanonical;
        }
        // ISSUE: fixed variable is out of scope
        // ISSUE: __unpin statement
        __unpin(chPtr);
        int path = (int) this._info.Offset.Path;
        int num2 = (int) this._info.Offset.Path;
        if (flag1)
        {
          if (this.IsFile && !this.IsUncPath)
            this._string = !this.IsImplicitFile ? this._syntax.SchemeName + Uri.SchemeDelimiter : string.Empty;
          this._info.Offset.Path = (ushort) this._string.Length;
          path = (int) this._info.Offset.Path;
        }
        if (this.DisablePathAndQueryCanonicalization)
        {
          if (flag1)
            this._string = ((ReadOnlySpan<char>) this._string).ToString() + this._originalUnicodeString.AsSpan(num2);
          string str3 = this._string;
          int num3;
          if (this.IsImplicitFile || (flags2 & UriSyntaxFlags.MayHaveQuery) == UriSyntaxFlags.None)
          {
            num3 = str3.Length;
          }
          else
          {
            num3 = str3.IndexOf('?');
            if (num3 == -1)
              num3 = str3.Length;
          }
          this._info.Offset.Query = (ushort) num3;
          this._info.Offset.Fragment = (ushort) str3.Length;
          this._info.Offset.End = (ushort) str3.Length;
        }
        else
        {
          if (flag1)
          {
            int start = num2;
            if (this.IsImplicitFile || (flags2 & (UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment)) == UriSyntaxFlags.None)
            {
              num2 = this._originalUnicodeString.Length;
            }
            else
            {
              ReadOnlySpan<char> span = this._originalUnicodeString.AsSpan(num2);
              int num4 = !this._syntax.InFact(UriSyntaxFlags.MayHaveQuery) ? span.IndexOf<char>('#') : (!this._syntax.InFact(UriSyntaxFlags.MayHaveFragment) ? span.IndexOf<char>('?') : span.IndexOfAny<char>('?', '#'));
              num2 = num4 == -1 ? this._originalUnicodeString.Length : num4 + num2;
            }
            this._string += this.EscapeUnescapeIri(this._originalUnicodeString, start, num2, UriComponents.Path);
            length1 = this._string.Length <= (int) ushort.MaxValue ? this._string.Length : throw Uri.GetException(ParsingError.SizeLimit);
            if (this._string == this._originalUnicodeString)
              Uri.GetLengthWithoutTrailingSpaces(this._string, ref length1, path);
          }
          string str4 = this._string;
          IntPtr num5;
          if (str4 == null)
          {
            num5 = IntPtr.Zero;
          }
          else
          {
            fixed (char* chPtr = &str4.GetPinnableReference())
              num5 = (IntPtr) chPtr;
          }
          char* str5 = (char*) num5;
          Uri.Check check1 = this.IsImplicitFile || (flags2 & (UriSyntaxFlags.MayHaveQuery | UriSyntaxFlags.MayHaveFragment)) == UriSyntaxFlags.None ? this.CheckCanonical(str5, ref path, length1, char.MaxValue) : this.CheckCanonical(str5, ref path, length1, (flags2 & UriSyntaxFlags.MayHaveQuery) != UriSyntaxFlags.None ? '?' : (this._syntax.InFact(UriSyntaxFlags.MayHaveFragment) ? '#' : '\uFFFE'));
          if ((this._flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && (flags2 & UriSyntaxFlags.PathIsRooted) != UriSyntaxFlags.None && ((int) this._info.Offset.Path == length1 || str5[this._info.Offset.Path] != '/' && str5[this._info.Offset.Path] != '\\'))
            flags1 |= Uri.Flags.FirstSlashAbsent;
          // ISSUE: fixed variable is out of scope
          // ISSUE: __unpin statement
          __unpin(chPtr);
          bool flag2 = false;
          if (this.IsDosPath || (this._flags & Uri.Flags.AuthorityFound) != Uri.Flags.Zero && ((flags2 & (UriSyntaxFlags.ConvertPathSlashes | UriSyntaxFlags.CompressPath)) != UriSyntaxFlags.None || this._syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes)))
          {
            if ((check1 & Uri.Check.DotSlashEscaped) != Uri.Check.None && this._syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes))
            {
              flags1 |= Uri.Flags.PathNotCanonical | Uri.Flags.E_PathNotCanonical;
              flag2 = true;
            }
            if ((flags2 & UriSyntaxFlags.ConvertPathSlashes) != UriSyntaxFlags.None && (check1 & Uri.Check.BackslashInPath) != Uri.Check.None)
            {
              flags1 |= Uri.Flags.PathNotCanonical | Uri.Flags.E_PathNotCanonical;
              flag2 = true;
            }
            if ((flags2 & UriSyntaxFlags.CompressPath) != UriSyntaxFlags.None && ((flags1 & Uri.Flags.E_PathNotCanonical) != Uri.Flags.Zero || (check1 & Uri.Check.DotSlashAttn) != Uri.Check.None))
              flags1 |= Uri.Flags.ShouldBeCompressed;
            if ((check1 & Uri.Check.BackslashInPath) != Uri.Check.None)
              flags1 |= Uri.Flags.BackslashInPath;
          }
          else if ((check1 & Uri.Check.BackslashInPath) != Uri.Check.None)
          {
            flags1 |= Uri.Flags.E_PathNotCanonical;
            flag2 = true;
          }
          if ((check1 & Uri.Check.DisplayCanonical) == Uri.Check.None && ((this._flags & Uri.Flags.ImplicitFile) == Uri.Flags.Zero || (this._flags & Uri.Flags.UserEscaped) != Uri.Flags.Zero || (check1 & Uri.Check.ReservedFound) != Uri.Check.None))
          {
            flags1 |= Uri.Flags.PathNotCanonical;
            flag2 = true;
          }
          if ((this._flags & Uri.Flags.ImplicitFile) != Uri.Flags.Zero && (check1 & (Uri.Check.EscapedCanonical | Uri.Check.ReservedFound)) != Uri.Check.None)
            check1 &= ~Uri.Check.EscapedCanonical;
          if ((check1 & Uri.Check.EscapedCanonical) == Uri.Check.None)
            flags1 |= Uri.Flags.E_PathNotCanonical;
          if (this.IriParsing && !flag2 && (check1 & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
            flags1 |= Uri.Flags.PathIriCanonical;
          if (flag1)
          {
            int start1 = num2;
            if (num2 < this._originalUnicodeString.Length && this._originalUnicodeString[num2] == '?')
            {
              if ((flags2 & UriSyntaxFlags.MayHaveFragment) != UriSyntaxFlags.None)
              {
                int start2 = num2 + 1;
                int num6 = this._originalUnicodeString.AsSpan(start2).IndexOf<char>('#');
                num2 = num6 == -1 ? this._originalUnicodeString.Length : num6 + start2;
              }
              else
                num2 = this._originalUnicodeString.Length;
              this._string += this.EscapeUnescapeIri(this._originalUnicodeString, start1, num2, UriComponents.Query);
              length1 = this._string.Length <= (int) ushort.MaxValue ? this._string.Length : throw Uri.GetException(ParsingError.SizeLimit);
              if (this._string == this._originalUnicodeString)
                Uri.GetLengthWithoutTrailingSpaces(this._string, ref length1, path);
            }
          }
          this._info.Offset.Query = (ushort) path;
          string str6 = this._string;
          IntPtr num7;
          if (str6 == null)
          {
            num7 = IntPtr.Zero;
          }
          else
          {
            fixed (char* chPtr = &str6.GetPinnableReference())
              num7 = (IntPtr) chPtr;
          }
          char* str7 = (char*) num7;
          if (path < length1 && str7[path] == '?')
          {
            ++path;
            Uri.Check check2 = this.CheckCanonical(str7, ref path, length1, (flags2 & UriSyntaxFlags.MayHaveFragment) != UriSyntaxFlags.None ? '#' : '\uFFFE');
            if ((check2 & Uri.Check.DisplayCanonical) == Uri.Check.None)
              flags1 |= Uri.Flags.QueryNotCanonical;
            if ((check2 & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
              flags1 |= Uri.Flags.E_QueryNotCanonical;
            if (this.IriParsing && (check2 & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
              flags1 |= Uri.Flags.QueryIriCanonical;
          }
          // ISSUE: fixed variable is out of scope
          // ISSUE: __unpin statement
          __unpin(chPtr);
          if (flag1)
          {
            int start = num2;
            if (num2 < this._originalUnicodeString.Length && this._originalUnicodeString[num2] == '#')
            {
              int length2 = this._originalUnicodeString.Length;
              this._string += this.EscapeUnescapeIri(this._originalUnicodeString, start, length2, UriComponents.Fragment);
              length1 = this._string.Length <= (int) ushort.MaxValue ? this._string.Length : throw Uri.GetException(ParsingError.SizeLimit);
              Uri.GetLengthWithoutTrailingSpaces(this._string, ref length1, path);
            }
          }
          this._info.Offset.Fragment = (ushort) path;
          string str8 = this._string;
          IntPtr num8;
          if (str8 == null)
          {
            num8 = IntPtr.Zero;
          }
          else
          {
            fixed (char* chPtr = &str8.GetPinnableReference())
              num8 = (IntPtr) chPtr;
          }
          char* str9 = (char*) num8;
          if (path < length1 && str9[path] == '#')
          {
            ++path;
            Uri.Check check3 = this.CheckCanonical(str9, ref path, length1, '\uFFFE');
            if ((check3 & Uri.Check.DisplayCanonical) == Uri.Check.None)
              flags1 |= Uri.Flags.FragmentNotCanonical;
            if ((check3 & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) != Uri.Check.EscapedCanonical)
              flags1 |= Uri.Flags.E_FragmentNotCanonical;
            if (this.IriParsing && (check3 & (Uri.Check.EscapedCanonical | Uri.Check.DisplayCanonical | Uri.Check.BackslashInPath | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) == (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii))
              flags1 |= Uri.Flags.FragmentIriCanonical;
          }
          // ISSUE: fixed variable is out of scope
          // ISSUE: __unpin statement
          __unpin(chPtr);
          this._info.Offset.End = (ushort) path;
        }
      }
      this.InterlockedSetFlags(flags1 | Uri.Flags.AllUriInfoSet | Uri.Flags.RestUnicodeNormalized);
    }

    private static int ParseSchemeCheckImplicitFile(
      string uriString,
      ref ParsingError err,
      ref Uri.Flags flags,
      ref UriParser syntax)
    {
      int checkImplicitFile = 0;
      while ((uint) checkImplicitFile < (uint) uriString.Length && UriHelper.IsLWS(uriString[checkImplicitFile]))
        ++checkImplicitFile;
      bool flag1 = !OperatingSystem.IsWindows() && (uint) checkImplicitFile < (uint) uriString.Length && uriString[checkImplicitFile] == '/';
      if (flag1)
      {
        bool flag2 = (uint) (checkImplicitFile + 1) >= (uint) uriString.Length;
        if (!flag2)
        {
          bool flag3;
          switch (uriString[checkImplicitFile + 1])
          {
            case '/':
            case '\\':
              flag3 = true;
              break;
            default:
              flag3 = false;
              break;
          }
          flag2 = !flag3;
        }
        flag1 = flag2;
      }
      if (flag1)
      {
        flags |= Uri.Flags.AuthorityFound | Uri.Flags.ImplicitFile | Uri.Flags.UnixPath;
        syntax = UriParser.UnixFileUri;
        return checkImplicitFile;
      }
      int length = uriString.AsSpan(checkImplicitFile).IndexOf<char>(':');
      if ((uint) (checkImplicitFile + 2) >= (uint) uriString.Length || length == 0 || (uint) checkImplicitFile >= (uint) uriString.Length || (uint) (checkImplicitFile + 1) >= (uint) uriString.Length)
      {
        err = ParsingError.BadFormat;
        return 0;
      }
      bool flag4;
      switch (uriString[checkImplicitFile + 1])
      {
        case ':':
        case '|':
          flag4 = true;
          break;
        default:
          flag4 = false;
          break;
      }
      if (flag4)
      {
        if (char.IsAsciiLetter(uriString[checkImplicitFile]))
        {
          bool flag5;
          switch (uriString[checkImplicitFile + 2])
          {
            case '/':
            case '\\':
              flag5 = true;
              break;
            default:
              flag5 = false;
              break;
          }
          if (flag5)
          {
            flags |= Uri.Flags.AuthorityFound | Uri.Flags.DosPath | Uri.Flags.ImplicitFile;
            syntax = UriParser.FileUri;
            return checkImplicitFile;
          }
          err = ParsingError.MustRootedPath;
          return 0;
        }
        err = uriString[checkImplicitFile + 1] == ':' ? ParsingError.BadScheme : ParsingError.BadFormat;
        return 0;
      }
      bool flag6;
      switch (uriString[checkImplicitFile])
      {
        case '/':
        case '\\':
          flag6 = true;
          break;
        default:
          flag6 = false;
          break;
      }
      if (flag6)
      {
        bool flag7;
        switch (uriString[checkImplicitFile + 1])
        {
          case '/':
          case '\\':
            flag7 = true;
            break;
          default:
            flag7 = false;
            break;
        }
        if (flag7)
        {
          flags |= Uri.Flags.AuthorityFound | Uri.Flags.UncPath | Uri.Flags.ImplicitFile;
          syntax = UriParser.FileUri;
          int index = checkImplicitFile + 2;
          while (true)
          {
            bool flag8 = (uint) index < (uint) uriString.Length;
            if (flag8)
            {
              bool flag9;
              switch (uriString[index])
              {
                case '/':
                case '\\':
                  flag9 = true;
                  break;
                default:
                  flag9 = false;
                  break;
              }
              flag8 = flag9;
            }
            if (flag8)
              ++index;
            else
              break;
          }
          return index;
        }
        err = ParsingError.BadFormat;
        return 0;
      }
      if (length < 0)
      {
        err = ParsingError.BadFormat;
        return 0;
      }
      syntax = Uri.CheckSchemeSyntax(uriString.AsSpan(checkImplicitFile, length), ref err);
      return syntax == null ? 0 : checkImplicitFile + length + 1;
    }

    private static UriParser CheckSchemeSyntax(ReadOnlySpan<char> scheme, ref ParsingError error)
    {
      switch (scheme.Length)
      {
        case 2:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "ws", StringComparison.OrdinalIgnoreCase))
            return UriParser.WsUri;
          break;
        case 3:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "wss", StringComparison.OrdinalIgnoreCase))
            return UriParser.WssUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "ftp", StringComparison.OrdinalIgnoreCase))
            return UriParser.FtpUri;
          break;
        case 4:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "http", StringComparison.OrdinalIgnoreCase))
            return UriParser.HttpUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "file", StringComparison.OrdinalIgnoreCase))
            return UriParser.FileUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "uuid", StringComparison.OrdinalIgnoreCase))
            return UriParser.UuidUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "nntp", StringComparison.OrdinalIgnoreCase))
            return UriParser.NntpUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "ldap", StringComparison.OrdinalIgnoreCase))
            return UriParser.LdapUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "news", StringComparison.OrdinalIgnoreCase))
            return UriParser.NewsUri;
          break;
        case 5:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "https", StringComparison.OrdinalIgnoreCase))
            return UriParser.HttpsUri;
          break;
        case 6:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "mailto", StringComparison.OrdinalIgnoreCase))
            return UriParser.MailToUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "gopher", StringComparison.OrdinalIgnoreCase))
            return UriParser.GopherUri;
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "telnet", StringComparison.OrdinalIgnoreCase))
            return UriParser.TelnetUri;
          break;
        case 7:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "net.tcp", StringComparison.OrdinalIgnoreCase))
            return UriParser.NetTcpUri;
          break;
        case 8:
          if (MemoryExtensions.Equals(scheme, (ReadOnlySpan<char>) "net.pipe", StringComparison.OrdinalIgnoreCase))
            return UriParser.NetPipeUri;
          break;
      }
      if (scheme.Length == 0 || !char.IsAsciiLetter(scheme[0]) || scheme.ContainsAnyExcept<char>(Uri.s_schemeChars))
      {
        error = ParsingError.BadScheme;
        return (UriParser) null;
      }
      if (scheme.Length <= 1024)
        return UriParser.FindOrFetchAsUnknownV1Syntax(UriHelper.SpanToLowerInvariantString(scheme));
      error = ParsingError.SchemeLimit;
      return (UriParser) null;
    }

    private unsafe int CheckAuthorityHelper(
      char* pString,
      int idx,
      int length,
      ref ParsingError err,
      ref Uri.Flags flags,
      UriParser syntax,
      ref string newHost)
    {
      int end = length;
      int num1 = idx;
      int start = idx;
      newHost = (string) null;
      bool justNormalized = false;
      bool flag1 = Uri.IriParsingStatic(syntax);
      bool hasUnicode = (flags & Uri.Flags.HasUnicode) > Uri.Flags.Zero;
      bool flag2 = hasUnicode && (flags & Uri.Flags.HostUnicodeNormalized) == Uri.Flags.Zero;
      UriSyntaxFlags flags1 = syntax.Flags;
      if (flag2)
        newHost = this._originalUnicodeString.Substring(0, num1);
      char c;
      if (idx == length || (c = pString[idx]) == '/' || c == '\\' && Uri.StaticIsFile(syntax) || c == '#' || c == '?')
      {
        if (syntax.InFact(UriSyntaxFlags.AllowEmptyHost))
        {
          flags &= ~Uri.Flags.UncPath;
          if (Uri.StaticInFact(flags, Uri.Flags.ImplicitFile))
            err = ParsingError.BadHostName;
          else
            flags |= Uri.Flags.BasicHostType;
        }
        else
          err = ParsingError.BadHostName;
        if (flag2)
          flags |= Uri.Flags.HostUnicodeNormalized;
        return idx;
      }
      if ((flags1 & UriSyntaxFlags.MayHaveUserInfo) != UriSyntaxFlags.None)
      {
        for (; start < end; ++start)
        {
          if (start == end - 1 || pString[start] == '?' || pString[start] == '#' || pString[start] == '\\' || pString[start] == '/')
          {
            start = idx;
            break;
          }
          if (pString[start] == '@')
          {
            flags |= Uri.Flags.HasUserInfo;
            if (flag1 & flag2)
            {
              newHost += IriHelper.EscapeUnescapeIri(pString, num1, start + 1, UriComponents.UserInfo);
              if (newHost.Length > (int) ushort.MaxValue)
              {
                err = ParsingError.SizeLimit;
                return idx;
              }
            }
            ++start;
            c = pString[start];
            break;
          }
        }
      }
      if (c == '[' && syntax.InFact(UriSyntaxFlags.AllowIPv6Host) && IPv6AddressHelper.IsValid(pString, start + 1, ref end))
      {
        flags |= Uri.Flags.IPv6HostType;
        if (flag2)
        {
          newHost = ((ReadOnlySpan<char>) newHost).ToString() + new ReadOnlySpan<char>((void*) (pString + start), end - start);
          flags |= Uri.Flags.HostUnicodeNormalized;
          justNormalized = true;
        }
      }
      else if (char.IsAsciiDigit(c) && syntax.InFact(UriSyntaxFlags.AllowIPv4Host) && IPv4AddressHelper.IsValid(pString, start, ref end, false, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile), syntax.InFact(UriSyntaxFlags.V1_UnknownUri)))
      {
        flags |= Uri.Flags.IPv4HostType;
        if (flag2)
        {
          newHost = ((ReadOnlySpan<char>) newHost).ToString() + new ReadOnlySpan<char>((void*) (pString + start), end - start);
          flags |= Uri.Flags.HostUnicodeNormalized;
          justNormalized = true;
        }
      }
      else
      {
        int length1;
        if ((flags1 & UriSyntaxFlags.AllowDnsHost) != UriSyntaxFlags.None && !flag1 && DomainNameHelper.IsValid(new ReadOnlySpan<char>((void*) (pString + start), end - start), false, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile), out length1))
        {
          end = start + length1;
          flags |= Uri.Flags.DnsHostType;
          if (!new ReadOnlySpan<char>((void*) (pString + start), length1).ContainsAnyInRange<char>('A', 'Z'))
            flags |= Uri.Flags.CanonicalDnsHost;
        }
        else if ((flags1 & UriSyntaxFlags.AllowDnsHost) != UriSyntaxFlags.None && (flag2 || syntax.InFact(UriSyntaxFlags.AllowIdn)) && DomainNameHelper.IsValid(new ReadOnlySpan<char>((void*) (pString + start), end - start), true, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile), out length1))
        {
          end = start + length1;
          Uri.CheckAuthorityHelperHandleDnsIri(pString, start, end, hasUnicode, ref flags, ref justNormalized, ref newHost, ref err);
        }
        else if ((flags1 & UriSyntaxFlags.AllowUncHost) != UriSyntaxFlags.None && UncNameHelper.IsValid(pString, start, ref end, Uri.StaticNotAny(flags, Uri.Flags.ImplicitFile)) && end - start <= 256)
        {
          flags |= Uri.Flags.UncHostType;
          if (flag2)
          {
            newHost = ((ReadOnlySpan<char>) newHost).ToString() + new ReadOnlySpan<char>((void*) (pString + start), end - start);
            flags |= Uri.Flags.HostUnicodeNormalized;
            justNormalized = true;
          }
        }
      }
      if (end < length && pString[end] == '\\' && (flags & Uri.Flags.HostTypeMask) != Uri.Flags.Zero && !Uri.StaticIsFile(syntax))
      {
        if (syntax.InFact(UriSyntaxFlags.V1_UnknownUri))
        {
          err = ParsingError.BadHostName;
          flags |= Uri.Flags.HostTypeMask;
          return end;
        }
        flags &= ~Uri.Flags.HostTypeMask;
      }
      else if (end < length && pString[end] == ':')
      {
        if (syntax.InFact(UriSyntaxFlags.MayHavePort))
        {
          int num2 = 0;
          int num3 = end;
          for (idx = end + 1; idx < length; ++idx)
          {
            int num4 = (int) pString[idx] - 48;
            switch (num4)
            {
              case -13:
              case -1:
              case 15:
                goto label_49;
              case 0:
              case 1:
              case 2:
              case 3:
              case 4:
              case 5:
              case 6:
              case 7:
              case 8:
              case 9:
                if ((num2 = num2 * 10 + num4) <= (int) ushort.MaxValue)
                  continue;
                goto label_49;
              default:
                if (syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost) && syntax.NotAny(UriSyntaxFlags.V1_UnknownUri))
                {
                  flags &= ~Uri.Flags.HostTypeMask;
                  goto label_49;
                }
                else
                {
                  err = ParsingError.BadPort;
                  return idx;
                }
            }
          }
label_49:
          if (num2 > (int) ushort.MaxValue)
          {
            if (syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost))
            {
              flags &= ~Uri.Flags.HostTypeMask;
            }
            else
            {
              err = ParsingError.BadPort;
              return idx;
            }
          }
          if (hasUnicode & justNormalized)
            newHost = ((ReadOnlySpan<char>) newHost).ToString() + new ReadOnlySpan<char>((void*) (pString + num3), idx - num3);
        }
        else
          flags &= ~Uri.Flags.HostTypeMask;
      }
      if ((flags & Uri.Flags.HostTypeMask) == Uri.Flags.Zero)
      {
        flags &= ~Uri.Flags.HasUserInfo;
        if (syntax.InFact(UriSyntaxFlags.AllowAnyOtherHost))
        {
          flags |= Uri.Flags.BasicHostType;
          end = idx;
          while (end < length && pString[end] != '/' && pString[end] != '?' && pString[end] != '#')
            ++end;
          if (flag2)
          {
            string str = new string(pString, num1, end - num1);
            try
            {
              newHost += str.Normalize(NormalizationForm.FormC);
            }
            catch (ArgumentException ex)
            {
              err = ParsingError.BadHostName;
            }
            flags |= Uri.Flags.HostUnicodeNormalized;
          }
        }
        else if (syntax.InFact(UriSyntaxFlags.V1_UnknownUri))
        {
          bool flag3 = false;
          int startIndex = idx;
          for (end = idx; end < length && (!flag3 || pString[end] != '/' && pString[end] != '?' && pString[end] != '#'); ++end)
          {
            if (end < idx + 2 && pString[end] == '.')
            {
              flag3 = true;
            }
            else
            {
              err = ParsingError.BadHostName;
              flags |= Uri.Flags.HostTypeMask;
              return idx;
            }
          }
          flags |= Uri.Flags.BasicHostType;
          if (flag2)
          {
            string str = new string(pString, startIndex, end - startIndex);
            try
            {
              newHost += str.Normalize(NormalizationForm.FormC);
            }
            catch (ArgumentException ex)
            {
              err = ParsingError.BadFormat;
              return idx;
            }
            flags |= Uri.Flags.HostUnicodeNormalized;
          }
        }
        else if (syntax.InFact(UriSyntaxFlags.MustHaveAuthority) || syntax.InFact(UriSyntaxFlags.MailToLikeUri))
        {
          err = ParsingError.BadHostName;
          flags |= Uri.Flags.HostTypeMask;
          return idx;
        }
      }
      return end;
    }

    private static unsafe void CheckAuthorityHelperHandleDnsIri(
      char* pString,
      int start,
      int end,
      bool hasUnicode,
      ref Uri.Flags flags,
      ref bool justNormalized,
      ref string newHost,
      ref ParsingError err)
    {
      flags |= Uri.Flags.DnsHostType;
      if (hasUnicode)
      {
        string str = UriHelper.StripBidiControlCharacters(new ReadOnlySpan<char>((void*) (pString + start), end - start));
        try
        {
          newHost += str.Normalize(NormalizationForm.FormC);
        }
        catch (ArgumentException ex)
        {
          err = ParsingError.BadHostName;
        }
        justNormalized = true;
      }
      flags |= Uri.Flags.HostUnicodeNormalized;
    }

    private unsafe Uri.Check CheckCanonical(char* str, ref int idx, int end, char delim)
    {
      Uri.Check check = Uri.Check.None;
      bool flag1 = false;
      bool flag2 = false;
      bool iriParsing = this.IriParsing;
      int index;
      for (index = idx; index < end; ++index)
      {
        char ch1 = str[index];
        if (ch1 <= '\u001F' || ch1 >= '\u007F' && ch1 <= '\u009F')
        {
          flag1 = true;
          flag2 = true;
          check |= Uri.Check.ReservedFound;
        }
        else if (ch1 > '~')
        {
          if (iriParsing)
          {
            bool flag3 = false;
            check |= Uri.Check.FoundNonAscii;
            if (char.IsHighSurrogate(ch1))
            {
              if (index + 1 < end)
                flag3 = IriHelper.CheckIriUnicodeRange(ch1, str[index + 1], out bool _, true);
            }
            else
              flag3 = IriHelper.CheckIriUnicodeRange(ch1, true);
            if (!flag3)
              check |= Uri.Check.NotIriCanonical;
          }
          if (!flag1)
            flag1 = true;
        }
        else if ((int) ch1 != (int) delim && (delim != '?' || ch1 != '#' || this._syntax == null || !this._syntax.InFact(UriSyntaxFlags.MayHaveFragment)))
        {
          switch (ch1)
          {
            case '#':
              flag1 = true;
              if (this.IsImplicitFile || this._syntax != null && !this._syntax.InFact(UriSyntaxFlags.MayHaveFragment))
              {
                check |= Uri.Check.ReservedFound;
                flag2 = true;
                continue;
              }
              continue;
            case '?':
              if (this.IsImplicitFile || this._syntax != null && !this._syntax.InFact(UriSyntaxFlags.MayHaveQuery) && delim != '\uFFFE')
              {
                check |= Uri.Check.ReservedFound;
                flag2 = true;
                flag1 = true;
                continue;
              }
              continue;
            default:
              if (ch1 == '/' || ch1 == '\\')
              {
                if ((check & Uri.Check.BackslashInPath) == Uri.Check.None && ch1 == '\\')
                  check |= Uri.Check.BackslashInPath;
                if ((check & Uri.Check.DotSlashAttn) == Uri.Check.None && index + 1 != end && (str[index + 1] == '/' || str[index + 1] == '\\'))
                {
                  check |= Uri.Check.DotSlashAttn;
                  continue;
                }
                continue;
              }
              if (ch1 == '.')
              {
                if ((check & Uri.Check.DotSlashAttn) == Uri.Check.None && index + 1 == end || str[index + 1] == '.' || str[index + 1] == '/' || str[index + 1] == '\\' || str[index + 1] == '?' || str[index + 1] == '#')
                {
                  check |= Uri.Check.DotSlashAttn;
                  continue;
                }
                continue;
              }
              if (ch1 <= '"' && ch1 != '!' || ch1 >= '[' && ch1 <= '^' || ch1 == '>' || ch1 == '<' || ch1 == '`')
              {
                if (!flag1)
                  flag1 = true;
                if ((this._flags & Uri.Flags.HasUnicode) != Uri.Flags.Zero)
                {
                  check |= Uri.Check.NotIriCanonical;
                  continue;
                }
                continue;
              }
              if (ch1 >= '{' && ch1 <= '}')
              {
                flag1 = true;
                continue;
              }
              if (ch1 == '%')
              {
                if (!flag2)
                  flag2 = true;
                char ch2;
                if (index + 2 < end && (ch2 = UriHelper.DecodeHexChars((int) str[index + 1], (int) str[index + 2])) != char.MaxValue)
                {
                  if (ch2 == '.' || ch2 == '/' || ch2 == '\\')
                    check |= Uri.Check.DotSlashEscaped;
                  index += 2;
                  continue;
                }
                if (!flag1)
                {
                  flag1 = true;
                  continue;
                }
                continue;
              }
              continue;
          }
        }
        else
          break;
      }
      if (flag2)
      {
        if (!flag1)
          check |= Uri.Check.EscapedCanonical;
      }
      else
      {
        check |= Uri.Check.DisplayCanonical;
        if (!flag1)
          check |= Uri.Check.EscapedCanonical;
      }
      idx = index;
      return check;
    }

    private unsafe void GetCanonicalPath(ref ValueStringBuilder dest, UriFormat formatAs)
    {
      if (this.InFact(Uri.Flags.FirstSlashAbsent))
        dest.Append('/');
      if ((int) this._info.Offset.Path == (int) this._info.Offset.Query)
        return;
      int length1 = dest.Length;
      int securedPathIndex = this.SecuredPathIndex;
      if (formatAs == UriFormat.UriEscaped)
      {
        if (this.InFact(Uri.Flags.ShouldBeCompressed))
        {
          dest.Append(this._string.AsSpan((int) this._info.Offset.Path, (int) this._info.Offset.Query - (int) this._info.Offset.Path));
          if (this._syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes) && this.InFact(Uri.Flags.PathNotCanonical) && !this.IsImplicitFile)
          {
            fixed (char* pch = &dest.GetPinnableReference())
            {
              int length2 = dest.Length;
              Uri.UnescapeOnly(pch, length1, ref length2, '.', '/', this._syntax.InFact(UriSyntaxFlags.ConvertPathSlashes) ? '\\' : char.MaxValue);
              dest.Length = length2;
            }
          }
        }
        else if (this.InFact(Uri.Flags.E_PathNotCanonical) && this.NotAny(Uri.Flags.UserEscaped))
        {
          ReadOnlySpan<char> readOnlySpan = (ReadOnlySpan<char>) this._string;
          if (securedPathIndex != 0 && readOnlySpan[securedPathIndex + (int) this._info.Offset.Path - 1] == '|')
          {
            char[] array = readOnlySpan.ToArray();
            array[securedPathIndex + (int) this._info.Offset.Path - 1] = ':';
            readOnlySpan = (ReadOnlySpan<char>) array;
          }
          UriHelper.EscapeString(readOnlySpan.Slice((int) this._info.Offset.Path, (int) this._info.Offset.Query - (int) this._info.Offset.Path), ref dest, !this.IsImplicitFile, UriHelper.UnreservedReservedExceptQuestionMarkHash);
        }
        else
          dest.Append(this._string.AsSpan((int) this._info.Offset.Path, (int) this._info.Offset.Query - (int) this._info.Offset.Path));
        if (!OperatingSystem.IsWindows() && this.InFact(Uri.Flags.BackslashInPath) && this._syntax.NotAny(UriSyntaxFlags.ConvertPathSlashes) && this._syntax.InFact(UriSyntaxFlags.FileLikeUri) && !this.IsImplicitFile)
        {
          ValueStringBuilder valueStringBuilder = new ValueStringBuilder(stackalloc char[512]);
          valueStringBuilder.Append(dest.AsSpan(length1, dest.Length - length1));
          dest.Length = length1;
          UriHelper.EscapeString(MemoryMarshal.CreateReadOnlySpan<char>(ref valueStringBuilder.GetPinnableReference(), valueStringBuilder.Length), ref dest, true, UriHelper.UnreservedReserved);
          length1 = dest.Length;
          valueStringBuilder.Dispose();
        }
      }
      else
      {
        dest.Append(this._string.AsSpan((int) this._info.Offset.Path, (int) this._info.Offset.Query - (int) this._info.Offset.Path));
        if (this.InFact(Uri.Flags.ShouldBeCompressed) && this._syntax.InFact(UriSyntaxFlags.UnEscapeDotsAndSlashes) && this.InFact(Uri.Flags.PathNotCanonical) && !this.IsImplicitFile)
        {
          fixed (char* pch = &dest.GetPinnableReference())
          {
            int length3 = dest.Length;
            Uri.UnescapeOnly(pch, length1, ref length3, '.', '/', this._syntax.InFact(UriSyntaxFlags.ConvertPathSlashes) ? '\\' : char.MaxValue);
            dest.Length = length3;
          }
        }
      }
      int start = length1 + securedPathIndex;
      if (securedPathIndex != 0 && dest[start - 1] == '|')
        dest[start - 1] = ':';
      if (this.InFact(Uri.Flags.ShouldBeCompressed) && dest.Length - start > 0)
      {
        dest.Length = start + Uri.Compress(dest.RawChars.Slice(start, dest.Length - start), this._syntax);
        if (dest[length1] == '\\')
          dest[length1] = '/';
        if (formatAs == UriFormat.UriEscaped && this.NotAny(Uri.Flags.UserEscaped) && this.InFact(Uri.Flags.E_PathNotCanonical))
        {
          ValueStringBuilder valueStringBuilder = new ValueStringBuilder(stackalloc char[512]);
          valueStringBuilder.Append(dest.AsSpan(length1, dest.Length - length1));
          dest.Length = length1;
          UriHelper.EscapeString(MemoryMarshal.CreateReadOnlySpan<char>(ref valueStringBuilder.GetPinnableReference(), valueStringBuilder.Length), ref dest, !this.IsImplicitFile, UriHelper.UnreservedReservedExceptQuestionMarkHash);
          length1 = dest.Length;
          valueStringBuilder.Dispose();
        }
      }
      if (formatAs == UriFormat.UriEscaped || !this.InFact(Uri.Flags.PathNotCanonical))
        return;
      UnescapeMode unescapeMode;
      switch (formatAs)
      {
        case UriFormat.Unescaped:
          unescapeMode = this.IsImplicitFile ? UnescapeMode.CopyOnly : UnescapeMode.Unescape | UnescapeMode.UnescapeAll;
          break;
        case (UriFormat) 32767:
          unescapeMode = (UnescapeMode) ((this.InFact(Uri.Flags.UserEscaped) ? 2 : 3) | 4);
          if (this.IsImplicitFile)
          {
            unescapeMode &= ~UnescapeMode.Unescape;
            break;
          }
          break;
        default:
          unescapeMode = this.InFact(Uri.Flags.UserEscaped) ? UnescapeMode.Unescape : UnescapeMode.EscapeUnescape;
          if (this.IsImplicitFile)
          {
            unescapeMode &= ~UnescapeMode.Unescape;
            break;
          }
          break;
      }
      if (unescapeMode == UnescapeMode.CopyOnly)
        return;
      ValueStringBuilder valueStringBuilder1 = new ValueStringBuilder(stackalloc char[512]);
      valueStringBuilder1.Append(dest.AsSpan(length1, dest.Length - length1));
      dest.Length = length1;
      fixed (char* pStr = &valueStringBuilder1.GetPinnableReference())
        UriHelper.UnescapeString(pStr, 0, valueStringBuilder1.Length, ref dest, '?', '#', char.MaxValue, unescapeMode, this._syntax, false);
      valueStringBuilder1.Dispose();
    }

    private static unsafe void UnescapeOnly(
      char* pch,
      int start,
      ref int end,
      char ch1,
      char ch2,
      char ch3)
    {
      if (end - start < 3)
        return;
      char* chPtr1 = pch + end - 2;
      pch += start;
      char* chPtr2 = (char*) null;
      while (pch < chPtr1)
      {
        if (*pch++ == '%')
        {
          char ch4 = UriHelper.DecodeHexChars((int) *pch++, (int) *pch++);
          if ((int) ch4 == (int) ch1 || (int) ch4 == (int) ch2 || (int) ch4 == (int) ch3)
          {
            chPtr2 = pch - 2;
            *(chPtr2 - 1) = ch4;
            while (pch < chPtr1)
            {
              char* chPtr3 = chPtr2++;
              char* chPtr4 = pch++;
              int num1;
              char ch5 = (char) (num1 = (int) *chPtr4);
              *chPtr3 = (char) num1;
              if (ch5 == '%')
              {
                char* chPtr5 = chPtr2;
                char* chPtr6 = (char*) ((IntPtr) chPtr5 + 2);
                char* chPtr7 = pch++;
                int num2;
                char ch6 = (char) (num2 = (int) *chPtr7);
                *chPtr5 = (char) num2;
                int first = (int) ch6;
                char* chPtr8 = chPtr6;
                chPtr2 = (char*) ((IntPtr) chPtr8 + 2);
                char* chPtr9 = pch++;
                int num3;
                char ch7 = (char) (num3 = (int) *chPtr9);
                *chPtr8 = (char) num3;
                int second = (int) ch7;
                char ch8 = UriHelper.DecodeHexChars(first, second);
                if ((int) ch8 == (int) ch1 || (int) ch8 == (int) ch2 || (int) ch8 == (int) ch3)
                {
                  chPtr2 -= 2;
                  *(chPtr2 - 1) = ch8;
                }
              }
            }
            break;
          }
        }
      }
      char* chPtr10 = chPtr1 + 2;
      if ((IntPtr) chPtr2 == IntPtr.Zero)
        return;
      if (pch == chPtr10)
      {
        end -= (int) (pch - chPtr2);
      }
      else
      {
        char* chPtr11 = chPtr2;
        char* chPtr12 = (char*) ((IntPtr) chPtr11 + 2);
        int num4 = (int) *pch++;
        *chPtr11 = (char) num4;
        if (pch == chPtr10)
        {
          end -= (int) (pch - chPtr12);
        }
        else
        {
          char* chPtr13 = chPtr12;
          char* chPtr14 = (char*) ((IntPtr) chPtr13 + 2);
          int num5 = (int) *pch++;
          *chPtr13 = (char) num5;
          end -= (int) (pch - chPtr14);
        }
      }
    }

    private static void Compress(char[] dest, int start, ref int destLength, UriParser syntax)
    {
      destLength = start + Uri.Compress(dest.AsSpan<char>(start, destLength - start), syntax);
    }

    private static int Compress(Span<char> span, UriParser syntax)
    {
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      int num4 = 0;
      for (int index = span.Length - 1; index >= 0; --index)
      {
        char ch = span[index];
        if (ch == '\\' && syntax.InFact(UriSyntaxFlags.ConvertPathSlashes))
          span[index] = ch = '/';
        if (ch == '/')
        {
          ++num1;
        }
        else
        {
          if (num1 > 1)
            num2 = index + 1;
          num1 = 0;
        }
        if (ch == '.')
        {
          ++num3;
        }
        else
        {
          if (num3 != 0)
          {
            if ((!syntax.NotAny(UriSyntaxFlags.CanonicalizeAsFilePath) || num3 <= 2 && ch == '/') && ch == '/' && (num2 == index + num3 + 1 || num2 == 0 && index + num3 + 1 == span.Length) && num3 <= 2)
            {
              int start = index + 1 + num3 + (num2 != 0 ? 1 : 0);
              span.Slice(start).CopyTo(span.Slice(index + 1));
              span = span.Slice(0, span.Length - (start - index - 1));
              num2 = index;
              if (num3 == 2)
                ++num4;
              num3 = 0;
              continue;
            }
            num3 = 0;
          }
          if (ch == '/')
          {
            if (num4 != 0)
            {
              --num4;
              span.Slice(num2 + 1).CopyTo(span.Slice(index + 1));
              span = span.Slice(0, span.Length - (num2 - index));
            }
            num2 = index;
          }
        }
      }
      if (span.Length != 0 && syntax.InFact(UriSyntaxFlags.CanonicalizeAsFilePath) && num1 <= 1)
      {
        if (num4 != 0 && span[0] != '/')
        {
          int start = num2 + 1;
          span.Slice(start).CopyTo(span);
          return span.Length - start;
        }
        if (num3 != 0 && (num2 == num3 || num2 == 0 && num3 == span.Length))
        {
          int start = num3 + (num2 != 0 ? 1 : 0);
          span.Slice(start).CopyTo(span);
          return span.Length - start;
        }
      }
      return span.Length;
    }

    private static string CombineUri(Uri basePart, string relativePart, UriFormat uriFormat)
    {
      char ch1 = relativePart[0];
      if (basePart.IsDosPath && (ch1 == '/' || ch1 == '\\') && (relativePart.Length == 1 || relativePart[1] != '/' && relativePart[1] != '\\'))
      {
        int num1 = basePart.OriginalString.IndexOf(':');
        if (basePart.IsImplicitFile)
          return basePart.OriginalString.AsSpan(0, num1 + 1).ToString() + (ReadOnlySpan<char>) relativePart;
        int num2 = basePart.OriginalString.IndexOf(':', num1 + 1);
        return basePart.OriginalString.AsSpan(0, num2 + 1).ToString() + (ReadOnlySpan<char>) relativePart;
      }
      if (Uri.StaticIsFile(basePart.Syntax) && (ch1 == '\\' || ch1 == '/'))
      {
        if (relativePart.Length >= 2 && (relativePart[1] == '\\' || relativePart[1] == '/'))
          return !basePart.IsImplicitFile ? "file:" + relativePart : relativePart;
        if (!basePart.IsUnc)
          return "file://" + relativePart;
        ReadOnlySpan<char> readOnlySpan = (ReadOnlySpan<char>) basePart.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.Unescaped);
        int num = readOnlySpan.Slice(1).IndexOf<char>('/');
        if (num >= 0)
          readOnlySpan = readOnlySpan.Slice(0, num + 1);
        return basePart.IsImplicitFile ? ((ReadOnlySpan<char>) "\\\\").ToString() + (ReadOnlySpan<char>) basePart.GetParts(UriComponents.Host, UriFormat.Unescaped) + readOnlySpan + (ReadOnlySpan<char>) relativePart : ((ReadOnlySpan<char>) "file://").ToString() + (ReadOnlySpan<char>) basePart.GetParts(UriComponents.Host, uriFormat) + readOnlySpan + (ReadOnlySpan<char>) relativePart;
      }
      bool flag = basePart.Syntax.InFact(UriSyntaxFlags.ConvertPathSlashes);
      if (ch1 == '/' || ch1 == '\\' & flag)
      {
        if (relativePart.Length >= 2 && relativePart[1] == '/')
          return basePart.Scheme + ":" + relativePart;
        string str;
        if (basePart.HostType == Uri.Flags.IPv6HostType)
        {
          DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(2, 3);
          interpolatedStringHandler.AppendFormatted(basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo, uriFormat));
          interpolatedStringHandler.AppendLiteral("[");
          interpolatedStringHandler.AppendFormatted(basePart.DnsSafeHost);
          interpolatedStringHandler.AppendLiteral("]");
          interpolatedStringHandler.AppendFormatted(basePart.GetParts(UriComponents.Port | UriComponents.KeepDelimiter, uriFormat));
          str = interpolatedStringHandler.ToStringAndClear();
        }
        else
          str = basePart.GetParts(UriComponents.SchemeAndServer | UriComponents.UserInfo, uriFormat);
        return !flag || ch1 != '\\' ? str + relativePart : ((ReadOnlySpan<char>) str).ToString() + (ReadOnlySpan<char>) "/" + relativePart.AsSpan(1);
      }
      string parts = basePart.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, basePart.IsImplicitFile ? UriFormat.Unescaped : uriFormat);
      int length = parts.Length;
      char[] chArray = new char[length + relativePart.Length];
      if (length > 0)
      {
        parts.CopyTo(0, chArray, 0, length);
        while (length > 0)
        {
          if (chArray[--length] == '/')
          {
            ++length;
            break;
          }
        }
      }
      relativePart.CopyTo(0, chArray, length, relativePart.Length);
      char ch2 = basePart.Syntax.InFact(UriSyntaxFlags.MayHaveQuery) ? '?' : char.MaxValue;
      char ch3 = basePart.IsImplicitFile || !basePart.Syntax.InFact(UriSyntaxFlags.MayHaveFragment) ? char.MaxValue : '#';
      ReadOnlySpan<char> readOnlySpan1 = (ReadOnlySpan<char>) string.Empty;
      int destLength;
      if (ch2 != char.MaxValue || ch3 != char.MaxValue)
      {
        int start = 0;
        while (start < relativePart.Length && (int) chArray[length + start] != (int) ch2 && (int) chArray[length + start] != (int) ch3)
          ++start;
        if (start == 0)
          readOnlySpan1 = (ReadOnlySpan<char>) relativePart;
        else if (start < relativePart.Length)
          readOnlySpan1 = relativePart.AsSpan(start);
        destLength = length + start;
      }
      else
        destLength = length + relativePart.Length;
      string str1;
      if (basePart.HostType == Uri.Flags.IPv6HostType)
      {
        if (basePart.IsImplicitFile)
          str1 = "\\\\[" + basePart.DnsSafeHost + "]";
        else
          str1 = basePart.GetParts(UriComponents.Scheme | UriComponents.UserInfo, uriFormat) + "[" + basePart.DnsSafeHost + "]" + basePart.GetParts(UriComponents.Port | UriComponents.KeepDelimiter, uriFormat);
      }
      else if (basePart.IsImplicitFile)
      {
        if (basePart.IsDosPath)
        {
          Uri.Compress(chArray, 3, ref destLength, basePart.Syntax);
          return ((ReadOnlySpan<char>) chArray.AsSpan<char>(1, destLength - 1)).ToString() + readOnlySpan1;
        }
        str1 = OperatingSystem.IsWindows() || !basePart.IsUnixPath ? "\\\\" + basePart.GetParts(UriComponents.Host, UriFormat.Unescaped) : basePart.GetParts(UriComponents.Host, UriFormat.Unescaped);
      }
      else
        str1 = basePart.GetParts(UriComponents.SchemeAndServer | UriComponents.UserInfo, uriFormat);
      Uri.Compress(chArray, basePart.SecuredPathIndex, ref destLength, basePart.Syntax);
      return ((ReadOnlySpan<char>) str1).ToString() + (ReadOnlySpan<char>) chArray.AsSpan<char>(0, destLength) + readOnlySpan1;
    }

    private static string PathDifference(string path1, string path2, bool compareCase)
    {
      int num = -1;
      int index;
      for (index = 0; index < path1.Length && index < path2.Length && ((int) path1[index] == (int) path2[index] || !compareCase && (int) char.ToLowerInvariant(path1[index]) == (int) char.ToLowerInvariant(path2[index])); ++index)
      {
        if (path1[index] == '/')
          num = index;
      }
      if (index == 0)
        return path2;
      if (index == path1.Length && index == path2.Length)
        return string.Empty;
      StringBuilder stringBuilder = new StringBuilder();
      for (; index < path1.Length; ++index)
      {
        if (path1[index] == '/')
          stringBuilder.Append("../");
      }
      return stringBuilder.Length == 0 && path2.Length - 1 == num ? "./" : stringBuilder.Append(path2.AsSpan(num + 1)).ToString();
    }

    #nullable enable
    /// <summary>Determines the difference between two <see cref="T:System.Uri" /> instances.</summary>
    /// <param name="toUri">The URI to compare to the current URI.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="toUri" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this method is valid only for absolute URIs.</exception>
    /// <returns>If the hostname and scheme of this URI instance and <paramref name="toUri" /> are the same, then this method returns a <see cref="T:System.String" /> that represents a relative URI that, when appended to the current URI instance, yields the <paramref name="toUri" /> parameter.
    /// 
    /// If the hostname or scheme is different, then this method returns a <see cref="T:System.String" /> that represents the <paramref name="toUri" /> parameter.</returns>
    [Obsolete("Uri.MakeRelative has been deprecated. Use MakeRelativeUri(Uri uri) instead.")]
    public string MakeRelative(Uri toUri)
    {
      ArgumentNullException.ThrowIfNull((object) toUri, nameof (toUri));
      if (this.IsNotAbsoluteUri || toUri.IsNotAbsoluteUri)
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      return this.Scheme == toUri.Scheme && this.Host == toUri.Host && this.Port == toUri.Port ? Uri.PathDifference(this.AbsolutePath, toUri.AbsolutePath, !this.IsUncOrDosPath) : toUri.ToString();
    }

    /// <summary>Converts the internally stored URI to canonical form.</summary>
    /// <exception cref="T:System.InvalidOperationException">This instance represents a relative URI, and this method is valid only for absolute URIs.</exception>
    /// <exception cref="T:System.UriFormatException">The URI is incorrectly formed.</exception>
    [Obsolete("Uri.Canonicalize has been deprecated and is not supported.")]
    protected virtual void Canonicalize()
    {
    }

    /// <summary>Parses the URI of the current instance to ensure it contains all the parts required for a valid URI.</summary>
    /// <exception cref="T:System.UriFormatException">The Uri passed from the constructor is invalid.</exception>
    [Obsolete("Uri.Parse has been deprecated and is not supported.")]
    protected virtual void Parse()
    {
    }

    /// <summary>Converts any unsafe or reserved characters in the path component to their hexadecimal character representations.</summary>
    /// <exception cref="T:System.UriFormatException">The URI passed from the constructor is invalid. This exception can occur if a URI has too many characters or the URI is relative.</exception>
    [Obsolete("Uri.Escape has been deprecated and is not supported.")]
    protected virtual void Escape()
    {
    }

    /// <summary>Converts the specified string by replacing any escape sequences with their unescaped representation.</summary>
    /// <param name="path">The string to convert.</param>
    /// <returns>The unescaped value of the <paramref name="path" /> parameter.</returns>
    [Obsolete("Uri.Unescape has been deprecated. Use GetComponents() or Uri.UnescapeDataString() to unescape a Uri component or a string.")]
    protected virtual string Unescape(string path)
    {
      char[] dest = new char[path.Length];
      int destPosition = 0;
      return new string(UriHelper.UnescapeString(path, 0, path.Length, dest, ref destPosition, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.Unescape | UnescapeMode.UnescapeAll, (UriParser) null, false), 0, destPosition);
    }

    /// <summary>Converts a string to its escaped representation.</summary>
    /// <param name="str">The string to transform to its escaped representation.</param>
    /// <returns>The escaped representation of the string.</returns>
    [Obsolete("Uri.EscapeString has been deprecated. Use GetComponents() or Uri.EscapeDataString to escape a Uri component or a string.")]
    protected static string EscapeString(string? str)
    {
      return str != null ? UriHelper.EscapeString(str, true, UriHelper.UnreservedReservedExceptQuestionMarkHash) : string.Empty;
    }

    /// <summary>Calling this method has no effect.</summary>
    [Obsolete("Uri.CheckSecurity has been deprecated and is not supported.")]
    protected virtual void CheckSecurity()
    {
    }

    /// <summary>Determines whether the specified character is a reserved character.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns>
    /// <see langword="true" /> if the specified character is a reserved character otherwise, <see langword="false" />.</returns>
    [Obsolete("Uri.IsReservedCharacter has been deprecated and is not supported.")]
    protected virtual bool IsReservedCharacter(char character)
    {
      return character == ';' || character == '/' || character == ':' || character == '@' || character == '&' || character == '=' || character == '+' || character == '$' || character == ',';
    }

    /// <summary>Determines whether the specified character should be escaped.</summary>
    /// <param name="character">The character to test.</param>
    /// <returns>
    /// <see langword="true" /> if the specified character should be escaped; otherwise, <see langword="false" />.</returns>
    [Obsolete("Uri.IsExcludedCharacter has been deprecated and is not supported.")]
    protected static bool IsExcludedCharacter(char character)
    {
      return character <= ' ' || character >= '\u007F' || character == '<' || character == '>' || character == '#' || character == '%' || character == '"' || character == '{' || character == '}' || character == '|' || character == '\\' || character == '^' || character == '[' || character == ']' || character == '`';
    }

    /// <summary>Indicates whether a character is invalid in a file system name.</summary>
    /// <param name="character">The <see cref="T:System.Char" /> to test.</param>
    /// <returns>
    /// <see langword="true" /> if the specified character is invalid; otherwise, <see langword="false" />.</returns>
    [Obsolete("Uri.IsBadFileSystemCharacter has been deprecated and is not supported.")]
    protected virtual bool IsBadFileSystemCharacter(char character)
    {
      return character < ' ' || character == ';' || character == '/' || character == '?' || character == ':' || character == '&' || character == '=' || character == ',' || character == '*' || character == '<' || character == '>' || character == '"' || character == '|' || character == '\\' || character == '^';
    }

    #nullable disable
    [MemberNotNull("_string")]
    private void CreateThis(
      string uri,
      bool dontEscape,
      UriKind uriKind,
      in UriCreationOptions creationOptions = default (UriCreationOptions))
    {
      if (uriKind < UriKind.RelativeOrAbsolute || uriKind > UriKind.Relative)
        throw new ArgumentException(SR.Format(SR.net_uri_InvalidUriKind, (object) uriKind));
      this._string = uri ?? string.Empty;
      if (dontEscape)
        this._flags |= Uri.Flags.UserEscaped;
      if (creationOptions.DangerousDisablePathAndQueryCanonicalization)
        this._flags |= Uri.Flags.DisablePathAndQueryCanonicalization;
      UriFormatException e;
      this.InitializeUri(Uri.ParseScheme(this._string, ref this._flags, ref this._syntax), uriKind, out e);
      if (e != null)
        throw e;
    }

    private void InitializeUri(ParsingError err, UriKind uriKind, out UriFormatException e)
    {
      if (err == ParsingError.None)
      {
        if (this.IsImplicitFile)
        {
          if (this.NotAny(Uri.Flags.DosPath))
          {
            switch (uriKind)
            {
              case UriKind.Absolute:
                break;
              case UriKind.Relative:
                this._syntax = (UriParser) null;
                this._flags &= Uri.Flags.UserEscaped;
                e = (UriFormatException) null;
                return;
              default:
                if (this._string.Length >= 2 && (this._string[0] != '\\' || this._string[1] != '\\') || !OperatingSystem.IsWindows() && this.InFact(Uri.Flags.UnixPath))
                  goto case UriKind.Relative;
                else
                  break;
            }
          }
          if (uriKind == UriKind.Relative && this.InFact(Uri.Flags.DosPath))
          {
            this._syntax = (UriParser) null;
            this._flags &= Uri.Flags.UserEscaped;
            e = (UriFormatException) null;
            return;
          }
        }
      }
      else if (err > ParsingError.EmptyUriString)
      {
        this._string = (string) null;
        e = Uri.GetException(err);
        return;
      }
      bool flag = false;
      if (this.IriParsing && Uri.CheckForUnicodeOrEscapedUnreserved(this._string))
      {
        this._flags |= Uri.Flags.HasUnicode;
        flag = true;
        this._originalUnicodeString = this._string;
      }
      if (this._syntax != null)
      {
        if (this._syntax.IsSimple)
        {
          if ((err = this.PrivateParseMinimal()) != ParsingError.None)
          {
            if (uriKind != UriKind.Absolute && err <= ParsingError.EmptyUriString)
            {
              this._syntax = (UriParser) null;
              e = (UriFormatException) null;
              this._flags &= Uri.Flags.UserEscaped;
              return;
            }
            e = Uri.GetException(err);
          }
          else
            e = uriKind != UriKind.Relative ? (UriFormatException) null : Uri.GetException(ParsingError.CannotCreateRelative);
          if (!flag)
            return;
          try
          {
            this.EnsureParseRemaining();
          }
          catch (UriFormatException ex)
          {
            e = ex;
          }
        }
        else
        {
          this._syntax = this._syntax.InternalOnNewUri();
          this._flags |= Uri.Flags.UserDrivenParsing;
          this._syntax.InternalValidate(this, out e);
          if (e != null)
          {
            if (uriKind == UriKind.Absolute || err == ParsingError.None || err > ParsingError.EmptyUriString)
              return;
            this._syntax = (UriParser) null;
            e = (UriFormatException) null;
            this._flags &= Uri.Flags.UserEscaped;
          }
          else
          {
            if (err != ParsingError.None || this.InFact(Uri.Flags.ErrorOrParsingRecursion))
              this._flags = Uri.Flags.UserDrivenParsing | this._flags & Uri.Flags.UserEscaped;
            else if (uriKind == UriKind.Relative)
              e = Uri.GetException(ParsingError.CannotCreateRelative);
            if (!flag)
              return;
            try
            {
              this.EnsureParseRemaining();
            }
            catch (UriFormatException ex)
            {
              e = ex;
            }
          }
        }
      }
      else if (err != ParsingError.None && uriKind != UriKind.Absolute && err <= ParsingError.EmptyUriString)
      {
        e = (UriFormatException) null;
        this._flags &= Uri.Flags.UserEscaped | Uri.Flags.HasUnicode;
        if (!flag)
          return;
        this._string = this.EscapeUnescapeIri(this._originalUnicodeString, 0, this._originalUnicodeString.Length, (UriComponents) 0);
        int length = this._string.Length;
      }
      else
      {
        this._string = (string) null;
        e = Uri.GetException(err);
      }
    }

    private static bool CheckForUnicodeOrEscapedUnreserved(string data)
    {
      for (int index = 0; index < data.Length; ++index)
      {
        char ch = data[index];
        if (ch == '%')
        {
          if ((uint) (index + 2) < (uint) data.Length)
          {
            char c = UriHelper.DecodeHexChars((int) data[index + 1], (int) data[index + 2]);
            if (!char.IsAscii(c) || UriHelper.Unreserved.Contains(c))
              return true;
            index += 2;
          }
        }
        else if (ch > '\u007F')
          return true;
      }
      return false;
    }

    #nullable enable
    /// <summary>Creates a new <see cref="T:System.Uri" /> using the specified <see cref="T:System.String" /> instance and a <see cref="T:System.UriKind" />.</summary>
    /// <param name="uriString">The string representation of the <see cref="T:System.Uri" />.</param>
    /// <param name="uriKind">The type of the Uri.</param>
    /// <param name="result">When this method returns, contains the constructed <see cref="T:System.Uri" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> was successfully created; otherwise, <see langword="false" />.</returns>
    public static bool TryCreate([NotNullWhen(true), StringSyntax("Uri", new object[] {"uriKind"})] string? uriString, UriKind uriKind, [NotNullWhen(true)] out Uri? result)
    {
      if (uriString == null)
      {
        result = (Uri) null;
        return false;
      }
      UriFormatException e = (UriFormatException) null;
      result = Uri.CreateHelper(uriString, false, uriKind, ref e);
      return e == null && result != (Uri) null;
    }

    /// <summary>Creates a new <see cref="T:System.Uri" /> using the specified <see cref="T:System.String" /> instance and <see cref="T:System.UriCreationOptions" />.</summary>
    /// <param name="uriString">The string representation of the <see cref="T:System.Uri" />.</param>
    /// <param name="creationOptions">Options that control how the <see cref="T:System.Uri" /> is created and behaves.</param>
    /// <param name="result">When this method returns, contains the constructed <see cref="T:System.Uri" />.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> was successfully created; otherwise, <see langword="false" />.</returns>
    public static bool TryCreate(
      [NotNullWhen(true), StringSyntax("Uri")] string? uriString,
      in UriCreationOptions creationOptions,
      [NotNullWhen(true)] out Uri? result)
    {
      if (uriString == null)
      {
        result = (Uri) null;
        return false;
      }
      UriFormatException e = (UriFormatException) null;
      result = Uri.CreateHelper(uriString, false, UriKind.Absolute, ref e, in creationOptions);
      return e == null && result != (Uri) null;
    }

    /// <summary>Creates a new <see cref="T:System.Uri" /> using the specified base and relative <see cref="T:System.String" /> instances.</summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="relativeUri">The string representation of the relative URI to add to the base <see cref="T:System.Uri" />.</param>
    /// <param name="result">When this method returns, contains a <see cref="T:System.Uri" /> constructed from <paramref name="baseUri" /> and <paramref name="relativeUri" />. This parameter is passed uninitialized.</param>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> was successfully created; otherwise, <see langword="false" />.</returns>
    public static bool TryCreate(Uri? baseUri, string? relativeUri, [NotNullWhen(true)] out Uri? result)
    {
      Uri result1;
      if (Uri.TryCreate(relativeUri, UriKind.RelativeOrAbsolute, out result1))
      {
        if (!result1.IsAbsoluteUri)
          return Uri.TryCreate(baseUri, result1, out result);
        result = result1;
        return true;
      }
      result = (Uri) null;
      return false;
    }

    /// <summary>Creates a new <see cref="T:System.Uri" /> using the specified base and relative <see cref="T:System.Uri" /> instances.</summary>
    /// <param name="baseUri">The base URI.</param>
    /// <param name="relativeUri">The relative URI to add to the base <see cref="T:System.Uri" />.</param>
    /// <param name="result">When this method returns, contains a <see cref="T:System.Uri" /> constructed from <paramref name="baseUri" /> and <paramref name="relativeUri" />. This parameter is passed uninitialized.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="baseUri" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the <see cref="T:System.Uri" /> was successfully created; otherwise, <see langword="false" />.</returns>
    public static bool TryCreate(Uri? baseUri, Uri? relativeUri, [NotNullWhen(true)] out Uri? result)
    {
      result = (Uri) null;
      if ((object) baseUri == null || (object) relativeUri == null || baseUri.IsNotAbsoluteUri)
        return false;
      UriFormatException parsingError = (UriFormatException) null;
      string newUriString = (string) null;
      bool userEscaped;
      if (baseUri.Syntax.IsSimple)
      {
        userEscaped = relativeUri.UserEscaped;
        result = Uri.ResolveHelper(baseUri, relativeUri, ref newUriString, ref userEscaped);
      }
      else
      {
        userEscaped = false;
        newUriString = baseUri.Syntax.InternalResolve(baseUri, relativeUri, out parsingError);
        if (parsingError != null)
          return false;
      }
      if ((object) result == null)
        result = Uri.CreateHelper(newUriString, userEscaped, UriKind.Absolute, ref parsingError);
      return parsingError == null && result != (Uri) null && result.IsAbsoluteUri;
    }

    /// <summary>Gets the specified components of the current instance using the specified escaping for special characters.</summary>
    /// <param name="components">A bitwise combination of the <see cref="T:System.UriComponents" /> values that specifies which parts of the current instance to return to the caller.</param>
    /// <param name="format">One of the enumeration values that controls how special characters are escaped.</param>
    /// <exception cref="T:System.ArgumentOutOfRangeException">
    /// <paramref name="components" /> is not a combination of valid <see cref="T:System.UriComponents" /> values.</exception>
    /// <exception cref="T:System.InvalidOperationException">The current <see cref="T:System.Uri" /> is not an absolute URI. Relative URIs cannot be used with this method.</exception>
    /// <returns>The components of the current instance.</returns>
    public string GetComponents(UriComponents components, UriFormat format)
    {
      return !this.DisablePathAndQueryCanonicalization || (components & UriComponents.PathAndQuery) == (UriComponents) 0 ? this.InternalGetComponents(components, format) : throw new InvalidOperationException(SR.net_uri_GetComponentsCalledWhenCanonicalizationDisabled);
    }

    #nullable disable
    private string InternalGetComponents(UriComponents components, UriFormat format)
    {
      if ((components & UriComponents.SerializationInfoString) != (UriComponents) 0 && components != UriComponents.SerializationInfoString)
        throw new ArgumentOutOfRangeException(nameof (components), (object) components, SR.net_uri_NotJustSerialization);
      if ((format & ~UriFormat.SafeUnescaped) != (UriFormat) 0)
        throw new ArgumentOutOfRangeException(nameof (format));
      if (this.IsNotAbsoluteUri)
      {
        if (components == UriComponents.SerializationInfoString)
          return this.GetRelativeSerializationString(format);
        throw new InvalidOperationException(SR.net_uri_NotAbsolute);
      }
      return this.Syntax.IsSimple ? this.GetComponentsHelper(components, format) : this.Syntax.InternalGetComponents(this, components, format);
    }

    #nullable enable
    /// <summary>Compares the specified parts of two URIs using the specified comparison rules.</summary>
    /// <param name="uri1">The first URI.</param>
    /// <param name="uri2">The second URI.</param>
    /// <param name="partsToCompare">A bitwise combination of the <see cref="T:System.UriComponents" /> values that specifies the parts of <paramref name="uri1" /> and <paramref name="uri2" /> to compare.</param>
    /// <param name="compareFormat">One of the enumeration values that specifies the character escaping used when the URI components are compared.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the culture, case, and sort rules for the comparison.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="comparisonType" /> is not a valid <see cref="T:System.StringComparison" /> value.</exception>
    /// <returns>A value that indicates the lexical relationship between the compared <see cref="T:System.Uri" /> components.
    /// 
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description><paramref name="uri1" /> is less than <paramref name="uri2" />.</description></item><item><term> Zero</term><description><paramref name="uri1" /> equals <paramref name="uri2" />.</description></item><item><term> Greater than zero</term><description><paramref name="uri1" /> is greater than <paramref name="uri2" />.</description></item></list></returns>
    public static int Compare(
      Uri? uri1,
      Uri? uri2,
      UriComponents partsToCompare,
      UriFormat compareFormat,
      StringComparison comparisonType)
    {
      if ((object) uri1 == null)
        return (object) uri2 == null ? 0 : -1;
      if ((object) uri2 == null)
        return 1;
      if (uri1.IsAbsoluteUri && uri2.IsAbsoluteUri)
        return string.Compare(uri1.GetParts(partsToCompare, compareFormat), uri2.GetParts(partsToCompare, compareFormat), comparisonType);
      if (uri1.IsAbsoluteUri)
        return 1;
      return !uri2.IsAbsoluteUri ? string.Compare(uri1.OriginalString, uri2.OriginalString, comparisonType) : -1;
    }

    /// <summary>Indicates whether the string used to construct this <see cref="T:System.Uri" /> was well-formed and does not require further escaping.</summary>
    /// <returns>
    /// <see langword="true" /> if the string was well-formed; otherwise, <see langword="false" />.</returns>
    public bool IsWellFormedOriginalString()
    {
      return this.IsNotAbsoluteUri || this.Syntax.IsSimple ? this.InternalIsWellFormedOriginalString() : this.Syntax.InternalIsWellFormedOriginalString(this);
    }

    /// <summary>Indicates whether the string is well-formed by attempting to construct a URI with the string and ensures that the string does not require further escaping.</summary>
    /// <param name="uriString">The string used to attempt to construct a <see cref="T:System.Uri" />.</param>
    /// <param name="uriKind">The type of the <see cref="T:System.Uri" /> in <paramref name="uriString" />.</param>
    /// <returns>
    /// <see langword="true" /> if the string was well-formed; otherwise, <see langword="false" />.</returns>
    public static bool IsWellFormedUriString([NotNullWhen(true), StringSyntax("Uri", new object[] {"uriKind"})] string? uriString, UriKind uriKind)
    {
      Uri result;
      return Uri.TryCreate(uriString, uriKind, out result) && result.IsWellFormedOriginalString();
    }

    internal unsafe bool InternalIsWellFormedOriginalString()
    {
      if (this.UserDrivenParsing)
        throw new InvalidOperationException(SR.Format(SR.net_uri_UserDrivenParsing, (object) this.GetType()));
      string str1 = this._string;
      IntPtr num;
      if (str1 == null)
      {
        num = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &str1.GetPinnableReference())
          num = (IntPtr) chPtr;
      }
      char* str2 = (char*) num;
      int idx = 0;
      if (!this.IsAbsoluteUri)
        return !Uri.CheckForColonInFirstPathSegment(this._string) && (this.CheckCanonical(str2, ref idx, this._string.Length, '\uFFFE') & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath)) == Uri.Check.EscapedCanonical;
      if (this.IsImplicitFile)
        return false;
      this.EnsureParseRemaining();
      Uri.Flags flags = this._flags & (Uri.Flags.E_CannotDisplayCanonical | Uri.Flags.IriCanonical);
      if ((flags & Uri.Flags.IriCanonical) != Uri.Flags.Zero)
      {
        if ((flags & (Uri.Flags.E_UserNotCanonical | Uri.Flags.UserIriCanonical)) == (Uri.Flags.E_UserNotCanonical | Uri.Flags.UserIriCanonical))
          flags &= ~(Uri.Flags.E_UserNotCanonical | Uri.Flags.UserIriCanonical);
        if ((flags & (Uri.Flags.E_PathNotCanonical | Uri.Flags.PathIriCanonical)) == (Uri.Flags.E_PathNotCanonical | Uri.Flags.PathIriCanonical))
          flags &= ~(Uri.Flags.E_PathNotCanonical | Uri.Flags.PathIriCanonical);
        if ((flags & (Uri.Flags.E_QueryNotCanonical | Uri.Flags.QueryIriCanonical)) == (Uri.Flags.E_QueryNotCanonical | Uri.Flags.QueryIriCanonical))
          flags &= ~(Uri.Flags.E_QueryNotCanonical | Uri.Flags.QueryIriCanonical);
        if ((flags & (Uri.Flags.E_FragmentNotCanonical | Uri.Flags.FragmentIriCanonical)) == (Uri.Flags.E_FragmentNotCanonical | Uri.Flags.FragmentIriCanonical))
          flags &= ~(Uri.Flags.E_FragmentNotCanonical | Uri.Flags.FragmentIriCanonical);
      }
      if ((flags & Uri.Flags.E_CannotDisplayCanonical & (Uri.Flags.E_UserNotCanonical | Uri.Flags.E_PathNotCanonical | Uri.Flags.E_QueryNotCanonical | Uri.Flags.E_FragmentNotCanonical)) != Uri.Flags.Zero)
        return false;
      if (this.InFact(Uri.Flags.AuthorityFound))
      {
        int index1 = (int) this._info.Offset.Scheme + this._syntax.SchemeName.Length + 2;
        int index2;
        if (index1 >= (int) this._info.Offset.User || this._string[index1 - 1] == '\\' || this._string[index1] == '\\' || this.InFact(Uri.Flags.DosPath | Uri.Flags.UncPath) && (index2 = index1 + 1) < (int) this._info.Offset.User && (this._string[index2] == '/' || this._string[index2] == '\\'))
          return false;
      }
      if (this.InFact(Uri.Flags.FirstSlashAbsent) && (int) this._info.Offset.Query > (int) this._info.Offset.Path || this.InFact(Uri.Flags.BackslashInPath) || this.IsDosPath && this._string[(int) this._info.Offset.Path + this.SecuredPathIndex - 1] == '|')
        return false;
      if ((this._flags & Uri.Flags.CanonicalDnsHost) == Uri.Flags.Zero && this.HostType != Uri.Flags.IPv6HostType)
      {
        int user = (int) this._info.Offset.User;
        Uri.Check check = this.CheckCanonical(str2, ref user, (int) this._info.Offset.Path, '/');
        if ((check & (Uri.Check.EscapedCanonical | Uri.Check.BackslashInPath | Uri.Check.ReservedFound)) != Uri.Check.EscapedCanonical && (!this.IriParsing || (check & (Uri.Check.DisplayCanonical | Uri.Check.NotIriCanonical | Uri.Check.FoundNonAscii)) != (Uri.Check.DisplayCanonical | Uri.Check.FoundNonAscii)))
          return false;
      }
      if ((this._flags & (Uri.Flags.SchemeNotCanonical | Uri.Flags.AuthorityFound)) == (Uri.Flags.SchemeNotCanonical | Uri.Flags.AuthorityFound))
      {
        int length = this._syntax.SchemeName.Length;
        do
          ;
        while (str2[length++] != ':');
        if (length + 1 >= this._string.Length || str2[length] != '/' || str2[length + 1] != '/')
          return false;
      }
      // ISSUE: fixed variable is out of scope
      // ISSUE: __unpin statement
      __unpin(chPtr);
      return true;
    }

    /// <summary>Converts a string to its unescaped representation.</summary>
    /// <param name="stringToUnescape">The string to unescape.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stringToUnescape" /> is <see langword="null" />.</exception>
    /// <returns>The unescaped representation of <paramref name="stringToUnescape" />.</returns>
    public static string UnescapeDataString(string stringToUnescape)
    {
      ArgumentNullException.ThrowIfNull((object) stringToUnescape, nameof (stringToUnescape));
      if (stringToUnescape.Length == 0)
        return string.Empty;
      int num = stringToUnescape.IndexOf('%');
      if (num == -1)
        return stringToUnescape;
      ValueStringBuilder dest = new ValueStringBuilder(stackalloc char[512]);
      dest.EnsureCapacity(stringToUnescape.Length);
      dest.Append(stringToUnescape.AsSpan(0, num));
      UriHelper.UnescapeString(stringToUnescape, num, stringToUnescape.Length, ref dest, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.Unescape | UnescapeMode.UnescapeAll, (UriParser) null, false);
      return dest.ToString();
    }

    /// <summary>Converts a URI string to its escaped representation.</summary>
    /// <param name="stringToEscape">The string to escape.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stringToEscape" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">The length of <paramref name="stringToEscape" /> exceeds 32766 characters.
    /// 
    /// Note: In .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.</exception>
    /// <returns>The escaped representation of <paramref name="stringToEscape" />.</returns>
    [Obsolete("Uri.EscapeUriString can corrupt the Uri string in some cases. Consider using Uri.EscapeDataString for query string components instead.", DiagnosticId = "SYSLIB0013", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    public static string EscapeUriString(string stringToEscape)
    {
      return UriHelper.EscapeString(stringToEscape, false, UriHelper.UnreservedReserved);
    }

    /// <summary>Converts a string to its escaped representation.</summary>
    /// <param name="stringToEscape">The string to escape.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="stringToEscape" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.UriFormatException">Note: In the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.FormatException" />, instead.
    /// 
    /// The length of <paramref name="stringToEscape" /> exceeds 32766 characters.</exception>
    /// <returns>The escaped representation of <paramref name="stringToEscape" />.</returns>
    public static string EscapeDataString(string stringToEscape)
    {
      return UriHelper.EscapeString(stringToEscape, false, UriHelper.Unreserved);
    }

    #nullable disable
    internal unsafe string EscapeUnescapeIri(
      string input,
      int start,
      int end,
      UriComponents component)
    {
      IntPtr pInput;
      if (input == null)
      {
        pInput = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &input.GetPinnableReference())
          pInput = (IntPtr) chPtr;
      }
      return IriHelper.EscapeUnescapeIri((char*) pInput, start, end, component);
    }

    private Uri(Uri.Flags flags, UriParser uriParser, string uri)
    {
      this._flags = flags;
      this._syntax = uriParser;
      this._string = uri;
    }

    internal static Uri CreateHelper(
      string uriString,
      bool dontEscape,
      UriKind uriKind,
      ref UriFormatException e,
      in UriCreationOptions creationOptions = default (UriCreationOptions))
    {
      if (uriKind < UriKind.RelativeOrAbsolute || uriKind > UriKind.Relative)
        throw new ArgumentException(SR.Format(SR.net_uri_InvalidUriKind, (object) uriKind));
      UriParser syntax = (UriParser) null;
      Uri.Flags flags = Uri.Flags.Zero;
      ParsingError scheme = Uri.ParseScheme(uriString, ref flags, ref syntax);
      if (dontEscape)
        flags |= Uri.Flags.UserEscaped;
      if (creationOptions.DangerousDisablePathAndQueryCanonicalization)
        flags |= Uri.Flags.DisablePathAndQueryCanonicalization;
      if (scheme != ParsingError.None)
        return uriKind != UriKind.Absolute && scheme <= ParsingError.EmptyUriString ? new Uri(flags & Uri.Flags.UserEscaped, (UriParser) null, uriString) : (Uri) null;
      Uri uri = new Uri(flags, syntax, uriString);
      try
      {
        uri.InitializeUri(scheme, uriKind, out e);
        return e == null ? uri : (Uri) null;
      }
      catch (UriFormatException ex)
      {
        e = ex;
        return (Uri) null;
      }
    }

    internal static Uri ResolveHelper(
      Uri baseUri,
      Uri relativeUri,
      ref string newUriString,
      ref bool userEscaped)
    {
      string relativeStr;
      if ((object) relativeUri != null)
      {
        if (relativeUri.IsAbsoluteUri)
          return relativeUri;
        relativeStr = relativeUri.OriginalString;
        userEscaped = relativeUri.UserEscaped;
      }
      else
        relativeStr = string.Empty;
      if (relativeStr.Length > 0 && (UriHelper.IsLWS(relativeStr[0]) || UriHelper.IsLWS(relativeStr[relativeStr.Length - 1])))
        relativeStr = relativeStr.Trim(UriHelper.s_WSchars);
      if (relativeStr.Length == 0)
      {
        newUriString = baseUri.GetParts(UriComponents.AbsoluteUri, baseUri.UserEscaped ? UriFormat.UriEscaped : UriFormat.SafeUnescaped);
        return (Uri) null;
      }
      if (relativeStr[0] == '#' && !baseUri.IsImplicitFile && baseUri.Syntax.InFact(UriSyntaxFlags.MayHaveFragment))
      {
        newUriString = baseUri.GetParts(UriComponents.HttpRequestUrl | UriComponents.UserInfo, UriFormat.UriEscaped) + relativeStr;
        return (Uri) null;
      }
      if (relativeStr[0] == '?' && !baseUri.IsImplicitFile && baseUri.Syntax.InFact(UriSyntaxFlags.MayHaveQuery))
      {
        newUriString = baseUri.GetParts(UriComponents.SchemeAndServer | UriComponents.UserInfo | UriComponents.Path, UriFormat.UriEscaped) + relativeStr;
        return (Uri) null;
      }
      if (relativeStr.Length >= 3 && (relativeStr[1] == ':' || relativeStr[1] == '|') && char.IsAsciiLetter(relativeStr[0]) && (relativeStr[2] == '\\' || relativeStr[2] == '/'))
      {
        if (baseUri.IsImplicitFile)
        {
          newUriString = relativeStr;
          return (Uri) null;
        }
        if (baseUri.Syntax.InFact(UriSyntaxFlags.AllowDOSPath))
        {
          string str = !baseUri.InFact(Uri.Flags.AuthorityFound) ? (baseUri.Syntax.InFact(UriSyntaxFlags.PathIsRooted) ? ":/" : ":") : (baseUri.Syntax.InFact(UriSyntaxFlags.PathIsRooted) ? ":///" : "://");
          newUriString = baseUri.Scheme + str + relativeStr;
          return (Uri) null;
        }
      }
      Uri.GetCombinedString(baseUri, relativeStr, userEscaped, ref newUriString);
      return (object) newUriString == (object) baseUri._string ? baseUri : (Uri) null;
    }

    private string GetRelativeSerializationString(UriFormat format)
    {
      switch (format)
      {
        case UriFormat.UriEscaped:
          return UriHelper.EscapeString(this._string, true, UriHelper.UnreservedReserved);
        case UriFormat.Unescaped:
          return Uri.UnescapeDataString(this._string);
        case UriFormat.SafeUnescaped:
          if (this._string.Length == 0)
            return string.Empty;
          ValueStringBuilder dest = new ValueStringBuilder(stackalloc char[512]);
          UriHelper.UnescapeString((ReadOnlySpan<char>) this._string, ref dest, char.MaxValue, char.MaxValue, char.MaxValue, UnescapeMode.EscapeUnescape, (UriParser) null, false);
          return dest.ToString();
        default:
          throw new ArgumentOutOfRangeException(nameof (format));
      }
    }

    internal string GetComponentsHelper(UriComponents uriComponents, UriFormat uriFormat)
    {
      if (uriComponents == UriComponents.Scheme)
        return this._syntax.SchemeName;
      if ((uriComponents & UriComponents.SerializationInfoString) != (UriComponents) 0)
        uriComponents |= UriComponents.AbsoluteUri;
      this.EnsureParseRemaining();
      if ((uriComponents & UriComponents.NormalizedHost) != (UriComponents) 0)
        uriComponents |= UriComponents.Host;
      if ((uriComponents & UriComponents.Host) != (UriComponents) 0)
        this.EnsureHostString(true);
      if (uriComponents == UriComponents.Port || uriComponents == UriComponents.StrongPort)
        return (this._flags & Uri.Flags.NotDefaultPort) != Uri.Flags.Zero || uriComponents == UriComponents.StrongPort && this._syntax.DefaultPort != -1 ? this._info.Offset.PortValue.ToString((IFormatProvider) CultureInfo.InvariantCulture) : string.Empty;
      if ((uriComponents & UriComponents.StrongPort) != (UriComponents) 0)
        uriComponents |= UriComponents.Port;
      if (uriComponents == UriComponents.Host && (uriFormat == UriFormat.UriEscaped || (this._flags & (Uri.Flags.HostNotCanonical | Uri.Flags.E_HostNotCanonical)) == Uri.Flags.Zero))
      {
        this.EnsureHostString(false);
        return this._info.Host;
      }
      switch (uriFormat)
      {
        case UriFormat.UriEscaped:
          return this.GetEscapedParts(uriComponents);
        case UriFormat.Unescaped:
        case UriFormat.SafeUnescaped:
        case (UriFormat) 32767:
          return this.GetUnescapedParts(uriComponents, uriFormat);
        default:
          throw new ArgumentOutOfRangeException(nameof (uriFormat));
      }
    }

    #nullable enable
    /// <summary>Determines whether the current <see cref="T:System.Uri" /> instance is a base of the specified <see cref="T:System.Uri" /> instance.</summary>
    /// <param name="uri">The specified URI to test.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="uri" /> is <see langword="null" />.</exception>
    /// <returns>
    /// <see langword="true" /> if the current <see cref="T:System.Uri" /> instance is a base of <paramref name="uri" />; otherwise, <see langword="false" />.</returns>
    public bool IsBaseOf(Uri uri)
    {
      ArgumentNullException.ThrowIfNull((object) uri, nameof (uri));
      if (!this.IsAbsoluteUri)
        return false;
      return this.Syntax.IsSimple ? this.IsBaseOfHelper(uri) : this.Syntax.InternalIsBaseOf(this, uri);
    }

    #nullable disable
    internal unsafe bool IsBaseOfHelper(Uri uriLink)
    {
      if (!this.IsAbsoluteUri || this.UserDrivenParsing)
        return false;
      if (!uriLink.IsAbsoluteUri)
      {
        string newUriString = (string) null;
        bool userEscaped = false;
        uriLink = Uri.ResolveHelper(this, uriLink, ref newUriString, ref userEscaped);
        if ((object) uriLink == null)
        {
          UriFormatException e = (UriFormatException) null;
          uriLink = Uri.CreateHelper(newUriString, userEscaped, UriKind.Absolute, ref e);
          if (e != null)
            return false;
        }
      }
      if (this.Syntax.SchemeName != uriLink.Syntax.SchemeName)
        return false;
      string parts1 = this.GetParts(UriComponents.HttpRequestUrl | UriComponents.UserInfo, UriFormat.SafeUnescaped);
      string parts2 = uriLink.GetParts(UriComponents.HttpRequestUrl | UriComponents.UserInfo, UriFormat.SafeUnescaped);
      IntPtr num1;
      if (parts1 == null)
      {
        num1 = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &parts1.GetPinnableReference())
          num1 = (IntPtr) chPtr;
      }
      char* selfPtr = (char*) num1;
      IntPtr num2;
      if (parts2 == null)
      {
        num2 = IntPtr.Zero;
      }
      else
      {
        fixed (char* chPtr = &parts2.GetPinnableReference())
          num2 = (IntPtr) chPtr;
      }
      char* otherPtr = (char*) num2;
      return UriHelper.TestForSubPath(selfPtr, parts1.Length, otherPtr, parts2.Length, this.IsUncOrDosPath || uriLink.IsUncOrDosPath);
    }

    [MemberNotNull("_string")]
    private void CreateThisFromUri(Uri otherUri)
    {
      this._info = (Uri.UriInfo) null;
      this._flags = otherUri._flags;
      if (this.InFact(Uri.Flags.MinimalUriInfoSet))
      {
        this._flags &= ~(Uri.Flags.IndexMask | Uri.Flags.MinimalUriInfoSet | Uri.Flags.AllUriInfoSet);
        int path = (int) otherUri._info.Offset.Path;
        if (this.InFact(Uri.Flags.NotDefaultPort))
        {
          while (otherUri._string[path] != ':' && path > (int) otherUri._info.Offset.Host)
            --path;
          if (otherUri._string[path] != ':')
            path = (int) otherUri._info.Offset.Path;
        }
        this._flags |= (Uri.Flags) path;
      }
      this._syntax = otherUri._syntax;
      this._string = otherUri._string;
      this._originalUnicodeString = otherUri._originalUnicodeString;
    }

    [System.Flags]
    internal enum Flags : ulong
    {
      Zero = 0,
      SchemeNotCanonical = 1,
      UserNotCanonical = 2,
      HostNotCanonical = 4,
      PortNotCanonical = 8,
      PathNotCanonical = 16, // 0x0000000000000010
      QueryNotCanonical = 32, // 0x0000000000000020
      FragmentNotCanonical = 64, // 0x0000000000000040
      CannotDisplayCanonical = FragmentNotCanonical | QueryNotCanonical | PathNotCanonical | PortNotCanonical | HostNotCanonical | UserNotCanonical | SchemeNotCanonical, // 0x000000000000007F
      E_UserNotCanonical = 128, // 0x0000000000000080
      E_HostNotCanonical = 256, // 0x0000000000000100
      E_PortNotCanonical = 512, // 0x0000000000000200
      E_PathNotCanonical = 1024, // 0x0000000000000400
      E_QueryNotCanonical = 2048, // 0x0000000000000800
      E_FragmentNotCanonical = 4096, // 0x0000000000001000
      E_CannotDisplayCanonical = E_FragmentNotCanonical | E_QueryNotCanonical | E_PathNotCanonical | E_PortNotCanonical | E_HostNotCanonical | E_UserNotCanonical, // 0x0000000000001F80
      ShouldBeCompressed = 8192, // 0x0000000000002000
      FirstSlashAbsent = 16384, // 0x0000000000004000
      BackslashInPath = 32768, // 0x0000000000008000
      IndexMask = BackslashInPath | FirstSlashAbsent | ShouldBeCompressed | E_CannotDisplayCanonical | CannotDisplayCanonical, // 0x000000000000FFFF
      HostTypeMask = 458752, // 0x0000000000070000
      HostNotParsed = 0,
      IPv6HostType = 65536, // 0x0000000000010000
      IPv4HostType = 131072, // 0x0000000000020000
      DnsHostType = IPv4HostType | IPv6HostType, // 0x0000000000030000
      UncHostType = 262144, // 0x0000000000040000
      BasicHostType = UncHostType | IPv6HostType, // 0x0000000000050000
      UnusedHostType = UncHostType | IPv4HostType, // 0x0000000000060000
      UnknownHostType = UnusedHostType | IPv6HostType, // 0x0000000000070000
      UserEscaped = 524288, // 0x0000000000080000
      AuthorityFound = 1048576, // 0x0000000000100000
      HasUserInfo = 2097152, // 0x0000000000200000
      LoopbackHost = 4194304, // 0x0000000000400000
      NotDefaultPort = 8388608, // 0x0000000000800000
      UserDrivenParsing = 16777216, // 0x0000000001000000
      CanonicalDnsHost = 33554432, // 0x0000000002000000
      ErrorOrParsingRecursion = 67108864, // 0x0000000004000000
      DosPath = 134217728, // 0x0000000008000000
      UncPath = 268435456, // 0x0000000010000000
      ImplicitFile = 536870912, // 0x0000000020000000
      MinimalUriInfoSet = 1073741824, // 0x0000000040000000
      AllUriInfoSet = 2147483648, // 0x0000000080000000
      IdnHost = 4294967296, // 0x0000000100000000
      HasUnicode = 8589934592, // 0x0000000200000000
      HostUnicodeNormalized = 17179869184, // 0x0000000400000000
      RestUnicodeNormalized = 34359738368, // 0x0000000800000000
      UnicodeHost = 68719476736, // 0x0000001000000000
      IntranetUri = 137438953472, // 0x0000002000000000
      UserIriCanonical = 549755813888, // 0x0000008000000000
      PathIriCanonical = 1099511627776, // 0x0000010000000000
      QueryIriCanonical = 2199023255552, // 0x0000020000000000
      FragmentIriCanonical = 4398046511104, // 0x0000040000000000
      IriCanonical = FragmentIriCanonical | QueryIriCanonical | PathIriCanonical | UserIriCanonical, // 0x0000078000000000
      UnixPath = 17592186044416, // 0x0000100000000000
      DisablePathAndQueryCanonicalization = 35184372088832, // 0x0000200000000000
      CustomParser_ParseMinimalAlreadyCalled = 4611686018427387904, // 0x4000000000000000
      Debug_LeftConstructor = 9223372036854775808, // 0x8000000000000000
    }

    private sealed class UriInfo
    {
      public Uri.Offset Offset;
      public string String;
      public string Host;
      public string IdnHost;
      public string PathAndQuery;
      public string ScopeId;
      private Uri.MoreInfo _moreInfo;

      public Uri.MoreInfo MoreInfo
      {
        get
        {
          if (this._moreInfo == null)
            Interlocked.CompareExchange<Uri.MoreInfo>(ref this._moreInfo, new Uri.MoreInfo(), (Uri.MoreInfo) null);
          return this._moreInfo;
        }
      }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct Offset
    {
      public ushort Scheme;
      public ushort User;
      public ushort Host;
      public ushort PortValue;
      public ushort Path;
      public ushort Query;
      public ushort Fragment;
      public ushort End;
    }

    private sealed class MoreInfo
    {
      public string Path;
      public string Query;
      public string Fragment;
      public string AbsoluteUri;
      public string RemoteUrl;
    }

    [System.Flags]
    private enum Check
    {
      None = 0,
      EscapedCanonical = 1,
      DisplayCanonical = 2,
      DotSlashAttn = 4,
      DotSlashEscaped = 128, // 0x00000080
      BackslashInPath = 16, // 0x00000010
      ReservedFound = 32, // 0x00000020
      NotIriCanonical = 64, // 0x00000040
      FoundNonAscii = 8,
    }
  }
}
