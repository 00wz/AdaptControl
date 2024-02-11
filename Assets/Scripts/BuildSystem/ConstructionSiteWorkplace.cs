using UnityEngine;
using UniRx;
using System;

public class ConstructionSiteWorkplace : Workplace
{
    [SerializeField]
    private float BuildTime = 3f;

    private BuildingConfig _buildingConfig;
    private int _numberOfBuilders = 0;
    private CompositeDisposable _buildEveryFrameSubscription = new();

    public static event  Action<GameObject, BuildingConfig> OnCompletBuild;

    public void Init(BuildingConfig buildingConfig)
    {
        _buildingConfig = buildingConfig;
    }

    public override void StartWork()
    {
        base.StartWork();
        _numberOfBuilders++;
        if(_buildEveryFrameSubscription.Count == 0)
        {
            Observable.EveryUpdate().Subscribe(_ => Build()).AddTo(_buildEveryFrameSubscription);
        }
    }

    public override void EndWork()
    {
        _numberOfBuilders--;
        if(_numberOfBuilders <= 0)
        {
            _buildEveryFrameSubscription.Clear();
        }
    }

    private void Build()
    {
        BuildTime -= (_numberOfBuilders * Time.deltaTime);
        if (BuildTime <= 0)
        {
            InstantiateBuilding();
        }
    }

    private void InstantiateBuilding()
    {
        var building = Instantiate(_buildingConfig.BuildingPrefab,
            transform.position, transform.rotation, transform.parent);
        OnCompletBuild?.Invoke(building, _buildingConfig);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _buildEveryFrameSubscription.Clear();
    }
}
