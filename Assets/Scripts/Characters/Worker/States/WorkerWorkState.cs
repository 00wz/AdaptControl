using System;
using UniRx;

public class WorkerWorkState : BaseState<Worker>
{
    private CompositeDisposable _everyFrameSubscriptions = new();

    public WorkerWorkState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }

    public override void Enter()
    {
        if(context.CurrentWorkplace == null || !context.CurrentWorkplace.CanAcceptAnWorker)
        {
            ChangeState<WorkerSearchState>();
            return;
        }
        context.ShowWorkStateView(context.CurrentWorkplace);
        Observable.EveryUpdate().Subscribe(_ => WorkplaceValidityCheck())
            .AddTo(_everyFrameSubscriptions);
        context.CurrentWorkplace.StartWork();
    }

    private void WorkplaceValidityCheck()
    {
        if(context.CurrentWorkplace == null)
        {
            ChangeState<WorkerSearchState>();
        }
    }

    public override void Exit()
    {
        _everyFrameSubscriptions.Clear();
        if(context.CurrentWorkplace != null)
        {
            context.CurrentWorkplace.EndWork();
        }
        context.ShowMainStateView();
    }
}
