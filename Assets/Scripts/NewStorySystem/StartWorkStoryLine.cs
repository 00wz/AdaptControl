using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem.Editor
{
    public class StartWorkStoryLine : StoryLine
    {
        [SerializeField]
        private Workplace Workplace;

        protected override void ReceiveExecute()
        {
            Workplace.OnStartWork += OnStartWork;
        }

        private void OnStartWork()
        {
            Workplace.OnStartWork -= OnStartWork;
            FinishExecute();
        }
    }

}