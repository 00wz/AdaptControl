using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class BuildSystem : MonoBehaviour
{
    [SerializeField]
    private LayerMask GroundLayers;
    [SerializeField]
    private Transform WorkplacesRoot;
    [Header("Building prototypes materials")]
    [SerializeField]
    private Material OnBuildingValidMaterial;
    [SerializeField]
    private Material OnBuildingInvalidMaterial;

    [Header("testing")]
    [SerializeField]
    private WorkplaceConfig workplace;
    private void Start()
    {
        StartCoroutine(Build(workplace));
    }

    private IEnumerator Build(WorkplaceConfig workplaceConfig)
    {
        int overlapObstacleCount = 0;
        CompositeDisposable triggerSubscriptions = new();

        //initialize prototype
        GameObject prototype = Instantiate(workplaceConfig.WorkplacePrototype, WorkplacesRoot);
        prototype.SetActive(false);
        Renderer prototypeRenderer = prototype.GetComponent<Renderer>();
        Collider prototypeCollider = prototype.GetComponent<Collider>();
        prototypeRenderer.material = OnBuildingValidMaterial;

        //subscribe to prototype overlap
        prototypeCollider.OnTriggerEnterAsObservable().Subscribe(AddOverlapObstacle)
            .AddTo(triggerSubscriptions);
        prototypeCollider.OnTriggerExitAsObservable().Subscribe(RemoveOverlapObstacle)
            .AddTo(triggerSubscriptions);
        prototype.SetActive(true);

        while(true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, GroundLayers))
            {
                prototype.transform.position = raycastHit.point;
            }
            //build workplace if valid
            if(Input.GetMouseButtonDown(0) && overlapObstacleCount <=0)
            {
                InstantiateWorkplace();
                break;
            }
            //exit
            if(Input.GetMouseButton(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                RemovePrototype();
                break;
            }
            yield return null;
        }

        // when the prototype is blocked by obstacles
        void AddOverlapObstacle(Collider other)
        {
            if (((1 << other.gameObject.layer) & GroundLayers) == 0)
            {
                overlapObstacleCount++;
                prototypeRenderer.material = OnBuildingInvalidMaterial;
            }
        }

        // when the prototype is NOT blocked by obstacles
        void RemoveOverlapObstacle(Collider other)
        {
            if (((1 << other.gameObject.layer) & GroundLayers) == 0)
            {
                overlapObstacleCount--;
                if (overlapObstacleCount <= 0)
                {
                    prototypeRenderer.material = OnBuildingValidMaterial;
                }
            }
        }

        //prototype is replaced by a real building
        void InstantiateWorkplace()
        {
            Vector3 position = prototype.transform.position;
            Quaternion rotation = prototype.transform.rotation;
            RemovePrototype();
            Instantiate(workplaceConfig.WorkplacePrefab, position, rotation, WorkplacesRoot);
        }

        void RemovePrototype()
        {
            triggerSubscriptions.Clear();
            Destroy(prototype);
        }
    }
}
