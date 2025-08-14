using UnityEngine;

namespace NullSave.GDTK
{
    public class BaseFlexList : MonoBehaviour
    {

        #region Fields

        [Tooltip("Layout orientation")] [SerializeField] internal Orientation orientation;
        [Tooltip("Layout type")] [SerializeField] internal ListLayout layout;
        [Tooltip("Grid cell size")] public Vector2 cellSize;
        [Tooltip("Grid cell spacing")] public Vector2 cellSpacing;
        [Tooltip("Layout padding")] public RectOffset padding;
        [Tooltip("List spacing")] public float listSpacing;

        [Tooltip("Event raised when selection changes")] public SelectedIndexChanged onSelectionChanged;

        internal int selIndex;
        [SerializeField] protected Nav2D navigation;

        #endregion

        #region Properties

        public virtual int selectedIndex { get; set; }

        public virtual int itemsPerRow { get; }

        #endregion

        #region Unity Methods

        public virtual void Awake()
        {
            navigation = new Nav2D();

            navigation.onBackH += NavigateBackH;
            navigation.onBackV += NavigateBackV;
            navigation.onNextH += NavigateNextH;
            navigation.onNextV += NavigateNextV;
        }

        public virtual void Update()
        {
            navigation.Update(Time.deltaTime);
        }

        #endregion

        #region Public Methods

        public virtual void SetGrid() { }

        public virtual void SetLayout(ListLayout mode) { }

        public virtual void SetList() { }

        #endregion

        #region Private Methods

        private void NavigateBackH()
        {
            if (layout == ListLayout.Grid || orientation == Orientation.Horizontal)
            {
                selectedIndex -= 1;
            }
        }

        private void NavigateBackV()
        {
            switch (layout)
            {
                case ListLayout.Grid:
                    if (selIndex >= itemsPerRow)
                    {
                        selectedIndex -= itemsPerRow;
                    }
                    break;
                case ListLayout.List:
                    if (orientation == Orientation.Vertical)
                    {
                        selectedIndex -= 1;
                    }
                    break;
            }
        }

        private void NavigateNextH()
        {
            if (layout == ListLayout.Grid || orientation == Orientation.Horizontal)
            {
                selectedIndex += 1;
            }
        }

        private void NavigateNextV()
        {
            switch (layout)
            {
                case ListLayout.Grid:
                    selectedIndex += itemsPerRow;
                    break;
                case ListLayout.List:
                    if (orientation == Orientation.Vertical)
                    {
                        selectedIndex += 1;
                    }
                    break;
            }
        }


        #endregion

    }
}