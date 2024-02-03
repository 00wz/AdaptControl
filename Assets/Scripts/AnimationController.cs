using UnityEngine;

[RequireComponent(typeof(Animator),typeof(GroundedCheck))]
public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private float SpeedChangeRate = 10.0f;
    [SerializeField]
    private float MaxRotateSpeed = 360f;
    [SerializeField]
    private AudioClip LandingAudioClip;
    [SerializeField]
    private AudioClip[] FootstepAudioClips;
    [Range(0, 1)] 
    public float FootstepAudioVolume = 0.5f;
    [SerializeField]
    private bool Grounded = true;
    [SerializeField]
    public bool RotateToMovementDirection = true;

    private Animator _animator;
    private GroundedCheck _groundedCheck;

    private Vector3 _lastPosition;
    private Vector3 _instantaneousSpeed;
    private Vector3 _smoothedSpeed;
    private float _rotationVelocity;

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _groundedCheck = GetComponent<GroundedCheck>();
        AssignAnimationIDs();
        _lastPosition = transform.position;
    }

    private void Update()
    {
        UpdateSpeed();
        GroundedCheck();
        Move();
    }

    private void UpdateSpeed()
    {
        var _currentPosition = transform.position;
        _instantaneousSpeed = (_currentPosition - _lastPosition) / Time.deltaTime;
        _lastPosition = _currentPosition;
        _smoothedSpeed = Vector3.Lerp(_smoothedSpeed, _instantaneousSpeed,
                Time.deltaTime * SpeedChangeRate);
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
    }

    private void GroundedCheck()
    {
        bool _currentGrounded = _groundedCheck.Grounded;
        if (Grounded && !_currentGrounded)
        {
            _animator.SetBool(_animIDGrounded, false);
            if (_smoothedSpeed.y > 0f)
                _animator.SetBool(_animIDJump, true);
            else
                _animator.SetBool(_animIDFreeFall, true);
        }
        else if (!Grounded && _currentGrounded)
        {
            _animator.SetBool(_animIDGrounded, true);
            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);
        }
        Grounded = _currentGrounded;
    }

    private void Move()
    {
        float smoothedHorizontalSpeed = new Vector3(_smoothedSpeed.x, 0.0f, _smoothedSpeed.z).magnitude;

        if(RotateToMovementDirection)
        {
            float targetRotation = Mathf.Atan2(_smoothedSpeed.x, _smoothedSpeed.z) * Mathf.Rad2Deg;
            targetRotation = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation, MaxRotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0.0f, targetRotation, 0.0f);
        }

        _animator.SetFloat(_animIDSpeed, smoothedHorizontalSpeed < 0.01f ? 0f : smoothedHorizontalSpeed);
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.position, FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.position, FootstepAudioVolume);
        }
    }

}