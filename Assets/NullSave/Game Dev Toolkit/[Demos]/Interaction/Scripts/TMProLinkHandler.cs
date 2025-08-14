using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NullSave.GDTK.InteractionDemo
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TMProLinkHandler : MonoBehaviour, IPointerClickHandler
    {

        #region Fields

        private TextMeshProUGUI tmpro;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            tmpro = GetComponent<TextMeshProUGUI>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {

            int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpro, Input.mousePosition, null);
            if (linkIndex != -1)
            {
                TMP_LinkInfo linkInfo = tmpro.textInfo.linkInfo[linkIndex];
                Application.OpenURL(linkInfo.GetLinkID());
            }
        }

        #endregion

    }
}