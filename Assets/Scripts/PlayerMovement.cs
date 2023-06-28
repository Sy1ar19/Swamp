using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _playerSpeed;
    [SerializeField] private float _jumpHeight;

    private const float VelocityMultiplier = -2f;

    private CharacterController _controller;
    private Vector3 _playerVelocity;
    private bool _isGrounded;
    private InputHandler _inputHandler;
    private Transform _cameraTransform;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _inputHandler = InputHandler.Instance;
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;

        if (_isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = -2f;
        }

        Vector2 movement = _inputHandler.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = _cameraTransform.forward * move.z + _cameraTransform.right * move.x;
        _controller.Move(move * Time.deltaTime * _playerSpeed);

        if (_inputHandler.PlayerJumped() && _isGrounded)
        {
            _playerVelocity.y += Mathf.Sqrt(_jumpHeight * VelocityMultiplier * Physics.gravity.y);
        }

        _playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        _controller.Move(_playerVelocity * Time.deltaTime);
    }
}
