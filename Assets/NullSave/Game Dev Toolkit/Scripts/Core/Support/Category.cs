using UnityEngine;

namespace NullSave.GDTK
{
    [CreateAssetMenu(menuName = "Tools/GDTK/Generic/Category")]
    public class Category : ScriptableObject
    {

        #region Fields

        [Tooltip("Sprite to display in UI")] public Sprite sprite;
        [Tooltip("ID used to find/use category")] public string id;
        [Tooltip("Title to display in UI")] public string title;
        [Tooltip("Description to display in UI"), TextArea(2, 5)] public string description;
        [Tooltip("Should this be hidden from the UI?")] public bool hideInUI;
        [Tooltip("Is there a parent for this category?")] public Category parentCategory;

        #endregion

    }
}