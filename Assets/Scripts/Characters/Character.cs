using System;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private Collider InteractiveTrigger;
    [SerializeField]
    private GroundedCheck GroundedCheck;
    [Tooltip("Turning speed when reaching the destination point")]
    [SerializeField]
    private float RotateSpeed = 360f;

    private NavMeshAgent _navMeshAgent;
    private Action _onReachDestinationEvent;
    private Action _onMissingDestinationEvent;
    private GameObject _destination;
    private CompositeDisposable _checkTargetSubscriptions = new CompositeDisposable();
    private CompositeDisposable _interactiveTriggerSubscription = new CompositeDisposable();
    private CompositeDisposable _groundingSubscription = new CompositeDisposable();
    //use the closest point, for correct movement to large objects
    private bool _useClosestPointToNavigate;
    private const float ROTATION_ERROR = 1f;

    public bool IsGoing { get; private set; } = false;

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;
        InteractiveTrigger.isTrigger = true;
        if (GroundedCheck == null)
        {
            GroundedCheck = GetComponentInChildren<GroundedCheck>();
        }
    }

    /// <summary>
    /// Forces the character to move toward a stationary target
    /// </summary>
    /// <param name="destination">Terget</param>
    /// <param name="OnReachDestination">Callback to reach the destination</param>
    /// <param name="OnMissingDestination">Destination loss callback</param>
    public void GoToStationary(GameObject destination, Action OnReachDestination,
        Action OnMissingDestination)
    {
        _useClosestPointToNavigate = true;
        GoTo(destination, OnReachDestination, DestinationValidityCheck, OnMissingDestination);
    }

    /// <summary>
    /// Forces the character to move toward a moving target
    /// </summary>
    /// <param name="destination">Target</param>
    /// <param name="OnReachDestination">Callback to reach the destination</param>
    /// <param name="OnMissingDestination">Destination loss callback</param>
    public void GoToDynamic(GameObject destination, Action OnReachDestination,
        Action OnMissingDestination)
    {
        _useClosestPointToNavigate = false;
        GoTo(destination, OnReachDestination, DestinationValidityCheckAndUpdate, OnMissingDestination);
    }

    private void GoTo(GameObject destination, Action OnReachDestination,
        Action OnEveryFrameEvent, Action OnMissingDestination)
    {
        if(IsGoing)
        {
            CancelGoing();
        }
        IsGoing = true;

        SetDestinationAfterLanding(destination, OnReachDestination, OnEveryFrameEvent, OnMissingDestination);
    }

    private void SetDestinationAfterLanding(GameObject destination, Action OnReachDestination,
        Action OnEveryFrameEvent, Action OnMissingDestination)
    {
        //grounding check every frame
        Observable.EveryUpdate().Subscribe(_ => SetDestinationIfGrounded(destination,
            OnReachDestination, OnEveryFrameEvent, OnMissingDestination))
        .AddTo(_groundingSubscription);
    }

    void SetDestinationIfGrounded(GameObject destination, Action OnReachDestination,
    Action OnEveryFrameEvent, Action OnMissingDestination)
    {
        if (GroundedCheck.Grounded)
        {
            _groundingSubscription.Clear();
            SetDestination(destination, OnReachDestination, OnEveryFrameEvent, OnMissingDestination);
        }
    }

    private void SetDestination(GameObject destination, Action OnReachDestination,
        Action OnEveryFrameEvent, Action OnMissingDestination)
    {
        if (!_navMeshAgent.enabled)
        {
            _navMeshAgent.enabled = true;
        }

        //recording callbacks and starting navigation
        _onReachDestinationEvent += OnReachDestination;
        _destination = destination;
        _navMeshAgent.SetDestination(
            _useClosestPointToNavigate?
            ClosestPoint(destination):
            destination.transform.position
            );
        Observable.EveryUpdate().Subscribe(_ => OnEveryFrameEvent.Invoke())
            .AddTo(_checkTargetSubscriptions);
        _onMissingDestinationEvent += OnMissingDestination;
        //the destination point is reached only when the InteractiveTrigger overlaps with the target
        InteractiveTrigger.OnTriggerStayAsObservable().Subscribe(ReachDestinationCheck)
            .AddTo(_interactiveTriggerSubscription);
    }

    private Vector3 ClosestPoint(GameObject target)
    {
        if(target.TryGetComponent<Collider>(out Collider coll))
        {
            return coll.ClosestPoint(transform.position);
        }
        return target.transform.position;
    }

    private void DestinationValidityCheck()
    {
        if(_destination==null)
        {
            _onMissingDestinationEvent?.Invoke();
            StopGoing();
        }
    }

    private void DestinationValidityCheckAndUpdate()
    {
        if (_destination == null)
        {
            _onMissingDestinationEvent?.Invoke();
            StopGoing();
            return;
        }
        _navMeshAgent.SetDestination(
            _useClosestPointToNavigate ?
            ClosestPoint(_destination) :
            _destination.transform.position
            );
    }

    private void ReachDestinationCheck(Collider other)
    {
            Debug.Log(other.name);
        if(other.gameObject.transform.IsChildOf(_destination.transform))
        {
            RotateToDestination(other.transform.position);
        }
    }

    ///navMeshAgent rotates only when moving.
    ///if the destination is close, you need to manually rotate it.
    private void RotateToDestination(Vector3 destinationPosition)
    {
        var direction = destinationPosition - transform.position;
        float targetRotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var delta = Mathf.DeltaAngle(transform.eulerAngles.y, targetRotation);
        if(Mathf.Abs(delta) > ROTATION_ERROR)
        {
            float newRotationY = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation,
                RotateSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0.0f, newRotationY, 0.0f);
        }
        else
        {
            ReachDestination();
        }
    }

    private void ReachDestination()
    {
        _onReachDestinationEvent?.Invoke();
        StopGoing();
    }
    
    /// <summary>
    /// Stop mooving
    /// </summary>
    public void StopGoing()
    {
        CancelGoing();
        _navMeshAgent.enabled = false;
        _destination = null;
        IsGoing = false;
    }

    //cancel the going, clears listeners and subscriptions without disabling navigation
    //more productive than StopGoing() to refresh the target
    private void CancelGoing()
    {
        _onReachDestinationEvent = null;
        _onMissingDestinationEvent = null;
        _groundingSubscription.Clear();
        _checkTargetSubscriptions.Clear();
        _interactiveTriggerSubscription.Clear();
    }

    protected virtual void OnDestroy()
    {
        CancelGoing();
    }
}
