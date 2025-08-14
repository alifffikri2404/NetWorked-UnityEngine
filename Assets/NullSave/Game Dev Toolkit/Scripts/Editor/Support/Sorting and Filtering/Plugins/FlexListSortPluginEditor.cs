namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(FlexListSortPlugin))]
    public class FlexListSortPluginEditor : UniversalPluginEditor
    {

        public override void OnInspectorGUI()
        {
            PropertyField("enabled");
            PropertyField("sortMode");
            if (IsDirty)
            {
                ((SortAndFilterPlugin)referencedObject).requiresUpdate?.Invoke();
            }
        }

    }
}