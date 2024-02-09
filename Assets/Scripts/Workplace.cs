using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workplace : MonoBehaviour
{
    [SerializeField]
    public GameObject WorkerWorkStateView;

    public virtual bool CanAcceptAnWorker { get; protected set; } = true;

    public virtual void StartWork()
    {
    }

    public virtual void EndWork()
    {
    }
}
