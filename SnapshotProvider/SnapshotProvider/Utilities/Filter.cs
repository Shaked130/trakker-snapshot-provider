using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SnapshotProvider.Utilities
{
    // CR: This could be internal class the server shouldn't know about this.
    public static class Filter
    {
        /// <summary>
        /// The function checks if the given drive type is supported.
        /// </summary>
        /// <param name="driveType"> The drive type</param>
        /// <returns> true if the drive type is supported, false if not. </returns>
        public static bool IsDriveTypeSupported(DriveType driveType)
        {
            return driveType switch
            {
                DriveType.CDRom => false,
                DriveType.Network => false,
                DriveType.Removable => false,
                DriveType.Unknown => false,
                _ => true
            };
        }

        // CR: This function (IsDriveSupported) is a bit odd. maybe think of another way to do that.
        // CR: (Kfir) The odd part is that it relies on supportedDrives list. Why does IsDriveSupported rely on GetDrivesMetdata,
        //     instead of GetDrivesMetadata relying on IsDriveSupported?..
        /// <summary>
        /// Check if the given drive name is supported
        /// </summary>
        /// <param name="driveName"> The drive name </param>
        /// <param name="supportedDrives">The supported drives </param>
        /// // CR: Fill the returns
        /// <returns></returns>
        public static bool IsDriveSupported(string driveName, List<System.IO.DriveInfo> supportedDrives)
        {
            return supportedDrives.All(drive => drive.Name != driveName);
        }
    }
}
