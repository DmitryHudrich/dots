// Decompiled with JetBrains decompiler
// Type: System.PlatformID
// Assembly: System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
// MVID: E22C0DD9-A217-4DDD-9F80-50156D17FADC
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.Private.CoreLib.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.Runtime.xml

using System.ComponentModel;

#nullable disable
namespace System
{
  /// <summary>Identifies the operating system, or platform, supported by an assembly.</summary>
  public enum PlatformID
  {
    /// <summary>The operating system is Win32s. This value is no longer in use.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)] Win32S,
    /// <summary>The operating system is Windows 95 or Windows 98. This value is no longer in use.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)] Win32Windows,
    /// <summary>The operating system is Windows NT or later.</summary>
    Win32NT,
    /// <summary>The operating system is Windows CE. This value is no longer in use.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)] WinCE,
    /// <summary>The operating system is Unix.</summary>
    Unix,
    /// <summary>The development platform is Xbox 360. This value is no longer in use.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)] Xbox,
    /// <summary>The operating system is Macintosh. This value was returned by Silverlight. On .NET Core, its replacement is Unix.</summary>
    [EditorBrowsable(EditorBrowsableState.Never)] MacOSX,
    /// <summary>Any other operating system. This includes Browser (WASM).</summary>
    Other,
  }
}
