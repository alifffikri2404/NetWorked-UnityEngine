using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    //[ExecuteInEditMode]
    [RequireComponent(typeof(Selectable))]
    [DefaultExecutionOrder(-160)]
    public class TabStopForSelectables : MonoBehaviour, ITabStop
    {

        #region Fields

        [SerializeField] private int m_parentStopId;
        [SerializeField] private int m_tabStopId;

        public UnityEvent onSelected, onDeselected;
        
        private bool selected;
        private Selectable target;

        #endregion

        #region Properties

        public GameObject attachedObject
        {
            get
            {
                if (gameObject == null) return null;
                return gameObject;
            }
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

        protected void Awake()
        {
            target = GetComponent<Selectable>();
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

        private void OnDisable()
        {
            ToolRegistry.RemoveComponent(this);
        }

        private void OnEnable()
        {
            ToolRegistry.RegisterComponent(this);
        }

        protected void Reset()
        {
            m_parentStopId = 0;
            m_tabStopId = -1;

            ITabStop tabStop;
            int highestIndex = -1;
            foreach (Component go in FindObjectsOfType<Component>())
            {
                if (go is ITabStop stop && go != this)
                {
                    tabStop = stop;
                    if (tabStop.parentStopId == parentStopId)
                    {
                        if (tabStop.tabStopId > highestIndex)
                        {
                            highestIndex = tabStop.tabStopId;
                        }
                    }
                }
            }

            tabStopId = highestIndex + 1;
        }

        private void Update()
        {
            if (!Application.isPlaying) return;

            if (EventSystem.current.currentSelectedGameObject == target.gameObject)
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