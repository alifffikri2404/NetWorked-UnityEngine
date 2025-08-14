using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace NullSave.GDTK
{
    [InitializeOnLoad]
    public class UniversalPluginManager<T>
    {

        #region Fields

        private static List<UniversalPluginInfo<T>> m_Plugins;

        #endregion

        #region Properties

        public static List<UniversalPluginInfo<T>> Plugins
        {
            get
            {
                return m_Plugins.ToList();
            }
        }

        #endregion

        #region Constructor

        static UniversalPluginManager()
        {
            m_Plugins = new List<UniversalPluginInfo<T>>();

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.IsSubclassOf(typeof(T))))
            {
                T Item = (T)Activator.CreateInstance(type);
                m_Plugins.Add(new UniversalPluginInfo<T>(Item, type));
            }
        }

        #endregion

    }
}