using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class House : MonoBehaviour
{
    [Tooltip("Simulates leaving the house, then is replaced by an instance of the CharacterPrefab")]
    [SerializeField]
    private Character CharacterPreview;
    [Tooltip("Where the CharacterPreview is going")]
    [SerializeField]
    private Collider Destination;
    [SerializeField]
    private Character CharacterPrefab;

    private void Awake()
    {
        //for the CharacterPreview to simulate leaving the house if an NavMeshObstacle is enabled
        if (TryGetComponent<NavMeshObstacle>(out NavMeshObstacle navMeshObstacle))
        {
            navMeshObstacle.enabled = false;
        }
    }

    void Start()
    {
        CharacterPreview.GoToStationary(Destination.gameObject, OnReachDestination, () => { });
    }

    private void OnReachDestination()
    {
        Vector3 position = CharacterPreview.transform.position;
        Quaternion rotation = CharacterPreview.transform.rotation;
        Destroy(CharacterPreview);
        Instantiate(CharacterPrefab, position, rotation);
        if (TryGetComponent<NavMeshObstacle>(out NavMeshObstacle navMeshObstacle))
        {
            navMeshObstacle.enabled = true;
        }
    }
}
