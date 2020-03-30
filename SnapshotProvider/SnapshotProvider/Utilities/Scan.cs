using System;
using System.Collections.Generic;
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
        /// <param name="dir">DirectoryInfo that contains the path to be scan</param>
        // TODO: Improve the performance of the function! 
        public static void DirectorySearch(ref TrakkerModels.DirectoryInfo dir)
        {
            try
            {
                foreach (var filePath in Directory.EnumerateFiles(dir.FullPath))
                {
                    dir.Children.Add(new TrakkerModels.FileInfo(filePath));
                }
                foreach (var directory in Directory.EnumerateDirectories(dir.FullPath))
                {
                    var newDir = new TrakkerModels.DirectoryInfo(directory);
                    dir.Children.Add(newDir);
                    DirectorySearch(ref newDir);
                    newDir.Size = newDir.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);
                }
            }
            catch (System.Exception ex)
            {
                //TODO: We need to decide what to do with an error case
            }
        }

    }
}
