using System.Collections;
using UnityEngine;

namespace Racer.EzUtilities.Core
{
    /// <summary>
    /// Static class providing extension methods for various Unity components.
    /// </summary>
    public static class EzExtensions
    {
        /// <summary>
        /// Destroys all child objects of the given parent transform in play mode.
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed.</param>
        public static void DestroyChildren(this Transform parent)
        {
            var childCount = parent.childCount;

            if (childCount <= 0) return;

            for (var i = childCount - 1; i >= 0; i--)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Destroys all child objects of the given parent transform immediately in the editor.
        /// </summary>
        /// <param name="parent">The parent transform whose children will be destroyed.</param>
        public static void DestroyChildrenImmediate(this Transform parent)
        {
            var childCount = parent.childCount;

            if (childCount <= 0) return;

            for (var i = childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Expands the bounds to include the given bounds.
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
        public static void RandomPositions(this Transform transform, float xPos, float yPos, float zPos)
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
        public static void RandomRotation(this Transform transform, float xRot, float yRot, float zRot)
        {
            transform.rotation = Quaternion.Euler(
               Random.Range(-xRot, xRot),
               Random.Range(-yRot, yRot),
               Random.Range(-zRot, zRot)
               );
        }

        /// <summary>
        /// Returns a new vector with the same x and z values but a new y value.
        /// </summary>
        /// <param name="vector">The original vector.</param>
        /// <param name="newY">The new y value.</param>
        /// <returns>The new vector with the updated y value.</returns>
        public static Vector3 WithNewY(this Vector3 vector, float newY) => new(vector.x, newY, vector.z);

        /// <summary>
        /// Shakes the transform for the specified duration and magnitude.
        /// </summary>
        /// <param name="myTransform">The transform to shake.</param>
        /// <param name="duration">The duration of the shake.</param>
        /// <param name="magnitude">The magnitude of the shake.</param>
        /// <returns>An IEnumerator for use in a coroutine.</returns>
        public static IEnumerator Shake(this Transform myTransform, float duration, float magnitude)
        {
            var originalPosition = myTransform.localPosition;

            var elapsed = 0.0f;

            while (elapsed < duration)
            {
                var x = Random.Range(-1f, 1f) * magnitude;
                var y = Random.Range(-1f, 1f) * magnitude;

                myTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                elapsed += Time.deltaTime;

                yield return null;
            }

            myTransform.localPosition = originalPosition;
        }

        /// <summary>
        /// Toggles the active state of the game object.
        /// </summary>
        /// <param name="gameObject">The game object to toggle.</param>
        /// <param name="state">The state to set the game object to.</param>
        public static void ToggleActive(this GameObject gameObject, bool state)
        {
            if (state)
            {
                if (!gameObject.activeInHierarchy)
                    gameObject.SetActive(true);
            }
            else
            {
                if (gameObject.activeInHierarchy)
                    gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Toggles the enabled state of the behaviour.
        /// </summary>
        /// <param name="behaviour">The behaviour to toggle.</param>
        /// <param name="state">The state to set the behaviour to.</param>
        public static void IsEnabled(this Behaviour behaviour, bool state)
        {
            if (state)
            {
                if (!behaviour.enabled)
                    behaviour.enabled = true;
            }
            else
            {
                if (behaviour.enabled)
                    behaviour.enabled = false;
            }
        }
    }
}