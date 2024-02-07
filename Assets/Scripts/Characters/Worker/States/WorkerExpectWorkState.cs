using System;
using UniRx;

public class WorkerExpectWorkState : BaseState<Worker>
{
    private CompositeDisposable _subscriptions = new();

    public WorkerExpectWorkState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }

    public override void Enter()
    {
        Observable.EveryUpdate().Subscribe(_ => Search()).AddTo(_subscriptions);
    }

    private void Search()
    {

    }

    public override void Exit()
    {
        _subscriptions.Clear();
    }
}
