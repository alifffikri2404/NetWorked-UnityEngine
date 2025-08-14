using System;
using UnityEngine;

namespace NullSave.GDTK
{
    [Serializable]
    public class AnimatorModifier 
    {

        #region Fields

        public string keyName;
        public AnimatorParamType paramType;
        public bool boolVal;
        public int intVal;
        public float floatVal;
        public AnimatorTriggerType triggerVal;

        #endregion

        #region Editor Only Properties

#if UNITY_EDITOR

        public bool Expanded { get; set; }

#endif

        #endregion

        #region Public Methods

        public void ApplyMod(Animator animator)
        {
            if (animator == null) return;

            switch (paramType)
            {
                case AnimatorParamType.Bool:
                    animator.SetBool(keyName, boolVal);
                    break;
                case AnimatorParamType.Float:
                    animator.SetFloat(keyName, floatVal);
                    break;
                case AnimatorParamType.Int:
                    animator.SetInteger(keyName, intVal);
                    break;
                case AnimatorParamType.Trigger:
                    if (triggerVal == AnimatorTriggerType.Reset)
                    {
                        animator.ResetTrigger(keyName);
                    }
                    else
                    {
                        animator.SetTrigger(keyName);
                    }
                    break;
            }
        }

        public bool CheckMod(Animator animator)
        {
            if (animator == null) return false;

            return paramType switch
            {
                AnimatorParamType.Bool => animator.GetBool(keyName) == boolVal,
                AnimatorParamType.Float => animator.GetFloat(keyName) == floatVal,
                AnimatorParamType.Int => animator.GetInteger(keyName) == intVal,
                _ => false,
            };
        }

        public AnimatorModifier Clone()
        {
            AnimatorModifier result = new AnimatorModifier();

            result.keyName = keyName;
            result.paramType = paramType;
            result.boolVal = boolVal;
            result.intVal = intVal;
            result.floatVal = floatVal;
            result.triggerVal = triggerVal;

            return result;
        }

        public string ToDescriptive()
        {
            return paramType switch
            {
                AnimatorParamType.Bool => "Set " + keyName + " = " + boolVal,
                AnimatorParamType.Float => "Set " + keyName + " = " + floatVal,
                AnimatorParamType.Int => "Set " + keyName + " = " + intVal,
                _ => triggerVal.ToString() + " trigger " + keyName,
            };
        }

        #endregion

    }
}