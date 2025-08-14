using UnityEditor;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(Progressbar))]
    [CanEditMultipleObjects]
    public class ProgressbarEditor : GDTKEditor
    {

        #region Fields

        private Progressbar myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is Progressbar progressbar)
            {
                myTarget = progressbar;
            }
        }

        public override void OnInspectorGUI()
        {
            float changeVal;
            Progressbar.Direction changeDir;
            Progressbar.FillMode changeFill;
            Image changeImg;

            MainContainerBegin();

            SectionHeader("Behavior");

            EditorGUI.BeginChangeCheck();
            changeImg = (Image) EditorGUILayout.ObjectField("Target Graphic", myTarget.targetGraphic, typeof(Image), true);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Target Graphic");
                myTarget.targetGraphic = changeImg;
            }

            EditorGUI.BeginChangeCheck();
            changeFill = (Progressbar.FillMode)EditorGUILayout.EnumPopup("Fill Mode", myTarget.fillMode);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Fill Mode");
                myTarget.fillMode = changeFill;
            }

            if(myTarget.fillMode == Progressbar.FillMode.ImageSize)
            {
                EditorGUI.BeginChangeCheck();
                changeDir = (Progressbar.Direction)EditorGUILayout.EnumPopup("Direction", myTarget.direction);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(target, "Changed Direction");
                    myTarget.direction = changeDir;
                }
            }

            EditorGUI.BeginChangeCheck();
            changeVal = EditorGUILayout.FloatField("Min Value", myTarget.minValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Min Value");
                myTarget.minValue = changeVal;
            }

            EditorGUI.BeginChangeCheck();
            changeVal = EditorGUILayout.FloatField("Max Value", myTarget.maxValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Max Value");
                myTarget.maxValue = changeVal;
            }

            EditorGUI.BeginChangeCheck();
            changeVal = EditorGUILayout.Slider("Value", myTarget.value, myTarget.minValue, myTarget.maxValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Changed Value");
                myTarget.value = changeVal;
            }

            SectionHeader("Events");
            SimpleProperty("onValueChanged");

            MainContainerEnd();
        }

        #endregion

    }
}