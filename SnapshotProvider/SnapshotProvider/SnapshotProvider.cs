using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using SnapshotProvider.Platforms;
using SnapshotProvider.Utilities;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public class SnapshotProvider : ISnapshotProvider
    {
        private readonly IPlatformHandler _platformHandler;

        public SnapshotProvider()
        {
            this._platformHandler = getPlatformHandler();
        }

        private IPlatformHandler getPlatformHandler()
        {
            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new WindowsPlatformHandler();
            }

            //TODO: Change this line
            return new WindowsPlatformHandler();
        }

        /// <summary>
        /// Returns the valid drives that found.
        /// </summary>
        /// <returns> Valid drives </returns>
        public List<System.IO.DriveInfo> GetDrivesMetadata()
        {
            return System.IO.DriveInfo.GetDrives()
                .Where(drive => Filter.IsDriveTypeSupported(drive.DriveType) && drive.IsReady).ToList();
        }


        /// <summary>
        /// The function scans the given drive and returns all the drive information.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <param name="checkDriveValidation"> Check if the drive is supported </param>
        /// <returns> A DriveInfo object with all the data </returns>
        public TrakkerModels.DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation = true)
        {
            if (!Directory.Exists(driveName))
            {
                throw new Utilities.Exceptions.DriveNotFoundException(driveName);
            }

            if (checkDriveValidation && Utilities.Filter.IsDriveSupported(driveName, GetDrivesMetadata()))
            {
                throw new Utilities.Exceptions.DriveNotSupportedException(driveName);
            }

            var drive = new TrakkerModels.DirectoryInfo(driveName);
            Utilities.Scan.DirectorySearch(drive);
            drive.Size = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            return new TrakkerModels.DriveInfo(driveName, drive, drive.Size);
        }

        public List<ProgramInfo> GetInstalledApps()
        {
            return this._platformHandler.GetInstalledPrograms();
        }
    }
}
