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
    public class Scan
    {
        /// <summary>
        /// Scanning the given path.
        /// </summary>
        /// <param name="currentDirectoryInfo">DirectoryInfo that contains the path to be scan</param>
        public static void DirectorySearch(TrakkerModels.DirectoryInfo currentDirectoryInfo)
        {
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(currentDirectoryInfo.FullPath))
                {
                    var file = new TrakkerModels.FileInfo(filePath);
                    currentDirectoryInfo.Children.Add(file);
                    currentDirectoryInfo.Size += file.Size;
                }
                foreach (var directory in Directory.EnumerateDirectories(currentDirectoryInfo.FullPath))
                {
                    var newDir = new TrakkerModels.DirectoryInfo(directory);
                    currentDirectoryInfo.Children.Add(newDir);
                    DirectorySearch(newDir);
                    newDir.Size += newDir.Size;
                }
            }
            catch (System.UnauthorizedAccessException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

    }
}
