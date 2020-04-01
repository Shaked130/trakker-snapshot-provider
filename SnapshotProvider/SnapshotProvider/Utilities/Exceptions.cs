using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Utilities
{
    // CR: This could be internal class the server shouldn't know about this.
    // CR: Make a different class for different exceptions in its own Directory (not utilities). 
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
