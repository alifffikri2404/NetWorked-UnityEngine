using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class BasicInfoUI : MonoBehaviour, IPointerClickHandler
    {

        #region Fields

        [Tooltip("Label used to display Id")] public Label id;
        [Tooltip("Label used to display Title")] public Label title;
        [Tooltip("Label used to display Abbreviation")] public Label abbreviation;
        [Tooltip("Label used to display Description")] public Label description;
        [Tooltip("Label used to display Group Name")] public Label groupName;
        [Tooltip("Image used to display Sprite")] public Image image;

        public UnityEvent onClick;

        #endregion

        #region Properties

        [AutoDoc("Gets the BasicInfo being displayed", "using NullSave.GDTK;<br/>using UnityEngine;<br/><br/>public class Example : MonoBehaviour<br/>{<br/><br/>    BasicInfoUI source;<br/><br/>    public void ExampleMethod()<br/>    {<br/>        BasicInfo info = source.basicInfo;<br/>    }<br/><br/>}")]
        public BasicInfo basicInfo { get; private set; }

        #endregion

        #region Unity Methods

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke();
        }

        #endregion

        #region Public Methods

        [AutoDoc("Load Basic Info for UI", "using NullSave.GDTK;<br/>using UnityEngine;<br/><br/>public class Example : MonoBehaviour<br/>{<br/><br/>    BasicInfoUI source;<br/><br/>    public void ExampleMethod()<br/>    {<br/>        BasicInfo info = new BasicInfo() { id = \"myId\", title = \"Hello World\" };<br/>        source.Load(info);<br/>    }<br/><br/>}")]
        [AutoDocParameter("Basic Info to load")]
        public void Load(BasicInfo info)
        {
            basicInfo = info;
            if(info == null)
            {
                if (id) id.text = string.Empty;
                if (title) title.text = string.Empty;
                if (abbreviation) abbreviation.text = string.Empty;
                if (description) description.text = string.Empty;
                if (groupName) groupName.text = string.Empty;
                if (image)
                {
                    image.sprite = null;
                    image.enabled = false;
                }

                return;
            }

            if (id) id.text = info.id;
            if (title) title.text = info.title;
            if (abbreviation) abbreviation.text = info.abbr;
            if (description) description.text = info.description;
            if (groupName) groupName.text = info.groupName;
            if (image)
            {
                image.sprite = info.image.GetImage();
                image.enabled = image.sprite != null;
            }
        }

        #endregion

    }
}