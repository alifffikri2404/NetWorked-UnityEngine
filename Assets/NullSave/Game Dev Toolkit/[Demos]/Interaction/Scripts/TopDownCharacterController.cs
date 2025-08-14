using UnityEngine;

namespace NullSave.GDTK.InteractionDemo
{
    [RequireComponent(typeof(Interactor2D))]
    public class TopDownCharacterController : MonoBehaviour
    {

        #region Fields

        public float speed;

        public string inputSprint = "Fire3";
        public float sprintSpeed = 2;
        public float sprintCost = 0.5f;
        public float stamina = 100;

        private Animator animator;
        private Interactor2D interactor;

        private bool wantsSprint;

        #endregion

        #region Properties

        public bool IsSprinting { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            animator = GetComponent<Animator>();
            interactor = GetComponent<Interactor2D>();
        }

        private void Reset()
        {
            speed = 3;
        }

        private void Update()
        {
            if(InterfaceManager.LockPlayerController)
            {
                return;
            }

            float useSpeed = speed;
            if (!IsSprinting)
            {
                if (InterfaceManager.Input.GetButtonDown(inputSprint) || wantsSprint)
                {
                    if (stamina >= sprintCost)
                    {
                        IsSprinting = true;
                        useSpeed *= sprintSpeed;
                        stamina -= sprintCost * Time.deltaTime;
                        wantsSprint = false;
                    }
                    else
                    {
                        wantsSprint = true;
                    }
                }
            }
            else
            {
                if (!InterfaceManager.Input.GetButton(inputSprint) || stamina == 0)
                {
                    IsSprinting = false;
                }
                else
                {
                    useSpeed *= sprintSpeed;
                    stamina -= sprintCost * Time.deltaTime;
                }
            }


            Vector2 dir = Vector2.zero;
            if (InterfaceManager.Input.GetKey(KeyCode.A))
            {
                dir.x = -1;
                animator.SetInteger("Direction", 3);
                interactor.castingDirection = Character2DDirection.Left;
            }
            else if (InterfaceManager.Input.GetKey(KeyCode.D))
            {
                dir.x = 1;
                animator.SetInteger("Direction", 2);
                interactor.castingDirection = Character2DDirection.Right;
            }

            if (InterfaceManager.Input.GetKey(KeyCode.W))
            {
                dir.y = 1;
                animator.SetInteger("Direction", 1);
                interactor.castingDirection = Character2DDirection.Up;
            }
            else if (InterfaceManager.Input.GetKey(KeyCode.S))
            {
                dir.y = -1;
                animator.SetInteger("Direction", 0);
                interactor.castingDirection = Character2DDirection.Down;
            }

            dir.Normalize();
            animator.SetBool("IsMoving", dir.magnitude > 0);

            GetComponent<Rigidbody2D>().velocity = useSpeed * dir;
        }

        #endregion

    }
}