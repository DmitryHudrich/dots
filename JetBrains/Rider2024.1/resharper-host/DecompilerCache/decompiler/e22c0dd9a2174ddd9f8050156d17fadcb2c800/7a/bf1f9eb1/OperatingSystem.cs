// Decompiled with JetBrains decompiler
// Type: System.OperatingSystem
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

#nullable enable
namespace System
{
  /// <summary>Represents information about an operating system, such as the version and platform identifier. This class cannot be inherited.</summary>
  public sealed class OperatingSystem : ISerializable, ICloneable
  {
    #nullable disable
    private readonly Version _version;
    private readonly PlatformID _platform;
    private readonly string _servicePack;
    private string _versionString;

    #nullable enable
    /// <summary>Initializes a new instance of the <see cref="T:System.OperatingSystem" /> class, using the specified platform identifier value and version object.</summary>
    /// <param name="platform">One of the <see cref="T:System.PlatformID" /> values that indicates the operating system platform.</param>
    /// <param name="version">A <see cref="T:System.Version" /> object that indicates the version of the operating system.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="version" /> is <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="platform" /> is not a <see cref="T:System.PlatformID" /> enumeration value.</exception>
    public OperatingSystem(PlatformID platform, Version version)
      : this(platform, version, (string) null)
    {
    }

    #nullable disable
    internal OperatingSystem(PlatformID platform, Version version, string servicePack)
    {
      if (platform < PlatformID.Win32S || platform > PlatformID.Other)
        throw new ArgumentOutOfRangeException(nameof (platform), (object) platform, SR.Format(SR.Arg_EnumIllegalVal, (object) platform));
      ArgumentNullException.ThrowIfNull((object) version, nameof (version));
      this._platform = platform;
      this._version = version;
      this._servicePack = servicePack;
    }

    #nullable enable
    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data necessary to deserialize this instance.</summary>
    /// <param name="info">The object to populate with serialization information.</param>
    /// <param name="context">The place to store and retrieve serialized data. Reserved for future use.</param>
    /// <exception cref="T:System.ArgumentNullException">
    /// <paramref name="info" /> is <see langword="null" />.</exception>
    [Obsolete("This API supports obsolete formatter-based serialization. It should not be called or extended by application code.", DiagnosticId = "SYSLIB0051", UrlFormat = "https://aka.ms/dotnet-warnings/{0}")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      throw new PlatformNotSupportedException();
    }

    /// <summary>Gets a <see cref="T:System.PlatformID" /> enumeration value that identifies the operating system platform.</summary>
    /// <returns>One of the <see cref="T:System.PlatformID" /> values.</returns>
    public PlatformID Platform => this._platform;

    /// <summary>Gets the service pack version represented by this <see cref="T:System.OperatingSystem" /> object.</summary>
    /// <returns>The service pack version, if service packs are supported and at least one is installed; otherwise, an empty string ("").</returns>
    public string ServicePack => this._servicePack ?? string.Empty;

    /// <summary>Gets a <see cref="T:System.Version" /> object that identifies the operating system.</summary>
    /// <returns>A <see cref="T:System.Version" /> object that describes the major version, minor version, build, and revision numbers for the operating system.</returns>
    public Version Version => this._version;

    /// <summary>Creates an <see cref="T:System.OperatingSystem" /> object that is identical to this instance.</summary>
    /// <returns>An <see cref="T:System.OperatingSystem" /> object that is a copy of this instance.</returns>
    public object Clone()
    {
      return (object) new OperatingSystem(this._platform, this._version, this._servicePack);
    }

    /// <summary>Converts the value of this <see cref="T:System.OperatingSystem" /> object to its equivalent string representation.</summary>
    /// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
    public override string ToString() => this.VersionString;

