using System;

public class WorkerGoingToWorkplaceState : BaseState<Worker>
{
    public WorkerGoingToWorkplaceState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }

    public override void Enter()
    {
        context.GoToStationary(context.CurrentWorkplace.gameObject,
            () => ChangeState<WorkerWorkState>(),
            () => ChangeState<WorkerSearchState>());
    }

    public override void Exit()
    {
        context.StopGoing();
    }
}
