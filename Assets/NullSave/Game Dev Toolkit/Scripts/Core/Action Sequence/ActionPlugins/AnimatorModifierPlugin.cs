using UnityEngine;

namespace NullSave.GDTK
{
    public class AnimatorModifierPlugin : ActionSequencePlugin
    {

        #region Fields

        public bool useRemoteTarget;
        public string keyName;
        public AnimatorParamType paramType;
        public bool boolVal;
        public int intVal;
        public float floatVal;
        public AnimatorTriggerType triggerVal;

        private Animator target;

        #endregion

        #region Properties

        public override Texture2D icon { get { return GetResourceImage("icons/animate"); } }

        public override string title { get { return "Animator Modifier"; } }

        public override string titlebarText
        {
            get
            {
                string prepend = useRemoteTarget ? "(Remote) " : string.Empty;

                return paramType switch
                {
                    AnimatorParamType.Bool => prepend + "Animator: Set " + keyName + " = " + boolVal,
                    AnimatorParamType.Float => prepend + "Animator: Set " + keyName + " = " + floatVal,
                    AnimatorParamType.Int => prepend + "Animator: Set " + keyName + " = " + intVal,
                    _ => prepend + "Animator: " + triggerVal.ToString() + " trigger " + keyName,
                };
            }
        }

        public override string description { get { return "Sets a value on the attached Animator (if any)."; } }

        #endregion

        #region Plugin Methods

        public override void StartAction(ActionSequence host)
        {
            isComplete = false;
            isStarted = true;

            if (useRemoteTarget)
            {
                target = host.remoteTarget.GetComponentInChildren<Animator>();
            }
            else
            {
                target = host.GetComponentInChildren<Animator>();
            }

            if (target != null)
            {
                switch (paramType)
                {
                    case AnimatorParamType.Bool:
                        target.SetBool(keyName, boolVal);
                        break;
                    case AnimatorParamType.Float:
                        target.SetFloat(keyName, floatVal);
                        break;
                    case AnimatorParamType.Int:
                        target.SetInteger(keyName, intVal);
                        break;
                    case AnimatorParamType.Trigger:
                        if (triggerVal == AnimatorTriggerType.Reset)
                        {
                            target.ResetTrigger(keyName);
                        }
                        else
                        {
                            target.SetTrigger(keyName);
                        }
                        break;
                }
            }

            isComplete = true;
        }

        #endregion

    }
}