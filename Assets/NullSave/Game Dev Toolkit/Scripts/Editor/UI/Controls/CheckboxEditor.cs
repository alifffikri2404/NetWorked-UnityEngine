using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Checkbox))]
    [CanEditMultipleObjects]
    public class CheckboxEditor : GDTKEditor
    {

        #region Fields

        private GUIContent m_VisualizeNavigation = new GUIContent("Visualize", "Show navigation flows between selectable UI elements.");

        private static List<CheckboxEditor> s_Editors = new List<CheckboxEditor>();
        private static bool s_ShowNavigation = false;
        private static string s_ShowNavigationKey = "SelectableEditor.ShowNavigation";
        private Checkbox myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is Checkbox checkbox)
            {
                myTarget = checkbox;
            }

            s_Editors.Add(this);
            RegisterStaticOnSceneGUI();

            s_ShowNavigation = EditorPrefs.GetBool(s_ShowNavigationKey);
        }

        protected virtual void OnDisable()
        {
            s_Editors.Remove(this);
            RegisterStaticOnSceneGUI();
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SimpleProperty("m_Interactable");
            SimpleProperty("m_Transition");
            EditorGUI.indentLevel++;
            switch (SimpleValue<int>("m_Transition"))
            {
                case 1: // Color Tint
                    SimpleProperty("m_TargetGraphic");
                    SimpleProperty("m_Colors");
                    break;
                case 2: // Sprite Swap
                    SimpleProperty("m_TargetGraphic");
                    SimpleProperty("m_SpriteState");
                    break;
                case 3: // Animation
                    SimpleProperty("m_TargetGraphic");
                    SimpleProperty("m_AnimationTriggers");
                    break;
            }
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



            SimpleProperty("m_IsOn");
            SimpleProperty("toggleTransition");
            SimpleProperty("graphic");
            SimpleProperty("useToggleGroup");
            if(SimpleValue<bool>("useToggleGroup"))
            {
                SimpleProperty("groupName");
            }

            SectionHeader("Text");
            SimpleProperty("target", "Label");
            string preText = serializedObject.FindProperty("m_Text").stringValue;
            SimpleProperty("m_Text");
            string text = SimpleValue<string>("m_Text");
            if (preText != text)
            {
                myTarget.text = text;
                EditorUtility.SetDirty(myTarget.target);
            }
            SimpleProperty("enabledTextColor");
            SimpleProperty("disabledTextColor");
            SimpleProperty("localize");

            SectionHeader("Events");
            SimpleProperty("onValueChanged");
            SimpleProperty("onTextChanged");
            SimpleProperty("onChecked");
            SimpleProperty("onUnchecked");

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