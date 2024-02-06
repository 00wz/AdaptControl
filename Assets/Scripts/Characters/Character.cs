using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour
{
    [SerializeField]
    private Collider InteractiveTrigger;

    private NavMeshAgent _navMeshAgent;
    private Action _onReachDestinationEvent;
    private GameObject _destination;
    private CompositeDisposable _navigationSubscriptions = new CompositeDisposable();

    protected virtual void Awake()
    {
        //_navMeshAgent = GetComponent<NavMeshAgent>();
        InteractiveTrigger.OnTriggerStayAsObservable().Subscribe(ReachDestinationCheck)
            .AddTo(_navigationSubscriptions);///
    }

    public void GoToStationary(GameObject destination, Action OnReachDestination)
    {
        StopNavigation();
        _onReachDestinationEvent += OnReachDestination;
        _destination = destination;
        _navMeshAgent.SetDestination(destination.transform.position);
        InteractiveTrigger.OnTriggerStayAsObservable().Subscribe(ReachDestinationCheck)
            .AddTo(_navigationSubscriptions);
    }

    public void GoToDynamic(GameObject destination, Action OnReachDestination)
    {
        GoToStationary(destination, OnReachDestination);
        Observable.EveryUpdate().Subscribe(_ => UpdateDestination())
            .AddTo(_navigationSubscriptions);
    }

    private void UpdateDestination()
    {
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
        //_navMeshAgent.ResetPath
        _navMeshAgent.isStopped = true;
        _onReachDestinationEvent = null;
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
