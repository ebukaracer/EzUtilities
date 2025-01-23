using UnityEngine;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Provides custom time management functionality.
    /// </summary>
    public class EzTime : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the local time scale.
        /// </summary>
        public static float LocalTimeScale { get; set; } = 1f;

        /// <summary>
        /// Gets the delta time scaled by the local time scale.
        /// </summary>
        public static float DeltaTime => LocalTimeScale * Time.deltaTime;

        /// <summary>
        /// Gets the time scale multiplied by the local time scale.
        /// </summary>
        public static float TimeScale => Time.timeScale * LocalTimeScale;

        /// <summary>
        /// Gets a value indicating whether the game is paused.
        /// </summary>
        public static bool IsPaused => LocalTimeScale == 0f;
    }
}