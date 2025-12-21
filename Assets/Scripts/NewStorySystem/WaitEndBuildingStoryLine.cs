using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem
{
    public class WaitEndBuildingStoryLine : StoryLine
    {
        [SerializeField]
        private BuildingConfig BuildingConfig;

        protected override void ReceiveExecute()
        {
            ConstructionSiteWorkplace.OnCompletBuild += CheckBuild;
        }

        private void CheckBuild(GameObject build, BuildingConfig buildingConfig)
        {
            if(buildingConfig == BuildingConfig)
            {
                ConstructionSiteWorkplace.OnCompletBuild -= CheckBuild;
                FinishExecute();
            }
        }
    }
}