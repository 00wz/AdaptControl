using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingConfig", menuName = "Configs/WorkplaceConfig")]
public class BuildingConfig : ScriptableObject
{
    [SerializeField]
    public GameObject BuildingPrefab;
    [SerializeField]
    public GameObject BuildingPrototype;
    [SerializeField]
    public ConstructionSiteWorkplace ConstructionSite;
}
