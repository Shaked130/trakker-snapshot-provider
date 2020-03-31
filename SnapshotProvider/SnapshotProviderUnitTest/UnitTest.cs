using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapshotProvider.Utilities;
using TrakkerModels;
using DriveInfo = System.IO.DriveInfo;

namespace SnapshotProviderUnitTest
{
    [TestClass]
    public class GeneralTests
    {
        /// <summary>
        /// The function checks if the drives metadata is correct.
        /// </summary>
        [TestMethod]
        public void GetDrivesMetadataChecker()
        {
            // Arrange
            var snapshotProvider = new SnapshotProvider.SnapshotProvider();

            // Act 
            var drivesMetadata = snapshotProvider.GetDrivesMetadata();

            // Assert
            if (drivesMetadata.Count(driveMetaData => !driveMetaData.IsReady || !Filter.IsDriveTypeSupported(driveMetaData.DriveType)) > 0)
            {
                Assert.Fail();
            }

        }

        /// <summary>
        /// Checks the drive total size
        /// </summary>
        [TestMethod]
        public void GetDriveTest()
        {
            // Arrange
            var snapshotProvider = new SnapshotProvider.SnapshotProvider();
            var randomDrive = DriveInfo.GetDrives()[0];

            
            var drive = snapshotProvider.GetDriveInfo(randomDrive.Name);

            // Assert
            Assert.AreEqual(drive.Size, randomDrive.TotalSize);
            
        }

    }

    [TestClass]
    public class FolderChecker
    {
        private static class NewFolder
        {
            private const string ValidChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            private static readonly Random Random = new Random();
            public static ulong TotalSize = 0;

            /// <summary>
            /// The function generates a new name with the given length.
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
                using var fs = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.None);
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
            Directory.Delete(newTempFolder,true);

            // Assert
            Assert.AreEqual(drive.Size, NewFolder.TotalSize);

        }



    }
}
