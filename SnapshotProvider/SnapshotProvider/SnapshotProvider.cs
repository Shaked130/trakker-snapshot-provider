﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using SnapshotProvider.Utilities;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public class SnapshotProvider : ISnapshotProvider
    {

        /// <summary>
        /// Returns the valid drives that found.
        /// </summary>
        /// <returns> Valid drives </returns>
        public List<System.IO.DriveInfo> GetDrivesMetadata()
        {
            return System.IO.DriveInfo.GetDrives()
                .Where(drive => Filter.IsDriveTypeSupported(drive.DriveType) && drive.IsReady).ToList();
        }

        // CR: Add <exception cref="ExceptionName">Thrown when ...</exception>

        /// <summary>
        /// The function scans the given drive and returns all the drive information.
        /// </summary>
        /// <param name="driveName"> The drive name</param>
        /// <param name="checkDriveValidation"> Check if the drive is supported </param>
        /// <returns> A DriveInfo object with all the data </returns>
        public TrakkerModels.DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation = true)
        {
            if (!Directory.Exists(driveName))
            {
                throw new Utilities.Exceptions.DriveNotFoundException(driveName);
            }
            
            if (checkDriveValidation && Utilities.Filter.IsDriveSupported(driveName, GetDrivesMetadata()))
            {
                throw new Utilities.Exceptions.DriveNotSupportedException(driveName);
            }

            var drive = new TrakkerModels.DirectoryInfo(driveName);
            // CR: I think you need to find a way to return a new drive and not change it inside, it will be better looking.
            // CR: (Kfir) I agree with Dvir about that. You can do the recursion with another method
            //     that returns DirectoryInfo, but the main method should return DriveInfo
            Utilities.Scan.DirectorySearch(drive);
            drive.Size = drive.Children.Aggregate<FileSystemNode, ulong>(0, (current, child) => current + child.Size);

            return new TrakkerModels.DriveInfo(driveName, drive, drive.Size);
        }
    }
}
