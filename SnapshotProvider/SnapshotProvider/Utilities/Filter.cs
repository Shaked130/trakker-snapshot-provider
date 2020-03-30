using System.IO;

namespace SnapshotProvider.Utilities
{
    public static class Filter
    {
        /// <summary>
        /// The function checks if the given drive type is supported.
        /// </summary>
        /// <param name="driveType"> The drive type</param>
        /// <returns> true if the drive type is supported, false if not. </returns>
        public static bool IsDriveSupported(DriveType driveType)
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
    }
}
