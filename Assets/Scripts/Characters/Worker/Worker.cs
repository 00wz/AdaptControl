using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Worker : Character, IDragHandlable
{
    [SerializeField]
    public float SearchRadius = 10f;
    [SerializeField]
    private GameObject WorkerMainStateView;

    [HideInInspector]
    public Workplace CurrentWorkplace;
    private GameObject _workStateView;
    private StateMachine<Worker, WorkerSearchState> _stateMachine;

    private void Start()
    {
        _stateMachine = new StateMachine<Worker, WorkerSearchState>(context: this);
        ShowMainStateView();
    }

    public void ShowWorkStateView(Workplace workplace)
    {
        WorkerMainStateView.SetActive(false);
        _workStateView = Instantiate<GameObject>(workplace.WorkerWorkStateView, transform.position,
            transform.rotation, this.transform);
    }

    public void ShowMainStateView()
    {
        if (_workStateView != null)
        {
            Destroy(_workStateView);
        }
        WorkerMainStateView.SetActive(true);
    }

    public void OnDragBegin()
    {
        _stateMachine.ChangeState<WorkerDraggedState>();
    }

    public void OnDragEnd()
    {
        _stateMachine.ChangeState<WorkerSearchState>();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SearchRadius);
    }

}
