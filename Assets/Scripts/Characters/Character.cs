using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private Collider InteractiveTrigger;

    private NavMeshAgent _navMeshAgent;
    private Action _onReachDestinationEvent;
    private Action _onMissingDestinationEvent;
    private GameObject _destination;
    private CompositeDisposable _navigationSubscriptions = new CompositeDisposable();

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.enabled = false;
    }

    private void GoTo(GameObject destination, Action OnReachDestination)
    {
        if(_navMeshAgent.enabled)
        {
            _onReachDestinationEvent = null;
            _onMissingDestinationEvent = null;
            _navigationSubscriptions.Clear();
        }
        else
        {
            _navMeshAgent.enabled = true;
        }

        _onReachDestinationEvent += OnReachDestination;
        _destination = destination;
        _navMeshAgent.SetDestination(destination.transform.position);
        InteractiveTrigger.OnTriggerStayAsObservable().Subscribe(ReachDestinationCheck)
            .AddTo(_navigationSubscriptions);
    }

    public void GoToStationary(GameObject destination, Action OnReachDestination,
        Action OnMissingDestination)
    {
        GoTo(destination, OnReachDestination);
        Observable.EveryUpdate().Subscribe(_ => DestinationValidityCheck())
    .AddTo(_navigationSubscriptions);
        _onMissingDestinationEvent += OnMissingDestination;
    }

    public void GoToDynamic(GameObject destination, Action OnReachDestination,
        Action OnMissingDestination)
    {
        GoTo(destination, OnReachDestination);
        Observable.EveryUpdate().Subscribe(_ => UpdateDestination())
            .AddTo(_navigationSubscriptions);
        _onMissingDestinationEvent += OnMissingDestination;
    }

    private void DestinationValidityCheck()
    {
        if(_destination==null)
        {
            _onMissingDestinationEvent?.Invoke();
            StopNavigation();
        }
    }

    private void UpdateDestination()
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
