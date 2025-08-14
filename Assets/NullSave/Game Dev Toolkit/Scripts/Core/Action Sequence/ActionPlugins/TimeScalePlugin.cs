using UnityEngine;

namespace NullSave.GDTK
{
    public class TimeScalePlugin : ActionSequencePlugin
    {

        #region Fields

        public float timeScale;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/wait"); } }

        public override string title { get { return "Time Scale"; } }

        public override string titlebarText
        {
            get
            {
                return "Set Time.timeScale to " + timeScale;
            }
        }

        public override string description { get { return "Sets the timescale to a provided value."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Time.timeScale = timeScale;
            isComplete = true;
        }

        #endregion

    }
}