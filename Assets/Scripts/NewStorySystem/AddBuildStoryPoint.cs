using System;
using System.Collections.Generic;
using StoryTool.BuiltInTasks;
using UnityEngine;

namespace NewStorySystem
{
    public class AddBuildStoryPoint : StoryPoint
    {
        [Serializable]
        public class Neasted
        {
            //public float[] someValue;
            public float someValue;
        }

        [SerializeField]
        private BuildingConfig Building;

        [SerializeField]
        private Neasted neasted;

        protected override void ReceiveExecute()
        {
            GameRoot.Instance.BuildSystem.AddBuild(Building);
        }
    }
}