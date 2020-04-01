using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TrakkerModels;
using FileInfo = System.IO.FileInfo;

namespace SnapshotProvider.Utilities
{
    
    internal static class Scan
    {
        /// <summary>
        /// Scanning the given path.
        /// </summary>
        /// <param name="currentDirectoryInfo">DirectoryInfo that contains the path to be scan</param>
        public static void ScanDrive(TrakkerModels.DirectoryInfo currentDirectoryInfo)
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
                // CR: (Kfir) This is not enough. In this case, return a DirectoryInfo with some boolean that says we had no access to it
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
