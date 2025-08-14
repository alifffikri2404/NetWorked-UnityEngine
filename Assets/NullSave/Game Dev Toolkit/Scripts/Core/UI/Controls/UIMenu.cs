using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [AutoDoc("This component provides graphic menus.")]
    [ExecuteInEditMode]
    public class UIMenu : Selectable
    {

        #region Fields

        [Tooltip("Automatically select the first item on start")] public bool autoSelectFirstItem;
        [Tooltip("Display as a context menu")] public bool contextMenu;

        [Tooltip("Navigation Details")] public UI1DNav menuNav;

        [Tooltip("Button to submit selected item")] public string submitButton;
        [Tooltip("Button to cancel out of menu")] public string cancelButton;

        [Tooltip("Key to submit selected item")] public KeyCode submitKey;
        [Tooltip("Key to cancel out of menu")] public KeyCode cancelKey;

        [Tooltip("Hide items that are disabled")] public bool hideDisabled;
        [Tooltip("Allow canceling from menu")] public bool canCancel;

        [Tooltip("Make menu modal")] public bool isModalMenu;
        [Tooltip("Automatically set modal mode")] public bool autoSetModal;
        [Tooltip("ID to associate with menu in modal mode")] public string dialogId;

        // Audio
        [Tooltip("Component used to play audio")] public AudioSource audioSource;
        [Tooltip("Sound to play on selection change")] public AudioClip changeSelection;
        [Tooltip("Sound to play on item submit")] public AudioClip submit;
        [Tooltip("Sound to play on cancel")] public AudioClip cancel;

        // Events
        [Tooltip("Event raised when selection changes")] public MenuItemEvent onSelectionChanged;
        [Tooltip("Event raised when canceling")] public UnityEvent onCancel;
        [Tooltip("Event raised when selected index changes")] public SelectedIndexChanged onSelectedIndexChanged;

        private List<UIMenuItem> menuItems;
        private int selIndex;

        #endregion

        #region Properties

        [AutoDoc("Active child menu.", "sampleObject.activeChildMenu.Close();")]
        public UIMenu activeChildMenu { get; private set; }

        [AutoDoc("Gets/Sets if this object should destroy itself on close.", "sampleObject.destroyOnClose = true;")]
        public bool destroyOnClose { get; set; }

        [AutoDoc("Gets/Sets the control that this is a sub-menu of.", "Debug.Log(sampleObject.parent);")]
        public UIMenu parent { get; set; }

        [AutoDoc("Gets/Sets the currently selected index", "sampleObject.selectedIndex = 0;")]
        public int selectedIndex
        {
            get { return selIndex; }
            set
            {
                if (selIndex < -1) selIndex = -1;
                if (selIndex >= menuItems.Count) selIndex = menuItems.Count - 1;
                if (selIndex == value) return;

                int orgIndex = selIndex;
                selIndex = value;

                if (orgIndex > -1 && orgIndex < menuItems.Count)
                {
                    menuItems[orgIndex].Refresh();
                }

                if (selIndex > -1 && selIndex < menuItems.Count)
                {
                    menuItems[selIndex].Refresh();
                }

                onSelectedIndexChanged?.Invoke(selIndex);
                onSelectionChanged?.Invoke(selectedItem);
            }
        }

        [AutoDoc("Gets/Sets the currently selected item", "sampleObject.selectedItem.text = \"Hello World\";")]
        public UIMenuItem selectedItem
        {
            get
            {
                if (selIndex < 0 || menuItems == null || selIndex > menuItems.Count - 1) return null;
                return menuItems[selIndex];
            }
            set
            {
                for (int i = 0; i < menuItems.Count; i++)
                {
                    if (menuItems[i] == value)
                    {
                        selectedIndex = i;
                        return;
                    }
                }
            }
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            menuItems = new List<UIMenuItem>();
            if (menuNav != null)
            {
                menuNav.onBack += () => PreviousItem();
                menuNav.onNext += () => NextItem();
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            selIndex = -1;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            FindChildren();
            if (autoSelectFirstItem)
            {
                ActivateFirstItem();
            }
            else
            {
                selIndex = -1;
            }
        }

#pragma warning disable CS0114 // Member hides inherited member; missing override keyword
        protected void Reset()
#pragma warning restore CS0114 // Member hides inherited member; missing override keyword
        {
            canCancel = true;
            submitKey = KeyCode.Return;
            cancelKey = KeyCode.Escape;
            submitButton = "Submit";
            cancelButton = "Cancel";

            audioSource = GetComponent<AudioSource>();
            targetGraphic = GetComponent<Image>();
        }

        public void Update()
        {
            if (!interactable || activeChildMenu != null) return;

            menuNav.Update(Time.deltaTime);

            //if(contextMenu && ( Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2)))
            //{
            //    // Auto-close as needed
            //    Vector2 position = EventSystem.current.currentInputModule.input.mousePosition;

            //    var eventData = new PointerEventData(EventSystem.current);
            //    eventData.position = position;

            //    List<RaycastResult> results = new List<RaycastResult>();
            //    EventSystem.current.RaycastAll(eventData, results);

            //    bool isMenuOrItem = false;
            //    foreach (RaycastResult result in results)
            //    {
            //        if (result.gameObject.GetComponent<UIMenu>() != null)
            //        {
            //            isMenuOrItem = true;
            //            break;
            //        }
            //        else if (result.gameObject.GetComponent<UIMenuItem>() != null)
            //        {
            //            isMenuOrItem = true;
            //            break;
            //        }
            //    }
            //    if(!isMenuOrItem)
            //    {
            //        Close();
            //        return;
            //    }
            //}

            //CheckNavigation();

            //if (hideDisabled)
            //{
            //    foreach (UIMenuItem menuItem in menuItems)
            //    {
            //        menuItem.gameObject.SetActive(menuItem.interactable);
            //    }
            //}

            //if ((navigationMode.HasFlag(UINavigationType.ByButton) && InterfaceManager.Input.GetButtonDown(submitButton)) ||
            //    (navigationMode.HasFlag(UINavigationType.ByKey) && InterfaceManager.Input.GetKeyDown(submitKey)))
            //{
            //    selectedItem.OnSubmit(null);
            //}

            //if (canCancel)
            //{
            //    if ((navigationMode.HasFlag(UINavigationType.ByButton) && InterfaceManager.Input.GetButtonDown(cancelButton)) ||
            //        (navigationMode.HasFlag(UINavigationType.ByKey) && InterfaceManager.Input.GetKeyDown(cancelKey)))
            //    {
            //        PlayAudio(cancel);
            //        onCancel?.Invoke();
            //    }
            //}
        }

        #endregion

        #region Public Methods

        [AutoDoc("Close the menu", "sampleObject.Close()")]
        public void Close()
        {
            if (parent != null)
            {
                parent.activeChildMenu = null;
                EventSystem.current.SetSelectedGameObject(parent.gameObject);
            }

            if (destroyOnClose)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        [AutoDoc("Select the next item in the list.", "sampleObject.NextItem()")]
        public void NextItem()
        {
            if (menuItems == null || !gameObject.activeSelf) return;

            int checkIndex = selIndex;
            while (true)
            {
                checkIndex += 1;
                if (checkIndex >= menuItems.Count)
                {
                    if (menuNav.allowAutoWrap)
                    {
                        checkIndex = 0;
                    }
                    else
                    {
                        return;
                    }
                }
                if (selIndex == checkIndex) return;

                if (menuItems[checkIndex].interactable)
                {
                    selectedIndex = checkIndex;
                    PlayAudio(changeSelection);
                    return;
                }
            }
        }

        [AutoDoc("Open another menu as a sub-menu of this one.", "sampleObject.OpenSubMenu(childMenu);")]
        [AutoDocParameter("Menu to display as a child.")]
        public void OpenSubMenu(UIMenu menu)
        {
            activeChildMenu = menu;
            menu.parent = this;
            menu.gameObject.SetActive(true);
            EventSystem.current.SetSelectedGameObject(menu.gameObject);
        }

        [AutoDoc("Select the previous item in the list.", "sampleObject.PreviousItem()")]
        public void PreviousItem()
        {
            if (menuItems == null || !gameObject.activeSelf) return;

            int checkIndex = selIndex;
            while (true)
            {
                checkIndex -= 1;
                if (checkIndex < 0)
                {
                    if (menuNav.allowAutoWrap)
                    {
                        checkIndex = menuItems.Count - 1;
                    }
                    else
                    {
                        return;
                    }
                }
                if (selIndex == checkIndex) return;


                if (menuItems[checkIndex].interactable)
                {
                    selectedIndex = checkIndex;
                    PlayAudio(changeSelection);
                    return;
                }
            }
        }

        [AutoDoc("Register a menu item as a child of this menu.", "sampleObject.RegisterMenuItem(menuItem);")]
        [AutoDocParameter("Menu item to register as a child.")]
        public void RegisterMenuItem(UIMenuItem item)
        {
            if (menuItems == null) menuItems = new List<UIMenuItem>();
            if (!menuItems.Contains(item))
            {
                menuItems.Add(item);
                item.onSubmit.AddListener(ItemSubmitted);
                if (selIndex == -1) selectedIndex = 0;
            }
        }

        #endregion

        #region Private Methods

        private void ActivateFirstItem()
        {
            selIndex = -1;
            bool found = false;
            for (int i = 0; i < menuItems.Count; i++)
            {
                if (!found && menuItems[i].enabled && menuItems[i].interactable)
                {
                    found = true;
                    selectedIndex = i;
                }
            }
        }

        //private void CheckNavigation()
        //{
        //    float input = 0;

        //    if (navigationMode.HasFlag(UINavigationType.ByButton))
        //    {
        //        input = InterfaceManager.Input.GetAxis(navigationButton);
        //    }

        //    if (navigationMode.HasFlag(UINavigationType.ByKey))
        //    {
        //        if(InterfaceManager.Input.GetKeyDown(backKey))
        //        {
        //            input = -1;
        //        }
        //        else if (InterfaceManager.Input.GetKeyDown(nextKey))
        //        {
        //            input = 1;
        //        }
        //    }

        //    if (invertInput) input = -input;

        //    if (input == 0)
        //    {
        //        nextRepeat = 0;
        //    }
        //    else if (input < -0.1f)
        //    {
        //        nextRepeat -= Time.deltaTime;
        //        if (nextRepeat <= 0)
        //        {
        //            PreviousItem();
        //            nextRepeat = repeatDelay;
        //        }
        //    }
        //    else if (input > 0.1f)
        //    {
        //        nextRepeat -= Time.deltaTime;
        //        if (nextRepeat <= 0)
        //        {
        //            NextItem();
        //            nextRepeat = repeatDelay;
        //        }
        //    }
        //}

        private void FindChildren()
        {
            menuItems.Clear();
            foreach (UIMenuItem item in GetComponentsInChildren<UIMenuItem>())
            {
                RegisterMenuItem(item);
            }
        }

        private void ItemSubmitted(UIMenuItem target)
        {
            PlayAudio(submit);
        }

        private void PlayAudio(AudioClip clip)
        {
            if (clip == null) return;
            audioSource.PlayOneShot(clip);
        }

        #endregion

    }
}