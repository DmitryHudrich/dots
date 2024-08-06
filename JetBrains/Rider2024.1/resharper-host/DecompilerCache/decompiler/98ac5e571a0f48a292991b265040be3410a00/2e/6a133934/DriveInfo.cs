// Decompiled with JetBrains decompiler
// Type: System.IO.DriveInfo
// Assembly: System.IO.FileSystem.DriveInfo, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 98AC5E57-1A0F-48A2-9299-1B265040BE34
// Assembly location: /usr/share/dotnet/shared/Microsoft.NETCore.App/8.0.6/System.IO.FileSystem.DriveInfo.dll
// XML documentation location: /usr/share/dotnet/packs/Microsoft.NETCore.App.Ref/8.0.6/ref/net8.0/System.IO.FileSystem.DriveInfo.xml

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using System.Runtime.Versioning;

#nullable enable
namespace System.IO
{
  /// <summary>Provides access to information on a drive.</summary>
  public sealed class DriveInfo : ISerializable
  {
    #nullable disable
    private readonly string _name;

    #nullable enable
    /// <summary>Creates a new instance of the <see cref="T:System.IO.DriveInfo" /> class.</summary>
    /// <param name="driveName">A valid drive letter or fully qualified path.</param>
    /// <exception cref="T:System.ArgumentNullException">The drive letter cannot be <see langword="null" />.</exception>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="driveName" /> does not refer to a valid drive.</exception>
    public DriveInfo(string driveName)
    {
      ArgumentNullException.ThrowIfNull((object) driveName, nameof (driveName));
      this._name = DriveInfo.NormalizeDriveName(driveName);
    }

    #nullable disable
    /// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the data needed to serialize the target object.</summary>
    /// <param name="info">The object to populate with data.</param>
    /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for this serialization.</param>
    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
      throw new PlatformNotSupportedException();
    }

    #nullable enable
    /// <summary>Gets the name of a drive, such as C:\.</summary>
    /// <returns>The name of the drive.</returns>
    public string Name => this._name;

    /// <summary>Gets a value that indicates whether a drive is ready.</summary>
    /// <returns>
    /// <see langword="true" /> if the drive is ready; <see langword="false" /> if the drive is not ready.</returns>
    public bool IsReady => Directory.Exists(this.Name);

    /// <summary>Gets the root directory of a drive.</summary>
    /// <returns>An object that contains the root directory of the drive.</returns>
    public DirectoryInfo RootDirectory => new DirectoryInfo(this.Name);

    /// <summary>Returns a drive name as a string.</summary>
    /// <returns>The name of the drive.</returns>
    public override string ToString() => this.Name;

    /// <summary>Retrieves the drive names of all logical drives on a computer.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The caller does not have the required permission.</exception>
    /// <returns>An array of type <see cref="T:System.IO.DriveInfo" /> that represents the logical drives on a computer.</returns>
    public static DriveInfo[] GetDrives()
    {
      string[] mountPoints = DriveInfo.GetMountPoints();
      DriveInfo[] drives = new DriveInfo[mountPoints.Length];
      for (int index = 0; index < drives.Length; ++index)
        drives[index] = new DriveInfo(mountPoints[index]);
      return drives;
    }

    #nullable disable
    private static string NormalizeDriveName(string driveName)
    {
      if (driveName.Contains(char.MinValue))
        throw new ArgumentException(SR.Format(SR.Arg_InvalidDriveChars, (object) driveName), nameof (driveName));
      return driveName.Length != 0 ? driveName : throw new ArgumentException(SR.Arg_MustBeNonEmptyDriveName, nameof (driveName));
    }

    #nullable enable
    /// <summary>Gets or sets the volume label of a drive.</summary>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission.</exception>
    /// <exception cref="T:System.UnauthorizedAccessException">The volume label is being set on a network or CD-ROM drive.
    /// 
    /// -or-
    /// 
    /// Access to the drive information is denied.</exception>
    /// <returns>The volume label.</returns>
    public string VolumeLabel
    {
      get => this.Name;
      [SupportedOSPlatform("windows")] [param: AllowNull] set
      {
        throw new PlatformNotSupportedException();
      }
    }

    /// <summary>Gets the drive type, such as CD-ROM, removable, network, or fixed.</summary>
    /// <returns>One of the enumeration values that specifies a drive type.</returns>
    public DriveType DriveType
    {
      get
      {
        DriveType type;
        if (Interop.Sys.GetFormatInfoForMountPoint(this.Name, out type) == 0)
          return type;
        switch (Interop.Sys.GetLastErrorInfo().Error)
        {
          case Interop.Error.ELOOP:
          case Interop.Error.ENAMETOOLONG:
          case Interop.Error.ENOENT:
          case Interop.Error.ENOTDIR:
            return DriveType.NoRootDirectory;
          default:
            return DriveType.Unknown;
        }
      }
    }

    /// <summary>Gets the name of the file system, such as NTFS or FAT32.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive does not exist or is not mapped.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The name of the file system on the specified drive.</returns>
    public string DriveFormat
    {
      get
      {
        string format;
        this.CheckStatfsResultAndThrowIfNecessary(Interop.Sys.GetFormatInfoForMountPoint(this.Name, out format));
        return format;
      }
    }

    /// <summary>Indicates the amount of available free space on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The amount of free space available on the drive, in bytes.</returns>
    public long AvailableFreeSpace
    {
      get
      {
        Interop.Sys.MountPointInformation mpi;
        this.CheckStatfsResultAndThrowIfNecessary(Interop.Sys.GetSpaceInfoForMountPoint(this.Name, out mpi));
        return checked ((long) mpi.AvailableFreeSpace);
      }
    }

    /// <summary>Gets the total amount of free space available on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The total free space available on a drive, in bytes.</returns>
    public long TotalFreeSpace
    {
      get
      {
        Interop.Sys.MountPointInformation mpi;
        this.CheckStatfsResultAndThrowIfNecessary(Interop.Sys.GetSpaceInfoForMountPoint(this.Name, out mpi));
        return checked ((long) mpi.TotalFreeSpace);
      }
    }

    /// <summary>Gets the total size of storage space on a drive, in bytes.</summary>
    /// <exception cref="T:System.UnauthorizedAccessException">Access to the drive information is denied.</exception>
    /// <exception cref="T:System.IO.DriveNotFoundException">The drive is not mapped or does not exist.</exception>
    /// <exception cref="T:System.IO.IOException">An I/O error occurred (for example, a disk error or a drive was not ready).</exception>
    /// <returns>The total size of the drive, in bytes.</returns>
    public long TotalSize
    {
      get
      {
        Interop.Sys.MountPointInformation mpi;
        this.CheckStatfsResultAndThrowIfNecessary(Interop.Sys.GetSpaceInfoForMountPoint(this.Name, out mpi));
        return checked ((long) mpi.TotalSize);
      }
    }

    private void CheckStatfsResultAndThrowIfNecessary(int result)
    {
      if (result == 0)
        return;
      Interop.ErrorInfo lastErrorInfo = Interop.Sys.GetLastErrorInfo();
      if (lastErrorInfo.Error == Interop.Error.ENOENT)
        throw new DriveNotFoundException(SR.Format(SR.IO_DriveNotFound_Drive, (object) this.Name));
      throw Interop.GetExceptionForIoErrno(lastErrorInfo);
    }

    #nullable disable
    private static string[] GetMountPoints() => Interop.Sys.GetAllMountPoints();
  }
}
