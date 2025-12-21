using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem
{
    public class StartWorkAnyStoryLine : StoryLine
    {
        [SerializeField]
        private Workplace[] Workplaces;

        protected override void ReceiveExecute()
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
            FinishExecute();
        }
    }
}