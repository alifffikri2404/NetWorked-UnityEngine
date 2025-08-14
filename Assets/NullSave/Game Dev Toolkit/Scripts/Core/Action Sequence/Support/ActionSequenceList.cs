using NullSave.GDTK.JSON;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NullSave.GDTK
{
    [Serializable]
    public class ActionSequenceList
    {

        #region Fields

        public string id;
        public List<ActionSequenceWrapper> actions;

        #endregion

        #region Constructor

        public ActionSequenceList()
        {
            actions = new List<ActionSequenceWrapper>();
        }

        #endregion

        #region Public Methods

        public void ApplyTo(ActionSequence target)
        {
            target.plugins = actions.ToList();
        }

        public ActionSequenceList Clone()
        {
            ActionSequenceList result = new ActionSequenceList();

            result.id = id;
            foreach (ActionSequenceWrapper asw in actions)
            {
                result.actions.Add(asw.Clone());
            }

            return result;
        }

        public static ActionSequenceList FromJSON(jsonActionSequenceList source)
        {
            ActionSequenceList result = new ActionSequenceList();

            result.id = source.id;
            foreach (jsonUniversalPlugin action in source.actions)
            {
                Type type = Type.GetType(action.pluginType);
                if (type == null)
                {
                    try
                    {
                        // Full fallback
                        type = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(type => type.FullName == action.pluginType).First();
                    }
                    catch
                    {
                        // Unknown plugin, possibly deleted
                        StringExtensions.LogError("ActionSequenceList", "FromJSON", "Unknown Plugin: " + action.pluginType);
                        return null;
                    }
                }
                ActionSequencePlugin plugin = (ActionSequencePlugin)SimpleJson.FromJSON(action.pluginData, type);
                ActionSequenceWrapper asw = new ActionSequenceWrapper(plugin, action.pluginData);
                result.actions.Add(asw);
            }

            return result;
        }

        public jsonActionSequenceList ToJSON()
        {
            jsonActionSequenceList result = new jsonActionSequenceList();

            result.id = id;
            foreach (ActionSequenceWrapper action in actions)
            {
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

        #endregion

    }
}