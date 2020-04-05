using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
using TrakkerModels;

namespace SnapshotProvider.Platforms
{
    internal class WindowsPlatformHandler : IPlatformHandler
    {

        /// <summary>
        /// Returns the installed programs.
        /// </summary>
        /// <returns> The installed programs</returns>
        public List<ProgramInfo> GetInstalledPrograms()
        {
            const string uninstallPath = @"Software\Microsoft\Windows\CurrentVersion\Uninstall";
            // CR: Don't ignore the ReSharper alerts.
            const string wow6432nodePath = @"Software\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";

            var standardUninstallLocation = Registry.LocalMachine.OpenSubKey(uninstallPath);
            var wow6432NodeLocation = Registry.LocalMachine.OpenSubKey(wow6432nodePath);
            var currentUserLocation = Registry.CurrentUser.OpenSubKey(uninstallPath);

            var programs = new List<ProgramInfo>();
            programs.AddRange(GetInstalledProgramsFromRegistry(standardUninstallLocation));
            programs.AddRange(GetInstalledProgramsFromRegistry(wow6432NodeLocation));
            programs.AddRange(GetInstalledProgramsFromRegistry(currentUserLocation));
            return programs;
        }
        // CR: This function should be after the next one.
        /// <summary>
        /// Checks if the given program registry key is visible
        /// </summary>
        /// <param name="key">The registry key</param>
        /// <returns> If the key is visible returns true </returns>
        private static bool IsProgramVisible(RegistryKey key)
        {
            const string displayName = "DisplayName";
            const string systemComponent = "SystemComponent";

            if (key.GetValue(displayName) == null) return false;
            if (key.GetValue(systemComponent) == null) return true;
            // CR: Move to const.
            return key.GetValue(systemComponent).ToString() != "1";
        }

        /// <summary>
        /// Scans the registry tree in the given path and returns the installed programs.
        /// </summary>
        /// <param name="registryKey"> The registry location </param>
        /// <returns> The installed programs that found </returns>
        private static IEnumerable<ProgramInfo> GetInstalledProgramsFromRegistry(RegistryKey registryKey)
        {
            const string displayNameKey = "DisplayName";
            const string publisherKey = "Publisher";
            const string displayIconKey = "DisplayIcon";
            const string installLocationKey = "InstallLocation";

            var programs = new List<ProgramInfo>();

            foreach (var subKeyName in registryKey.GetSubKeyNames())
            {
                using (var subKey = registryKey.OpenSubKey(subKeyName))
                {
                    // CR: Don't ignore the ReSharper alerts.
                    if (IsProgramVisible(subKey))
                    {
                        //CR: Use conditional access.
                        var name = subKey.GetValue(displayNameKey).ToString();
                        var publisher = subKey.GetValue(publisherKey) == null ? "" : subKey.GetValue(publisherKey).ToString();
                        var installLocation = subKey.GetValue(installLocationKey) == null ? "" : subKey.GetValue(installLocationKey).ToString();
                        var displayIcon = subKey.GetValue(displayIconKey) == null ? "" : Utilities.ConvertHelper.IconToBase64(subKey.GetValue(displayIconKey).ToString());

                        // This size is only the size of the installation folder
                        var installLocationSize = string.IsNullOrEmpty(installLocation) ? 0 : Utilities.Scan.GetDirectorySize(installLocation);

                        // CR: Spacing. 
                        // CR: (Kfir) Yield; don't put it in a list
                        programs.Add(new ProgramInfo(name, displayIcon, installLocation,publisher,installLocationSize));
                    }
                }
            }

            return programs;
        }

        // CR: (Kfir) Why is this unused?
        /// <summary>
        /// Search the icon of the given program.
        /// </summary>
        /// <param name="productName"> The program name </param>
        /// <returns> The path of the icon, if the function did not found it will return an empty string </returns>
        private static string GetIcon(string productName)
        {
            const string installerKey = @"Installer\Products";
            const string productNameKey = "ProductName";
            const string productIconKey = "ProductIcon";

            // CR: Don't ignore the ReSharper alerts.
            using (var installkeys = Registry.ClassesRoot.OpenSubKey(installerKey))
            {
                foreach (var name in installkeys.GetSubKeyNames())
                {
                    using var product = installkeys.OpenSubKey(name);
                    // CR: Don't ignore the ReSharper alerts.
                    if (product?.GetValue(productNameKey) != null)
                    {
                        // CR: Don't ignore the ReSharper alerts.
                        if (productName == product.GetValue(productNameKey).ToString())
                        {
                            if (product.GetValue(productIconKey) != null)
                            {
                                return Utilities.ConvertHelper.IconToBase64(product.GetValue(productIconKey).ToString());
                            }
                        }
                    }
                }
            }

            // CR: (Kfir) It is better to return null in such a case
            return "";
        }

    }
}
