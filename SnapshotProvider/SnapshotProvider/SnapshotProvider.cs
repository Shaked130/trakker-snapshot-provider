using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnapshotProvider.Utilities;
using TrakkerModels;

namespace SnapshotProvider
{
    public class SnapshotProvider : ISnapshotProvider
    {

        /// <summary>
        /// Returns the valid drives that found.
        /// </summary>
        /// <returns> Valid drives </returns>
        public List<System.IO.DriveInfo> GetDrivesMetadata()
        {
            return System.IO.DriveInfo.GetDrives()
                .Where(drive => Filter.IsDriveSupported(drive.DriveType) && drive.IsReady).ToList();
        }


        /// <summary>
        /// The function scans the given drive and returns all the drive information.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <returns> A DriveInfo object with all the data </returns>
        // TODO: Improve the performance of the function! 
        public TrakkerModels.DriveInfo GetDriveInfo(string driveName)
        {
            if (!Directory.Exists(driveName))
            {
                throw new Utilities.Exceptions.DriveNotFoundException();
            }

            if (GetDrivesMetadata().All(driver => driver.Name != driveName))
            {
                throw new Utilities.Exceptions.DriveNotSupportedException();
            }

            var drive = new TrakkerModels.DirectoryInfo(driveName);
            Utilities.Scan.DirectorySearch(ref drive);
            // TODO: Replace this
            drive.Size = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            return new TrakkerModels.DriveInfo(driveName,drive);
        }
    }
}
