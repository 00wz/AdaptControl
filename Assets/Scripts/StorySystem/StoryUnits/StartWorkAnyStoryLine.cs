using System;
using UnityEngine;

public class StartWorkAnyStoryLine : IStoryLine
{
    [SerializeField]
    private Workplace[] Workplaces;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        foreach(var workplace in Workplaces)
        {
            workplace.OnStartWork += OnStartWork;
        }
    }

    private void OnStartWork()
    {
        foreach (var workplace in Workplaces)
        {
            workplace.OnStartWork -= OnStartWork;
        }
        OnEndStoryLine.Invoke();
    }
}
