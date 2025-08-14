namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(StringFilterPlugin))]
    public class StringFilterPluginEditor : UniversalPluginEditor
    {

        public override void OnInspectorGUI()
        {
            PropertyField("enabled");
            PropertyField("filter");
            if (IsDirty)
            {
                ((SortAndFilterPlugin)referencedObject).requiresUpdate?.Invoke();
            }
        }

    }
}
