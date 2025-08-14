#if ENABLE_INPUT_SYSTEM
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace NullSave.GDTK
{
    [CreateAssetMenu(menuName = "Tools/GDTK/Controller Map", order = 0)]
    public class ControllerMap : ScriptableObject
    {

        #region Variables

        [Tooltip("Sets if this controller should be used if no compatible devices are found")] public bool isFallback;
        [Tooltip("Text Mesh Pro sprite asset to associate with this controller")] public TMP_SpriteAsset tmpSpriteAsset;
        [Tooltip("List of compatible devices")] public List<string> compatibleDevices;
        [Tooltip("List of associated inputs")] public List<InputMap> inputMaps;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets an input map associated with an action
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public InputMap GetAction(string actionName)
        {
            foreach (InputMap map in inputMaps)
            {
                if (map.inputName == actionName)
                {
                    map.TMPSpriteAsset = tmpSpriteAsset;
                    return map;
                }
            }

            return null;
        }

        /// <summary>
        /// Checks if there is an input map associated with an action name
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool HasAction(string actionName)
        {
            foreach(InputMap map in inputMaps)
            {
                if(map.inputName == actionName)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
#endif