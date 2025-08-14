using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    public class SpawnPrefabPlugin : ActionSequencePlugin
    {

        #region Fields

        [Tooltip("Object to spawn")] public GameObject prefab;
        [Tooltip("Offset to use when spawning")] public Vector3 offset;
        [Tooltip("Use current rotation")] public bool useRotation;
        [Tooltip("Parent object to current transfrom")] public bool parent;

        public string path;
        public string bundleName;
        public string assetName;
        public bool z_spawnError;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/object"); } }

        public override string title { get { return "Spawn prefab"; } }

        public override string titlebarText
        {
            get
            {
                if (parent)
                {
                    return "Spawn and parent " + GetObjectName() + " at " + offset;
                }
                return "Spawn " + GetObjectName() + " at " + offset;
            }
        }

        public override string description { get { return "Spawns an object at designated location."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            GameObject go;
            if (string.IsNullOrEmpty(bundleName))
            {
                // Resource
                go = InterfaceManager.ObjectManagement.InstantiateObject(Resources.Load<GameObject>(path), host.transform.parent.transform);
            }
            else
            {
                // Bundle
                go = InterfaceManager.ObjectManagement.InstantiateObject(GetBundle().LoadAsset<GameObject>(assetName), host.transform.parent.transform);
            }

            go.transform.localPosition = offset;
            if(parent)
            {
                if(useRotation)
                {
                    go.transform.rotation = Quaternion.Euler(Vector3.zero);
                }
            }
            else
            {
                go.transform.SetParent(null);
                if(useRotation)
                {
                    go.transform.rotation = host.transform.parent.transform.rotation;
                }
            }

            isComplete = true;
        }

        #endregion

        #region Private Methods

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, assetName));
        }

        private string GetObjectName()
        {
            if (prefab == null) return "(null)";
            return prefab.name;
        }

        #endregion

    }
}