using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class WeightedUnityItem
    {

        #region Fields

        public int Count;
        public UnityEngine.Object Object;
        public bool UseRandomCount;
        public int MinCount;
        public int MaxCount;
        public bool randomRotation;
        public Vector3 minRotation;
        public Vector3 maxRotation;

        #endregion

    }
}