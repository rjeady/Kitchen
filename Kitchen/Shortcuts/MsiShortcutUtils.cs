using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Kitchen.Shortcuts
{
    public static class MsiShortcutUtils
    {
        private static readonly string MsiShortcutTargetPrefix
            = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Installer", "{");

        /// <summary>Determines whether the specified shortcut is an MSI shortcut.</summary>
        /// <param name="shortcut">The shortcut.</param>
        public static bool IsMsiShortcut(ShellShortcut shortcut)
        {
            Contract.Requires(shortcut != null && shortcut.TargetPath != null);
            return shortcut.TargetPath.StartsWith(MsiShortcutTargetPrefix);
        }

        /// <summary>
        /// Gets the target path of an MSI shortcut. May return null if a target executable path could not be obtained.
        /// Must be called from an STA thread.
        /// </summary>
        /// <param name="shortcutFilePath">The shortcut file path.</param>
        /// <returns></returns>
        public static string GetMsiShortcutTarget(string shortcutFilePath)
        {
            var product = new StringBuilder(MaxGuidLength);
            var component = new StringBuilder(MaxGuidLength);

            if (MsiGetShortcutTarget(shortcutFilePath, product, null, component) != MsiResult.Success)
                return null;

            int pathLength = MaxPathLength;
            var path = new StringBuilder(pathLength);

            var installState = MsiGetComponentPath(product.ToString(), component.ToString(), path, ref pathLength);

            return installState == InstallState.Local ? path.ToString() : null;
        }

        [DllImport("msi.dll", CharSet = CharSet.Auto)]
        private static extern MsiResult MsiGetShortcutTarget(
            string targetFile,
            StringBuilder productCode,
            StringBuilder featureID,
            StringBuilder componentCode);

        [DllImport("msi.dll", CharSet = CharSet.Auto)]
        private static extern InstallState MsiGetComponentPath(
            string productCode,
            string componentCode,
            StringBuilder componentPath,
            ref int componentPathBufferSize);

        private const int MaxGuidLength = 38;
        private const int MaxPathLength = 1024;

        private enum InstallState
        {
            NotUsed = -7,
            BadConfig = -6,
            Incomplete = -5,
            SourceAbsent = -4,
            MoreData = -3,
            InvalidArg = -2,
            Unknown = -1,
            Broken = 0,
            Advertised = 1,
            Removed = 1,
            Absent = 2,
            Local = 3,
            Source = 4,
            Default = 5
        }

        private enum MsiResult : uint
        {
            Success = 0,
            Error = 1603, // occurs when called from a non-STA thread.
            Failed = 1627
        }
    }
}
