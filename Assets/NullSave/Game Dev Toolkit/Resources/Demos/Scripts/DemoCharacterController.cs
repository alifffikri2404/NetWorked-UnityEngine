using UnityEngine;

namespace NullSave.GDTK.Stats.Demos
{
    public class DemoCharacterController : MonoBehaviour
    {

        #region Fields

        [Header("Input")]
        public string inputHorizontal = "Horizontal";
        public string inputVertical = "Vertical";
        public string inputSprint = "Fire3";
        public bool inputLocked;
        public bool cameraTranslation = true;

        [Header("Speeds")]
        public float standingRotationSpeed = 360;
        public float movingRotationSpeed = 360;
        public float moveSpeedMultiplier = 1;
        public float speed = 1;
        public float sprintSpeed = 2;
        public float sprintCost = 0.5f;
        public float stamina = 100;

        [Header("Animator Settings")]
        public string movement = "MoveSpeed";

        private bool wantsSprint;

        #endregion

        #region Properties

        public Animator Animator { get; private set; }

        public CharacterController CharacterController { get; private set; }

        public bool InAction { get; set; }

        public bool InputLocked
        {
            get { return inputLocked; }
            set
            {
                inputLocked = value;
            }
        }

        public bool IsSprinting { get; private set; }

        public Vector3 Movement { get; set; }

        public bool PreventMovement { get; set; }

        public Camera ViewCamera { get; private set; }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
            CharacterController = GetComponent<CharacterController>();
            ViewCamera = Camera.main;
        }

        private void Update()
        {
            UpdateInput();
            Move();
        }

        #endregion

        #region Private Methods

        private void Move()
        {
            if (!enabled || Movement.magnitude < 0.1f)
            {
                Animator.SetFloat(movement, 0, 0.1f, Time.deltaTime);
                CharacterController.Move(Physics.gravity * Time.deltaTime);
                return;
            }

            Vector3 move = Movement;
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);

            Vector3 groundNormal = Vector3.up;
            if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out RaycastHit hitInfo, 0.3f))
            {
                groundNormal = hitInfo.normal;
            }

            // Apply rotation
            if (!InAction && Movement.magnitude > 0)
            {
                float turnAmount = Mathf.Atan2(move.x, move.z);
                float turnSpeed = Mathf.Lerp(standingRotationSpeed, movingRotationSpeed, move.z);
                transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
            }

            if (PreventMovement || InAction)
            {
                Animator.SetFloat(movement, 0, 0.1f, Time.deltaTime);
                return;
            }

            move = Vector3.ProjectOnPlane(move, groundNormal);

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

            // Update Animator
            Animator.SetFloat(movement, IsSprinting ? move.z * sprintSpeed : move.z, 0.1f, Time.deltaTime);

            if (!Animator.applyRootMotion)
            {
                Vector3 moveDir = transform.forward * (useSpeed * moveSpeedMultiplier) + Physics.gravity;
                CharacterController.Move(moveDir * Time.deltaTime);
            }
            else
            {
                CharacterController.Move(Physics.gravity * Time.deltaTime);
            }
        }

        private void UpdateInput()
        {
            if(inputLocked)
            {
                Movement = Vector3.zero;
                return;
            }

            float v = InterfaceManager.Input.GetAxis(inputVertical);
            float h = InterfaceManager.Input.GetAxis(inputHorizontal);

            // Normalize input
            if (cameraTranslation && ViewCamera != null)
            {
                Movement = v * Vector3.Scale(ViewCamera.transform.forward, new Vector3(1, 0, 1)).normalized + h * ViewCamera.transform.right;
            }
            else
            {
                Movement = v * Vector3.forward + h * Vector3.right;
            }
        }

        #endregion

    }
}