    /// <summary>Gets the concatenated string representation of the platform identifier, version, and service pack that are currently installed on the operating system.</summary>
    /// <returns>The string representation of the values returned by the <see cref="P:System.OperatingSystem.Platform" />, <see cref="P:System.OperatingSystem.Version" />, and <see cref="P:System.OperatingSystem.ServicePack" /> properties.</returns>
    public string VersionString
    {
      get
      {
        if (this._versionString == null)
        {
          string str1;
          switch (this._platform)
          {
            case PlatformID.Win32S:
              str1 = "Microsoft Win32S ";
              break;
            case PlatformID.Win32Windows:
              str1 = this._version.Major > 4 || this._version.Major == 4 && this._version.Minor > 0 ? "Microsoft Windows 98 " : "Microsoft Windows 95 ";
              break;
            case PlatformID.Win32NT:
              str1 = "Microsoft Windows NT ";
              break;
            case PlatformID.WinCE:
              str1 = "Microsoft Windows CE ";
              break;
            case PlatformID.Unix:
              str1 = "Unix ";
              break;
            case PlatformID.Xbox:
              str1 = "Xbox ";
              break;
            case PlatformID.MacOSX:
              str1 = "Mac OS X ";
              break;
            case PlatformID.Other:
              str1 = "Other ";
              break;
            default:
              str1 = "<unknown> ";
              break;
          }
          Span<char> span = stackalloc char[128];
          string str2;
          if (!string.IsNullOrEmpty(this._servicePack))
          {
            IFormatProvider provider1 = (IFormatProvider) null;
            IFormatProvider provider2 = provider1;
            Span<char> initialBuffer1 = span;
            Span<char> initialBuffer2 = initialBuffer1;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 3, provider1, initialBuffer1);
            interpolatedStringHandler.AppendFormatted(str1);
            interpolatedStringHandler.AppendFormatted(this._version.ToString(3));
            interpolatedStringHandler.AppendLiteral(" ");
            interpolatedStringHandler.AppendFormatted(this._servicePack);
            ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
            str2 = string.Create(provider2, initialBuffer2, ref local);
          }
          else
          {
            IFormatProvider provider3 = (IFormatProvider) null;
            IFormatProvider provider4 = provider3;
            Span<char> initialBuffer3 = span;
            Span<char> initialBuffer4 = initialBuffer3;
            DefaultInterpolatedStringHandler interpolatedStringHandler = new DefaultInterpolatedStringHandler(0, 2, provider3, initialBuffer3);
            interpolatedStringHandler.AppendFormatted(str1);
            interpolatedStringHandler.AppendFormatted<Version>(this._version);
            ref DefaultInterpolatedStringHandler local = ref interpolatedStringHandler;
            str2 = string.Create(provider4, initialBuffer4, ref local);
          }
          this._versionString = str2;
        }
        return this._versionString;
      }
    }

