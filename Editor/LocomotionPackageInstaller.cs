// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.IO;
using UnityEditor;
using RealityToolkit.Editor;
using RealityToolkit.Editor.Utilities;
using RealityToolkit.Extensions;

namespace RealityToolkit.Locomotion.Editor
{
    [InitializeOnLoad]
    internal static class LocomotionPackageInstaller
    {
        private static readonly string DefaultPath = $"{MixedRealityPreferences.ProfileGenerationPath}Locomotion";
        private static readonly string HiddenPath = Path.GetFullPath($"{PathFinderUtility.ResolvePath<IPathFinder>(typeof(locomotionPathFinder)).ForwardSlashes()}{Path.DirectorySeparatorChar}{MixedRealityPreferences.HIDDEN_PACKAGE_ASSETS_PATH}");

        static LocomotionPackageInstaller()
        {
            EditorApplication.delayCall += CheckPackage;
        }

        [MenuItem("Reality Toolkit/Packages/Install Locomotion Package Assets...", true)]
        private static bool ImportPackageAssetsValidation()
        {
            return !Directory.Exists($"{DefaultPath}{Path.DirectorySeparatorChar}");
        }

        [MenuItem("Reality Toolkit/Packages/Install Locomotion Package Assets...")]
        private static void ImportPackageAssets()
        {
            EditorPreferences.Set($"{nameof(LocomotionPackageInstaller)}.Assets", false);
            EditorApplication.delayCall += CheckPackage;
        }

        private static void CheckPackage()
        {
            if (!EditorPreferences.Get($"{nameof(LocomotionPackageInstaller)}.Assets", false))
            {
                EditorPreferences.Set($"{nameof(LocomotionPackageInstaller)}.Assets", PackageInstaller.TryInstallAssets(HiddenPath, DefaultPath));
            }
        }
    }
}
