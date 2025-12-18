using StoryTool.BuiltInTasks;
using StoryTool.Runtime;
using UnityEngine;

namespace NewStorySystem
{
    public class ShowTargetStoryLine : StoryLine
    {
        [SerializeField]
        private Transform Target;
        [SerializeField]
        private float ShowTime = 1f;
        [SerializeField]
        private bool Refundable = false;

        protected override void ReceiveExecute()
        {
            GameRoot.Instance.CameraMove.ShowTarget(Target, FinishExecute, ShowTime, Refundable);
        }
    }
}