#if UNITY_EDITOR
using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using Debug = UnityEngine.Debug;

namespace Racer.EzUtilities.Editor
{
    /// <summary>
    /// Adds a context menu option to open a file with the default program or notepad.
    /// </summary>
    internal static class OpenWithMenuOption
    {
        private const string RootPath = "Assets/Open With/";
        private const string NotepadPath = RootPath + "Notepad";
        private const string DefaultProgramPath = RootPath + "Default Program";


        [MenuItem(DefaultProgramPath, false)]
        public static void OpenWithDefaultProgram()
        {
            Process.Start(CurrentSelectionPath());
        }

        [MenuItem(NotepadPath, false)]
        private static void OpenWithNotepad()
        {
            Process.Start("Notepad.exe", CurrentSelectionPath());
        }

        [MenuItem(DefaultProgramPath, true)]
        private static bool ValidateOpenWithDefaultProgram()
        {
            return !string.IsNullOrEmpty(CurrentSelectionPath());
        }

        [MenuItem(NotepadPath, true)]
        private static bool ValidateOpenWithNotepad()
        {
            return !string.IsNullOrEmpty(CurrentSelectionPath());
        }

        private static string CurrentSelectionPath()
        {
            try
            {
                if (Selection.assetGUIDs.Length == 0)
                    return null;

                var clickedAssetGuid = Selection.assetGUIDs[0];
                var clickedPath = AssetDatabase.GUIDToAssetPath(clickedAssetGuid);
                var clickedPathFull = Path.Combine(Directory.GetCurrentDirectory(), clickedPath);

                var attribute = File.GetAttributes(clickedPathFull);

                return attribute == FileAttributes.Archive ? clickedPathFull : null;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return null;
            }
        }
    }
}
#endif