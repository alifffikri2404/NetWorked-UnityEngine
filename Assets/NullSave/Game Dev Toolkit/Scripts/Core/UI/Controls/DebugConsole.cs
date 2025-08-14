using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("A UI control capable of sending and receiving messages over the `Broadcast System` to enable in-game debugging.")]
    public class DebugConsole : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("Method used to show/hide console.")] public NavigationTypeSimple showHide;
        [Tooltip("Key used to show/hide console.")] public KeyCode showHideKey;
        [Tooltip("Button used to show/hide console.")] public string showHideButton;
        [Tooltip("Show the console immediately.")] public bool startShown;
        [Tooltip("Animator bool parameter updated with show/hide state.")] public string showHideAnimBool;

        [Tooltip("Receive errors for Unity.")] public bool receiveErrors;
        [Tooltip("Where should we show output text?")] public TextMeshProUGUI output;
        [Tooltip("Where do we get command input?")] public TMP_InputField input;
        [Tooltip("How many commands do we keep and display?")] public int bufferSize;
        [Tooltip("Should we auto-size the output's parent transform?")] public bool autoSizeContainer;
        [Tooltip("When auto-size, what padding should we apply?")] public Vector2 sizePadding;
        [Tooltip("If supplied scrollbar will scroll to bottom everytime a command or message is received.")] public Scrollbar scrollbar;

        [Tooltip("Event fired when the console is shown.")] public UnityEvent onShow;
        [Tooltip("Event fired when the console is hidden.")] public UnityEvent onHide;

        private List<string> buffer;
        private RectTransform rtOutput, rtContainer;
        private bool shown;

        #endregion

        #region Properties

        [AutoDoc("Animator used to control show/hide animations.", "sampleObject.consoleAnimator = animator;")]
        public Animator consoleAnimator { get; set; }

        #endregion

        #region Unity Methods

        public void Awake()
        {
            consoleAnimator = GetComponentInChildren<Animator>();
            buffer = new List<string>();
            rtOutput = output.GetComponent<RectTransform>();
            rtContainer = output.transform.parent.GetComponent<RectTransform>();
            if (autoSizeContainer)
            {
                rtContainer.sizeDelta = rtOutput.sizeDelta + sizePadding;
            }
            Broadcaster.SubscribeToPublic(this);
        }

        public void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
            Application.logMessageReceived -= Application_logMessageReceived;
        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            AddMessage(type.ToString() + ": " + condition);
        }

        public void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);
            if (receiveErrors)
            {
                Application.logMessageReceived += Application_logMessageReceived;
            }
        }

        public void Reset()
        {
            output = GetComponentInChildren<TextMeshProUGUI>();
            input = GetComponentInChildren<TMP_InputField>();
            autoSizeContainer = true;
            bufferSize = 30;
            showHideAnimBool = "Shown";
        }

        public void Start()
        {
            if (input != null)
            {
                input.onSubmit.AddListener(SendCommand);
            }

            shown = startShown;
            if(shown)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Update()
        {
            switch(showHide)
            {
                case NavigationTypeSimple.ByButton:
                    if(InterfaceManager.Input.GetButtonDown(showHideButton))
                    {
                        ToggleShown();
                    }
                    break;
                case NavigationTypeSimple.ByKey:
                    if(InterfaceManager.Input.GetKeyDown(showHideKey))
                    {
                        ToggleShown();
                    }
                    break;
            }
        }

        #endregion

        #region Public Methods

        [AutoDoc("Hide the console.", "sampleObject.Hide();")]
        public void Hide()
        {
            shown = false;
            if (consoleAnimator)
            {
                consoleAnimator.SetBool(showHideAnimBool, shown);
            }
            if (input)
            {
                input.interactable = false;
                input.text = string.Empty;
            }
            onHide.Invoke();
        }

        [AutoDoc("Show the console.", "sampleObject.Hide();")]
        public void Show()
        {
            shown = true;
            if (consoleAnimator)
            {
                consoleAnimator.SetBool(showHideAnimBool, shown);
            }
            if (input)
            {
                input.interactable = true;
                if(EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(input.gameObject);
                }
            }
            onShow?.Invoke();
        }

        [AutoDoc("Toggle the show/hide state of the console.", "sampleObject.ToggleShown();")]
        public void ToggleShown()
        {
            if(shown)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        #endregion

        #region Broadcast Methods

        [AutoDocSuppress]
        public void BroadcastReceived(object sender, string channel, string message, object[] args) { }

        [AutoDocSuppress]
        public void PublicBroadcastReceived(object sender, string message)
        {
            if(message.StartsWith(Messages.DEBUG_CONSOLE_RESPONSE))
            {
                AddMessage(message.Substring(Messages.DEBUG_CONSOLE_RESPONSE.Length).Trim());
            }
        }

        #endregion

        #region Private Methods

        private void AddMessage(string message)
        {
            buffer.Add(message);
            if (buffer.Count > bufferSize)
            {
                buffer.RemoveAt(0);
            }

            StringBuilder sb = new StringBuilder();
            foreach (string msg in buffer)
            {
                sb.AppendLine(msg);
            }

            output.text = sb.ToString();
            rtOutput.sizeDelta = output.GetPreferredValues(output.text, 9999, 9999);
            if (autoSizeContainer)
            {
                rtContainer.sizeDelta = rtOutput.sizeDelta + sizePadding;
            }

            if (scrollbar != null)
            {
                StartCoroutine(ScrollDown());
            }
        }

        private void SendCommand(string value)
        {
            if(value.ToLower() == "quit")
            {
                Application.Quit();
                return;
            }

            if(value.ToLower() == "--identify")
            {
                Broadcaster.PublicBroadcast(this, Messages.DEBUG_CONSOLE_IDENTIFY);
                input.text = string.Empty;
                if (EventSystem.current != null)
                {
                    EventSystem.current.SetSelectedGameObject(input.gameObject);
                    input.ActivateInputField();
                }
                return;
            }

            AddMessage("<color=#cccccc><i>" + input.text + "</i></color>\r\n");
            Broadcaster.PublicBroadcast(this, Messages.DEBUG_CONSOLE_REQUEST + " " + input.text);
            input.text = string.Empty;
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(input.gameObject);
                input.ActivateInputField();
            }
        }

        private IEnumerator ScrollDown()
        {
            yield return new WaitForEndOfFrame();
            scrollbar.value = 0;
        }

        #endregion

    }
}
