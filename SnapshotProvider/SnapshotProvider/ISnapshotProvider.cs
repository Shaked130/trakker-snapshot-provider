using System.Collections.Generic;
using System.IO;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public interface ISnapshotProvider
    {
        List<System.IO.DriveInfo> GetDrivesMetadata();

        DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation);

        List<ProgramInfo> GetInstalledApps();

    }
}
