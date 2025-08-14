using System;

namespace NullSave.GDTK
{
    public class UniversalPluginInfo<T>
    {

        #region Fields

        public T plugin;
        public Type type;

        #endregion

        #region Constructors

        public UniversalPluginInfo(T plugin, Type type)
        {
            this.plugin = plugin;
            this.type = type;
        }

        public UniversalPluginInfo(T plugin, string type)
        {
            this.plugin = plugin;
            this.type = Type.GetType(type);
        }

        #endregion

    }
}
