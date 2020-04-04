using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;
using FileInfo = System.IO.FileInfo;

namespace SnapshotProvider.Utilities
{
    
    internal static class Scan
    {
        /// <summary>
        /// Scanning the given path.
        /// </summary>
        /// <param name="currentDirectoryInfo">DirectoryInfo that contains the path to be scan</param>
        private static void ScanDrive(TrakkerModels.DirectoryInfo currentDirectoryInfo)
        {
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(currentDirectoryInfo.FullPath))
                {
                    var file = new TrakkerModels.FileInfo((ulong)new System.IO.FileInfo(filePath).Length, filePath);
                    currentDirectoryInfo.Children.Add(file);
                    currentDirectoryInfo.Size += file.Size;
                }
                

                foreach (var directoryPath in Directory.EnumerateDirectories(currentDirectoryInfo.FullPath))
                {
                    var directoryInfo = new TrakkerModels.DirectoryInfo(directoryPath);
                    currentDirectoryInfo.Children.Add(directoryInfo);
                    ScanDrive(directoryInfo);
                    currentDirectoryInfo.Size += directoryInfo.Size;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                currentDirectoryInfo.CanAccess = false;
                Debug.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Returns a driveInfo object with all the data.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <returns> DriveInfo with all the data </returns>
        public static DriveInfo GetDriveInfo(string driveName)
        {
            var drive = new TrakkerModels.DirectoryInfo(driveName);
            Utilities.Scan.ScanDrive(drive);
            drive.Size = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            return new TrakkerModels.DriveInfo(driveName, drive, drive.Size);
        }

    }
}
