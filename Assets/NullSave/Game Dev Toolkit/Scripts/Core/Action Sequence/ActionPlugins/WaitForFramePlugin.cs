using UnityEngine;

namespace NullSave.GDTK
{
    public class WaitForFramePlugin : ActionSequencePlugin
    {

        #region Fields

        private int remainingCount;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/hourglass"); } }

        public override string title { get { return "Wait for End of Frame"; } }

        public override string description { get { return "Waits for a end of frame before continuing to the next action."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            remainingCount = 1;
        }

        public override void UpdateAction()
        {
            remainingCount -= 1;
            if (remainingCount < 0)
            {
                isComplete = true;
            }
        }

        #endregion

    }
}