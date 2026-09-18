using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.Time
{
    /// <summary>
    /// Manages local timescale and provides scaled delta time for frame-independent gameplay mechanics.
    /// </summary>
    public class EzTimeScale : MonoBehaviour
    {
        /// <summary>
        /// Gets or sets the local timescale.
        /// </summary>
        public static float LocalTimeScale { get; set; } = 1f;

        /// <summary>
        /// Gets the delta time scaled by the local timescale.
        /// </summary>
        public static float DeltaTime => LocalTimeScale * UnityEngine.Time.deltaTime;

        /// <summary>
        /// Gets the timescale multiplied by the local timescale.
        /// </summary>
        public static float TimeScale => UnityEngine.Time.timeScale * LocalTimeScale;

        /// <summary>
        /// Gets a value indicating whether the game is paused.
        /// </summary>
        public static bool IsPaused => LocalTimeScale == 0f;
    }
}