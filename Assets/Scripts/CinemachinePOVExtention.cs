using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtention : CinemachineExtension
{
    [SerializeField] private float _clampAngle = 80f;
    [SerializeField] private float _horizontalSpeed = 10f;
    [SerializeField] private float _verticalSpeed = 10f;

    private InputHandler _inputHandler;
    private Vector3 _startingRotation;

    protected void Awake()
    {
        _inputHandler = InputHandler.Instance;
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (_startingRotation == null) _startingRotation = transform.localRotation.eulerAngles;
                Vector2 deltaInput = _inputHandler.GetMouseDelta();
                _startingRotation.x += deltaInput.x * _verticalSpeed * Time.deltaTime;
                _startingRotation.y += deltaInput.y * _horizontalSpeed * Time.deltaTime;
                _startingRotation.y = Mathf.Clamp(_startingRotation.y, -_clampAngle, _clampAngle);
                state.RawOrientation = Quaternion.Euler(-_startingRotation.y, _startingRotation.x, 0f);
            }
        }
    }
}
