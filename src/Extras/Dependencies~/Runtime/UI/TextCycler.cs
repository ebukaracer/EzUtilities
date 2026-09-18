#if DOTWEEN
using System.Linq;
using DG.Tweening;
using Racer.EzUtilities.Common.Runtime;
using TMPro;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextCycler : MonoBehaviour
    {
        private Sequence _sequence;
        private TMP_Text _textComponent;

        [SerializeField] private float cycleInterval = 2.5f;
        [SerializeField] [TextArea(2, 4)] private string[] texts;


        private void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();

            // Set initial text
            if (texts.Length > 0) _textComponent.text = texts[0];
        }

        private void PlaySequence()
        {
            if (_sequence == null)
                _sequence = CreateSequence();
            else
                _sequence.Play();
        }

        private void PauseSequence() => _sequence?.Pause();

        private Sequence CreateSequence()
        {
            var cycleIndex = 0;
            var s = DOTween.Sequence();

            s.AppendInterval(cycleInterval)
                .Append(_textComponent.DOFade(0f, 0.4f))
                .AppendCallback(() =>
                {
                    cycleIndex = (cycleIndex + 1) % texts.Length;
                    _textComponent.text = texts[cycleIndex];
                })
                .Append(_textComponent.DOFade(1f, 0.4f))
                .SetLoops(-1)
                .SetAutoKill(false)
                .SetUpdate(true);

            return s;
        }

        public void UpdateTexts(string[] newTexts)
        {
            if (ReferenceEquals(newTexts, texts)) return;

            texts = newTexts;

            if (texts.Length <= 0) return;

            _textComponent.text = texts[0];
            _sequence.Restart();
        }

        public void UpdateTextByIndex(int index, string text)
        {
            if (index < 0 || index >= texts.Length) return;

            _textComponent.text = texts[index] = text;
            _sequence.Restart();
        }

        [Button]
        private void CycleTexts()
        {
            if (texts.Length == 0) return;
            Awake();
            var currentIndex = texts.ToList().FindIndex(t => t == _textComponent.text);
            var nextIndex = (currentIndex + 1) % texts.Length;
            _textComponent.text = texts[nextIndex];
        }
    }
}
#endif