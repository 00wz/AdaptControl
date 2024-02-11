using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitBuildConstructionSiteStoryLine : IStoryLine
{
    [SerializeField]
    private BuildingConfig BuildingConfig;

    public event Action OnEndStoryLine;

    public void BeginStoryLine()
    {
        GameRoot.Instance.BuildSystem.OnBuildConstructionSite += CheckBuilding;
    }

    private void CheckBuilding(BuildingConfig buildingConfig)
    {
        if(buildingConfig == BuildingConfig)
        {
            GameRoot.Instance.BuildSystem.OnBuildConstructionSite -= CheckBuilding;
            OnEndStoryLine.Invoke();
        }
    }
}
