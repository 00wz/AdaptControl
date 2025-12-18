using StoryTool.Editor;
using StoryTool.Editor.BuiltInTasks;
using UnityEditor;

namespace NewStorySystem.Editor
{
    [StoryTaskNodeDrawer(typeof(ShowDialogStoryLine), typeof(ShowDialogStoryPoint))]
    public class ShowDialogStoryLineNode : StoryLineNode
    {
        public ShowDialogStoryLineNode(SerializedProperty taskProperty) : base(taskProperty)
        {
        }

        protected override void BuildContent()
        {
            base.BuildContent();
            style.width = 250f;
        }
    }
}