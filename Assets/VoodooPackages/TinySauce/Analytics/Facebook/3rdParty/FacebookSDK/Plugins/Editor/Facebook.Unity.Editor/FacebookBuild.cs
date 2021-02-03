/**
 * Copyright (c) 2014-present, Facebook, Inc. All rights reserved.
 *
 * You are hereby granted a non-exclusive, worldwide, royalty-free license to use,
 * copy, modify, and distribute this software in source code or binary form for use
 * in connection with the web services and APIs provided by Facebook.
 *
 * As with any software that integrates with the Facebook platform, your use of
 * this software is subject to the Facebook Developer Principles and Policies
 * [http://developers.facebook.com/policy/]. This copyright notice shall be
 * included in all copies or substantial portions of the software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
 * FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
 * COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

namespace Facebook.Unity.Editor
{
    using System.Globalization;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    internal class FacebookBuild
    {
        private const string FacebookPath = "Assets/FacebookSDK/";
        private const string SDKPath = "Assets/FacebookSDK/SDK/";
        private const string ExamplesPath = "Assets/FacebookSDK/Examples/";
        private const string PluginsPath = "Assets/FacebookSDK/Plugins/";
        private const string PlayServicesResolverPath = "Assets/PlayServicesResolver/";

        public enum Target
        {
            DEBUG,
            RELEASE
        }

        private static string PackageName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "facebook-unity-sdk-{0}.unitypackage",
                    FacebookSdkVersion.Build);
            }
        }

        private static string OutputPath
        {
            get
            {
                DirectoryInfo projectRoot = Directory.GetParent(Directory.GetCurrentDirectory());
                var outputDirectory = new DirectoryInfo(Path.Combine(projectRoot.FullName, "out"));

                // Create the directory if it doesn't exist
                outputDirectory.Create();
                return Path.Combine(outputDirectory.FullName, FacebookBuild.PackageName);
            }
        }

        // Exporting the *.unityPackage for Asset store
        public static string ExportPackage()
        {
            Debug.Log("Exporting Facebook Unity Package...");
            string path = OutputPath;
            try
            {
                if (!File.Exists(Path.Combine(Application.dataPath, "Temp")))
                {
                    AssetDatabase.CreateFolder("Assets", "Temp");
                }

                AssetDatabase.MoveAsset(SDKPath + "Resources/FacebookSettings.asset", "Assets/Temp/FacebookSettings.asset");
                AssetDatabase.DeleteAsset(PluginsPath + "Android/AndroidManifest.xml");
                AssetDatabase.DeleteAsset(PluginsPath + "Android/AndroidManifest.xml.meta");

                string[] facebookFiles = (string[])Directory.GetFiles(FacebookPath, "*.*", SearchOption.TopDirectoryOnly);
                string[] sdkFiles = (string[])Directory.GetFiles(SDKPath, "*.*", SearchOption.AllDirectories);
                string[] exampleFiles = (string[])Directory.GetFiles(ExamplesPath, "*.*", SearchOption.AllDirectories);
                string[] pluginsFiles = (string[])Directory.GetFiles(PluginsPath, "*.*", SearchOption.AllDirectories);

                string[] playServicesResolverFiles = (string[])Directory.GetFiles(PlayServicesResolverPath, "*.*",
                                                                                  SearchOption.AllDirectories);
                string[] files = new string[facebookFiles.Length + sdkFiles.Length + exampleFiles.Length +
                                            pluginsFiles.Length + playServicesResolverFiles.Length];
                facebookFiles.CopyTo(files, 0);
                sdkFiles.CopyTo(files, facebookFiles.Length);
                exampleFiles.CopyTo(files, sdkFiles.Length + facebookFiles.Length);
                pluginsFiles.CopyTo(files, sdkFiles.Length + facebookFiles.Length + exampleFiles.Length);
                playServicesResolverFiles.CopyTo(files, sdkFiles.Length + facebookFiles.Length + exampleFiles.Length + pluginsFiles.Length);

                AssetDatabase.ExportPackage(
                    files,
                    path,
                    ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Recurse);
            }
            finally
            {
                // Move files back no matter what
                AssetDatabase.MoveAsset("Assets/Temp/FacebookSettings.asset", SDKPath + "Resources/FacebookSettings.asset");
                AssetDatabase.DeleteAsset("Assets/Temp");

                // regenerate the manifest
                ManifestMod.GenerateManifest();
            }

            Debug.Log("Finished exporting!");

            return path;
        }
    }
}
