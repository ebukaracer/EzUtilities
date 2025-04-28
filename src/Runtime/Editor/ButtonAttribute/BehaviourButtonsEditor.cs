#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Racer.EzUtilities.Editor
{
    public class BehaviourButtonsHelper
    {
        private static object[] _emptyParamList = Array.Empty<object>();

        private IList<MethodInfo> _methods = new List<MethodInfo>();
        private Object _targetObject;

        public void Init(Object targetObject)
        {
            _targetObject = targetObject;

            var type = targetObject.GetType();
            var methods = new List<MethodInfo>();

            while (type != null && type != typeof(MonoBehaviour))
            {
                var typeMethods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance |
                                                  BindingFlags.DeclaredOnly)
                    .Where(m =>
                        m.GetCustomAttributes(typeof(ButtonAttribute), false).Length == 1 &&
                        m.GetParameters().Length == 0 &&
                        !m.ContainsGenericParameters
                    );

                methods.AddRange(typeMethods);
                type = type.BaseType;
            }

            _methods = methods;
        }

        public void DrawButtons()
        {
            if (_methods.Count > 0)
                ShowMethodButtons();
        }

        private void ShowMethodButtons()
        {
            foreach (var method in _methods)
            {
                var buttonText = ObjectNames.NicifyVariableName(method.Name);

                if (!GUILayout.Button(buttonText)) continue;

                method.Invoke(_targetObject, _emptyParamList);
                EditorUtility.SetDirty(_targetObject);
            }
        }
    }

    [CustomEditor(typeof(MonoBehaviour), true)]
    [CanEditMultipleObjects]
    public class BehaviourButtonsEditor : UnityEditor.Editor
    {
        private BehaviourButtonsHelper _helper = new();

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            _helper.DrawButtons();
        }

        private void OnEnable()
        {
            _helper.Init(target);
        }
    }
}
#endif