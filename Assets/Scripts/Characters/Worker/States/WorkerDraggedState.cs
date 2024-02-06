using System;

public class WorkerDraggedState : BaseState<Worker>
{
    public WorkerDraggedState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }

    public override void Enter()
    {
        context.StopNavigation();
    }
}
