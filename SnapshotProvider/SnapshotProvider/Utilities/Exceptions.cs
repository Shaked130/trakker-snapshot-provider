using System;
using System.Collections.Generic;
using System.Text;

namespace SnapshotProvider.Utilities
{
    public static class Exceptions
    {
        public class DriveNotFoundException : Exception
        {
        }

        public class DriveNotSupportedException : Exception
        {
        }
    }
}
