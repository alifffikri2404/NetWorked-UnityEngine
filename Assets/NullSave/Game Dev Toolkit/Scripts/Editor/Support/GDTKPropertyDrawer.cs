using System;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class GDTKPropertyDrawer : PropertyDrawer
    {
        #region Serialized Property Methods

        public T SimpleValue<T>(SerializedProperty property, string relativeName)
        {
            return SimpleValue<T>(property.FindPropertyRelative(relativeName));
        }

        private T SimpleValue<T>(SerializedProperty prop)
        {
            object result = null;
            Type type = typeof(T);

            if (type == typeof(bool))
            {
                result = prop.boolValue;
            }
            else if (type == typeof(BoundsInt))
            {
                result = prop.boundsIntValue;
            }
            else if (type == typeof(Bounds))
            {
                result = prop.boundsValue;
            }
            else if (type == typeof(Color))
            {
                result = prop.colorValue;
            }
            else if (type == typeof(double))
            {
                result = prop.doubleValue;
            }
            else if (type == typeof(float))
            {
                result = prop.floatValue;
            }
            else if (type == typeof(int))
            {
                result = prop.intValue;
            }
            else if (type == typeof(long))
            {
                result = prop.longValue;
            }
            else if (type == typeof(Quaternion))
            {
                result = prop.quaternionValue;
            }
            else if (type == typeof(RectInt))
            {
                result = prop.rectIntValue;
            }
            else if (type == typeof(Rect))
            {
                result = prop.rectValue;
            }
            else if (type == typeof(string))
            {
                result = prop.stringValue;
            }
            else if (type == typeof(Vector2Int))
            {
                result = prop.vector2IntValue;
            }
            else if (type == typeof(Vector2))
            {
                result = prop.vector2Value;
            }
            else if (type == typeof(Vector3Int))
            {
                result = prop.vector3IntValue;
            }
            else if (type == typeof(Vector3))
            {
                result = prop.vector3Value;
            }
            else if (type == typeof(Vector4))
            {
                result = prop.vector4Value;
            }

            return (T)result;
        }

        public void SimpleValue<T>(SerializedProperty prop, T value)
        {
            Type type = typeof(T);
            object setValue = value;


            if (type == typeof(bool))
            {
                prop.boolValue = (bool)setValue;
            }
            else if (type == typeof(BoundsInt))
            {
                prop.boundsIntValue = (BoundsInt)setValue;
            }
            else if (type == typeof(Bounds))
            {
                prop.boundsValue = (Bounds)setValue;
            }
            else if (type == typeof(Color))
            {
                prop.colorValue = (Color)setValue;
            }
            else if (type == typeof(double))
            {
                prop.doubleValue = (double)setValue;
            }
            else if (type == typeof(float))
            {
                prop.floatValue = (float)setValue;
            }
            else if (type == typeof(int))
            {
                prop.intValue = (int)setValue;
            }
            else if (type == typeof(long))
            {
                prop.longValue = (long)setValue;
            }
            else if (type == typeof(Quaternion))
            {
                prop.quaternionValue = (Quaternion)setValue;
            }
            else if (type == typeof(RectInt))
            {
                prop.rectIntValue = (RectInt)setValue;
            }
            else if (type == typeof(Rect))
            {
                prop.rectValue = (Rect)setValue;
            }
            else if (type == typeof(string))
            {
                prop.stringValue = (string)setValue;
            }
            else if (type == typeof(Vector2Int))
            {
                prop.vector2IntValue = (Vector2Int)setValue;
            }
            else if (type == typeof(Vector2))
            {
                prop.vector2Value = (Vector2)setValue;
            }
            else if (type == typeof(Vector3Int))
            {
                prop.vector3IntValue = (Vector3Int)setValue;
            }
            else if (type == typeof(Vector3))
            {
                prop.vector3Value = (Vector3)setValue;
            }
            else if (type == typeof(Vector4))
            {
                prop.vector4Value = (Vector4)setValue;
            }
        }

        public void SimpleValue<T>(SerializedProperty prop, string relativeName, T value)
        {
            SimpleValue<T>(prop.FindPropertyRelative(relativeName), value);
        }

        #endregion

        #region Private Methods

        public void SimpleProperty(Rect rect, SerializedProperty source, string propertyName)
        {
            SerializedProperty field = source.FindPropertyRelative(propertyName);
            EditorGUI.PropertyField(rect, source.FindPropertyRelative(propertyName), new GUIContent(field.displayName, field.tooltip));
        }

        public void SimpleProperty(Rect rect, SerializedProperty source, string propertyName, string title, string tooltip = "")
        {
            EditorGUI.PropertyField(rect, source.FindPropertyRelative(propertyName), new GUIContent(title, tooltip));
        }

        public bool SimpleBool(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).boolValue;
        }

        public void SimpleBool(SerializedProperty property, string relativeName, bool value)
        {
            property.FindPropertyRelative(relativeName).boolValue = value;
        }

        public void SimpleEnumFlagsContext(Rect position, SerializedProperty property, Type enumType)
        {
            SimpleEnumFlagsContext(position, property, ObjectNames.NicifyVariableName(property.name), enumType);
        }

        public void SimpleEnumFlagsContext(Rect position, SerializedProperty property, string title, Type enumType)
        {
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent(title));

            Enum ev = (Enum)Enum.ToObject(enumType, property.intValue);
            string enumTitle;
            if (property.intValue == 0)
            {
                enumTitle = "(None)";
            }
            else
            {
                enumTitle = ev.ToString();
            }

            if(GUI.Button(position, new GUIContent(enumTitle), EditorStyles.popup))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(position, new EnumFlagsPopupWindow()
                {
                    width = position.width,
                    value = ev,
                    onSelection = i =>
                    {
                        property.serializedObject.Update();
                        property.intValue = i;
                        property.serializedObject.ApplyModifiedProperties();
                    }
                });
            }

        }

        public static float SimpleFloat(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).floatValue;
        }

        public static void SimpleFloat(SerializedProperty property, string relativeName, float value)
        {
            property.FindPropertyRelative(relativeName).floatValue = value;
        }

        public static int SimpleInt(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).intValue;
        }

        public static void SimpleInt(SerializedProperty property, string relativeName, int value)
        {
            property.FindPropertyRelative(relativeName).intValue = value;
        }

        public string SimpleString(SerializedProperty property, string relativeName)
        {
            return property.FindPropertyRelative(relativeName).stringValue;
        }

        public void SimpleString(SerializedProperty property, string relativeName, string value)
        {
            property.FindPropertyRelative(relativeName).stringValue = value;
        }


        #endregion
    }
}
