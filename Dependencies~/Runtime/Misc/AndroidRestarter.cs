#if UNITY_ANDROID || UNITY_EDITOR
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.Misc
{
    /// <summary>
    /// Utility class for restarting an Android application.
    /// </summary>
    public static class AndroidRestarter
    {
        /// <summary>
        /// Restarts the Android application.
        /// </summary>
        public static void Restart()
        {
            // Return if running in the Unity editor
            if (Application.isEditor)
            {
                Debug.LogWarning("");
                return;
            }

            // Create an instance of the UnityPlayer class
            using var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            // Constants for intent flags
            const int kIntentFlagActivityClearTask = 0x00008000;
            const int kIntentFlagActivityNewTask = 0x10000000;

            // Get the current activity
            var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            // Get the package manager
            var pm = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            // Get the launch intent for the current package
            var intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", Application.identifier);

            // Set the intent flags
            intent.Call<AndroidJavaObject>("setFlags", kIntentFlagActivityNewTask | kIntentFlagActivityClearTask);
            // Start the activity
            currentActivity.Call("startActivity", intent);
            // Finish the current activity
            currentActivity.Call("finish");
            // Get the process class
            var process = new AndroidJavaClass("android.os.Process");
            // Get the current process ID
            var pid = process.CallStatic<int>("myPid");
            // Kill the current process
            process.CallStatic("killProcess", pid);
        }
    }
}
#endif