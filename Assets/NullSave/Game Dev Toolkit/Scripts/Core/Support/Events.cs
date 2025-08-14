using System;
using UnityEngine;
using UnityEngine.Events;

namespace NullSave.GDTK
{

    [Serializable]
    public class GameObjectEvent : UnityEvent<GameObject> { }

    [Serializable]
    public class LanguageChanged : UnityEvent<string> { }

    [Serializable]
    public class SelectedIndexChanged : UnityEvent<int> { }

    [Serializable]
    public delegate void SimpleEvent();

    [Serializable]
    public delegate void TooltipEvent(TooltipDisplay display);

    [Serializable]
    public class StateChanged : UnityEvent<bool> { }

    [Serializable]
    public class ValueChanged : UnityEvent<float> { }

    [Serializable]
    public class InteractableChanged: UnityEvent<InteractableObject> { }

    [Serializable]
    public delegate void SequenceComplete(int index);

    [Serializable]
    public class MenuItemEvent : UnityEvent<UIMenuItem> { }

    [Serializable]
    public class MenuItemHoverStateChanged : UnityEvent<UIMenuItem, bool> { }

    [Serializable]
    public class MenuItemInteractableChanged : UnityEvent<UIMenuItem, bool> { }

}