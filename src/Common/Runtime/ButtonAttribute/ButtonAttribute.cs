using System;

namespace Racer.EzUtilities.Common.Runtime
{
    /// <summary>
    /// Adds a button to the inspector for the decorated method by using its name.
    /// <remarks>
    /// Allows optional tooltip, spacing before the button, and color (HTML or named).
    /// Applicable to MonoBehaviour and ScriptableObject.
    /// Named colors include: red, green, blue, yellow, orange, purple.
    /// </remarks>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ButtonAttribute : Attribute
    {
        public readonly string Color;
        public readonly string Tooltip;
        public readonly float Spacing;


        public ButtonAttribute(string tooltip = null, float spacing = 0f, string color = null)
        {
            Color = color;
            Tooltip = tooltip;
            Spacing = spacing;
        }
    }
}