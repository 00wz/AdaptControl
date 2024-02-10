using System;

public interface IStoryLine : IStoryUnit
{
    public void BeginStoryLine();
    public event Action OnEndStoryLine;
}
