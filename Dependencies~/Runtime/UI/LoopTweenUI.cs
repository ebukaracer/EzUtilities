#if DOTWEEN
using DG.Tweening;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.UI
{
    public enum JuiceType
    {
        QuickPunchScale,
        QuickPulse
    }

    public class LoopTweenUI : MonoBehaviour
    {
        private bool _wasPlaying;
        private Sequence _shakeSequence;

        [SerializeField] private JuiceType juiceType;
        [SerializeField] private float interval = 4;


        private void OnDestroy()
        {
            StopSequence();
        }

        private Sequence CreateSequence()
        {
            var sequence = DOTween.Sequence();

            switch (juiceType)
            {
                case JuiceType.QuickPunchScale:
                    sequence
                        .AppendInterval(interval) // Idle time
                        .Append(transform.DOShakeRotation(0.5f, 15f, 10, 90)) // Shake
                        .Join(transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f),
                            0.5f)) // Punch Scale simultaneously
                        .SetAutoKill(false)
                        .SetUpdate(true) // Use unscaled time
                        .SetLoops(-1); // Loop infinitely        
                    break;
                case JuiceType.QuickPulse:
                    sequence
                        .AppendInterval(interval) // Idle time
                        .Append(transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutQuad)) // Scale up
                        .Append(transform.DOScale(1f, 0.1f).SetEase(Ease.InQuad)) // Scale back down
                        .SetAutoKill(false)
                        .SetUpdate(true) // Use unscaled time
                        .SetLoops(-1); // Loop infinitely
                    break;
            }

            return sequence;
        }

        public void PlaySequence()
        {
            if (_shakeSequence != null)
            {
                if (!_shakeSequence.IsPlaying())
                    _shakeSequence.Play();
            }
            else
                _shakeSequence = CreateSequence();
        }

        public void PauseSequence()
        {
            if (_shakeSequence == null || !_shakeSequence.IsPlaying()) return;

            _shakeSequence.Complete();
            _shakeSequence.Pause();
        }

        public void StopSequence()
        {
            _shakeSequence?.Kill(true);
            _shakeSequence = null;
        }
    }
}
#endif