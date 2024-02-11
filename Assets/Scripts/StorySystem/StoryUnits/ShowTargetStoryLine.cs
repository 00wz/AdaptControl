using System;
using UnityEngine;

public class ShowTargetStoryLine : IStoryLine
{
    [SerializeField]
    private Transform Target;
    [SerializeField]
    private float ShowTime = 1f;
    [SerializeField]
    private bool Refundable = false;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        GameRoot.Instance.CameraMove.ShowTarget(Target, OnEndStoryLine, ShowTime, Refundable);
    }
}
