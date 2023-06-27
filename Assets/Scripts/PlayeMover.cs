using InfimaGames.LowPolyShooterPack;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayeMover : MonoBehaviour
{
    [SerializeField] private float _speedWalking = 5.0f;
    [SerializeField] private float _speedRunning = 9.0f;

    private float _maxVelocityChange = 10f;
    public KeyCode _sprintKey = KeyCode.LeftShift;
    public KeyCode _jumpKey = KeyCode.Space;
    public float _jumpPower = 1f;

    private Vector3 Velocity
    {
        //Getter.
        get => _rigidBody.velocity;
        //Setter.
        set => _rigidBody.velocity = value;
    }

    private Rigidbody _rigidBody;
    private CapsuleCollider _capsule;
    private bool _isGrounded;
    private CharacterBehaviour _playerCharacter;
    private WeaponBehaviour _equippedWeapon;
    private readonly RaycastHit[] _groundHits = new RaycastHit[8];

    private void Awake()
    {
        _playerCharacter = ServiceLocator.Current.Get<IGameModeService>().GetPlayerCharacter();
    }

    private void Start()
    {
        //Rigidbody Setup.
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        //Cache the CapsuleCollider.
        _capsule = GetComponent<CapsuleCollider>();

    }

    private void OnCollisionStay()
    {
        //Bounds.
        Bounds bounds = _capsule.bounds;
        //Extents.
        Vector3 extents = bounds.extents;
        //Radius.
        float radius = extents.x - 0.01f;

        //Cast. This checks whether there is indeed ground, or not.
        Physics.SphereCastNonAlloc(bounds.center, radius, Vector3.down,
            _groundHits, extents.y - radius * 0.5f, ~0, QueryTriggerInteraction.Ignore);

        //We can ignore the rest if we don't have any proper hits.
        if (!_groundHits.Any(hit => hit.collider != null && hit.collider != _capsule))
            return;

        //Store RaycastHits.
        for (var i = 0; i < _groundHits.Length; i++)
            _groundHits[i] = new RaycastHit();

        //Set grounded. Now we know for sure that we're grounded.
        _isGrounded = true;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
        _isGrounded = false;
    }

    private void MoveCharacter()
    {
        Vector2 frameInput = _playerCharacter.GetInputMovement();
        Vector3 targetVelocity = new Vector3(frameInput.x, 0.0f, frameInput.y);
        if (Input.GetKey(_sprintKey) && _playerCharacter.IsRunning())
        {
            targetVelocity = transform.TransformDirection(targetVelocity) * _speedRunning;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _rigidBody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;

            _rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
        else
        {
            targetVelocity = transform.TransformDirection(targetVelocity) * _speedWalking;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _rigidBody.velocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;

            _rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);
        }
    }

    public void Jump()
    {
        if (_isGrounded == true)
        {
            _rigidBody.AddForce(0f, _jumpPower * 1.5f, 0f, ForceMode.VelocityChange);
            _isGrounded = false;
        }
    }
}
