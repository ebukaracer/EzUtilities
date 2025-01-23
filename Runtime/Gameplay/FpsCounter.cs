using UnityEngine;
using UnityEngine.UI;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// A simple FPS counter that displays the best, average, and worst frame duration.
    /// </summary>
    public class FpsCounter : MonoBehaviour
    {
        private float _frameDuration,
            _bestDuration = float.MaxValue,
            _worstDuration;

        private int _frames;
        private float _duration;

        // @formatter:off
        [Header("Display Mode")]
        [SerializeField, Tooltip("Frame per second or Millisecond?")]
        private Mode mode;

        [Space(10)]

        [Header("Text Components")]
        [SerializeField] private Text fpsBestT;
        [SerializeField] private Text fpsAverageT;
        [SerializeField] private Text fpsWorstT;

        [Space(10)]

        [Tooltip("As we want recent information we have to reset and start over frequently, sampling a new average")]
        [SerializeField, Range(.1f, 2f)]
        private float sampleDuration = 1f;
        // @formatter:on


        private void Update()
        {
            _frameDuration = Time.unscaledDeltaTime;
            _frames += 1;
            _duration += _frameDuration;

            if (_frameDuration < _bestDuration)
                _bestDuration = _frameDuration;

            if (_frameDuration > _worstDuration)
                _worstDuration = _frameDuration;

            if (!(_duration >= sampleDuration)) return;

            switch (mode)
            {
                case Mode.FPS:
                    fpsBestT.text = $"Best: {1f / _bestDuration:1}";
                    // fpsAverageT.text = $"Average: {1f * _frames / _duration:1}";
                    fpsAverageT.text = $"Average: {_frames / _duration:1}";
                    fpsWorstT.text = $"Worst: {1f / _worstDuration:1}";
                    break;

                case Mode.MS:
                    fpsBestT.text = $"Best: {1000f * _bestDuration:1}";
                    fpsAverageT.text = $"Average: {1000f * _frames / _duration:1}";
                    fpsWorstT.text = $"Worst: {1000f * _worstDuration:1}";
                    break;
            }

            _frames = 0;
            _duration = 0;
            _bestDuration = float.MaxValue;
            _worstDuration = 0f;
        }

        private enum Mode
        {
            FPS,
            MS
        }
    }
}