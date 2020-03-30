using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnapshotProvider.Utilities;
using TrakkerModels;
using DriveInfo = System.IO.DriveInfo;

namespace SnapshotProviderUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void GetDrivesMetadataChecker()
        {
            // Arrange
            var snapshotProvider = new SnapshotProvider.SnapshotProvider();

            // Act 
            var drivesMetadata = snapshotProvider.GetDrivesMetadata();
           
            // Assert
            if (drivesMetadata.Count(driveMetaData => !driveMetaData.IsReady || !Filter.IsDriveSupported(driveMetaData.DriveType)) > 0)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public void GetDriveTest()
        {
            // Arrange
            var snapshotProvider = new SnapshotProvider.SnapshotProvider();
            var randomDrive = DriveInfo.GetDrives()[0];

            // Act 
            var drive = snapshotProvider.GetDriveInfo(randomDrive.Name);
            var driveSize = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            // Assert
            Assert.Equals(driveSize,randomDrive.TotalSize);

        }

    }
}
