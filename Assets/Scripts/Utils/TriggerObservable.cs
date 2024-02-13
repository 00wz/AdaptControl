using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UniRx .OnTriggerStayAsObservable() works very strangely, so use this script.
/// </summary>
public class TriggerObservable : MonoBehaviour
{
    public event Action<Collider> OnTriggerStayEvent;

    private void OnTriggerStay(Collider other)
    {
        OnTriggerStayEvent?.Invoke(other);
    }
}
