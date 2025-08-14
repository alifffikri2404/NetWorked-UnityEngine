using System;
using System.Collections.Generic;

namespace NullSave.GDTK.JSON
{
    [Serializable]
    public class jsonActionSequence
    {

        #region Fields

        public string id;
        public List<jsonUniversalPlugin> actions;

        #endregion

        #region Constructor

        public jsonActionSequence()
        {
            actions = new List<jsonUniversalPlugin>();
        }

        #endregion

        #region Public Methods

        public static jsonActionSequence FromModel(ActionSequence source)
        {
            jsonActionSequence result = new jsonActionSequence();

            foreach (ActionSequenceWrapper action in source.plugins)
            {
                //result.actions.Add(jsonUniversalPlugin.FromModel(action));
                jsonUniversalPlugin jup = new jsonUniversalPlugin()
                {
                    pluginData = action.serializationData,
                    pluginNamespace = action.serializationNamespace,
                    pluginType = action.serializationType
                };
                result.actions.Add(jup);
            }

            return result;
        }

        public void ToModel(ActionSequence model, bool clearExisting)
        {
            if (clearExisting)
            {
                model.plugins.Clear();
            }

            foreach (jsonUniversalPlugin action in actions)
            {
                Type type = Type.GetType(action.pluginType);
                ActionSequencePlugin plugin = (ActionSequencePlugin)SimpleJson.FromJSON(action.pluginData, type);
                ActionSequenceWrapper asw = new ActionSequenceWrapper(plugin, action.pluginData);
                model.plugins.Add(asw);
            }
        }

        public List<UniversalPluginWrapper<ActionSequencePlugin>> ToSequenceList()
        {
            List<UniversalPluginWrapper<ActionSequencePlugin>> result = new List<UniversalPluginWrapper<ActionSequencePlugin>>();

            foreach (jsonUniversalPlugin action in actions)
            {
                Type type = Type.GetType(action.pluginType);
                ActionSequencePlugin plugin = (ActionSequencePlugin)SimpleJson.FromJSON(action.pluginData, type);
                UniversalPluginWrapper<ActionSequencePlugin> asw = new UniversalPluginWrapper<ActionSequencePlugin>(plugin, action.pluginData);
                result.Add(asw);
            }

            return result;
        }

        #endregion

    }
}