using StoryTool.BuiltInTasks;
using StoryTool.Runtime;
using UnityEngine;

namespace NewStorySystem
{
    [StoryTaskMenu("Dialodue/ShowDialogStoryPoint")]
    public class ShowDialogStoryPoint : StoryPoint
    {
        [SerializeField]
        private string DialogHeader;
        [SerializeField]
        [TextArea(3, 5)]
        private string DialogBody;

        protected override void ReceiveExecute()
        {
            GameRoot.Instance.DialogView.ShowDialog(DialogHeader, DialogBody);
        }
    }
}
