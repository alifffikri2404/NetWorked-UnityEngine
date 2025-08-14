using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerButtonMove : MonoBehaviour
{
    // [SerializeField] private Button leftButton;
    // [SerializeField] private Button rightButton;
    [SerializeField] private Button jumpButton;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        jumpButton.onClick.AddListener(Jump);
    }

    private void Jump()
    {
        playerMovement.Jump();
    }
}
