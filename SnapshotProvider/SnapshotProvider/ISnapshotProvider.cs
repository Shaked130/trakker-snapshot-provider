using System.Collections.Generic;
using System.IO;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    internal interface ISnapshotProvider
    {
        List<System.IO.DriveInfo> GetDrivesMetadata();

        DriveInfo GetDriveInfo(string driveName);

    }
}
