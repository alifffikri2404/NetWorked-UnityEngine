using UnityEngine;

namespace NullSave.GDTK
{
    public class GravityPlugins : ActionSequencePlugin
    {

        #region Fields

        public Vector3 gravity;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/heavy"); } }

        public override string title { get { return "Gravity"; } }

        public override string titlebarText
        {
            get
            {
                return "Set Physics.gravity to " + gravity;
            }
        }

        public override string description { get { return "Adjusts the gravity to a provided value."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;
            Physics.gravity = gravity;
            isComplete = true;
        }

        #endregion

    }
}