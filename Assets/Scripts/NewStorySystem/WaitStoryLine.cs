using StoryTool.BuiltInTasks;
using UniRx;
using UnityEngine;

namespace NewStorySystem.Editor
{
    public class WaitStoryLine : StoryLine
    {
        [SerializeField]
        private float WaitingTime;

        protected override void ReceiveExecute()
        {
            Observable.Timer(System.TimeSpan.FromSeconds(WaitingTime))
            .Subscribe(_ => FinishExecute());
        }
    }
}