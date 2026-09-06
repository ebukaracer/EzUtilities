#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable ClassNeverInstantiated.Global

namespace Racer.EzUtilities.Common.Runtime
{
    internal static class ButtonReflectionCache
    {
        private static readonly Dictionary<Type, List<(MethodInfo method, ButtonAttribute attr)>> Cache
            = new();

        private static readonly Assembly UnityAsm = typeof(Object).Assembly;

        internal static List<(MethodInfo method, ButtonAttribute attr)> GetMethods(Type type)
        {
            if (Cache.TryGetValue(type, out var cached))
                return cached;

            var list = new List<(MethodInfo, ButtonAttribute)>();
            var overriddenBaseMethodHandles = new HashSet<RuntimeMethodHandle>();
            var t = type;

            while (t != null && t.Assembly != UnityAsm)
            {
                var methods = t.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static |
                    BindingFlags.DeclaredOnly);

                foreach (var m in methods)
                {
                    var baseDefinition = m.GetBaseDefinition();
                    var baseHandle = baseDefinition.MethodHandle;

                    if (overriddenBaseMethodHandles.Contains(baseHandle))
                        continue;

                    var attr = m.GetCustomAttribute<ButtonAttribute>();
                    if (attr == null)
                        continue;

                    list.Add((m, attr));

                    if (baseDefinition != m)
                        overriddenBaseMethodHandles.Add(baseHandle);
                }

                t = t.BaseType;
            }

            Cache[type] = list;
            return list;
        }
    }


    public class BehaviourButtonsHelper
    {
        private static readonly object[] NoParams = Array.Empty<object>();
        private Object _target;

        public void Init(Object target)
        {
            _target = target;
        }

        public void DrawButtons()
        {
            var methods = ButtonReflectionCache.GetMethods(_target.GetType());
            if (methods.Count == 0) return;

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);

            foreach (var (method, attr) in methods)
            {
                // Add spacing before each button
                if (attr.Spacing > 0)
                    GUILayout.Space(attr.Spacing);

                DrawStyledButton(method, attr);
            }
        }

        private void DrawStyledButton(MethodInfo method, ButtonAttribute attr)
        {
            var label = ObjectNames.NicifyVariableName(method.Name);
            var guiContent = new GUIContent(label, attr.Tooltip);

            // Save color state
            var defaultColor = GUI.color;

            // Apply color if given
            if (!string.IsNullOrEmpty(attr.Color))
            {
                GUI.color = ColorUtility.TryParseHtmlString(attr.Color, out var parsed)
                    ? parsed
                    : GetNamedColor(attr.Color);
            }

            var clicked = GUILayout.Button(guiContent);

            // Restore GUI color
            GUI.color = defaultColor;

            if (!clicked) return;

            try
            {
                method.Invoke(method.IsStatic ? null : _target, NoParams);
                Save(_target);
            }
            catch (Exception e)
            {
                var ex = e is TargetInvocationException { InnerException: not null } tie
                    ? tie.InnerException
                    : e;

                Debug.LogError($"[Button] Error invoking [{method.Name}]", _target);
                Debug.LogException(ex, _target);
            }
        }

        private static void Save(Object obj)
        {
            EditorUtility.SetDirty(obj);

            if (obj is ScriptableObject)
                AssetDatabase.SaveAssets();
        }

        private static Color GetNamedColor(string name)
        {
            return name.ToLower() switch
            {
                "red" => Color.red,
                "green" => Color.green,
                "blue" => Color.cyan,
                "yellow" => Color.yellow,
                "orange" => new Color(1f, 0.6f, 0f),
                "purple" => new Color(0.6f, 0.2f, 1f),
                _ => Color.white,
            };
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    internal class MonoBehaviourButtonEditor : Editor
    {
        private BehaviourButtonsHelper _helper = new();

        private void OnEnable()
        {
            // Only initialize if single object
            if (targets.Length == 1)
                _helper.Init(target);
        }

        public override void OnInspectorGUI()
        {
            // Skip drawing buttons if multiple objects are selected
            if (targets.Length > 1)
            {
                DrawDefaultInspector();
                return;
            }

            _helper.DrawButtons();
            EditorGUILayout.Space();
            DrawDefaultInspector();
        }
    }

    [CustomEditor(typeof(ScriptableObject), true)]
    internal class ScriptableObjectButtonEditor : Editor
    {
        private BehaviourButtonsHelper _helper = new();

        private void OnEnable()
        {
            // Only initialize if single object
            if (targets.Length == 1)
                _helper.Init(target);
        }

        public override void OnInspectorGUI()
        {
            // Skip drawing buttons if multiple objects are selected
            if (targets.Length > 1)
            {
                DrawDefaultInspector();
                return;
            }

            DrawDefaultInspector();
            _helper.DrawButtons();
        }
    }
}
#endif