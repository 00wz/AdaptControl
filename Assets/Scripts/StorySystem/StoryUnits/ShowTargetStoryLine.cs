using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTargetStoryLine : IStoryLine
{
    [SerializeField]
    private Transform Target;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        GameRoot.Instance.CameraMove.ShowTarget(Target, OnEndStoryLine);
    }
}
