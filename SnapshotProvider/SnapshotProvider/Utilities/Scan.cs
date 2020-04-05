using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;
using FileInfo = System.IO.FileInfo;

namespace SnapshotProvider.Utilities
{
    
    internal static class Scan
    {
        /// <summary>
        /// Scan the given path.
        /// </summary>
        /// <param name="path">Path of the directory to be scanned</param>
        /// <returns>DirectoryInfo including the children</returns>
        private static TrakkerModels.DirectoryInfo ScanDirectory(string path)
        {
            var currentDirectoryInfo = new TrakkerModels.DirectoryInfo(path);

            try
            {
                foreach (var filePath in Directory.EnumerateFiles(currentDirectoryInfo.FullPath))
                {
                    // CR: (Kfir) Move file size logic to private static GetFileSize
                    // CR: (Kfir) Rename file to fileInfo to be consistent with "directoryInfo" in the next block
                    var file = new TrakkerModels.FileInfo((ulong)new System.IO.FileInfo(filePath).Length, filePath);
                    currentDirectoryInfo.Children.Add(file);
                    currentDirectoryInfo.Size += file.Size;
                }
                
                foreach (var directoryPath in Directory.EnumerateDirectories(currentDirectoryInfo.FullPath))
                {
                    var directoryInfo = ScanDirectory(directoryPath);
                    currentDirectoryInfo.Children.Add(directoryInfo);
                    currentDirectoryInfo.Size += directoryInfo.Size;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                currentDirectoryInfo.CanAccess = false;
                Debug.WriteLine(ex.Message);
            }

            return currentDirectoryInfo;
        }


        /// <summary>
        /// Returns a driveInfo object with all the data.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <returns> DriveInfo with all the data </returns>
        public static DriveInfo GetDriveInfo(string driveName)
        {
            var driveDirectory = Utilities.Scan.ScanDirectory(driveName);
            
            // CR: (Kfir) See CR in DriveInfo model
            return new TrakkerModels.DriveInfo(driveName, driveDirectory, driveDirectory.Size);
        }

    }
}
