#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Racer.EzUtilities.Editor
{
    internal static class EzUtilitiesEditor
    {
        private static RemoveRequest _removeRequest;

        private const string RootPath = "Assets/EzUtilities";
        private const string ContextMenuPath = "Racer/EzUtilities/";
        private const string ImportScriptsContextMenuPath = ContextMenuPath + "Import Scripts(Force)";

        private const string PkgId = "com.racer.ez_utilities";
        private const string AssetPkgId = "EzUtilities.unitypackage";


        [MenuItem(ImportScriptsContextMenuPath, false)]
        private static void ImportTemplate()
        {
            var packagePath = $"Packages/{PkgId}/Runtime/{AssetPkgId}";

            if (File.Exists(packagePath))
                AssetDatabase.ImportPackage(packagePath, true);
            else
                EditorUtility.DisplayDialog("Missing Package File", $"{AssetPkgId} not found in the package.", "OK");
        }


        [MenuItem(ContextMenuPath + "Remove Package(recommended)")]
        private static void RemovePackage()
        {
            _removeRequest = Client.Remove(PkgId);
            EditorApplication.update += RemoveRequest;
        }


        private static void RemoveRequest()
        {
            if (!_removeRequest.IsCompleted) return;

            switch (_removeRequest.Status)
            {
                case StatusCode.Success:
                {
                    if (!Directory.Exists(RootPath)) return;
                    
                    if (EditorUtility.DisplayDialog($"Remove {AssetPkgId}",
                            $"Also delete the Imported scripts' folder?\n\nPath: {RootPath}",
                            "Yes", "No"))
                    {
                        DirUtils.DeleteDirectory(RootPath);
                        AssetDatabase.Refresh();
                    }

                    break;
                }
                case >= StatusCode.Failure:
                    Debug.LogError($"Failed to remove package: '{PkgId}'\n{_removeRequest.Error.message}");
                    break;
            }

            EditorApplication.update -= RemoveRequest;
        }
    }

    internal static class DirUtils
    {
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            Directory.Delete(path, true);
            DeleteEmptyMetaFiles(path);
        }

        private static void DeleteEmptyMetaFiles(string directory)
        {
            if (Directory.Exists(directory)) return;

            var metaFile = directory + ".meta";

            if (File.Exists(metaFile))
                File.Delete(metaFile);
        }
    }
}

#endif