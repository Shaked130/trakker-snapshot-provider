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
            // CR: (Kfir) Why ToList()?
            return System.IO.DriveInfo.GetDrives()
                .Where(drive => Filter.IsDriveTypeSupported(drive.DriveType) && drive.IsReady).ToList();
        }

        // CR: (Kfir) remove "checkDriveValidation" and apply code from https://stackoverflow.com/questions/358196/c-sharp-internal-access-modifier-when-doing-unit-testing
        //     to make the Scan class testable
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

            return Utilities.Scan.GetDriveInfo(driveName);
        }
    }
}
