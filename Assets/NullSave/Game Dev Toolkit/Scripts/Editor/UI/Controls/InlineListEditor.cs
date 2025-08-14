using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(InlineList))]
    public class InlineListEditor : GDTKEditor
    {

        #region Fields

        private GUIContent m_VisualizeNavigation = new GUIContent("Visualize", "Show navigation flows between selectable UI elements.");

        private static List<InlineListEditor> s_Editors = new List<InlineListEditor>();
        private static bool s_ShowNavigation = false;
        private static string s_ShowNavigationKey = "SelectableEditor.ShowNavigation";
        private InlineList myTarget;

        ReorderableList optionsList;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if (target is InlineList list)
            {
                myTarget = list;
            }

            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();

            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);

            optionsList = new ReorderableList(serializedObject, serializedObject.FindProperty("options"), true, true, true, true);
            optionsList.elementHeight = EditorGUIUtility.singleLineHeight + 2;
            optionsList.drawHeaderCallback = (Rect rect) => { EditorGUI.LabelField(rect, "Options"); };
            optionsList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), optionsList.serializedProperty.GetArrayElementAtIndex(index), new GUIContent(string.Empty, null, string.Empty));
                rect.y += EditorGUIUtility.singleLineHeight + 2;
            };

        }

        protected virtual void OnDisable()
        {
            s_Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("leftArrow");
            SimpleProperty("optionText");
            SimpleProperty("rightArrow");

            SimpleProperty("m_Interactable");
            EditorGUI.indentLevel++;
            SimpleProperty("m_Colors");
            EditorGUI.indentLevel--;

            GUILayout.Space(8);
            SimpleProperty("m_Navigation");
            EditorGUI.BeginChangeCheck();
            Rect toggleRect = EditorGUILayout.GetControlRect();
            toggleRect.xMin += EditorGUIUtility.labelWidth;
            s_ShowNavigation = GUI.Toggle(toggleRect, s_ShowNavigation, m_VisualizeNavigation, EditorStyles.miniButton);
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(s_ShowNavigationKey, s_ShowNavigation);
                SceneView.RepaintAll();
            }
            GUILayout.Space(8);

            SimpleProperty("inputType");
            switch((NavigationTypeSimple)SimpleValue<int>("inputType"))
            {
                case NavigationTypeSimple.ByButton:
                    SimpleProperty("inputButton");
                    break;
                case NavigationTypeSimple.ByKey:
                    SimpleProperty("previousKey");
                    SimpleProperty("nextKey");
                    break;
            }
            SimpleProperty("allowLooping");

            GUILayout.Space(8);
            optionsList.DoLayoutList();

            MainContainerEnd();
        }

        #endregion

        #region Private Methods

        private void RegisterStaticOnSceneGUI()
        {
            SceneView.duringSceneGui -= StaticOnSceneGUI;
            if (s_Editors.Count > 0)
            {
                SceneView.duringSceneGui += StaticOnSceneGUI;
            }
        }

        private static void StaticOnSceneGUI(SceneView view)
        {
            if (!s_ShowNavigation)
                return;

            for (int i = 0; i < Selectable.allSelectablesArray.Length; i++)
            {
                DrawNavigationForSelectable(Selectable.allSelectablesArray[i]);
            }
        }

        private static void DrawNavigationForSelectable(Selectable sel)
        {
            if (sel == null)
                return;

            Transform transform = sel.transform;
            bool active = Selection.transforms.Any(e => e == transform);
            Handles.color = new Color(1.0f, 0.9f, 0.1f, active ? 1.0f : 0.4f);
            DrawNavigationArrow(-Vector2.right, sel, sel.FindSelectableOnLeft());
            DrawNavigationArrow(Vector2.right, sel, sel.FindSelectableOnRight());
            DrawNavigationArrow(Vector2.up, sel, sel.FindSelectableOnUp());
            DrawNavigationArrow(-Vector2.up, sel, sel.FindSelectableOnDown());
        }

        const float kArrowThickness = 2.5f;
        const float kArrowHeadSize = 1.2f;

        private static void DrawNavigationArrow(Vector2 direction, Selectable fromObj, Selectable toObj)
        {
            if (fromObj == null || toObj == null)
                return;
            Transform fromTransform = fromObj.transform;
            Transform toTransform = toObj.transform;

            Vector2 sideDir = new Vector2(direction.y, -direction.x);
            Vector3 fromPoint = fromTransform.TransformPoint(GetPointOnRectEdge(fromTransform as RectTransform, direction));
            Vector3 toPoint = toTransform.TransformPoint(GetPointOnRectEdge(toTransform as RectTransform, -direction));
            float fromSize = HandleUtility.GetHandleSize(fromPoint) * 0.05f;
            float toSize = HandleUtility.GetHandleSize(toPoint) * 0.05f;
            fromPoint += fromTransform.TransformDirection(sideDir) * fromSize;
            toPoint += toTransform.TransformDirection(sideDir) * toSize;
            float length = Vector3.Distance(fromPoint, toPoint);
            Vector3 fromTangent = fromTransform.rotation * direction * length * 0.3f;
            Vector3 toTangent = toTransform.rotation * -direction * length * 0.3f;

            Handles.DrawBezier(fromPoint, toPoint, fromPoint + fromTangent, toPoint + toTangent, Handles.color, null, kArrowThickness);
            Handles.DrawAAPolyLine(kArrowThickness, toPoint, toPoint + toTransform.rotation * (-direction - sideDir) * toSize * kArrowHeadSize);
            Handles.DrawAAPolyLine(kArrowThickness, toPoint, toPoint + toTransform.rotation * (-direction + sideDir) * toSize * kArrowHeadSize);
        }

        private static Vector3 GetPointOnRectEdge(RectTransform rect, Vector2 dir)
        {
            if (rect == null)
                return Vector3.zero;
            if (dir != Vector2.zero)
                dir /= Mathf.Max(Mathf.Abs(dir.x), Mathf.Abs(dir.y));
            dir = rect.rect.center + Vector2.Scale(rect.rect.size, dir * 0.5f);
            return dir;
        }

        #endregion

    }
}