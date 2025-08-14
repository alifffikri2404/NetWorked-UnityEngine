using System;

namespace NullSave.GDTK
{

    public enum AnimatorParamType
    {
        Bool = 0,
        Float = 1,
        Int = 2,
        Trigger = 4
    }

    public enum AnimatorTriggerType
    {
        Set = 0,
        Reset = 1,
    }

    public enum CanvasTargeting
    {
        FirstOrCreate = 0,
        AsAssigned = 1,
        FindByTag = 2
    }

    public enum Character2DDirection
    {
        Down = 0,
        Left = 1,
        Up = 2,
        Right = 3
    }

    public enum DataSource
    {
        Local = 0,
        Resources = 1,
        AssetBundle = 2,
        PersistentData = 3,
    }

    public enum DictionaryLookupMode
    {
        StoreInMemory = 0,
        ReadOnRequest = 1
    }

    public enum DictionarySource
    {
        ResourceFile = 0,
        AssetBundle = 1
    }

    public enum EndOfPathInstruction {
        Loop = 0,
        Reverse = 1,
        Stop = 2
    };

    public enum ImageSource
    {
        None = 0,
        Resources = 1,
        AssetBundle = 2,
        PersistentData = 3,
    }

    [Flags]
    public enum InputFlags
    {
        Click = 1,
        Axis = 2,
        Key = 4,
    }

    [Flags]
    public enum InterfaceStateFlags
    {
        None = 0,
        PreventInteractionUI = 1,
        PreventPrompts = 2,
        PreventWindows = 4,
        AdjustTime = 8,
        LockPlayerController = 16,
    }

    public enum ListLayout
    {
        List = 0,
        Grid = 1,
    }

    public enum MenuItemTransition
    {
        None = 0,
        ColorTint = 1,
        SpriteSwap = 2,
    }

    public enum NavigationType
    {
        Manual = 0,
        ByButton = 1,
        ByKey = 2,
        ByClick = 3
    }

    public enum NavigationTypeSimple
    {
        Manual = 0,
        ByButton = 1,
        ByKey = 2,
    }

    public enum Orientation
    {
        Horizontal = 0,
        Vertical = 1
    }

    public enum PathSpace
    {
        xyz = 0,
        xy = 1,
        xz = 2
    }

    public enum SpawnSource
    {
        Resources = 0,
        AssetBundle = 1,
    }

    public enum TargetingMethod
    {
        ExistingLockOn = 0,
        ClosestTarget = 1,
        ManualList = 2
    }

    public enum TextEncoding
    {
        UTF8 = 0,
        UTF32 = 1
    }

    [Flags]
    public enum UINavigationType
    {
        ByButton = 1,
        ByKey = 2,
        ByClick = 4
    }

    public enum UIScaleMode
    {
        ConstantPixelSize,
        ScaleWithScreenSize,
        ConstantPhysicalSize
    }

    public enum UIScreenMatchMode
    {
        MatchWidthOrHeight = 0,
        Expand = 1,
        Shrink = 2
    }

    public enum UIUnit
    {
        Centimeters,
        Millimeters,
        Inches,
        Points,
        Picas
    }

}