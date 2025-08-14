using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NullSave.GDTK
{
    [DefaultExecutionOrder(-200)]
    public class ToolRegistryHelper : MonoBehaviour
    {

        #region Fields

        public List<Object> registerItems;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            foreach(Object obj in registerItems)
            {
                ToolRegistry.RegisterComponent(obj);
            }
        }

        #endregion

    }
}