using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class OpacitySetter : MonoBehaviour
    {

        #region Fields

        public List<Graphic> targets;

        #endregion

        #region Public Methods

        public void SetOpacity(float value)
        {
            foreach(Graphic g in targets)
            {
                g.color = new Color(g.color.r, g.color.g, g.color.b, value);
            }
        }

        #endregion

    }
}
