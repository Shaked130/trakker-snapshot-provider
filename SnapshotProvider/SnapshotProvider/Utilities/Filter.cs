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

        
        /// <summary>
        /// Check if the given drive name is supported
        /// </summary>
        /// <param name="driveName"> The drive name </param>
        /// <returns> The function returns true if the given drive is supported if the drive is not supported the function will return false </returns>
        public static bool IsDriveSupported(string driveName)
        {
            return DriveInfo.GetDrives().Any(drive => drive.Name == driveName && IsDriveTypeSupported(drive.DriveType));
        }
    }
}
