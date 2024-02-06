using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Character, IDragHandlable
{
    private StateMachine<Worker, WorkerExpectWorkState> StateMachine;

    void Start()
    {
        StateMachine = new StateMachine<Worker, WorkerExpectWorkState>(this);
    }

    public void OnDragBegin()
    {
        StateMachine.ChangeState<WorkerDraggedState>();
    }

    public void OnDragEnd()
    {
        StateMachine.ChangeState<WorkerExpectWorkState>();
    }

}
