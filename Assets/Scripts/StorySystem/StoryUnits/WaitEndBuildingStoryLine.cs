using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitEndBuildingStoryLine : IStoryLine
{
    [SerializeField]
    private BuildingConfig BuildingConfig;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        ConstructionSiteWorkplace.OnCompletBuild += CheckBuild;
    }

    private void CheckBuild(GameObject build, BuildingConfig buildingConfig)
    {
        if(buildingConfig == BuildingConfig)
        {
            ConstructionSiteWorkplace.OnCompletBuild -= CheckBuild;
            OnEndStoryLine.Invoke();
        }
    }
}
