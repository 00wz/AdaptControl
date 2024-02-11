using System;
using UnityEngine;

public class StartWorkStoryLine : IStoryLine
{
    [SerializeField]
    private Workplace Workplace;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        Workplace.OnStartWork += OnStartWork;
    }

    private void OnStartWork()
    {
        Workplace.OnStartWork -= OnStartWork;
        OnEndStoryLine.Invoke();
    }
}
