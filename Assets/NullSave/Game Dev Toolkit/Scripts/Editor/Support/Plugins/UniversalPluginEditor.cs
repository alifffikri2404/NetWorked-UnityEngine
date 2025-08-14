using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class UniversalPluginEditor
    {

        #region Fields

        public object referencedObject;
        public List<FieldInfo> fields;
        public List<PropertyInfo> properties;

        private GUIStyle sectionHeader, subHeader;

        static List<string> layers;
        static string[] layerNames;

        private Dictionary<FieldInfo, UniversalPluginEditor> subEditors;

        #endregion

        #region Properties

        public bool IsDirty { get; set; }

        public GUIStyle SectionHeaderStyle
        {
            get
            {
                if (sectionHeader == null || sectionHeader.normal.background == null)
                {
                    sectionHeader = new GUIStyle(GUI.skin.label);
                    sectionHeader.fontSize = 14;
                }

                return sectionHeader;
            }
        }

        public GUIStyle SubHeaderStyle
        {
            get
            {
                if (subHeader == null || subHeader.normal.background == null)
                {
                    subHeader = new GUIStyle(GUI.skin.label);
                    subHeader.fontSize = 12;
                    subHeader.fontStyle = FontStyle.Bold;
                }

                return subHeader;
            }
        }

        #endregion

        #region Public Methods
        public static UniversalPluginEditor CreateEditor(object target)
        {
            if (target == null) return null;

            UniversalPluginEditor editor = FindCustomEditorForType(target.GetType());
            if (editor == null)
            {
                editor = new UniversalPluginEditor();
            }

            editor.referencedObject = target;
            editor.fields = new List<FieldInfo>();
            editor.properties = new List<PropertyInfo>();

            // Reflect fields
            FieldInfo[] fields = target.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                if (field.IsPublic)
                {
                    editor.fields.Add(field);
                }
                else
                {
                    if (field.GetCustomAttribute(typeof(SerializedProperty)) != null)
                    {
                        Debug.Log("hidden field: " + field.Name);
                        editor.fields.Add(field);
                    }
                }
            }

            // Reflect properties
            PropertyInfo[] properties = target.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    editor.properties.Add(property);
                }
            }

            editor.OnEnable();
            return editor;
        }

        public virtual void OnEnable() { }

        public virtual void OnInspectorGUI()
        {
            if (fields.Count == 0)
            {
                GUILayout.Label("{ No Configuration Settings }");
                return;
            }

            foreach (FieldInfo field in fields)
            {
                if (!Attribute.IsDefined(field, typeof(HideInInspector)))
                {
                    PropertyField(field, field.FieldType, ObjectNames.NicifyVariableName(field.Name));
                }
            }
        }

        public void ApplyUpdates(ref string serializationData)
        {
            if (!IsDirty) return;
            serializationData = SimpleJson.ToJSON(referencedObject);
            IsDirty = false;
        }

        #endregion

        #region GUI Methods

        public object ObjectField(object value, Type type, string label)
        {
            object result = value;
            if (type == null) return value;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            switch (type.ToString())
            {
                case "System.string":
                case "System.String":
                    result = GUILayout.TextField((string)value);
                    break;
                case "System.Boolean":
                    result = EditorGUILayout.Toggle((bool)value);
                    break;
                case "System.Double":
                    result = EditorGUILayout.DoubleField((double)value);
                    break;
                case "System.Int64":
                    result = EditorGUILayout.LongField((long)value);
                    break;
                case "System.UInt64":
                    result = (UInt64)EditorGUILayout.DoubleField((double)(UInt64)value);
                    break;
                case "System.Int":
                case "System.Int32":
                    result = EditorGUILayout.IntField((int)value);
                    break;
                case "System.Single":
                case "System.Float":
                    result = EditorGUILayout.FloatField((float)value);
                    break;
                case "UnityEngine.Bounds":
                    result = EditorGUILayout.BoundsField((Bounds)value);
                    break;
                case "UnityEngine.BoundsInt":
                    result = EditorGUILayout.BoundsIntField((BoundsInt)value);
                    break;
                case "UnityEngine.Color":
                    result = EditorGUILayout.ColorField((Color)value);
                    break;
                case "UnityEngine.AnimationCurve":
                    result = EditorGUILayout.CurveField((AnimationCurve)value);
                    break;
                case "UnityEngine.Gradient":
                    result = EditorGUILayout.GradientField((Gradient)value);
                    break;
                case "UnityEngine.Rect":
                    result = EditorGUILayout.RectField((Rect)value);
                    break;
                case "UnityEngine.RectInt":
                    result = EditorGUILayout.RectIntField((RectInt)value);
                    break;
                case "UnityEngine.Vector2":
                    result = EditorGUILayout.Vector2Field(string.Empty, (Vector2)value);
                    break;
                case "UnityEngine.Vector2Int":
                    result = EditorGUILayout.Vector2IntField(string.Empty, (Vector2Int)value);
                    break;
                case "UnityEngine.Vector3":
                    result = EditorGUILayout.Vector3Field(string.Empty, (Vector3)value);
                    break;
                case "UnityEngine.Vector3Int":
                    result = EditorGUILayout.Vector3IntField(string.Empty, (Vector3Int)value);
                    break;
                case "UnityEngine.Vector4":
                    result = EditorGUILayout.Vector4Field(string.Empty, (Vector4)value);
                    break;
                case "UnityEngine.Quaternion":
                    Quaternion q = (Quaternion)value;
                    Vector4 v4 = new Vector4() { w = q.w, x = q.x, y = q.y, z = q.z };
                    v4 = EditorGUILayout.Vector4Field(string.Empty, v4);
                    q.w = v4.w;
                    q.x = v4.x;
                    q.y = v4.y;
                    q.z = v4.z;
                    result = q;
                    break;
                case "UnityEngine.LayerMask":
                    LoadLayerMaskData();
                    result = (LayerMask)EditorGUILayout.MaskField((LayerMask)value, layerNames);
                    break;
                default:
                    if (type.IsEnum)
                    {
                        result = EditorGUILayout.EnumPopup((Enum)value);
                    }
                    else
                    {
                        try
                        {
                            result = EditorGUILayout.ObjectField((UnityEngine.Object)value, type, true);
                        }
                        catch
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Unsupported", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
                            GUILayout.Label(type.Name);
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
            }
            GUILayout.EndHorizontal();

            return result;
        }

        public void PropertyEnumFlagsField(string fieldName)
        {
            FieldInfo field = FindField(fieldName);
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(ObjectNames.NicifyVariableName(fieldName));
            SetFieldValue(field, EditorGUILayout.EnumFlagsField((Enum)field.GetValue(referencedObject)));
            GUILayout.EndHorizontal();
        }

        public void PropertyField(string fieldName)
        {
            FieldInfo field = FindField(fieldName);
            PropertyField(field, field.FieldType, ObjectNames.NicifyVariableName(fieldName));
        }

        public void PropertyField(string fieldName, string label)
        {
            FieldInfo field = FindField(fieldName);
            PropertyField(field, field.FieldType, label);
        }

        public bool PropertyBoolValue(string fieldName)
        {
            return (bool)FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyBoolValue(string fieldName, bool value)
        {
            SetFieldValue(FindField(fieldName), value);
        }

        public float PropertyFloatValue(string fieldName)
        {
            return (float)FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyFloatValue(string fieldName, float value)
        {
            SetFieldValue(FindField(fieldName), value);
        }

        public int PropertyIntValue(string fieldName)
        {
            return (int)FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyIntValue(string fieldName, int value)
        {
            SetFieldValue(FindField(fieldName), value);
        }

        public object PropertyObjectValue(string fieldName)
        {
            return FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyObjectValue(string fieldName, object value, bool forceSave = false)
        {
            SetFieldValue(FindField(fieldName), value, forceSave);
        }

        public string PropertyStringValue(string fieldName)
        {
            return (string)FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyStringValue(string fieldName, string value)
        {
            SetFieldValue(FindField(fieldName), value);
        }

        public List<string> PropertyStringListValue(string fieldName)
        {
            return (List<string>)FindField(fieldName).GetValue(referencedObject);
        }

        public void PropertyStringListValue(string fieldName, List<string> value)
        {
            SetFieldValue(FindField(fieldName), value);
        }

        public void SimpleStringListProperty(string fieldName, string title)
        {
            bool setDirty = false;
            List<string> list = (List<string>)FindField(fieldName).GetValue(referencedObject);
            if (list == null)
            {
                list = new List<string>();
                setDirty = true;
            }

            EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.Label("(" + list.Count + ")");
            GUILayout.EndHorizontal();

            int removeAt = -1;
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                GUILayout.BeginVertical(GDTKEditor.Styles.SlimBox);
                string result = GUILayout.TextField(list[i]);
                if (result != list[i])
                {
                    list[i] = result;
                    setDirty = true;
                }
                GUILayout.EndVertical();
                if (GUILayout.Button(GDTKEditor.GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth + 4);
            if (GUILayout.Button(new GUIContent("Add New", GDTKEditor.GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                list.Add(string.Empty);
                IsDirty = true;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (removeAt > -1)
            {
                list.RemoveAt(removeAt);
                setDirty = true;
            }

            if (setDirty)
            {
                SetFieldValue(FindField(fieldName), list, true);
                IsDirty = true;
            }
        }

        public void SimpleStringSearchListProperty(string fieldName, string title, Action<List<string>> buildOptions)
        {
            bool setDirty = false;
            List<string> list = (List<string>)FindField(fieldName).GetValue(referencedObject);
            if (list == null)
            {
                list = new List<string>();
                setDirty = true;
            }

            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            GUILayout.Label(title, EditorStyles.label, GUILayout.Width(EditorGUIUtility.labelWidth));
            GUILayout.Label("(" + list.Count + ")");
            GUILayout.EndHorizontal();

            int removeAt = -1;
            for (int i = 0; i < list.Count; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(EditorGUIUtility.labelWidth + 4);
                GUILayout.BeginVertical(GDTKEditor.Styles.SlimBox);
                GUILayout.Label(list[i]);
                GUILayout.EndVertical();
                if (GUILayout.Button(GDTKEditor.GetIcon("icons/trash-small"), GUILayout.Width(32), GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                {
                    GUI.FocusControl(null);
                    removeAt = i;
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Space(EditorGUIUtility.labelWidth + 4);
            if (GUILayout.Button(new GUIContent("Add New", GDTKEditor.GetIcon("icons/add-small")), GUILayout.Height(24)))
            {
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    buildOptions = buildOptions,
                    onSelection = s =>
                    {
                        list.Add(s);
                        SetFieldValue(FindField(fieldName), list, true);
                        IsDirty = true;
                    }
                });

            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (removeAt > -1)
            {
                list.RemoveAt(removeAt);
                setDirty = true;
            }

            if (setDirty)
            {
                SetFieldValue(FindField(fieldName), list, true);
                IsDirty = true;
            }
        }

        public void SimpleStringSearchProperty(string fieldName, string title, Action<List<string>> buildOptions, bool showClear = true, bool endHorizontal = true)
        {
            FieldInfo field = FindField(fieldName);
            string curVal = (string)field.GetValue(referencedObject);
            Rect r = EditorGUILayout.GetControlRect(true, GUILayout.ExpandWidth(true));
            GUILayout.Space(-EditorGUIUtility.singleLineHeight);

            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(title);

            if (GUILayout.Button(curVal, EditorStyles.popup))
            {
                r.x += EditorGUIUtility.labelWidth + 2;
                r.width -= EditorGUIUtility.labelWidth + 2;
                GUI.FocusControl(null);
                PopupWindow.Show(r, new SearchabelStringPopupWindow()
                {
                    width = r.width,
                    buildOptions = buildOptions,
                    onSelection = s =>
                    {
                        //property.serializedObject.Update();
                        //property.stringValue = s;
                        //property.serializedObject.ApplyModifiedProperties();
                        PropertyStringValue(fieldName, s);
                    }
                });
            }

            if (showClear && !string.IsNullOrEmpty(curVal))
            {
                if (GUILayout.Button("Clear", GUILayout.Width(48)))
                {
                    GUI.FocusControl(null);
                    PropertyStringValue(fieldName, string.Empty);
                }
            }

            if (endHorizontal)
            {
                GUILayout.EndHorizontal();
            }
        }

        #endregion

        #region Private Methods

        private static UniversalPluginEditor FindCustomEditorForType(Type targetType)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i++)
            {
                Type[] types = assemblies[i].GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(UniversalPluginEditor)))
                    {
                        foreach (var attrib in type.CustomAttributes)
                        {
                            if (attrib.AttributeType == typeof(CustomUniversalPluginEditor))
                            {
                                foreach (var arg in attrib.ConstructorArguments)
                                {
                                    if (arg.Value.Equals(targetType))
                                    {
                                        return (UniversalPluginEditor)Activator.CreateInstance(type);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private FieldInfo FindField(string fieldName)
        {
            foreach (FieldInfo field in fields)
            {
                if (field.Name == fieldName)
                {
                    return field;
                }
            }

            throw new Exception("Cannot find field name " + fieldName);
        }

        private void GetUseCustomEditor(FieldInfo field)
        {
            if (subEditors == null) subEditors = new Dictionary<FieldInfo, UniversalPluginEditor>();

            object refObj = field.GetValue(referencedObject);
            if (!subEditors.ContainsKey(field))
            {
                UniversalPluginEditor subEditor = CreateEditor(refObj);
                subEditor.OnEnable();
                subEditors.Add(field, subEditor);
            }

            subEditors[field].OnInspectorGUI();
        }

        private void LoadLayerMaskData()
        {
            if (layers == null)
            {
                layers = new List<string>();
                layerNames = new string[4];
            }
            else
            {
                layers.Clear();
            }

            int emptyLayers = 0;
            for (int i = 0; i < 32; i++)
            {
                string layerName = LayerMask.LayerToName(i);

                if (layerName != "")
                {

                    for (; emptyLayers > 0; emptyLayers--) layers.Add("Layer " + (i - emptyLayers));
                    layers.Add(layerName);
                }
                else
                {
                    emptyLayers++;
                }
            }

            if (layerNames.Length != layers.Count)
            {
                layerNames = new string[layers.Count];
            }
            for (int i = 0; i < layerNames.Length; i++) layerNames[i] = layers[i];
        }

        private void SetFieldValue(FieldInfo field, object value, bool forceSave = false)
        {
            object fieldValue = field.GetValue(referencedObject);
            if (value == null && fieldValue == null) return;
            if (!forceSave && fieldValue != null && fieldValue.Equals(value)) return;
            field.SetValue(referencedObject, value);
            IsDirty = true;
        }

        private void PropertyField(FieldInfo field, Type type, string label)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(label);
            switch (type.ToString())
            {
                case "System.string":
                case "System.String":
                    SetFieldValue(field, GUILayout.TextField((string)field.GetValue(referencedObject)));
                    break;
                case "System.Boolean":
                    SetFieldValue(field, EditorGUILayout.Toggle((bool)field.GetValue(referencedObject)));
                    break;
                case "System.Double":
                    SetFieldValue(field, EditorGUILayout.DoubleField((double)field.GetValue(referencedObject)));
                    break;
                case "System.Int64":
                    SetFieldValue(field, EditorGUILayout.LongField((long)field.GetValue(referencedObject)));
                    break;
                case "System.UInt64":
                    SetFieldValue(field, EditorGUILayout.DoubleField((double)(UInt64)field.GetValue(referencedObject)));
                    break;
                case "System.Int":
                case "System.Int32":
                    SetFieldValue(field, EditorGUILayout.IntField((int)field.GetValue(referencedObject)));
                    break;
                case "System.Single":
                case "System.Float":
                    SetFieldValue(field, EditorGUILayout.FloatField((float)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Bounds":
                    SetFieldValue(field, EditorGUILayout.BoundsField((Bounds)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.BoundsInt":
                    SetFieldValue(field, EditorGUILayout.BoundsIntField((BoundsInt)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Color":
                    SetFieldValue(field, EditorGUILayout.ColorField((Color)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.AnimationCurve":
                    SetFieldValue(field, EditorGUILayout.CurveField((AnimationCurve)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Gradient":
                    SetFieldValue(field, EditorGUILayout.GradientField((Gradient)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Rect":
                    SetFieldValue(field, EditorGUILayout.RectField((Rect)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.RectInt":
                    SetFieldValue(field, EditorGUILayout.RectIntField((RectInt)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Vector2":
                    SetFieldValue(field, EditorGUILayout.Vector2Field(string.Empty, (Vector2)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Vector2Int":
                    SetFieldValue(field, EditorGUILayout.Vector2IntField(string.Empty, (Vector2Int)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Vector3":
                    SetFieldValue(field, EditorGUILayout.Vector3Field(string.Empty, (Vector3)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Vector3Int":
                    SetFieldValue(field, EditorGUILayout.Vector3IntField(string.Empty, (Vector3Int)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.Vector4":
                    SetFieldValue(field, EditorGUILayout.Vector4Field(string.Empty, (Vector4)field.GetValue(referencedObject)));
                    break;
                case "UnityEngine.LayerMask":
                    LoadLayerMaskData();
                    SetFieldValue(field, (LayerMask)EditorGUILayout.MaskField((LayerMask)field.GetValue(referencedObject), layerNames));
                    break;
                default:
                    if (type.IsEnum)
                    {
                        SetFieldValue(field, EditorGUILayout.EnumPopup((Enum)field.GetValue(referencedObject)));
                    }
                    else
                    {
                        try
                        {
                            SetFieldValue(field, EditorGUILayout.ObjectField((UnityEngine.Object)field.GetValue(referencedObject), field.FieldType, true));
                        }
                        catch
                        {
                            if (type.IsClass)
                            {
                                GUILayout.EndHorizontal();

                                GetUseCustomEditor(field);
                                return;
                            }
                            GUILayout.BeginHorizontal();
                            GUILayout.Label("Unsupported", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
                            GUILayout.Label(type.Name);
                            GUILayout.EndHorizontal();
                        }
                    }
                    break;
            }
            GUILayout.EndHorizontal();
        }

        #endregion

    }
}