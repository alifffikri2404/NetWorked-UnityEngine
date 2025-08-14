using UnityEngine;

namespace NullSave.GDTK
{
    public class ObjectManager : ScriptableObject
    {

        #region Public Methods

        /// <summary>
        /// Clones the object original and returns the clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <returns></returns>
        public virtual T InstantiateObject<T>(T original) where T : Object { throw new System.NotImplementedException(); }

        /// <summary>
        /// Clones the object original and returns the clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual T InstantiateObject<T>(T original, Transform parent) where T : Object { throw new System.NotImplementedException(); }

        /// <summary>
        /// Clones the object original and returns the clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <returns></returns>
        public virtual T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation) where T : Object { throw new System.NotImplementedException(); }

        /// <summary>
        /// Clones the object original and returns the clone
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public virtual T InstantiateObject<T>(T original, Vector3 position, Quaternion rotation, Transform parent) where T : Object { throw new System.NotImplementedException(); }

        /// <summary>
        /// Removes a GameObject, component or asset
        /// </summary>
        /// <param name="target"></param>
        public new virtual void DestroyObject(Object target) { }

        #endregion

    }
}