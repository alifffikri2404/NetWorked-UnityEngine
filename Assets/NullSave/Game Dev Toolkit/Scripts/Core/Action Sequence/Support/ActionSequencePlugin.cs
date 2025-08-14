using System;

namespace NullSave.GDTK
{
    [Serializable]
    public class ActionSequencePlugin : UniversalPlugin
    {

        #region Properties

        public virtual bool isComplete { get; set; }

        public virtual bool isStarted { get; set; }

        public virtual float progress { get; set; }

        #endregion

        #region Public Methods

        public virtual void StartAction(ActionSequence host) { }

        public virtual void FixedUpdateAction() { }

        public virtual void UpdateAction() { }

        #endregion

    }
}