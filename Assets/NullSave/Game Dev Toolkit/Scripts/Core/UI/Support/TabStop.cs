using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    //[ExecuteInEditMode]
    [DefaultExecutionOrder(-160)]
    public class TabStop : Selectable, ITabStop
    {

        #region Fields

        [SerializeField] private int m_parentStopId;
        [SerializeField] private int m_tabStopId;

        public UnityEvent onSelected, onDeselected;
        private bool selected;

        #endregion

        #region Properties

        public GameObject attachedObject
        {
            get { return gameObject; }
        }

        public int parentStopId
        {
            get { return m_parentStopId; }
            set { m_parentStopId = value; }
        }

        public int tabStopId
        {
            get { return m_tabStopId; }
            set { m_tabStopId = value; }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            ToolRegistry.RegisterComponent(this);
            if (EventSystem.current.currentSelectedGameObject == this)
            {
                selected = true;
                onSelected?.Invoke();
            }
            else
            {
                selected = false;
                onDeselected?.Invoke();
            }
        }

        //public override void Reset()
        //{
        //    base.Reset();

        //    m_parentStopId = 0;
        //    m_tabStopId = -1;

        //    ITabStop tabStop;
        //    int highestIndex = -1;
        //    foreach (Component go in FindObjectsOfType<Component>())
        //    {
        //        if (go is ITabStop && go != this)
        //        {
        //            tabStop = (ITabStop)go;
        //            if (tabStop.parentStopId == parentStopId)
        //            {
        //                if (tabStop.tabStopId > highestIndex)
        //                {
        //                    highestIndex = tabStop.tabStopId;
        //                }
        //            }
        //        }
        //    }

        //    tabStopId = highestIndex + 1;
        //}

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (EventSystem.current.currentSelectedGameObject == gameObject)
            {
                if (!selected)
                {
                    selected = true;
                    onSelected?.Invoke();
                }
            }
            else if (selected)
            {
                selected = false;
                onDeselected?.Invoke();
            }
        }


        #endregion

    }
}