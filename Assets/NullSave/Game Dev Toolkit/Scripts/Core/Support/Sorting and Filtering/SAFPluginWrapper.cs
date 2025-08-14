using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class SAFPluginWrapper : ISerializationCallbackReceiver
    {

        #region Fields

        [NonSerialized] public SortAndFilterPlugin plugin;
        public string serializationType;
        public string serializationData;
        public string serializationNamespace;

        private string id;

#if UNITY_EDITOR
        [SerializeField] private bool z_expanded;
#endif

        #endregion

        #region Properties

        public string instanceId
        {
            get
            {
                if (string.IsNullOrEmpty(id))
                {
                    id = Guid.NewGuid().ToString();
                }
                return id;
            }
        }

        #endregion

        #region Constructors

        public SAFPluginWrapper() { }

        public SAFPluginWrapper(SortAndFilterPlugin plugin)
        {
            this.plugin = plugin;
            Type t = plugin.GetType();
            serializationData = SimpleJson.ToJSON(plugin);
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        public SAFPluginWrapper(SortAndFilterPlugin plugin, string data)
        {
            this.plugin = plugin;
            serializationData = data;
            Type t = plugin.GetType();
            serializationType = t.ToString();
            serializationNamespace = t.Assembly.GetName().Name;
        }

        #endregion

        #region Public Methods

        public SAFPluginWrapper Clone()
        {
            SAFPluginWrapper result = new SAFPluginWrapper();

            if (string.IsNullOrEmpty(serializationNamespace))
            {
                result.plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                result.plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }

            result.serializationData = serializationData;
            result.serializationNamespace = serializationNamespace;
            result.serializationType = serializationType;

            return result;
        }

        public void DataLoad(Stream stream)
        {
            serializationType = stream.ReadStringPacket();
            serializationData = stream.ReadStringPacket();
            serializationNamespace = stream.ReadStringPacket();
        }

        public void DataSave(Stream stream)
        {
            stream.WriteStringPacket(serializationType);
            stream.WriteStringPacket(serializationData);
            stream.WriteStringPacket(serializationNamespace);
        }

        #endregion

        #region Serialization

        public void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(serializationNamespace))
            {
                plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType));
            }
            else
            {
                plugin = (SortAndFilterPlugin)SimpleJson.FromJSON(serializationData, Type.GetType(serializationType + "," + serializationNamespace));
            }
        }

        public void OnBeforeSerialize() { }

        #endregion

    }
}