using System;
using System.Collections.Generic;
using System.Text;
using TrakkerModels;

namespace SnapshotProvider.Platforms
{
    public interface IPlatformHandler
    {
        List<ProgramInfo> GetInstalledPrograms();
    }
}
