using System.Collections.Generic;
using System.IO;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public interface ISnapshotProvider
    {
        IEnumerable<System.IO.DriveInfo> GetDrivesMetadata();

        DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation);

    }
}
