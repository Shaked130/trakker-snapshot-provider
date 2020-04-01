using System.Collections.Generic;
using System.IO;
using TrakkerModels;
using DriveInfo = TrakkerModels.DriveInfo;

namespace SnapshotProvider
{
    public interface ISnapshotProvider
    {
        // CR: (Kfir) Why list and not IEnumerable?
        List<System.IO.DriveInfo> GetDrivesMetadata();

        // CR: (Kfir) Why does external code have control over validation?
        DriveInfo GetDriveInfo(string driveName, bool checkDriveValidation);

    }
}
