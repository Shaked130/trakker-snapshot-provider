using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Exceptions
{
    public class DriveNotFoundException : Exception
    {
        public DriveNotFoundException(string driveName) : base($"The drive: {driveName} was not found!")
        { }
    }
}
