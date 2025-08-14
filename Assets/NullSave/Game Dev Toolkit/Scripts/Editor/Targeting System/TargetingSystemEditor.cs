using UnityEditor;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(TargetingSystem))]
    [CanEditMultipleObjects]
    public class TargetingSystemEditor : GDTKEditor
    {

        #region Fields

        private TargetingSystem myTarget;

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            if(target is TargetingSystem system)
            {
                myTarget = system;
            }
        }

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            SectionHeader("Behavior");
            SimpleProperty("is2DMode");
            SimpleProperty("layerMask");
            SimpleProperty("lockRadius");

            SectionHeader("Lock-On");
            SimpleProperty("autoLockOn");
            if (!SimpleValue<bool>("autoLockOn"))
            {
                SimpleProperty("withButton", "Button Lock On");
                if (SimpleValue<bool>("withButton"))
                {
                    SimpleProperty("lockOnButton", "Button");
                }

                SimpleProperty("withKey", "Key Lock On");
                if (SimpleValue<bool>("withKey"))
                {
                    SimpleProperty("lockOnKey", "Key");
                }
            }

            SectionHeader("Remove Lock-On");
            SimpleProperty("removeWithButton", "Button Lock On");
            if (SimpleValue<bool>("removeWithButton"))
            {
                SimpleProperty("removeLockOnButton", "Button");
            }

            SimpleProperty("removeWithKey", "Key Lock On");
            if (SimpleValue<bool>("removeWithKey"))
            {
                SimpleProperty("removeLockOnKey", "Key");
            }

            SectionHeader("Line of Sight");
            SimpleProperty("requireLineOfSight", "Required");
            if (SimpleValue<bool>("requireLineOfSight"))
            {
                SimpleProperty("obstructionLayer");
                SimpleProperty("losOffset", "Offset");
            }

            SectionHeader("Indicators");
            bool org = SimpleValue<bool>("m_showIndicators");
            bool res = EditorGUILayout.Toggle("Show Indicators", org);
            if(res != org)
            {
                myTarget.showIndicators = res;
                EditorUtility.SetDirty(target);
            }
            SimpleProperty("availableTargetPrefab");
            SimpleProperty("lockedTargetPrefab");

            MainContainerEnd();
        }

        #endregion

    }
}