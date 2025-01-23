using System;
using System.Diagnostics;
using UnityEngine;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Provides a timer functionality using a Stopwatch.
    /// </summary>
    public class EzTimer : MonoBehaviour
    {
        /// <summary>
        /// The Stopwatch instance used to measure elapsed time.
        /// </summary>
        private Stopwatch _stopwatch;

        /// <summary>
        /// Gets the elapsed time measured by the Stopwatch.
        /// </summary>
        public TimeSpan Time => _stopwatch.Elapsed;

        /// <summary>
        /// Initializes the Stopwatch instance.
        /// </summary>
        protected void Awake()
        {
            _stopwatch = new Stopwatch();
        }

        /// <summary>
        /// Starts the timer if it is not already running.
        /// </summary>
        public void StartTimer()
        {
            if (!_stopwatch.IsRunning)
                _stopwatch.Start();
        }

        /// <summary>
        /// Stops the timer if it is currently running.
        /// </summary>
        public void StopTimer()
        {
            if (_stopwatch.IsRunning)
                _stopwatch.Stop();
        }

        /// <summary>
        /// Stops the timer when the object is disabled.
        /// </summary>
        private void OnDisable()
        {
            StopTimer();
        }
    }
}