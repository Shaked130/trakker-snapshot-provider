using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using TrakkerModels;
using DirectoryInfo = System.IO.DirectoryInfo;
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
                    var file = new TrakkerModels.FileInfo((ulong)new FileInfo(filePath).Length, filePath);
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

        /// <summary>
        /// Returns the given directory size
        /// </summary>
        /// <param name="directoryPath"> The directory path </param>
        /// <returns> The size of the given directory </returns>
        public static ulong GetDirectorySize(string directoryPath)
        {
            try
            {
                var directory = new DirectoryInfo(directoryPath);
                return (ulong)directory.EnumerateFiles("*.*", SearchOption.AllDirectories).Sum(fi => fi.Length);
            }
            catch (System.IO.IOException)
            {
                return 0;
            }

        }

    }
}
