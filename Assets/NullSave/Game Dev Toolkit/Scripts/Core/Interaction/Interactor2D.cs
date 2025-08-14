using UnityEngine;

namespace NullSave.GDTK
{
    public class Interactor2D : Interactor
    {

        #region Fields

        [Tooltip("Direction to send raycast")] public Character2DDirection castingDirection;

        #endregion

        #region Properties

        #endregion

        #region Unity Methods

        public override void OnDrawGizmos()
        {
            Vector3 start = transform.position + emissionOffset;
            Gizmos.color = new Color(0.1876112f, 0.7025714f, 0.8773585f);
            Gizmos.DrawLine(start, start + (GetCastDirection() * maxDistance));
        }

        public override void Update()
        {
            if (InterfaceManager.PreventInteractions)
            {
                if (Target != null) Target = null;
                return;
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position + emissionOffset, GetCastDirection(), maxDistance, interactionLayer);
            if (hit.collider != null)
            {
                InteractableObject checkHit = hit.transform.gameObject.GetComponent<InteractableObject>();
                if (checkHit == null)
                {
                    InteractableChild child = hit.transform.gameObject.GetComponent<InteractableChild>();
                    if (child != null)
                    {
                        Target = child.parentInteractable;
                    }
                    else
                    {
                        Target = null;
                    }
                }
                else
                {
                    Target = checkHit;
                }
            }
            else
            {
                Target = null;
            }

            if (Target != null)
            {
                CheckInteractionInput();
            }
        }

        #endregion

        #region Private Methods

        private Vector3 GetCastDirection()
        {
            return castingDirection switch
            {
                Character2DDirection.Down => Vector3.down,
                Character2DDirection.Left => Vector3.left,
                Character2DDirection.Right => Vector3.right,
                _ => Vector3.up,
            };
        }

        #endregion

    }
}