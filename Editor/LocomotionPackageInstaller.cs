// Copyright (c) Reality Collective. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using RealityCollective.Editor.Utilities;
using RealityCollective.Extensions;
using RealityCollective.ServiceFramework.Editor;
using RealityCollective.ServiceFramework.Editor.Packages;
using RealityToolkit.Editor;
using System.IO;
using UnityEditor;

namespace RealityToolkit.Locomotion.Editor
{
    [InitializeOnLoad]
    internal static class LocomotionPackageInstaller
    {
        private static readonly string destinationPath = $"{MixedRealityPreferences.ProfileGenerationPath}Locomotion";
        private static readonly string sourcePath = Path.GetFullPath($"{PathFinderUtility.ResolvePath<IPathFinder>(typeof(LocomotionPackagePathFinder)).ForwardSlashes()}{Path.DirectorySeparatorChar}{MixedRealityPreferences.HIDDEN_PACKAGE_ASSETS_PATH}");

        static LocomotionPackageInstaller()
        {
            EditorApplication.delayCall += CheckPackage;
        }

        [MenuItem("Reality Toolkit/Packages/Install Locomotion Package Assets...", true)]
        private static bool ImportPackageAssetsValidation()
        {
            return !Directory.Exists($"{destinationPath}{Path.DirectorySeparatorChar}");
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
                EditorPreferences.Set($"{nameof(LocomotionPackageInstaller)}.Assets", AssetsInstaller.TryInstallAssets(sourcePath, destinationPath));
            }
        }
    }
}
