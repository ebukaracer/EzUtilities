using UnityEngine;

namespace Racer.EzUtilities.Core.Scripts.Runtime
{
    /// <summary>
    /// Manages the initialization of prefabs marked as DontDestroyOnLoad to ensure they persist across scenes.
    /// Prevents redundant reinitialization by spawning these prefabs only once during the application's lifetime.
    /// </summary>
    /// <remarks>
    /// For this to work, add this script to a prefab and move it to "Assets/Resources" location.
    /// The (DDOL)prefabs should be nested under this gameobject as its children.
    /// </remarks>
    [DefaultExecutionOrder(-100), DisallowMultipleComponent]
    public class DdolPrefabsLoader : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void SpawnDdolPrefabs()
        {
            var go = Resources.Load<GameObject>(nameof(DdolPrefabsLoader));

            if (!go)
                return;

            Destroy(Instantiate(go), .5f);
        }
    }
}