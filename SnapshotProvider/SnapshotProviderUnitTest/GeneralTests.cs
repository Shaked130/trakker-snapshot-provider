using System;
using System.IO;
using System.Linq;
using System.Text;
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

}
