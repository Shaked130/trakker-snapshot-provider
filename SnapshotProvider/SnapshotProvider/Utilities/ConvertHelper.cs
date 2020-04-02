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

        /// <summary>
        /// Converts the given icon object to bytes array.
        /// </summary>
        /// <param name="icon"> The icon </param>
        /// <returns> The icon object in bytes array </returns>
        public static byte[] IconToBytes(Icon icon)
        {
            using (var ms = new MemoryStream())
            {
                icon.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts the given icon path to base64.
        /// </summary>
        /// <param name="path"> The path of the icon or the associated exe file </param>
        /// <returns>The encoded icon in base64 </returns>
        public static string IconToBase64(string path)
        {
            path = RemoveZeroFromEnd(path);
            try
            {
                if (Path.GetExtension(path).ToLower() == ".exe")
                {
                    return Convert.ToBase64String(IconToBytes(Icon.ExtractAssociatedIcon(path)));
                }

                return Convert.ToBase64String(IconToBytes(new Icon(path)));
            }
            catch (System.IO.IOException exception)
            {
                Debug.WriteLine(exception.Message);
                return "";
            }
            catch (System.ArgumentException exception)
            {
                Debug.WriteLine(exception.Message);
                return "";
            }


        }

        /// <summary>
        /// Remove the Zero from the end
        /// </summary>
        /// <param name="str"> The string </param>
        /// <returns> The string without the Zero in the end </returns>
        private static string RemoveZeroFromEnd(string str)
        {
            return str.Split('\\').Last().EndsWith(",0") ? str.Remove(str.Length - 2, 2) : str;
        }
    }
}
