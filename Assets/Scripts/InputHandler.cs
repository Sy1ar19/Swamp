using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private static InputHandler _instance;
    private PlayerInputController _controller;

    public static InputHandler Instance => _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        _controller = new PlayerInputController();
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        _controller.Enable();
    }

    private void OnDisable()
    {
        _controller.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return _controller.Player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return _controller.Player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumped()
    {
        return _controller.Player.Jump.triggered; 
    }
}

