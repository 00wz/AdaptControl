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

    private NavMeshAgent _navMeshAgent;
    private Action _onReachDestinationEvent;
    private Action _onMissingDestinationEvent;
    private GameObject _destination;
    private CompositeDisposable _navigationSubscriptions = new CompositeDisposable();

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;
        if (GroundedCheck == null)
        {
            GroundedCheck = GetComponentInChildren<GroundedCheck>();
        }
    }

    private IEnumerator GoTo(GameObject destination, Action OnReachDestination,
        Action OnEveryFrameEvent, Action OnMissingDestination)
    {
        //waiting for landing
        yield return new WaitUntil(() => GroundedCheck.Grounded);

        //clear old subscriptions if navigation is active
        if (_navMeshAgent.enabled)
        {
            _onReachDestinationEvent = null;
            _onMissingDestinationEvent = null;
            _navigationSubscriptions.Clear();
        }
        else
        {
            _navMeshAgent.enabled = true;
        }

        //recording callbacks and starting navigation
        _onReachDestinationEvent += OnReachDestination;
        _destination = destination;
        _navMeshAgent.SetDestination(destination.transform.position);
        InteractiveTrigger.OnTriggerStayAsObservable().Subscribe(ReachDestinationCheck)
            .AddTo(_navigationSubscriptions);
        Observable.EveryUpdate().Subscribe(_ => OnEveryFrameEvent.Invoke())
            .AddTo(_navigationSubscriptions);
        _onMissingDestinationEvent += OnMissingDestination;
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
        StopAllCoroutines();
        StartCoroutine(GoTo(destination, OnReachDestination, DestinationValidityCheck,
            OnMissingDestination));
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
        StopAllCoroutines();
        StartCoroutine(GoTo(destination, OnReachDestination, DestinationValidityCheckAndUpdate,
            OnMissingDestination));
    }

    private void DestinationValidityCheck()
    {
        if(_destination==null)
        {
            _onMissingDestinationEvent?.Invoke();
            StopNavigation();
        }
    }

    private void DestinationValidityCheckAndUpdate()
    {
        if (_destination == null)
        {
            _onMissingDestinationEvent?.Invoke();
            StopNavigation();
            return;
        }
        _navMeshAgent.SetDestination(_destination.transform.position);
    }

    private void ReachDestinationCheck(Collider other)
    {
        if(other.gameObject != _destination)
        {
            return;
        }
        ReachDestination();
    }
    
    /// <summary>
    /// Stop mooving
    /// </summary>
    public void StopNavigation()
    {
        _navMeshAgent.enabled = false;
        _onReachDestinationEvent = null;
        _onMissingDestinationEvent = null;
        _destination = null;
        _navigationSubscriptions.Clear();
    }

    private void ReachDestination()
    {
        _onReachDestinationEvent?.Invoke();
        StopNavigation();
    }

    private void OnDestroy()
    {
        StopNavigation();
    }
}
