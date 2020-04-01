using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Win32;
using TrakkerModels;

namespace SnapshotProvider.Platforms
{
    class WindowsPlatformHandler : IPlatformHandler
    {
        const string registry_key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

        public List<ProgramInfo> GetInstalledPrograms()
        {
            var result = new List<ProgramInfo>();
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry32));
            result.AddRange(GetInstalledProgramsFromRegistry(RegistryView.Registry64));
            return new List<ProgramInfo>();
        }


        private static IEnumerable<ProgramInfo> GetInstalledProgramsFromRegistry(RegistryView registryView)
        {
            var programs = new List<ProgramInfo>();

            using (var key = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, registryView).OpenSubKey(registry_key))
            {
                foreach (var subkey_name in key.GetSubKeyNames())
                {
                    using var subkey = key.OpenSubKey(subkey_name);
                    if (IsProgramVisible(subkey))
                    {
                        var program = new ProgramInfo((string)subkey.GetValue("DisplayName"));

                        if (subkey.GetValue("DisplayIcon") != null)
                        {
                            var iconPath = (subkey.GetValue("DisplayIcon").ToString());
                            program.Icon = Utilities.ConvertHelper.IconToBase64(iconPath);
                            
                        }
                        else
                        {
                            program.Icon = GetIcon(subkey.GetValue("DisplayName").ToString());
                        }
                        
                        programs.Add(program);
                    }
                }
            }

            return programs;
        }

        private static string GetIcon(string productName)
        {
            const string installerKey = @"Installer\Products";

            using (var installkeys = Registry.ClassesRoot.OpenSubKey(installerKey))
            {
                foreach (var name in installkeys.GetSubKeyNames())
                {
                    using var product = installkeys.OpenSubKey(name);
                    if (product?.GetValue("ProductName") != null)
                    {
                        if (productName == product.GetValue("ProductName").ToString())
                        {
                            if (product.GetValue("ProductIcon") != null)
                            {
                                return Utilities.ConvertHelper.IconToBase64(product.GetValue("ProductIcon").ToString());
                            }
                        }
                    }
                }
            }

            return "";
        }


        private static bool IsProgramVisible(RegistryKey subkey)
        {
            var name = (string)subkey.GetValue("DisplayName");
            var releaseType = (string)subkey.GetValue("ReleaseType");
            var systemComponent = subkey.GetValue("SystemComponent");
            var parentName = (string)subkey.GetValue("ParentDisplayName");

            return
                !string.IsNullOrEmpty(name)
                && string.IsNullOrEmpty(releaseType)
                && string.IsNullOrEmpty(parentName)
                && (systemComponent == null);
        }
    }
}
