using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;

namespace SnapshotProvider.Utilities
{
    public class ConvertHelper
    {

        public static byte[] IconToBytes(Icon icon)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }

        public static string FileToBase64(string path)
        {
            var bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        public static string IconToBase64(string path)
        {
            path = path.Trim('\\');
            try
            {
                if (Path.GetExtension(path) == ".exe")
                {
                    return Convert.ToBase64String(IconToBytes(Icon.ExtractAssociatedIcon(path)));
                }

                return FileToBase64(path);
            }
            catch (System.IO.IOException exception)
            {
                Debug.WriteLine(exception.Message);
                return "";
            }


        }
    }
}
