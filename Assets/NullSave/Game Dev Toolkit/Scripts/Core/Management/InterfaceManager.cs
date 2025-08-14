using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NullSave.GDTK
{
    [DefaultExecutionOrder(-150)]
    public class InterfaceManager : MonoBehaviour
    {

        #region Fields

        [Tooltip("Mark this object as DontDestroyOnLoad")] public bool persist;

        // Input
        [Tooltip("Input manager to use")] public InputManager inputManager;

        // Object
        [Tooltip("Object manager to use")] public ObjectManager objectManager;

        [Tooltip("Localization settings to use")][SerializeField] private LocalizationSettings m_localizationSettings;

        // Basic UI
        public bool useExistingCanvas;
        public Canvas useCanvas;

        [Tooltip("Determines how UI elements in the Canvas are scaled.")]
        [SerializeField] private UIScaleMode uiScaleMode;

        [Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
        [SerializeField] protected float referencePixelsPerUnit;

        [Tooltip("Scales all UI elements in the Canvas by this factor.")]
        [SerializeField] protected float scaleFactor;

        [Tooltip("The resolution the UI layout is designed for. If the screen resolution is larger, the UI will be scaled up, and if it's smaller, the UI will be scaled down. This is done in accordance with the Screen Match Mode.")]
        [SerializeField] protected Vector2 referenceResolution;

        [Tooltip("A mode used to scale the canvas area if the aspect ratio of the current resolution doesn't fit the reference resolution.")]
        [SerializeField] protected UIScreenMatchMode screenMatchMode;

        [Tooltip("Determines if the scaling is using the width or height as reference, or a mix in between.")]
        [Range(0, 1)]
        [SerializeField] protected float matchWidthOrHeight;

        [Tooltip("The physical unit to specify positions and sizes in.")]
        [SerializeField] protected UIUnit physicalUnit;

        [Tooltip("The DPI to assume if the screen DPI is not known.")]
        [SerializeField] protected float fallbackScreenDPI;

        [Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
        [SerializeField] protected float defaultSpriteDPI;

        [Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
        [SerializeField] protected float dynamicPixelsPerUnit;

        [Tooltip("Add a Raycaster component to the canvas.")]
        public bool includeRaycaster;

        // Tooltip
        [Tooltip("Prefab to use when displaying a tooltip.")] public TooltipDisplay tooltipPrefab;
        [Tooltip("Pixel offset from cursor to use when displaying tooltip.")] public Vector2 tipOffset;
        [Tooltip("Seconds to wait after cursor is over item to display tooltip")] public float displayDelay;

        private TooltipDisplay tooltip;
        private TooltipDisplay customTooltip;

        private Vector2 lastPos;
        private TooltipClient lastTip;

        // Interaction
        [Tooltip("Interaction UI prefab to use by default")] public InteractorUI interactorPrefab;
        [Tooltip("Method of confirming interaction")] public NavigationTypeSimple interactionType;
        [Tooltip("Button to use for confirming interaction")] public string interactionButton;
        [Tooltip("Key to use for confirming interaction")] public KeyCode interactionKey;

        // Tabstop
        [Tooltip("Automatically activate the first TabStop")] public bool activateFirstTabStop;
        [Tooltip("Method of performing tab stops")] public NavigationTypeSimple tabStyle;
        [Tooltip("Button to use for tabbing")] public string tabButton;
        [Tooltip("Key to use for tabbing")] public KeyCode tabKey;

        private static InterfaceManager current;

        private Canvas uiCanvas;
        private CanvasScaler canvasScaler;

        private List<GameObject> activeModals;
        private GameObject currentModal;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the current instance
        /// </summary>
        /// <returns></returns>
        public static InterfaceManager Current
        {
            get
            {
                if (current == null)
                {
                    // Check if something called us before we awoke.
                    current = FindObjectOfType<InterfaceManager>();
                    if (current != null) return current;

                    GameObject go = new GameObject("GDTK Interface Manager");
                    current = go.AddComponent<InterfaceManager>();
                    current.Reset();
                }

                return current;
            }
        }

        /// <summary>
        /// Returns the current Input Manager
        /// </summary>
        public static InputManager Input { get { return Current.inputManager; } }

        /// <summary>
        /// Get/Set Interface Flags
        /// </summary>
        public InterfaceStateFlags InterfaceFlags { get; set; }

        /// <summary>
        /// Returns current Localization Settings
        /// </summary>
        public static LocalizationSettings localizationSettings
        {
            get
            {
                return Current.m_localizationSettings;
            }
        }

        /// <summary>
        /// Returns true if Interface Flags contains LockPlayerController
        /// </summary>
        public static bool LockPlayerController
        {
            get { return Current.InterfaceFlags.HasFlag(InterfaceStateFlags.LockPlayerController); }
            set
            {
                if (value)
                {
                    Current.InterfaceFlags |= InterfaceStateFlags.LockPlayerController;
                }
                else
                {
                    Current.InterfaceFlags &= InterfaceStateFlags.LockPlayerController;

                }
            }
        }

        /// <summary>
        /// Returns current Object Manager
        /// </summary>
        public static ObjectManager ObjectManagement { get { return Current.objectManager; } }

        /// <summary>
        /// Returns true if Interface Flags contains PreventInteractionUI
        /// </summary>
        public static bool PreventInteractions {
            get { return Current.InterfaceFlags.HasFlag(InterfaceStateFlags.PreventInteractionUI); }
            set
            {
                if(value)
                {
                    Current.InterfaceFlags |= InterfaceStateFlags.PreventInteractionUI;
                }
                else
                {
                    Current.InterfaceFlags &= InterfaceStateFlags.PreventInteractionUI;
                    
                }
            }
        }

        /// <summary>
        /// Returns true if Interface Flags contains PreventPrompts
        /// </summary>
        public static bool PreventPrompts { get { return Current.InterfaceFlags.HasFlag(InterfaceStateFlags.PreventPrompts); } }

        /// <summary>
        /// Returns true if Interface Flags contains PreventWindows
        /// </summary>
        public static bool PreventWindows { get { return Current.InterfaceFlags.HasFlag(InterfaceStateFlags.PreventWindows); } }

        /// <summary>
        /// Returns current UI Canvas
        /// </summary>
        public static Canvas UICanvas
        {
            get
            {
                if (Current.uiCanvas == null)
                {
                    if (Current.useExistingCanvas)
                    {
                        Current.uiCanvas = current.useCanvas;
                    }
                    else
                    {
                        Current.CreateUICanvas();
                    }
                }
                return Current.uiCanvas;
            }
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if(persist)
            {

                if(current != null && current != this)
                {
                    Destroy(gameObject);
                    return;
                }

                if (Application.isPlaying)
                {
                    transform.SetParent(null);
                    SceneManager.sceneLoaded += SceneManager_sceneLoaded;
                    SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
                    DontDestroyOnLoad(this);
                }
            }
            else
            {
                if(current != null && current != this)
                {
                    Destroy(gameObject);
                    return;
                }
            }

            current = this;
        }

        private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            ResetObjects();
        }

        private void OnDestroy()
        {
            if(uiCanvas != null)
            {
                Destroy(uiCanvas.gameObject);
            }
        }

        private void Reset()
        {
            inputManager = Resources.Load<InputManager>("Input Managers/Unity Input Manager");
            objectManager = Resources.Load<ObjectManager>("Object Managers/Unity Object Manager");
            m_localizationSettings = Resources.Load<LocalizationSettings>("Localization/Default Localization");
            uiScaleMode = UIScaleMode.ScaleWithScreenSize;
            referenceResolution = new Vector2(1920, 1080);
            referencePixelsPerUnit = 100;
            scaleFactor = 1;
            screenMatchMode = UIScreenMatchMode.MatchWidthOrHeight;
            physicalUnit = UIUnit.Points;
            fallbackScreenDPI = 96;
            defaultSpriteDPI = 96;
            dynamicPixelsPerUnit = 1;

            interactorPrefab = Resources.Load<InteractorUI>("Interaction/Interactor UI");

            tooltipPrefab = Resources.Load<TooltipDisplay>("Tooltips/Tooltip_WhiteOnBlack");
            tipOffset = new Vector2(16, -40);

            includeRaycaster = true;
            activateFirstTabStop = true;
        }

        private void SceneManager_sceneUnloaded(Scene arg0)
        {
            // Destroy existing canvas reference so we can create a new one.
            uiCanvas = null;
        }

        private void Start()
        {
            ResetObjects();
        }

        private void Update()
        {
            if (EventSystem.current == null || EventSystem.current.currentInputModule == null) return;

            if (tabStyle == NavigationTypeSimple.ByButton && inputManager.GetButtonDown(tabButton))
            {
                NextTab();
            }
            else if (tabStyle == NavigationTypeSimple.ByKey && inputManager.GetKeyDown(tabKey))
            {
                NextTab();
            }

            if (tooltip != null)
            {
                Vector2 position = EventSystem.current.currentInputModule.input.mousePosition;
                if (position == lastPos)
                {
                    if (lastTip != null && (lastTip.gameObject == null || !lastTip.gameObject.activeSelf))
                    {
                        HideTooltip();
                        lastPos = new Vector2(-100, -100);
                    }
                    return;
                }

                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = position;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                if (results.Count > 0)
                {
                    lastTip = results[0].gameObject.GetComponent<TooltipClient>();
                    if (lastTip != null)
                    {
                        if (lastTip.customTooltip != null && customTooltip == null)
                        {
                            lastTip.onInitCustomTip?.Invoke();
                            customTooltip = Instantiate(lastTip.customTooltip, uiCanvas.transform);
                            customTooltip.gameObject.name = "_TOOLTIP_";
                            customTooltip.gameObject.SetActive(false);
                            customTooltip.gameObject.hideFlags = HideFlags.HideInHierarchy;
                        }

                        if (!TargetActive())
                        {
                            lastPos = position;
                            StartCoroutine(WaitToDisplay(uiCanvas, lastTip.modifyDelay));
                        }
                        else
                        {
                            PositionTooltip(position);
                        }
                    }
                }
                else
                {
                    if (lastTip != null) lastTip.onHide?.Invoke();
                    lastTip = null;
                }

                if (lastTip == null)
                {
                    HideTooltip();
                }
                lastPos = position;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Hide any currently active tooltip
        /// </summary>
        public static void HideTooltip()
        {
            current.lastTip = null;
            current.tooltip.gameObject.SetActive(false);
            if (current.customTooltip != null)
            {
                Destroy(current.customTooltip.gameObject);
                current.customTooltip = null;
            }
        }

        public static bool IsBlockedByModal(GameObject obj)
        {
            GameObject curModal = current.currentModal;

            if (curModal == null) return false;
            if (curModal == obj || obj.IsChildOf(curModal)) return false;

            return true;
        }

        /// <summary>
        /// Loads a scene
        /// </summary>
        /// <param name="index"></param>
        public void LoadScene(int index)
        {
            SceneManager.LoadScene(index);
        }

        /// <summary>
        /// Loads a scene
        /// </summary>
        /// <param name="sceneName"></param>
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Set focus to next Tab Stop
        /// </summary>
        public void NextTab()
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                ActivateFirstTab();
                return;
            }

            ITabStop currentStop = null;
            foreach(Component c in EventSystem.current.currentSelectedGameObject.GetComponents<Component>())
            {
                if(c is ITabStop stop)
                {
                    currentStop = stop;
                    break;
                }
            }

            if(currentStop == null)
            {
                ActivateFirstTab();
                return;
            }

            List<ITabStop> stops = ToolRegistry.GetComponents<ITabStop>().Where(x => x!= currentStop && x.parentStopId == currentStop.parentStopId && x.tabStopId >= currentStop.tabStopId).OrderBy(x => x.tabStopId).ToList();
            if (stops.Count > 0)
            {
                ActivateStop(stops[0]);
            }
            else
            {
                stops = ToolRegistry.GetComponents<ITabStop>().Where(x => x != currentStop && x.parentStopId == currentStop.parentStopId && x.tabStopId <= currentStop.tabStopId).OrderBy(x => x.tabStopId).ToList();
                if (stops.Count > 0)
                {
                    ActivateStop(stops[0]);
                }
            }
        }

        public static void RemoveActiveModal(GameObject obj)
        {
            if (current.activeModals == null) current.activeModals = new List<GameObject>();
            if (!current.activeModals.Contains(obj)) return;

            current.activeModals.Remove(obj);

            if (current.currentModal == obj)
            {
                current.currentModal = current.activeModals.Count == 0 ? null : current.activeModals[current.activeModals.Count - 1];
            }
        }

        public static void SetActiveModal(GameObject obj)
        {
            if (current.activeModals == null) current.activeModals = new List<GameObject>();
            if (!current.activeModals.Contains(obj))
            {
                current.activeModals.Add(obj);
            }
            current.currentModal = obj;
        }

        #endregion

        #region Private Methods

        private void ActivateFirstTab()
        {
            if (EventSystem.current == null) return;

            int lowestId = int.MaxValue;
            ITabStop activate = null;
            foreach(ITabStop tabStop in ToolRegistry.GetComponents<ITabStop>())
            {
                if(tabStop.parentStopId == 0 && tabStop.tabStopId < lowestId)
                {
                    lowestId = tabStop.tabStopId;
                    activate = tabStop;
                }
            }

            ActivateStop(activate);
        }

        private void ActivateStop(ITabStop activate)
        {
            if (activate != null)
            {
                if (activate.attachedObject != null)
                {
                    EventSystem.current.SetSelectedGameObject(activate.attachedObject);
                    activate.attachedObject.GetComponent<Selectable>().Select();
                }
            }
        }

        private void CreateUICanvas()
        {
            GameObject goCanvas = new GameObject("GDTK UI Canvas");
            
            uiCanvas = goCanvas.AddComponent<Canvas>();
            uiCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            uiCanvas.sortingOrder = 999;

            canvasScaler = goCanvas.AddComponent<CanvasScaler>();
            canvasScaler.defaultSpriteDPI = defaultSpriteDPI;
            canvasScaler.dynamicPixelsPerUnit = dynamicPixelsPerUnit;
            canvasScaler.fallbackScreenDPI = fallbackScreenDPI;
            canvasScaler.matchWidthOrHeight = matchWidthOrHeight;
            canvasScaler.physicalUnit = (CanvasScaler.Unit)physicalUnit;
            canvasScaler.referencePixelsPerUnit = referencePixelsPerUnit;
            canvasScaler.referenceResolution = referenceResolution;
            canvasScaler.scaleFactor = scaleFactor;
            canvasScaler.screenMatchMode = (CanvasScaler.ScreenMatchMode)screenMatchMode;
            canvasScaler.uiScaleMode = (CanvasScaler.ScaleMode)uiScaleMode;

            if(includeRaycaster)
            {
                goCanvas.AddComponent<GraphicRaycaster>();
            }
        }

        private void PositionTooltip(Vector2 position)
        {
            RectTransform target = customTooltip == null ? tooltip.RectTransform : customTooltip.RectTransform;
            Vector3 desiredPos = position + tipOffset;
            desiredPos.x = Mathf.Clamp(desiredPos.x, 0, uiCanvas.pixelRect.width - target.sizeDelta.x * uiCanvas.scaleFactor);
            desiredPos.y = Mathf.Clamp(desiredPos.y, target.sizeDelta.y * uiCanvas.scaleFactor, uiCanvas.pixelRect.height);

            target.position = desiredPos;
        }

        private void ResetObjects()
        {
            if (!useExistingCanvas && uiCanvas == null)
            {
                CreateUICanvas();
            }

            if (tooltipPrefab != null && tooltip == null)
            {
                tooltip = Instantiate(tooltipPrefab, uiCanvas.transform);
                tooltip.gameObject.name = "_TOOLTIP_";
                tooltip.gameObject.SetActive(false);
                tooltip.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }

            if(customTooltip != null)
            {
                Destroy(customTooltip.gameObject);
            }

            if (activateFirstTabStop) ActivateFirstTab();

            if(inputManager != null)
            {
                inputManager.Initialize();
            }
        }

        private bool TargetActive()
        {
            if (customTooltip != null) return customTooltip.gameObject.activeSelf;
            return tooltip.gameObject.activeSelf;
        }

        private IEnumerator WaitToDisplay(Canvas canvas, float modDelay)
        {
            bool ignore = false;
            float elapsed = 0;
            while(elapsed < displayDelay + modDelay)
            {
                yield return new WaitForEndOfFrame();
                elapsed += Time.deltaTime;
                if(lastTip == null)
                {
                    ignore = true;
                    break;
                }
            }

            if (!ignore && lastTip != null)
            {
                TooltipDisplay target = customTooltip == null ? tooltip : customTooltip;
                lastTip.onPostInit?.Invoke(target);

                lastTip.onPreDisplay?.Invoke();
                target.ShowTip(lastTip.tipText);
                
                Vector3 desiredPos = lastPos + tipOffset;
                desiredPos.x = Mathf.Clamp(desiredPos.x, 0, canvas.pixelRect.width - target.RectTransform.sizeDelta.x * canvas.scaleFactor);
                desiredPos.y = Mathf.Clamp(desiredPos.y, target.RectTransform.sizeDelta.y * canvas.scaleFactor, canvas.pixelRect.height);

                target.RectTransform.position = desiredPos;
                target.gameObject.SetActive(true);

                lastTip.onDisplay?.Invoke();
            }
        }

        #endregion

    }
}