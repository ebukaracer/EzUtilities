namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Example of a <see cref="ToggleProvider"/> implementation.
    /// </summary>
    public class ToggleExample : ToggleProvider
    {
        private protected override void Awake()
        {
            // NB: You must call the base method!
            base.Awake();
        }

        protected override void SaveToggleChanges()
        {
            // Save the toggle changes. e.g. Saving to PlayerPrefs
        }

        protected override void RetrieveToggleChanges()
        {
            // Retrieve the toggle changes. e.g. Retrieving from PlayerPrefs
        }

        protected override void ToggleOn()
        {
            // Logic for enabling the toggle. e.g. Toggling Sound ON
        }

        protected override void ToggleOff()
        {
            // Logic for disabling the toggle. e.g. Toggling Sound OFF
        }
    }
}