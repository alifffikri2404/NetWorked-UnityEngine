using System;
using UnityEngine;

namespace NullSave.GDTK
{
    public class CustomActionPluginEditor : Attribute
    {

        #region Fields

        internal Type m_InspectedType;

        #endregion

        #region Constructor

        public CustomActionPluginEditor(Type inspectedType)
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