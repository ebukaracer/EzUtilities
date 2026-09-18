using System;
using TMPro;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.UI
{
    public class TextConsoleSimulator : MonoBehaviour
    {
        private const float SpeedPerChar = .02f;

        // private SoundCore _soundCore;
        private TMP_Text _tmpText;
        private Action _onComplete;

        private string _previousText;
        private float _delayTimer;
        private float _revealTimer;
        private int _currentCount;
        private int _targetCount;
        private bool _isAnimating;
        private bool _isRevealing;
        private bool _waitingForDelay;

        [SerializeField] private float speed = SpeedPerChar;


        /*
        private void Awake()
        {
            _soundCore = SoundCore.Instance;
        }
        */

        private void Update()
        {
            if (!_isAnimating)
                return;

            if (_waitingForDelay)
            {
                _delayTimer -= UnityEngine.Time.deltaTime;

                if (_delayTimer > 0)
                    return;

                _waitingForDelay = false;
            }

            _revealTimer += UnityEngine.Time.deltaTime;

            while (_revealTimer >= speed)
            {
                _revealTimer -= speed;

                if (_isRevealing)
                {
                    _currentCount++;

                    if (_currentCount >= _targetCount)
                    {
                        _currentCount = _targetCount;
                        SetMaxVisible(_tmpText, _currentCount);

                        CompleteAnimation();
                        return;
                    }
                }
                else
                {
                    _currentCount--;

                    if (_currentCount <= 0)
                    {
                        _currentCount = 0;
                        SetMaxVisible(_tmpText, _currentCount);

                        CompleteAnimation();
                        return;
                    }
                }

                SetMaxVisible(_tmpText, _currentCount);
            }
        }

        public void RevealText(
            TMP_Text tmpText,
            Action onComplete = null,
            float delay = 0,
            bool animate = false,
            bool forceReveal = false,
            float? speedPerChar = SpeedPerChar)
        {
            StopAnimation();

            if (_previousText == tmpText.text && !forceReveal)
            {
                onComplete?.Invoke();
                return;
            }

            speed = speedPerChar ?? speed;
            _previousText = tmpText.text;
            _tmpText = tmpText;
            _onComplete = onComplete;

            _tmpText.ForceMeshUpdate();

            _targetCount = GetRevealCount(_tmpText.textInfo);

            if (!animate)
            {
                SetMaxVisible(_tmpText, _targetCount);
                onComplete?.Invoke();
                return;
            }

            _currentCount = 0;
            SetMaxVisible(_tmpText, 0);

            _delayTimer = delay;
            _revealTimer = 0;

            _waitingForDelay = delay > 0;
            _isAnimating = true;
            _isRevealing = true;

            ToggleSfx();
        }

        public void HideText(
            TMP_Text tmpText,
            Action onComplete = null,
            bool animate = false,
            bool forceHide = false,
            float? speedPerChar = SpeedPerChar)
        {
            StopAnimation();

            if (_previousText == tmpText.text && !forceHide)
            {
                onComplete?.Invoke();
                return;
            }

            speed = speedPerChar ?? speed;
            _previousText = tmpText.text;
            _tmpText = tmpText;
            _onComplete = onComplete;

            _tmpText.ForceMeshUpdate();

            _targetCount = GetRevealCount(_tmpText.textInfo);

            if (!animate)
            {
                SetMaxVisible(_tmpText, 0);
                onComplete?.Invoke();
                return;
            }

            _currentCount = _targetCount;
            SetMaxVisible(_tmpText, _currentCount);

            _delayTimer = 0;
            _revealTimer = 0;

            _waitingForDelay = false;
            _isAnimating = true;
            _isRevealing = false;

            ToggleSfx();
        }

        public void StopAnimation()
        {
            if (!_isAnimating)
                return;

            _isAnimating = false;
            _waitingForDelay = false;

            ToggleSfx(false);
        }

        private void CompleteAnimation()
        {
            _isAnimating = false;
            _waitingForDelay = false;

            ToggleSfx(false);

            _onComplete?.Invoke();
        }

        private static int GetRevealCount(TMP_TextInfo tmpTextInfo)
        {
            return tmpTextInfo.characterCount;
        }

        private static void SetMaxVisible(TMP_Text tmpText, int count)
        {
            tmpText.maxVisibleCharacters = count;
        }

        private void ToggleSfx(bool play = true)
        {
            /*
             var sfxSrc = _soundCore.GetSfxSrc(1);

            if (!sfxSrc || !sfxSrc.enabled)
                return;

            if (play)
            {
                if (!sfxSrc.isPlaying)
                    sfxSrc.Play();
            }
            else
            {
                if (sfxSrc.isPlaying)
                    sfxSrc.Stop();
            }
            */
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void OnDestroy()
        {
            StopAnimation();
        }
    }
}