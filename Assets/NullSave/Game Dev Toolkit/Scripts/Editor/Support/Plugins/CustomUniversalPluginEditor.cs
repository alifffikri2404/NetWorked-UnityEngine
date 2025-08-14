using System;
using UnityEngine;

namespace NullSave.GDTK
{
    public class CustomUniversalPluginEditor : Attribute
    {

        #region Fields

        internal Type m_InspectedType;

        #endregion

        #region Constructor

        public CustomUniversalPluginEditor(Type inspectedType)
        {
            if (inspectedType == null)
            {
                Debug.LogError("Failed to load CustomActionPluginEditor inspected type");
            }
            m_InspectedType = inspectedType;
        }

        #endregion

    }
}