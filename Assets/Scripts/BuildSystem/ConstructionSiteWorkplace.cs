using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionSiteWorkplace : Workplace
{
    private GameObject _buildingPrefab;

    public void Init(GameObject BuildingPrefab)
    {
        _buildingPrefab = BuildingPrefab;
    }
}
