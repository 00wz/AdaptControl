using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerExpectWorkState : BaseState<Worker>
{
    public WorkerExpectWorkState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }
}
