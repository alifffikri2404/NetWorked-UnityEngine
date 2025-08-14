using UnityEngine;

namespace NullSave.GDTK
{
    public class InteractableSign : InteractableObject
    {

        #region Fields

        public string windowId;
        public Sprite sprite;
        public string title;
        [TextArea(2,5)] public string dialogue;

        #endregion

        #region Public Methods

        public override bool Interact(Interactor source)
        {
            if (!IsInteractable) return false;

            Broadcaster.Broadcast(audioPoolChannel, "Play", new object[] { actionSound, transform.position });
            onInteract?.Invoke();

            //!!InterfaceManager.ShowDialogue(windowId, sprite, title, dialogue);

            return true;
        }
        #endregion

    }
}