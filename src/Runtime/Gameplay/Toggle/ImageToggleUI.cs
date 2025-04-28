using UnityEngine;
using UnityEngine.UI;

namespace Racer.EzUtilities.Gameplay
{
    /// <summary>
    /// Example of a <see cref="ToggleUI"/> implementation.
    /// </summary>
    public class ImageToggleUI : ToggleUI
    {
        [SerializeField, Tooltip("Base UI image")]
        private Image parentIcon;

        [SerializeField, Tooltip("Sprite for the 'ON' state")]
        private Sprite onIcon;

        [SerializeField, Tooltip("Sprite for the 'OFF' state")]
        private Sprite offIcon;


        public override void ApplyUIChanges(ToggleState toggleState)
        {
            parentIcon.sprite = toggleState == ToggleState.On ? onIcon : offIcon;
        }
    }
}