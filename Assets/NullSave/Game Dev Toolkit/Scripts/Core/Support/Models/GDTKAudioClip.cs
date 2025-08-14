using System;
using System.IO;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class GDTKAudioClip
    {

        #region Fields

        [Tooltip("Source of audio")] public ImageSource source;
        [Tooltip("Path of audio")] public string path;
        [Tooltip("Asset Bundle containing audio")] public string bundleName;
        [Tooltip("Name of audio in the bundle")] public string assetName;

        [SerializeField] private AudioClip m_audioClip;

#if UNITY_EDITOR
        [SerializeField] private bool z_resourceError;
#endif

        #endregion

        #region Properties

        public AudioClip audioClip
        {
            get
            {
                if (m_audioClip == null)
                {
                    switch (source)
                    {
                        case ImageSource.AssetBundle:
                            m_audioClip = GetBundle().LoadAsset<AudioClip>(assetName);
                            break;
                        case ImageSource.PersistentData:
                            throw new NotImplementedException();
                        case ImageSource.Resources:
                            m_audioClip = Resources.Load<AudioClip>(path);
                            break;
                    }
                }

                return m_audioClip;
            }
        }

        #endregion

        #region Public Methods

        public GDTKAudioClip Clone()
        {
            GDTKAudioClip result = new GDTKAudioClip();

            result.source = source;
            result.path = path;
            result.bundleName = bundleName;

            return result;
        }

        public void DataLoad(Stream stream)
        {
            source = (ImageSource)stream.ReadInt();
            switch (source)
            {
                case ImageSource.AssetBundle:
                    path = stream.ReadStringPacket();
                    bundleName = stream.ReadStringPacket();
                    assetName = stream.ReadStringPacket();
                    break;
                case ImageSource.PersistentData:
                    path = stream.ReadStringPacket();
                    break;
                case ImageSource.Resources:
                    path = stream.ReadStringPacket();
                    break;
            }
        }

        public void DataSave(Stream stream)
        {
            stream.WriteInt((int)source);
            switch (source)
            {
                case ImageSource.AssetBundle:
                    stream.WriteStringPacket(path);
                    stream.WriteStringPacket(bundleName);
                    stream.WriteStringPacket(assetName);
                    break;
                case ImageSource.PersistentData:
                    stream.WriteStringPacket(path);
                    break;
                case ImageSource.Resources:
                    stream.WriteStringPacket(path);
                    break;
            }
        }

        #endregion

        #region Private Methods

        private AssetBundle GetBundle()
        {
            foreach (AssetBundle bundle in AssetBundle.GetAllLoadedAssetBundles())
            {
                if (bundle.name == bundleName) return bundle;
            }

            return AssetBundle.LoadFromFile(Path.Combine(Application.persistentDataPath, path));
        }

        #endregion

    }
}