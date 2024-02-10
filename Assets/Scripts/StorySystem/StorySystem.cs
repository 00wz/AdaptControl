using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TNRD;
using System;

public class StorySystem : MonoBehaviour
{
    [SerializeField]
    private List<SerializableInterface<IStoryUnit>> StoryUnits;

    private IEnumerator<SerializableInterface<IStoryUnit>> _storyUnitsEnumerator;

    private void Start()
    {
        _storyUnitsEnumerator = StoryUnits.GetEnumerator();
        BeginNextStoryUnit();
    }

    private void BeginNextStoryUnit()
    {
        if(_storyUnitsEnumerator.MoveNext())
        {
            //Debug.Log()
            var storyUnit = _storyUnitsEnumerator.Current.Value;
            switch (storyUnit)
            {
                case IStoryPoint storyPoint:
                    storyPoint.BeginStoryPoint();
                    BeginNextStoryUnit();
                    break;
                case IStoryLine storyLine:
                    Action onEndStoryLine = null;
                    onEndStoryLine =() => 
                    { 
                        storyLine.OnEndStoryLine -= onEndStoryLine;
                        BeginNextStoryUnit();
                    };
                    storyLine.OnEndStoryLine += onEndStoryLine;
                    storyLine.BeginStoryLine();
                    break;
                case null:
                    BeginNextStoryUnit();
                    break;
                default:
                    throw new Exception($"{ nameof(storyUnit) } " +
                        $"does not implement IStoryLine or IStoryPoint interfaces");
            }
        }
    }
}
