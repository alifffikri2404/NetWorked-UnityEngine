using System;
using UnityEngine;

namespace NullSave.GDTK
{
    public class WaitPlugin : ActionSequencePlugin
    {

        #region Fields

        public float secondsToWait;
        public bool realTime;

        private long startTime;
        private long waitUntil;
        private float waited;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/hourglass"); } }

        public override string title { get { return "Wait"; } }

        public override string titlebarText
        {
            get
            {
                if (realTime)
                {
                    return "Wait for " + secondsToWait + "s (realtime)";
                }

                return "Wait for " + secondsToWait + "s";
            }
        }

        public override string description { get { return "Waits for a set amount of time before continuing to the next action."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            if (realTime)
            {
                startTime = DateTime.Now.Ticks;
                waitUntil = DateTime.Now.AddSeconds(secondsToWait).Ticks;
            }
            else
            {
                waited = 0;
            }

            isComplete = false;
            isStarted = true;
        }

        public override void UpdateAction()
        {
            if (realTime)
            {
                long ticks = DateTime.Now.Ticks;

                progress = (ticks - startTime) / (waitUntil - startTime);

                if (ticks >= waitUntil)
                {
                    isComplete = true;
                }
            }
            else
            {
                waited += Time.deltaTime;
                progress = waited / secondsToWait;

                if (waited >= secondsToWait)
                {
                    isComplete = true;
                }
            }
        }

        #endregion

    }
}