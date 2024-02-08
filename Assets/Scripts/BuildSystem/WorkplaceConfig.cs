using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorkplaceConfig", menuName = "Configs/WorkplaceConfig")]
public class WorkplaceConfig : ScriptableObject
{
    [SerializeField]
    public GameObject WorkplacePrefab;
    [SerializeField]
    public GameObject WorkplacePrototype;
}
