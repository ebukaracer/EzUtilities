using UnityEngine;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Enum representing the current state of the toggle.
    /// </summary>
    public enum ToggleState
    {
        On,
        Off
    }

    /// <summary>
    /// Derive from this class to represent UI states and changes.
    /// </summary>
    public abstract class ToggleUI : MonoBehaviour
    {
        public abstract void ApplyUIChanges(ToggleState toggleState);
    }

    /// <summary>
    /// Abstract class providing functionality for a toggle mechanism.
    /// </summary>
    public abstract class ToggleProvider : MonoBehaviour
    {
        // @formatter:off
        [field: Header("INTERNAL STATE(readonly)"), Tooltip("Inspect the current state of the toggle")]
        [field: SerializeField] public ToggleState ToggleState { get; protected set; }

        [Space(5), SerializeField] private ToggleUI toggleUI;
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
            ApplyUIChanges();
        }

        /// <summary>
        /// Toggles the state between On and Off, saves the state, and updates the UI.
        /// </summary>
        public void Toggle()
        {
            ToggleState = ToggleState == ToggleState.Off ? ToggleState.On : ToggleState.Off;

            SaveToggleChanges();
            ApplyUIChanges();
        }

        /// <summary>
        /// Applies the appropriate image based on the current toggle state.
        /// </summary>
        private void ApplyUIChanges()
        {
            ApplyToggleChanges();
            toggleUI.ApplyUIChanges(ToggleState);
        }

        /// <summary>
        /// Calls the appropriate method based on the current toggle state.
        /// </summary>
        private void ApplyToggleChanges()
        {
            switch (ToggleState)
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