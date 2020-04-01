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
    // CR: (Kfir) This class should be static
    public class Scan
    {
        // CR: (Kfir) Pick a better name for "DirectorySearch", e.g. "ScanDrive"
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
                    // CR: (Kfir) I actually think 'file' is okay in this context
                    var file = new TrakkerModels.FileInfo(filePath);
                    currentDirectoryInfo.Children.Add(file);
                    currentDirectoryInfo.Size += file.Size;
                }
                // CR: (Kfir) Leave an empty line between blocks (blocks can be for, foreach, switch, if, etc...)
                // CR: (Kfir) 'directory' is a misleading name, because this is just the path
                foreach (var directory in Directory.EnumerateDirectories(currentDirectoryInfo.FullPath))
                {
                    // CR: newDir is not a good name also.
                    var newDir = new TrakkerModels.DirectoryInfo(directory);
                    currentDirectoryInfo.Children.Add(newDir);
                    DirectorySearch(newDir);
                    currentDirectoryInfo.Size += newDir.Size;
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
