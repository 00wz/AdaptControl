using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [SerializeField]
    public GameObject WorkerWorkStateView;

    public virtual bool CanAcceptAnWorker { get; protected set; } = true;
    public event Action OnStartWork;

    public virtual void StartWork()
    {
        OnStartWork?.Invoke();
    }

    public virtual void EndWork()
    {
    }
}
