using System.Collections;
using NullSave.GDTK;
using UnityEngine;
using UnityEngine.UI;

public class Image2DemoScript : MonoBehaviour, IBroadcastReceiver
{

    #region Fields

    private Image img;
    private float elapsed;

    #endregion

    #region Unity Methods

    public void Awake()
    {
        img = GetComponent<Image>();
    }

    private void OnDisable()
    {
        Broadcaster.UnsubscribeFromPublic(this);
    }

    private void OnEnable()
    {
        Broadcaster.SubscribeToPublic(this);
    }

    #endregion

    #region Broadcaster Methods

    // We're not listening to Channel broadcasts
    // But the interface requires this method
    // So we just leave it empty
    public void BroadcastReceived(object sender, string channel, string message, object[] args) { }

    public void PublicBroadcastReceived(object sender, string message)
    {
        switch (message.ToLower())
        {
            case "fadein":
                StartCoroutine(FadeIn());
                break;
            case "fadeout":
                StartCoroutine(FadeOut());
                break;
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator FadeIn()
    {
        elapsed = 0;
        img.color = new Color(1, 1, 1, 0);
        while(elapsed < 1)
        {
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
            img.color = new Color(1, 1, 1, elapsed / 1);
        }
    }

    private IEnumerator FadeOut()
    {
        elapsed = 0;
        img.color = Color.white;
        while (elapsed < 1)
        {
            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
            img.color = new Color(1, 1, 1, 1 - elapsed / 1);
        }
    }

    #endregion

}
