using NullSave.GDTK;
using UnityEngine;
using UnityEngine.UI;

public class Image1DemoScript : MonoBehaviour, IBroadcastReceiver
{

    #region Fields

    private Image img;
    private RectTransform rt;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        Broadcaster.SubscribeToChannel(this, "Image1");
        img = GetComponent<Image>();
        rt = GetComponent<RectTransform>();
    }

    #endregion

    #region Broadcaster Methods

    public void BroadcastReceived(object sender, string channel, string message, object[] args)
    {
        // If we subscribed to multiple channels we could check the channel name
        // But we're only subscribing to one, so we always know what channel we're
        // Receiving this message from

        switch (message.ToLower())
        {
            case "rotate":
                switch (args.Length)
                {
                    case 1:
                        if (args[0] is Vector3 vector)
                        {
                            rt.rotation = Quaternion.Euler(vector);
                        }
                        else if (args[0] is string @string)
                        {
                            string[] parts = @string.Replace(',', ' ').Replace("  ", " ").Split(' ');
                            rt.rotation = Quaternion.Euler(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
                        }
                        else
                        {
                            Debug.LogWarning("Invalid Rotate args");
                        }
                        break;
                    case 3:
                        rt.rotation = Quaternion.Euler(float.Parse((string)args[0]), float.Parse((string)args[1]), float.Parse((string)args[2]));
                        break;
                }
                break;
            case "active":
                if (args.Length == 1)
                {
                    if (args[0] is bool boolean)
                    {
                        gameObject.SetActive(boolean);
                    }
                    else
                    {
                        gameObject.SetActive(((string)args[0]).ToBool());
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid Active args");
                }
                break;
            case "color":
                switch (args.Length)
                {
                    case 1:
                        if (args[0] is Color color1)
                        {
                            img.color = color1;
                        }
                        else if (args[0] is string @string)
                        {
                            string[] parts = @string.Replace(',', ' ').Replace("  ", " ").Split(' ');
                            if (parts.Length == 1)
                            {
                                if (ColorUtility.TryParseHtmlString(parts[0], out Color color))
                                {
                                    img.color = color;
                                }
                            }
                            else if (parts.Length == 3)
                            {
                                img.color = new Color(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
                            }
                        }
                        else
                        {
                            Debug.LogWarning("Invalid Color args");
                        }
                        break;
                    case 3:
                        img.color = new Color(float.Parse((string)args[0]), float.Parse((string)args[1]), float.Parse((string)args[2]));
                        break;
                }
                break;
            default:
                Debug.LogWarning("Unrecognized command '" + message + "'.");
                break;
        }
    }

    // We're not listening to Public broadcasts
    // But the interface requires this method
    // So we just leave it empty
    public void PublicBroadcastReceived(object sender, string message) { }

    #endregion

}