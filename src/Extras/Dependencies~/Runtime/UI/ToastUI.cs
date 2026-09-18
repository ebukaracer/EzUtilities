#if DOTWEEN
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ToastUI : MonoBehaviour
    {
        public enum ToastEffect
        {
            Fade,
            SlideUp,
            Pop,
            PunchScale,
            Shake
        }

        private bool _isEditorValidation;

        private Sequence _sequence;
        private Vector2 _initialOffset;
        private Vector3 _initialScale;
        private Quaternion _initialRotation;

        // Core references
        private RectTransform _rectTransform;
        private TextMeshProUGUI _descTxt;
        private CanvasGroup _canvasGroup;

        [SerializeField] private ToastEffect defaultEffect = ToastEffect.Fade;
        [SerializeField] private float duration = 0.25f;
        [SerializeField] private float slideDistance = 72f;
        [SerializeField] private float punchScale = 0.12f;
        [SerializeField] private float shakeStrength = 12f;

        public static ToastUI Instance { get; private set; }


        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _descTxt = GetComponentInChildren<TextMeshProUGUI>();

            if (!_rectTransform || !_canvasGroup || !_descTxt)
            {
                Debug.LogWarning(
                    "One or more required components are not assigned in the gameobject See details below." +
                    "\nEnsure that:" +
                    $"\n- A {nameof(RectTransform)} component is attached." +
                    $"\n- A {nameof(CanvasGroup)} component is attached." +
                    $"\n- A {nameof(TextMeshProUGUI)} component is present in the children.",
                    this);

                enabled = !_isEditorValidation;
                return;
            }

            if (_isEditorValidation) return;

            if (!Instance)
                Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            _initialOffset = _rectTransform.anchoredPosition;
            _initialScale = _rectTransform.localScale;
            _initialRotation = _rectTransform.localRotation;
            _canvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            _sequence?.Kill();
        }

        public void ShowToast(string desc, bool autoDestroy = true, Vector2 offset = default, float durationExtent = 0f)
        {
            var visibleDuration = autoDestroy || durationExtent > 0f ? duration + durationExtent : -1f;
            ShowToast(desc, defaultEffect, visibleDuration, offset);
        }

        public void ShowToast(string desc, ToastEffect effect, float visibleDuration = 1.5f, Vector2 offset = default)
        {
            if (_descTxt.text != desc)
                _descTxt.text = desc;

            _sequence?.Kill();

            var targetOffset = offset != default ? offset : _initialOffset;

            _rectTransform.anchoredPosition = targetOffset;
            _rectTransform.localScale = _initialScale;
            _rectTransform.localRotation = _initialRotation;
            _canvasGroup.alpha = 0f;

            _sequence = DOTween
                .Sequence()
                .SetRecyclable(true)
                .SetLink(gameObject);

            switch (effect)
            {
                case ToastEffect.Fade:
                    _sequence.Append(_canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
                    break;

                case ToastEffect.SlideUp:
                    _rectTransform.anchoredPosition = targetOffset + Vector2.down * slideDistance;
                    _sequence.Append(_rectTransform.DOAnchorPos(targetOffset, duration).SetEase(Ease.OutCubic));
                    _sequence.Join(_canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
                    break;

                case ToastEffect.Pop:
                    _rectTransform.localScale = _initialScale * 0.85f;
                    _sequence.Append(_rectTransform.DOScale(_initialScale, duration).SetEase(Ease.OutBack));
                    _sequence.Join(_canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
                    break;

                case ToastEffect.PunchScale:
                    _sequence.Append(_rectTransform.DOPunchScale(Vector3.one * punchScale, duration, 8, 0.8f));
                    _sequence.Join(_canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
                    break;

                case ToastEffect.Shake:
                    _sequence.Append(_rectTransform.DOShakeAnchorPos(duration, shakeStrength, 12, 90f, false,
                        true));
                    _sequence.Join(_canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad));
                    break;
            }

            if (visibleDuration < 0f)
                return;

            _sequence.AppendInterval(visibleDuration);

            switch (effect)
            {
                case ToastEffect.SlideUp:
                    _sequence.Append(_rectTransform.DOAnchorPos(targetOffset + Vector2.up * 12f, duration)
                        .SetEase(Ease.InCubic));
                    _sequence.Join(_canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad));
                    break;

                case ToastEffect.Pop:
                case ToastEffect.PunchScale:
                    _sequence.Append(_rectTransform.DOScale(_initialScale * 0.92f, duration)
                        .SetEase(Ease.InQuad));
                    _sequence.Join(_canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad));
                    break;

                case ToastEffect.Shake:
                    _sequence.Append(_rectTransform.DOShakeAnchorPos(duration, shakeStrength * 0.5f, 8, 90f,
                        false, true));
                    _sequence.Join(_canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad));
                    break;

                default:
                    _sequence.Append(_canvasGroup.DOFade(0f, duration).SetEase(Ease.InQuad));
                    break;
            }
        }

        private void ToggleOnOff()
        {
            _isEditorValidation = true;
            Awake();
            _canvasGroup.alpha = _canvasGroup.alpha == 0 ? 1 : 0;
            _isEditorValidation = false;
        }
    }

#if UNITY_EDITOR
    // It's absolutely necessary to have this editor class here.
    [UnityEditor.CustomEditor(typeof(ToastUI))]
    internal class ToastUIEditor : UnityEditor.Editor
    {
        // ReSharper disable InconsistentNaming
        private UnityEditor.SerializedProperty defaultEffect;
        private UnityEditor.SerializedProperty duration;
        private UnityEditor.SerializedProperty slideDistance;
        private UnityEditor.SerializedProperty punchScale;
        private UnityEditor.SerializedProperty shakeStrength;

        private void OnEnable()
        {
            defaultEffect = serializedObject.FindProperty($"{nameof(defaultEffect)}");
            duration = serializedObject.FindProperty($"{nameof(duration)}");
            slideDistance = serializedObject.FindProperty($"{nameof(slideDistance)}");
            punchScale = serializedObject.FindProperty($"{nameof(punchScale)}");
            shakeStrength = serializedObject.FindProperty($"{nameof(shakeStrength)}");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            UnityEditor.EditorGUI.BeginChangeCheck();

            UnityEditor.EditorGUILayout.PropertyField(defaultEffect);
            UnityEditor.EditorGUILayout.PropertyField(duration);

            var effect = (ToastUI.ToastEffect)defaultEffect.enumValueIndex;

            switch (effect)
            {
                case ToastUI.ToastEffect.SlideUp:
                    UnityEditor.EditorGUILayout.PropertyField(slideDistance);
                    break;

                case ToastUI.ToastEffect.PunchScale:
                    UnityEditor.EditorGUILayout.PropertyField(punchScale);
                    break;

                case ToastUI.ToastEffect.Shake:
                    UnityEditor.EditorGUILayout.PropertyField(shakeStrength);
                    break;
            }

            UnityEditor.EditorGUILayout.Space();

            if (GUILayout.Button("Toggle On/Off"))
            {
                var method = typeof(ToastUI).GetMethod(
                    "ToggleOnOff",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

                method?.Invoke(target, null);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
#endif