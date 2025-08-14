using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class BroadcasterTerminal : MonoBehaviour, IBroadcastReceiver
    {

        #region Fields

        [Tooltip("Where should we show output text?")] public TextMeshProUGUI output;
        [Tooltip("Where do we get command input?")] public TMP_InputField input;
        [Tooltip("How many commands do we keep and display?")] public int bufferSize;
        [Tooltip("Should we auto-size the output's parent transform?")] public bool autoSizeContainer;
        [Tooltip("When auto-size, what padding should we apply?")] public Vector2 sizePadding;
        [Tooltip("If supplied scrollbar will scroll to bottom everytime a command or message is received.")] public Scrollbar scrollbar;

        private List<string> buffer;
        private RectTransform rtOutput, rtContainer;

        #endregion

        #region Unity Methods

        public void Awake()
        {
            buffer = new List<string>();
            rtOutput = output.GetComponent<RectTransform>();
            rtContainer = output.transform.parent.GetComponent<RectTransform>();
            if (autoSizeContainer)
            {
                rtContainer.sizeDelta = rtOutput.sizeDelta;
            }
            Broadcaster.SubscribeToAll(this);
        }

        public void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
        }

        public void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);
        }

        public void Reset()
        {
            output = GetComponentInChildren<TextMeshProUGUI>();
            input = GetComponentInChildren<TMP_InputField>();
            autoSizeContainer = true;
            bufferSize = 30;
        }

        public void Start()
        {
            if (input != null) input.onSubmit.AddListener(ProcessCommand);
        }

        #endregion

        #region Public Methods

        public void BroadcastReceived(object sender, string channel, string message, object[] args)
        {
            if (args == null || args.Length == 0)
            {
                AddMessage("<b>Channel Broadcast Received</b>\r\nSender: " + sender + "\r\nChannel: " + channel + "\r\nMessage: " + message + "\r\n");
            }
            else
            {
                string argData = "\r\n";
                for (int i = 0; i < args.Length; i++)
                {
                    argData += $"args[{i}]: " + args[i] + "\r\n";
                }
                AddMessage("<b>Channel Broadcast Received</b>\r\nSender: " + sender + "\r\nChannel: " + channel + "\r\nMessage: " + message + argData);
            }
        }

        public static void ExecuteCommand(object sender, List<string> parts)
        {
            // Process
            if (parts.Count == 1)
            {
                Broadcaster.PublicBroadcast(sender, parts[0]);
            }
            else if (parts.Count == 2)
            {
                Broadcaster.Broadcast(sender, parts[0], parts[1]);
            }
            else
            {
                object[] args = new object[parts.Count - 2];
                for (int i = 2; i < parts.Count; i++)
                {
                    args[i - 2] = parts[i];
                }
                Broadcaster.Broadcast(sender, parts[0], parts[1], args);
            }
        }

        public static List<string> ParseCommand(string command)
        {
            List<string> parts = new List<string>();
            int i, e;

            command = command.Trim();

            // Parse
            while (!string.IsNullOrWhiteSpace(command))
            {
                i = command.IndexOf(' ');
                e = command.IndexOf('"');

                if ((i > -1 && e > -1 && i < e) || e < 0)
                {
                    // Space or end before quote
                    if (i < 0)
                    {
                        // Whole command
                        parts.Add(command);
                        command = string.Empty;
                    }
                    else
                    {
                        parts.Add(command.Substring(0, i).Trim());
                        command = command.Substring(i + 1);
                    }
                }
                else
                {
                    // We found a quoted area
                    i = command.IndexOf('"', e + 1);
                    if (i < 0)
                    {
                        // Entire command
                        parts.Add(command);
                        command = string.Empty;
                    }
                    else
                    {
                        parts.Add(command.Substring(e + 1, i - e - 1));
                        command = command.Substring(i + 1).Trim();
                    }
                }
            }

            return parts;
        }

        public void ProcessCommand(string command)
        {
            ExecuteCommand(this, ParseCommand(command));
            if (input != null) input.text = string.Empty;
        }

        public void PublicBroadcastReceived(object sender, string message)
        {
            AddMessage("<b>Public Broadcast Received</b>\r\nMessage: " + message + "\r\n");
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

        private IEnumerator ScrollDown()
        {
            yield return new WaitForEndOfFrame();
            scrollbar.value = 0;
        }

        #endregion

    }
}