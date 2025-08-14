using UnityEngine;

namespace NullSave.GDTK
{
    public static class GameObjectExtensions 
    {

        #region Public Methods

        public static bool IsChildOf(this GameObject gameObject, GameObject parent)
        {
            Transform t = gameObject.transform;
            Transform target = parent.transform;

            while(true)
            {
                if (t.parent == null) return false;
                if (t.parent == target) return true;
                t = t.parent;
            }
        }

        public static void SetLayer(this GameObject gameObject, int layer, bool recursive)
        {
            if(recursive)
            {
                foreach(Transform t in gameObject.GetComponentsInChildren<Transform>())
                {
                    t.gameObject.layer = layer;
                }
            }
            else
            {
                gameObject.layer = layer;
            }
        }

        #endregion

    }
}