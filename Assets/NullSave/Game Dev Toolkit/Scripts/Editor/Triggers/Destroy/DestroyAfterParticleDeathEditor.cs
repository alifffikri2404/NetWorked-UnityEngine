using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    [CustomEditor(typeof(DestroyAfterParticleDeath))]
    public class DestroyAfterParticleDeathEditor : GDTKEditor
    {

        #region Unity Methods

        public override void OnInspectorGUI()
        {
            MainContainerBegin();

            GUILayout.BeginVertical("box");
            GUILayout.Label("This component will destroy the Game Object as soon as the particle system is no longer alive.", Styles.WrappedTextStyle);
            GUILayout.EndVertical();

            MainContainerEnd();
        }

        #endregion

    }
}