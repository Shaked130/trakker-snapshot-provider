using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Utilities
{
    public static class Exceptions
    {
        public class DriveNotFoundException : Exception
        {
            public DriveNotFoundException(string driveName) : base($"The drive: {driveName} not found!")
            { }
        }

        public class DriveNotSupportedException : Exception
        {
            public DriveNotSupportedException(string driveName) : base($"The drive: {driveName} not supported!")
            {
            }
        }
    }
}
