using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnapshotProviderUnitTest
{
    [TestClass]
    public class FolderChecker
    {
        // CR: (Kfir) Why is this static? I see you use TotalSize, which seems more like a property of a regular class rather than a
        //     static thing
        private static class NewFolder
        {
            private const string ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private static readonly Random Random = new Random();
            public static ulong TotalSize = 0;

            /// <summary>
            /// Generates a new name with the given length.
            /// </summary>
            /// <param name="length"> The length of the new name </param>
            /// <returns> The new name </returns>
            private static string GenerateName(int length)
            {
                var chars = new char[length];
                for (var i = 0; i < length; i++)
                {
                    chars[i] = ValidChars[Random.Next(ValidChars.Length)];
                }
                return new string(chars);
            }

            /// <summary>
            /// The function creates a new file in the given path with the given size. (Random Content)
            /// </summary>
            /// <param name="filepath"> The new file path </param>
            /// <param name="sizeInBytes">The new file size </param>
            private static void CreateNewFile(string filepath, long sizeInBytes)
            {
                using var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
                fs.SetLength(sizeInBytes);
            }

            /// <summary>
            /// The function creates temporary folders and files.
            /// </summary>
            /// <param name="currentDirectory"> The current Directory </param>
            /// <param name="depth">The depth of the new folder </param>
            public static void GenerateFolders(string currentDirectory, int depth)
            {
                const int folderNameLength = 10;
                const int filesCountMin = 0;
                const int filesCountMax = 10;
                const int folderCountMin = 1;
                const int folderCountMax = 15;
                const int fileSizeMin = 1024;
                const int fileSizeMax = fileSizeMin * 2;

                if (depth == 0)
                {
                    return;
                }
                depth--;

                var filesCount = Random.Next(filesCountMin, filesCountMax);
                var folderCount = Random.Next(folderCountMin, folderCountMax);
                var filesSize = Random.Next(fileSizeMin, fileSizeMax);
                TotalSize += (ulong)(filesSize * filesCount);

                for (var i = 0; i < filesCount; i++)
                {
                    CreateNewFile(Path.Combine(currentDirectory, Path.GetRandomFileName()), filesSize);
                }

                for (var i = 0; i < folderCount; i++)
                {
                    var newDirPath = Path.Combine(currentDirectory, NewFolder.GenerateName(folderNameLength));
                    Directory.CreateDirectory(newDirPath);
                    GenerateFolders(newDirPath, depth);
                }
            }
        }

        /// <summary>
        /// The function creates a new temp folder and runs the snapshot provider that scans the temp folder.
        /// </summary>
        [TestMethod]
        public void TestFolderSize()
        {
            const string newTempFolderName = "TempFolder";
            const int newTempFolderDepth = 2;

            // Arrange
            var snapshotProvider = new SnapshotProvider.SnapshotProvider();
            var newTempFolder = Path.Combine(Directory.GetCurrentDirectory(), newTempFolderName);
            Directory.CreateDirectory(newTempFolder);
            NewFolder.GenerateFolders(newTempFolder, newTempFolderDepth);

            // Act 
            var drive = snapshotProvider.GetDriveInfo(newTempFolder, false);
            Directory.Delete(newTempFolder, true);

            // Assert
            Assert.AreEqual(drive.Size, NewFolder.TotalSize);

        }

    }
}
