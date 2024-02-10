using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class WaitStoryLine : IStoryLine
{
    [SerializeField]
    private float WaitingTime;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        Observable.Timer(System.TimeSpan.FromSeconds(3))
        .Subscribe(_ => OnEndStoryLine?.Invoke());
    }
}
