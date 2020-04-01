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
    // CR: This could be internal class the server shouldn't know about this.
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
                    // CR: file is not a good name. I think directoryFile is better.
                    var file = new TrakkerModels.FileInfo(filePath);
                    currentDirectoryInfo.Children.Add(file);
                    currentDirectoryInfo.Size += file.Size;
                }
                foreach (var directory in Directory.EnumerateDirectories(currentDirectoryInfo.FullPath))
                {
                    // CR: newDir is not a good name also.
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
