using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.EventSystems;
using System;

public class BuildSystem : MonoBehaviour
{
    [SerializeField]
    private LayerMask GroundLayers;
    [SerializeField]
    private Transform BuildingsRoot;

    [Header("Building prototypes materials")]
    [SerializeField]
    private Material OnBuildingValidMaterial;
    [SerializeField]
    private Material OnBuildingInvalidMaterial;

    private const string BUILD_SYSTEM_VIEW_PATH = "BuildSystemView";
    private ButtonGroup BuildSystemView;

    public event Action<BuildingConfig> OnBuildConstructionSite;

    private void Start()
    {
        BuildSystemView = GameObject.Instantiate<ButtonGroup>(
            Resources.Load<ButtonGroup>(BUILD_SYSTEM_VIEW_PATH), GameRoot.Instance.Canvas.transform);
    }

    public void AddBuild(BuildingConfig buildingConfig)
    {
        BuildSystemView.AddButton(buildingConfig.Icon, () => StartBuilding(buildingConfig));
    }

    private void StartBuilding(BuildingConfig buildingConfig)
    {
        StartCoroutine(Build(buildingConfig));
    }

    private IEnumerator Build(BuildingConfig buildingConfig)
    {
        int overlapObstacleCount = 0;
        CompositeDisposable triggerSubscriptions = new();

        //initialize building prototype
        GameObject prototype = Instantiate(buildingConfig.BuildingPrototype, BuildingsRoot);
        prototype.SetActive(false);
        if(!prototype.TryGetComponent<Rigidbody>(out Rigidbody prototypeRigidbody))
        {
            prototypeRigidbody = prototype.AddComponent<Rigidbody>();
        }
        prototypeRigidbody.isKinematic = true;
        Renderer prototypeRenderer = prototype.GetComponentInChildren<Renderer>();
        Collider prototypeCollider = prototype.GetComponentInChildren<Collider>();
        prototypeCollider.isTrigger = true;
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
            //remove prototype and exit
            if((Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
                || Input.GetMouseButton(1)
                || Input.GetKeyDown(KeyCode.Escape))
            {
                RemovePrototype();
                break;
            }
            //build a construction site if valid
            if (Input.GetMouseButtonDown(0) && overlapObstacleCount <=0)
            {
                InstantiateConstructionSiteAndRemovePrototype();
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

        void InstantiateConstructionSiteAndRemovePrototype()
        {
            Vector3 position = prototype.transform.position;
            Quaternion rotation = prototype.transform.rotation;
            RemovePrototype();
            var constructionSite = Instantiate(buildingConfig.ConstructionSite, position, rotation, BuildingsRoot);
            constructionSite.Init(buildingConfig);
            OnBuildConstructionSite?.Invoke(buildingConfig);
        }

        void RemovePrototype()
        {
            triggerSubscriptions.Clear();
            Destroy(prototype);
        }
    }
}
