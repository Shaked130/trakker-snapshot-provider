using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Utilities
{
    // CR: This could be internal class the server shouldn't know about this.
    // CR: (Kfir) Exception classes actually should be public, because the external code may want to use them in "catch" statements
    // CR: Make a different class for different exceptions in its own Directory (not utilities). 
    public static class Exceptions
    {
        public class DriveNotFoundException : Exception
        {
            public DriveNotFoundException(string driveName) : base($"The drive: {driveName} was not found!")
            { }
        }

        public class DriveNotSupportedException : Exception
        {
            public DriveNotSupportedException(string driveName) : base($"The drive: {driveName} is not supported!")
            {
            }
        }
    }
}
