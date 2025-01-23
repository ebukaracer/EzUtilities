using UnityEngine;
using UnityEngine.UI;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Enum representing the state of the toggle.
    /// </summary>
    public enum ToggleState
    {
        Off,
        On
    }

    /// <summary>
    /// Abstract class providing functionality for a toggle mechanism.
    /// </summary>
    public abstract class ToggleProvider : MonoBehaviour
    {
        // @formatter:off
        [Header("INTERNAL STATE(readonly)"),
         SerializeField, Tooltip("Inspect the current state of the toggle")]
        private ToggleState toggleState;

        [Space(5),
         Header("TARGET GRAPHICS")]
        [SerializeField, Tooltip("Base UI image")] private Image parentIcon;
        [SerializeField, Tooltip("Sprite for the 'ON' state")] private Sprite onIcon;
        [SerializeField, Tooltip("Sprite for the 'OFF' state")] private Sprite offIcon;
        // @formatter:on

        /// <summary>
        /// Unity method called when the script instance is being loaded.
        /// Initializes the toggle.
        /// </summary>
        private protected virtual void Awake()
        {
            InitToggle();
        }

        /// <summary>
        /// Initializes the toggle by retrieving and applying the current state.
        /// </summary>
        private void InitToggle()
        {
            RetrieveToggleChanges();
            ApplyImageChanges();
        }

        /// <summary>
        /// Toggles the state between On and Off, saves the state, and updates the UI.
        /// </summary>
        public void Toggle()
        {
            toggleState = toggleState == ToggleState.Off ? ToggleState.On : ToggleState.Off;

            SaveToggleChanges();
            ApplyImageChanges();
        }

        /// <summary>
        /// Applies the appropriate image based on the current toggle state.
        /// </summary>
        private void ApplyImageChanges()
        {
            ApplyToggleChanges();
            parentIcon.sprite = toggleState == ToggleState.On ? onIcon : offIcon;
        }

        /// <summary>
        /// Calls the appropriate method based on the current toggle state.
        /// </summary>
        private void ApplyToggleChanges()
        {
            switch (toggleState)
            {
                case ToggleState.On:
                    ToggleOn();
                    break;
                case ToggleState.Off:
                    ToggleOff();
                    break;
            }
        }

        /// <summary>
        /// Abstract method to be implemented by subclasses to define behavior when toggled on.
        /// </summary>
        protected abstract void ToggleOn();

        /// <summary>
        /// Abstract method to be implemented by subclasses to define behavior when toggled off.
        /// </summary>
        protected abstract void ToggleOff();

        // @formatter:off
        /// <summary>
        /// Virtual method to save the current toggle state. Can be overridden by subclasses.
        /// </summary>
        protected virtual void SaveToggleChanges() { }

        /// <summary>
        /// Virtual method to retrieve the current toggle state. Can be overridden by subclasses.
        /// </summary>
        protected virtual void RetrieveToggleChanges() { }
        // @formatter:on
    }
}