using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = System.Random;

namespace Racer.EzUtilities.Core
{
    /// <summary>
    /// Utility class that contains helper methods for common tasks.
    /// </summary>
    public static class EzUtility
    {
        #region CameraMain

        private static Camera _cameraMain;

        /// <summary>
        /// Gets a one time reference to the Camera.Main Method. 
        /// </summary>
        public static Camera CameraMain
        {
            get
            {
                if (_cameraMain == null)
                    _cameraMain = Camera.main;

                return _cameraMain;
            }
        }

        #endregion

        #region FindByTag

        /// <summary>
        /// Finds and returns a gameobject's component by the specified tag.
        /// </summary>
        /// <typeparam name="T">Type of component to return</typeparam>
        /// <param name="tag">Tag specified in the Inspector.</param>
        public static T FindByTag<T>(string tag) where T : MonoBehaviour
        {
            return GameObject.FindGameObjectWithTag(tag).GetComponent<T>();
        }

        #endregion

        #region GetWaitForSeconds

        private static readonly Dictionary<float, WaitForSeconds> WaitDelay = new();

        /// <summary>
        /// Container that stores/reuses newly created WaitForSeconds.
        /// </summary>
        /// <param name="time">time(s) to wait</param>
        /// <returns>new WaitForSeconds</returns>
        public static WaitForSeconds GetWaitForSeconds(float time)
        {
            if (WaitDelay.TryGetValue(time, out var waitForSeconds)) return waitForSeconds;

            WaitDelay[time] = new WaitForSeconds(time);

            return WaitDelay[time];
        }

        #endregion

        #region IsPointerOverUI

        private static PointerEventData _eventDataCurrentPosition;

        private static readonly List<RaycastResult> RaycastResults = new();

        /// <summary>
        /// Checks if the mouse/pointer is over a UI element.
        /// </summary>
        /// <returns>true if the pointer is over a UI element</returns>
        public static bool IsPointerOverUI()
        {
            _eventDataCurrentPosition = new PointerEventData(EventSystem.current)
                { position = Input.mousePosition };

            EventSystem.current.RaycastAll(_eventDataCurrentPosition, RaycastResults);

            return RaycastResults.Count > 0;
        }

        #endregion

        #region GetWorldPositionOfCanvasElement

        /// <summary>
        /// Gets the world position of a canvas element, can be used to spawn a 3d element in the 2d canvas.
        /// </summary>
        /// <param name="rectTransform"> Canvas element(ui elements) </param>
        /// <returns>The position of the canvas element in world space</returns>
        public static Vector2 GetWorldPositionOfCanvasElement(RectTransform rectTransform)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform,
                rectTransform.position,
                CameraMain,
                out var output);

            return output;
        }

        #endregion

        #region ScreenDimension

        /// <summary>
        /// Returns the current screen dimension set.
        /// </summary>
        public static Vector3 ScreenDimension =>
            CameraMain.ScreenToWorldPoint(new Vector3(Screen.width,
                Screen.height,
                0));

        #endregion

        #region ColliderCheck

        /// <summary>
        /// Checks if an object with collider is present in the specified <paramref name="spawnPoint"/>.
        /// </summary>
        /// <param name="spawnPoint">Position to check for object's presence.</param>
        /// <param name="objects">Objects(with collider) to detect or include during check.</param>
        /// <param name="radius">Objects within this range would be accounted for.</param>
        /// <param name="queryTrigger">Whether or not to hit trigger objects.</param>
        /// <returns>True if an object with collider is present otherwise false.</returns>
        public static bool IsColliderPresent(Vector3 spawnPoint,
            LayerMask objects,
            float radius = 1.5f,
            QueryTriggerInteraction queryTrigger = QueryTriggerInteraction.Collide)
        {
            var size = Physics.OverlapSphereNonAlloc(spawnPoint, radius, new Collider[1], objects, queryTrigger);

            return size > 0;
        }

        #endregion

        #region RandomizeTexts

        /// <summary>
        /// Returns an array of shuffled strings.
        /// </summary>
        /// <param name="texts">String Array to Shuffle</param>
        /// <returns>Shuffled string[]</returns>
        public static string[] GetRandomizedTexts(string[] texts)
        {
            return texts.OrderBy(_ => new Random().Next())
                .ToArray();
        }

        #endregion

        #region ConvertMsToSeconds

        /// <summary>
        /// Converts a millisecond value to seconds.
        /// </summary>
        /// <param name="value">Millisecond value</param>
        public static int MsToSeconds(float value)
        {
            var timeMultiplier = 1000;

            return (int)(value * timeMultiplier);
        }

        #endregion

        #region GetAnimId

        /// <summary>
        /// Generates a parameter id(int) from an animator string.
        /// </summary>
        /// <param name="id">Parameter name(string) from animator</param>
        /// <returns></returns>
        public static int GetAnimId(string id)
        {
            return Animator.StringToHash(id);
        }

        #endregion
    }
}