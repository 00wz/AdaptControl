using System;

public class WorkerDraggedState : BaseState<Worker>
{
    /// <summary>
    /// A plug. The worker does nothing while dragging.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="changeStateCallback"></param>
    public WorkerDraggedState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }
}
