using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuildStoryPoint : IStoryPoint
{
    [SerializeField]
    private BuildingConfig Building;

    public void BeginStoryPoint()
    {
        GameRoot.Instance.BuildSystem.AddBuild(Building);
    }
}