    /// <summary>Indicates whether the current application is running on the specified platform.</summary>
    /// <param name="platform">The case-insensitive platform name. Examples: Browser, Linux, FreeBSD, Android, iOS, macOS, tvOS, watchOS, Windows.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on the specified platform; <see langword="false" /> otherwise.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOSPlatform(string platform)
    {
      ArgumentNullException.ThrowIfNull((object) platform, nameof (platform));
      return platform.Equals("LINUX", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Checks if the operating system version is greater than or equal to the specified platform version. This method can be used to guard APIs that were added in the specified OS version.</summary>
    /// <param name="platform">The case-insensitive platform name. Examples: Browser, Linux, FreeBSD, Android, iOS, macOS, tvOS, watchOS, Windows.</param>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number (optional).</param>
    /// <param name="build">The build release number (optional).</param>
    /// <param name="revision">The revision release number (optional).</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on the specified platform and is at least in the version specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsOSPlatformVersionAtLeast(
      string platform,
      int major,
      int minor = 0,
      int build = 0,
      int revision = 0)
    {
      return OperatingSystem.IsOSPlatform(platform) && OperatingSystem.IsOSVersionAtLeast(major, minor, build, revision);
    }

    /// <summary>Indicates whether the current application is running as WASM in a browser.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running as WASM in a browser; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsBrowser() => false;

    /// <summary>Indicates whether the current application is running as WASI.</summary>
    [NonVersionable]
    public static bool IsWasi() => false;

    /// <summary>Indicates whether the current application is running on Linux.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on Linux; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsLinux() => true;

    /// <summary>Indicates whether the current application is running on FreeBSD.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on FreeBSD; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsFreeBSD() => false;

    /// <summary>Checks if the FreeBSD version (returned by the Linux command <c>uname</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <param name="revision">The revision release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on a FreeBSD version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsFreeBSDVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on Android.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on Android; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsAndroid() => false;

    /// <summary>Checks if the Android version (returned by the Linux command <c>uname</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <param name="revision">The revision release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on an Android version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsAndroidVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on iOS or MacCatalyst.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on iOS or MacCatalyst; <see langword="false" /> otherwise.</returns>
    [SupportedOSPlatformGuard("maccatalyst")]
    [NonVersionable]
    public static bool IsIOS() => false;

    /// <summary>Checks if the iOS/MacCatalyst version (returned by <c>libobjc.get_operatingSystemVersion</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified iOS version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on an iOS/MacCatalyst version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    [SupportedOSPlatformGuard("maccatalyst")]
    [NonVersionable]
    public static bool IsIOSVersionAtLeast(int major, int minor = 0, int build = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on macOS.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on macOS; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsMacOS() => false;

    /// <summary>Checks if the macOS version (returned by <c>libobjc.get_operatingSystemVersion</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified macOS version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on an macOS version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsMacOSVersionAtLeast(int major, int minor = 0, int build = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on Mac Catalyst.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on Mac Catalyst; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsMacCatalyst() => false;

    /// <summary>Check for the Mac Catalyst version (iOS version as presented in Apple documentation) with a ≤ version comparison. Used to guard APIs that were added in the given Mac Catalyst release.</summary>
    /// <param name="major">The version major number.</param>
    /// <param name="minor">The version minor number.</param>
    /// <param name="build">The version build number.</param>
    /// <returns>
    /// <see langword="true" /> if the Mac Catalyst version is greater or equal than the specified version comparison; <see langword="false" /> otherwise.</returns>
    public static bool IsMacCatalystVersionAtLeast(int major, int minor = 0, int build = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on tvOS.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on tvOS; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsTvOS() => false;

    /// <summary>Checks if the tvOS version (returned by <c>libobjc.get_operatingSystemVersion</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified tvOS version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on a tvOS version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsTvOSVersionAtLeast(int major, int minor = 0, int build = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on watchOS.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on watchOS; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsWatchOS() => false;

    /// <summary>Checks if the watchOS version (returned by <c>libobjc.get_operatingSystemVersion</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified watchOS version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on a watchOS version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsWatchOSVersionAtLeast(int major, int minor = 0, int build = 0)
    {
      if (true)
        ;
      return false;
    }

    /// <summary>Indicates whether the current application is running on Windows.</summary>
    /// <returns>
    /// <see langword="true" /> if the current application is running on Windows; <see langword="false" /> otherwise.</returns>
    [NonVersionable]
    public static bool IsWindows() => false;

    /// <summary>Checks if the Windows version (returned by <c>RtlGetVersion</c>) is greater than or equal to the specified version. This method can be used to guard APIs that were added in the specified Windows version.</summary>
    /// <param name="major">The major release number.</param>
    /// <param name="minor">The minor release number.</param>
    /// <param name="build">The build release number.</param>
    /// <param name="revision">The revision release number.</param>
    /// <returns>
    /// <see langword="true" /> if the current application is running on a Windows version that is at least what was specified in the parameters; <see langword="false" /> otherwise.</returns>
    public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
    {
      if (true)
        ;
      return false;
    }

    private static bool IsOSVersionAtLeast(int major, int minor, int build, int revision)
    {
      Version version = Environment.OSVersion.Version;
      if (version.Major != major)
        return version.Major > major;
      if (version.Minor != minor)
        return version.Minor > minor;
      if (version.Build != build)
        return version.Build > build;
      if (version.Revision >= revision)
        return true;
      return version.Revision == -1 && revision == 0;
    }
  }
}
