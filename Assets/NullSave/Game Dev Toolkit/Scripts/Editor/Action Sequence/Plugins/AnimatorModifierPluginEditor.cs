namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(AnimatorModifierPlugin))]
    public class AnimatorModifierPluginEditor : UniversalPluginEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            PropertyField("useRemoteTarget");
            PropertyField("keyName");
            PropertyField("paramType");
            switch ((AnimatorParamType)PropertyIntValue("paramType"))
            {
                case AnimatorParamType.Bool:
                    PropertyField("boolVal", "Value");
                    break;
                case AnimatorParamType.Float:
                    PropertyField("floatVal", "Value");
                    break;
                case AnimatorParamType.Int:
                    PropertyField("intVal", "Value");
                    break;
                case AnimatorParamType.Trigger:
                    PropertyField("triggerVal", "Value");
                    break;
            }
        }

        #endregion

    }
}