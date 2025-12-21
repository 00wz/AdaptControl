using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem
{
    public class WaitBuildConstructionSiteStoryLine : StoryLine
    {
        [SerializeField]
        private BuildingConfig BuildingConfig;

        protected override void ReceiveExecute()
        {
            GameRoot.Instance.BuildSystem.OnBuildConstructionSite += CheckBuilding;
        }

        private void CheckBuilding(BuildingConfig buildingConfig)
        {
            if(buildingConfig == BuildingConfig)
            {
                GameRoot.Instance.BuildSystem.OnBuildConstructionSite -= CheckBuilding;
                FinishExecute();
            }
        }
    }
}