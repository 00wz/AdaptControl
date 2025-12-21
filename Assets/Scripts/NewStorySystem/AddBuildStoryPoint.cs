using System;
using System.Collections.Generic;
using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem
{
    public class AddBuildStoryPoint : StoryPoint
    {
        [SerializeField]
        private BuildingConfig Building;

        protected override void ReceiveExecute()
        {
            GameRoot.Instance.BuildSystem.AddBuild(Building);
        }
    }
}