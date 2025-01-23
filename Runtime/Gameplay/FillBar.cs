using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Class representing a fill bar UI element with functionality to decrease fill over time.
    /// <remarks>
    /// Still in development.
    /// </remarks>
    /// </summary>
    public class FillBar : MonoBehaviour
    {
        private Coroutine _decreaseFill;

        private float _initialValue;

        /// <summary>
        /// Event triggered when the decrease starts.
        /// </summary>
        public event Action OnDecreaseStarted;

        /// <summary>
        /// Event triggered when the decrease ends.
        /// </summary>
        public event Action OnDecreaseEnded;

        [SerializeField, Tooltip("Image component representing the fill.")]
        private Image fill;

        [field: SerializeField, Tooltip("Speed at which the fill decreases.")]
        public float FlowSpeed { get; set; } = .3f;

        /// <summary>
        /// Flag to stop the decrease routine.
        /// </summary>
        public bool StopRoutine { get; set; }

        /// <summary>
        /// Changes the sprite of the fill image.
        /// </summary>
        /// <param name="spr">The new sprite.</param>
        public void ChangeFillSprite(Sprite spr) => fill.sprite = spr;

        /// <summary>
        /// Gets or sets the initial fill value.
        /// </summary>
        public float InitialValue
        {
            get => fill.fillAmount;
            set
            {
                _initialValue = value / 100f;
                fill.fillAmount = _initialValue;
            }
        }

        /// <summary>
        /// Coroutine to decrease the fill amount over time.
        /// </summary>
        private IEnumerator OnDecreaseFill()
        {
            OnDecreaseStarted?.Invoke();

            var end = Time.time + FlowSpeed;
            var changeRate = InitialValue / FlowSpeed;

            while (Time.time < end)
            {
                if (StopRoutine)
                    yield break;

                ModifyFill(-changeRate * Time.smoothDeltaTime);
                yield return 0;
            }

            _decreaseFill = null;
            OnDecreaseEnded?.Invoke();
        }

        /// <summary>
        /// Modifies the fill amount by a specified amount.
        /// </summary>
        /// <param name="amount">The amount to modify the fill by.</param>
        private void ModifyFill(float amount)
        {
            fill.fillAmount = amount;
        }

        /// <summary>
        /// Starts the full decrease routine if not already running.
        /// </summary>
        public void FullDecrease()
        {
            _decreaseFill ??= StartCoroutine(OnDecreaseFill());
        }

        /// <summary>
        /// Modifies the fill amount by a specified percentage.
        /// </summary>
        /// <param name="amount">The percentage to modify the fill by (default is 10).</param>
        public void Modify(float amount = 10)
        {
            amount = Mathf.Max(0, amount / 100);
            ModifyFill(amount);
        }
    }
}