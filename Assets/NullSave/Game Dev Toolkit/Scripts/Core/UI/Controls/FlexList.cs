using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    public class FlexList : BaseFlexList
    {

        #region Enumerations

        public enum SortMode
        {
            StringDesc = 0,
            StringAsc = 1,
            HasImageDesc = 2,
            HasImageAsc = 3,
        }

        #endregion

        #region Fields

        [Tooltip("Automatically select the first item on start")] public bool autoSelectFirstItem;
        [Tooltip("Prefab to create for displaying item in grid")] public FlexListItem gridTemplate;
        [Tooltip("Prefab to create for displaying item in list")] public FlexListItem listTemplate;
        [Tooltip("Transform to place prefabs inside")] public ScrollRect scrollRect;
        [Tooltip("List of filter/sort plugins")] public List<SAFPluginWrapper> plugins;

        [SerializeField] private List<FlexListOption> m_options;

        private List<FlexListItem> loaded;
        private int ipr;
        private bool layoutSet;
        private bool loading;

        #endregion

        #region Properties

        public override int itemsPerRow { get { return ipr; } }

        public IReadOnlyList<FlexListOption> options { get { return m_options; } }

        public override int selectedIndex
        {
            get { return selIndex; }
            set
            {
                if (navigation.lockInput) return;

                if (loaded == null) return;
                if (loaded.Count <= value) value = loaded.Count - 1;
                if (value < 0) value = 0;
                if (selIndex <= loaded.Count - 1 && selIndex >= 0) loaded[selIndex].selected = false;
                selIndex = value;

                if (selIndex >= 0 && selIndex <= loaded.Count - 1)
                {
                    loaded[selIndex].selected = true;
                }

                if (selectedItem != null)
                {
                    scrollRect.EnsureItemVisibility(selectedItem.gameObject);
                }

                onSelectionChanged?.Invoke(selIndex);
            }
        }

        public FlexListItem selectedItem
        {
            get
            {
                if (selIndex <= loaded.Count - 1 && selIndex >= 0) return loaded[selIndex];
                return null;
            }
            set
            {
                if (navigation.lockInput) return;

                for (int i = 0; i < loaded.Count; i++)
                {
                    if (loaded[i] == value)
                    {
                        selectedIndex = i;
                        return;
                    }
                }

                selectedIndex = -1;
            }
        }

        #endregion

        #region Unity Methods

        public override void Awake()
        {
            base.Awake();

            loaded = new List<FlexListItem>();

            if (gridTemplate != null && gridTemplate.gameObject.scene.buildIndex != -1)
            {
                gridTemplate.gameObject.SetActive(false);
            }

            if (listTemplate != null && listTemplate.gameObject.scene.buildIndex != -1)
            {
                listTemplate.gameObject.SetActive(false);
            }

            foreach(SAFPluginWrapper wrapper in plugins)
            {
                wrapper.plugin.requiresUpdate += UpdateSortAndFilter;
            }

        }

        private void OnEnable()
        {
            if (!layoutSet)
            {
                StartCoroutine(UpdateLayoutMode());
            }
        }

        private void Reset()
        {
            scrollRect = GetComponent<ScrollRect>();
            autoSelectFirstItem = true;
            orientation = Orientation.Vertical;
            layout = ListLayout.List;
        }

        #endregion

        #region Public Methods

        public void AddOption(FlexListOption option)
        {
            m_options.Add(option);
            LoadItem(option);
        }

        public void SetFilter(string filter)
        {
            foreach (SAFPluginWrapper wrapper in plugins)
            {
                if (wrapper.plugin.isEnabled)
                {
                    wrapper.plugin.filterBy = filter;
                }
            }
        }

        public void SetFilterFromInput(TMPro.TMP_InputField source)
        {
            SetFilter(source.text);
        }

        public void SetRequireImage(bool required)
        {
            foreach (SAFPluginWrapper wrapper in plugins)
            {
                if (wrapper.plugin.isEnabled)
                {
                    wrapper.plugin.filterBy = required;
                }
            }
        }

        public void SetRequireImageByCheckbox(Checkbox source)
        {
            SetRequireImage(source.isOn);
        }

        public void SetSort(SortMode mode)
        {
            foreach (SAFPluginWrapper wrapper in plugins)
            {
                if (wrapper.plugin.isEnabled)
                {
                    wrapper.plugin.sortBy = mode;
                }
            }
        }

        public void SetSortByDropdown(TMPro.TMP_Dropdown source)
        {
            SetSort((SortMode)source.value);
        }

        #endregion

        #region Private Methods

        private int CalculateItemsPerRow(GridLayoutGroup glg)
        {
            RectTransform rt = scrollRect.GetComponent<RectTransform>();
            float w = rt.sizeDelta.x;
            if (w <= 0) w = rt.rect.width;
            float availWidth = w - glg.padding.horizontal;
            return Mathf.FloorToInt((availWidth + glg.spacing.x) / (glg.cellSize.x + glg.spacing.x));
        }

        private void Clear()
        {
            foreach(FlexListItem item in loaded)
            {
                Destroy(item.gameObject);
            }
            loaded.Clear();
        }

        private void LoadItem(FlexListOption option)
        {
            FlexListItem item;
            switch (layout)
            {
                case ListLayout.Grid:
                    item = Instantiate(gridTemplate, scrollRect.content.transform);
                    break;
                default:
                    item = Instantiate(listTemplate, scrollRect.content.transform);
                    break;
            }

            if (item.label != null)
            {
                item.label.text = option.text;
            }

            if (item.image != null)
            {
                item.image.sprite = option.image;
                if (option.image == null)
                {
                    item.image.enabled = false;
                }
            }

            loaded.Add(item);
            item.gameObject.SetActive(true);

            selIndex = -1;
            if (autoSelectFirstItem)
            {
                selectedIndex = 0;
            }

            loading = false;
        }

        private IEnumerator UpdateLayoutMode()
        {
            while (loading) yield return null;
            loading = true;

            HorizontalLayoutGroup hlg = scrollRect.content.GetComponent<HorizontalLayoutGroup>();
            VerticalLayoutGroup vlg = scrollRect.content.GetComponent<VerticalLayoutGroup>();
            GridLayoutGroup glg = scrollRect.content.GetComponent<GridLayoutGroup>();
            ContentSizeFitter csf = scrollRect.content.GetComponent<ContentSizeFitter>();

            if (hlg != null) Destroy(hlg);
            if (vlg != null) Destroy(vlg);
            if (glg != null) Destroy(glg);
            yield return new WaitForEndOfFrame();

            if (csf == null) csf = scrollRect.content.gameObject.AddComponent<ContentSizeFitter>();
            switch (orientation)
            {
                case Orientation.Horizontal:
                    csf.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                    csf.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
                    break;
                case Orientation.Vertical:
                    csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                    csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    break;
            }

            switch (layout)
            {
                case ListLayout.Grid:
                    glg = scrollRect.content.gameObject.AddComponent<GridLayoutGroup>();
                    glg.padding = padding;
                    glg.cellSize = cellSize;
                    glg.spacing = cellSpacing;
                    ipr = CalculateItemsPerRow(glg);
                    break;
                case ListLayout.List:
                    switch (orientation)
                    {
                        case Orientation.Horizontal:
                            hlg = scrollRect.content.gameObject.AddComponent<HorizontalLayoutGroup>();
                            hlg.childControlHeight = true;
                            hlg.childControlWidth = false;
                            hlg.childScaleHeight = false;
                            hlg.childScaleWidth = false;
                            hlg.childForceExpandHeight = true;
                            hlg.childForceExpandWidth = false;
                            hlg.spacing = listSpacing;
                            hlg.padding = padding;
                            ipr = 1;
                            break;
                        case Orientation.Vertical:
                            vlg = scrollRect.content.gameObject.AddComponent<VerticalLayoutGroup>();
                            csf.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                            csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                            vlg.childControlHeight = false;
                            vlg.childControlWidth = true;
                            vlg.childScaleHeight = false;
                            vlg.childScaleWidth = false;
                            vlg.childForceExpandHeight = false;
                            vlg.childForceExpandWidth = true;
                            vlg.spacing = listSpacing;
                            vlg.padding = padding;
                            ipr = 1;
                            break;
                    }
                    break;
            }

            layoutSet = true;
            UpdateSortAndFilter();
        }

        private void UpdateSortAndFilter()
        {
            Clear();

            List<FlexListOption> filteredList = m_options.ToList();
            foreach(SAFPluginWrapper wrapper in plugins)
            {
                if (wrapper.plugin.isEnabled)
                {
                    wrapper.plugin.SortAndFilter(filteredList);
                }
            }

            foreach (FlexListOption option in filteredList)
            {
                LoadItem(option);
            }
        }

        #endregion

    }
}