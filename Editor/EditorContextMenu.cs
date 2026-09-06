using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Scripts.Editor
{
    internal static class EditorContextMenu
    {
        private static bool _canDeleteImportedScripts;

        private static RemoveRequest _removeRequest;

        private const string RootPath = "Assets/EzUtilities";
        private const string ContextMenuPath = "Racer/EzUtilities.Extras/";
        private const string ImportScriptsContextMenuPath = ContextMenuPath + "Import Scripts (force)";

        private const string PkgId = "com.racer.ezutilities.extras";
        private const string AssetPkgId = "EzUtilities.Extras.unitypackage";


        [MenuItem(ImportScriptsContextMenuPath, false)]
        private static void ImportScripts()
        {
            var packagePath = $"Packages/{PkgId}/Runtime~/{AssetPkgId}";

            if (File.Exists(packagePath))
                AssetDatabase.ImportPackage(packagePath, true);
            else
                EditorUtility.DisplayDialog("Missing Package File", $"{AssetPkgId} not found in the package.", "OK");
        }

        [MenuItem(ContextMenuPath + "Remove Package (recommended)")]
        private static void RemovePackage()
        {
            if (Directory.Exists(RootPath))
                _canDeleteImportedScripts = EditorUtility.DisplayDialog("Delete imported scripts?",
                    $"Also delete the Imported scripts' folder?\n\nPath: {RootPath}",
                    "Yes", "No");

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
                    if (_canDeleteImportedScripts)
                    {
                        AssetDatabase.DeleteAsset(RootPath);
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
}