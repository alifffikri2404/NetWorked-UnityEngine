using UnityEditor;
using UnityEngine;

namespace NullSave.GDTK
{
    public class MenuItems 
    {

        [MenuItem("GameObject/GDTK/Interface Manager", false)]
        private static void AddInterfaceManager()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<InterfaceManager>() == null)
            {
                Selection.activeGameObject.AddComponent<InterfaceManager>();
                return;
            }

            GameObject go = new GameObject("Interface Manager");
            go.AddComponent<InterfaceManager>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/GDTK/Interactor", false)]
        private static void AddInteractor()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Interactor>() == null)
            {
                Selection.activeGameObject.AddComponent<Interactor>();
                return;
            }

            GameObject go = new GameObject("Interactor");
            go.AddComponent<Interactor>();
            Selection.activeGameObject = go;
        }

        [MenuItem("GameObject/GDTK/Interactor 2D", false)]
        private static void AddInteractor2D()
        {
            if (Selection.activeGameObject != null && Selection.activeGameObject.GetComponent<Interactor>() == null)
            {
                Selection.activeGameObject.AddComponent<Interactor>();
                return;
            }

            GameObject go = new GameObject("Interactor 2D");
            go.AddComponent<Interactor>();
            Selection.activeGameObject = go;
        }


    }
}