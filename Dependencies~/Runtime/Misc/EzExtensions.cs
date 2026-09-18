using System.Collections;
using UnityEngine;

namespace Racer.EzUtilities.Extras.Runtime.Misc
{
    /// <summary>
    /// Static class providing extension methods for various Unity components.
    /// </summary>
    public static class EzExtensions
    {
        /// <summary>
        /// Detects if editor is in playmode or not and destroys all child objects of the given parent transform.
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed.</param>
        public static void DestroyChildren(this Transform parent)
        {
            var childCount = parent.childCount;

            if (childCount <= 0) return;

            // Check if we're in play mode
            if (Application.isPlaying)
            {
                for (var i = childCount - 1; i >= 0; i--)
                    Object.Destroy(parent.GetChild(i).gameObject);
            }
            else
            {
                for (var i = childCount - 1; i >= 0; i--)
                    Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Expands an existing bounds to include the given bounds.
        /// </summary>
        /// <param name="a">The original bounds.</param>
        /// <param name="b">The bounds to include.</param>
        /// <returns>The expanded bounds.</returns>
        public static Bounds GrowBounds(this Bounds a, Bounds b)
        {
            var max = Vector3.Max(a.max, b.max);
            var min = Vector3.Min(a.min, b.min);

            a = new Bounds((max + min) * 0.5f, max - min);

            return a;
        }

        /// <summary>
        /// Sets the transform's local position to a random position within the specified ranges.
        /// </summary>
        /// <param name="transform">The transform to set the position of.</param>
        /// <param name="xPos">The range for the x position.</param>
        /// <param name="yPos">The range for the y position.</param>
        /// <param name="zPos">The range for the z position.</param>
        public static void RandomizePosition(this Transform transform, float xPos, float yPos, float zPos)
        {
            transform.localPosition = new Vector3(
                Random.Range(-xPos, xPos),
                Random.Range(-yPos, yPos),
                Random.Range(-zPos, zPos)
            );
        }

        /// <summary>
        /// Sets the transform's rotation to a random rotation within the specified ranges.
        /// </summary>
        /// <param name="transform">The transform to set the rotation of.</param>
        /// <param name="xRot">The range for the x rotation.</param>
        /// <param name="yRot">The range for the y rotation.</param>
        /// <param name="zRot">The range for the z rotation.</param>
        public static void RandomizeRotation(this Transform transform, float xRot, float yRot, float zRot)
        {
            transform.rotation = Quaternion.Euler(
                Random.Range(-xRot, xRot),
                Random.Range(-yRot, yRot),
                Random.Range(-zRot, zRot)
            );
        }

        /// <summary>
        /// Shakes the transform for the specified duration and magnitude.
        /// </summary>
        /// <param name="myTransform">The transform to shake.</param>
        /// <param name="duration">The duration of the shake.</param>
        /// <param name="magnitude">The magnitude of the shake.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator ShakePosition(this Transform myTransform, float duration, float magnitude)
        {
            var originalPosition = myTransform.localPosition;

            var elapsed = 0.0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * magnitude;
                var y = Random.Range(-1f, 1f) * magnitude;

                myTransform.localPosition =
                    new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                elapsed += UnityEngine.Time.deltaTime;

                yield return null;
            }

            myTransform.localPosition = originalPosition;
        }
    }
}