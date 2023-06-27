using StarterAssets;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputHandler _inputHandler;
    private Transform _cameraTransform;
    

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        _inputHandler = InputHandler.Instance;
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = _inputHandler.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = _cameraTransform.forward * move.z + _cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

/*        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }*/

        if (_inputHandler.PlayerJumped() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
