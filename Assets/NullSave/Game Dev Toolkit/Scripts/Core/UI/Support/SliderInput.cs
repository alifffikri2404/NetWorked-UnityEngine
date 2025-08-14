using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [RequireComponent(typeof(Slider))]
    public class SliderInput : MonoBehaviour
    {

        #region Fields

        public string slideAxis;
        public float sensitivity;
        public bool integerValues;
        public float repeatDelay;

        private Slider slider;
        private bool inputStarted;
        private bool inputPos;
        private float elapsed;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            slider = GetComponent<Slider>();
            if (sensitivity < 1) sensitivity = 1;
        }

        private void OnEnable()
        {
            inputStarted = false;
            elapsed = 0;
        }

        private void Reset()
        {
            slideAxis = "Horizontal";
            sensitivity = 1;
            repeatDelay = 0.25f;
        }

        private void Update()
        {
            float input = InterfaceManager.Input.GetAxis(slideAxis) * Time.deltaTime * sensitivity;

            if (integerValues)
            {
                if(!inputStarted)
                {
                    if(input > 0)
                    {
                        inputStarted = true;
                        inputPos = true;
                        slider.value += 1;
                    }
                    else if (input < 0)
                    {
                        inputStarted = true;
                        inputPos = false;
                        slider.value -= 1;
                    }
                }
                else
                {
                    if(input > 0 && inputPos || input < 0 && !inputPos)
                    {
                        elapsed += Time.deltaTime;
                        if(elapsed >= repeatDelay)
                        {
                            elapsed -= repeatDelay;
                            slider.value += inputPos ? 1 : -1;
                        }
                    }
                    else
                    {
                        inputStarted = false;
                        elapsed = 0;
                    }
                }
            }
            else
            {
                slider.value += input;
            }
        }

        #endregion


    }
}