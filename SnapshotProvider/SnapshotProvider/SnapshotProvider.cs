using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnapshotProvider.Utilities;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public class SnapshotProvider : ISnapshotProvider
    {

        /// <summary>
        /// Returns the valid drives that found.
        /// </summary>
        /// <returns> Valid drives </returns>
        public IEnumerable<System.IO.DriveInfo> GetDrivesMetadata()
        {
            return System.IO.DriveInfo.GetDrives()
                .Where(drive => Filter.IsDriveTypeSupported(drive.DriveType) && drive.IsReady).ToList();
        }


        /// <summary>
        /// The function scans the given drive and returns all the drive information.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <param name="checkDriveValidation"> Check if the drive is supported </param>
        /// <exception cref="Exceptions.DriveNotFoundException"> Thrown when the given drive doesn't exist </exception>
        /// <exception cref="Exceptions.DriveNotSupportedException"> Thrown when the given drive is not supported  </exception>
        /// <returns> A DriveInfo object with all the data </returns>
        public TrakkerModels.DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation = true)
        {
            if (!Directory.Exists(driveName))
            {
                throw new Exceptions.DriveNotFoundException(driveName);
            }
            
            if (checkDriveValidation && !Utilities.Filter.IsDriveSupported(driveName))
            {
                throw new Exceptions.DriveNotSupportedException(driveName);
            }

            var drive = new TrakkerModels.DirectoryInfo(driveName);
            // CR: I think you need to find a way to return a new drive and not change it inside, it will be better looking.
            // CR: (Kfir) I agree with Dvir about that. You can do the recursion with another method
            //     that returns DirectoryInfo, but the main method should return DriveInfo
            Utilities.Scan.ScanDrive(drive);
            drive.Size = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            return new TrakkerModels.DriveInfo(driveName, drive, drive.Size);
        }
    }
}
