using UnityEngine;

namespace NullSave.GDTK
{
    [CustomUniversalPluginEditor(typeof(ImageFilterPlugin))]
    public class ImageFilterPluginEditor : UniversalPluginEditor
    {

        public override void OnInspectorGUI()
        {
            PropertyField("enabled");
            PropertyField("requireImage");
            if(Application.isPlaying && IsDirty)
            {
                ((SortAndFilterPlugin)referencedObject).requiresUpdate?.Invoke();
            }
        }

    }
}