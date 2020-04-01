using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Exceptions
{
    public class DriveNotSupportedException : Exception
    {
        public DriveNotSupportedException(string driveName) : base($"The drive: {driveName} is not supported!")
        {
        }
    }
}
