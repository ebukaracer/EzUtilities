using UnityEngine;
#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace Racer.EzUtilities.Core.Scripts.Runtime
{
    /// <summary>
    /// Defines different types of haptic feedback (vibration) that can be triggered on Android devices.
    /// All are expressed in ms, where the value represents the duration of the vibration.
    /// </summary>
    public enum Weight
    {
        Light = 30,
        Medium = 50,
        Heavy = 100
    }

    /// <summary>
    /// Provides methods to control haptic feedback (vibration) on Android devices.
    /// Ensure to add vibration permissions in your Android manifest: com.android.permission.VIBRATE
    /// </summary>
    public static class HapticsManager
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// Reference to the UnityPlayer class for accessing Android functionality.
        /// </summary>
        private static AndroidJavaClass _unityPlayer = new("com.unity3d.player.UnityPlayer");

        /// <summary>
        /// Reference to the current Android activity.
        /// </summary>
        private static AndroidJavaObject _currentActivity =
 _unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        /// <summary>
        /// Reference to the Android vibrator service.
        /// </summary>
        private static AndroidJavaObject _vibrator =
 _currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
        /// <summary>
        /// Placeholder for the Android vibrator service when not on an Android device.
        /// </summary>
        private static AndroidJavaObject _vibrator;
#endif

        /// <summary>
        /// Checks if the current platform is Android.
        /// </summary>
        /// <returns>True if the platform is Android, false otherwise.</returns>
        private static bool IsAndroid()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }

        /// <summary>
        /// Gets a value indicating whether the device has a vibrator and vibration is not muted.
        /// </summary>
        private static bool HasVibrator => !IsMuteVibration && IsAndroid();

        /// <summary>
        /// Triggers a vibration on the device for a specified duration.
        /// </summary>
        /// <param name="weight">The duration of the vibration, specified as a Weight enum value.</param>
        public static void Vibrate(Weight weight = Weight.Medium)
        {
            if (HasVibrator)
                _vibrator.Call("vibrate", (long)weight);
        }

        /// <summary>
        /// Triggers a vibration on the device for a specified duration.
        /// </summary>
        /// <param name="weight">The duration of the vibration, specified as an integer.</param>
        public static void Vibrate(int weight)
        {
            if (HasVibrator)
                _vibrator.Call("vibrate", (long)weight);
        }

        /// <summary>
        /// Triggers a vibration on the device with a specified pattern and repeat index.
        /// </summary>
        /// <param name="pattern">An array of longs specifying the vibration pattern.</param>
        /// <param name="repeat">The index in the pattern at which to repeat, or -1 for no repeat.</param>
        public static void Vibrate(long[] pattern, int repeat)
        {
            if (HasVibrator)
                _vibrator.Call("vibrate", pattern, repeat);
        }

        /// <summary>
        /// Cancels any ongoing vibration on the device.
        /// </summary>
        public static void Cancel()
        {
            if (HasVibrator)
                _vibrator.Call("cancel");
        }

        /// <summary>
        /// Mutes or unmutes the vibration functionality.
        /// </summary>
        /// <param name="state">True to mute vibration, false to unmute.</param>
        public static void Mute(bool state)
        {
            IsMuteVibration = state;
        }

        /// <summary>
        /// Gets or sets a value indicating whether vibration is muted.
        /// </summary>
        private static bool IsMuteVibration { get; set; }
    }
}