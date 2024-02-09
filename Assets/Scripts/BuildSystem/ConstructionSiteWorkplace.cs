using UnityEngine;
using UniRx;

public class ConstructionSiteWorkplace : Workplace
{
    [SerializeField]
    private float BuildTime = 3f;

    private GameObject _buildingPrefab;
    private int _numberOfBuilders = 0;
    private CompositeDisposable _buildEveryFrameSubscription = new();

    public void Init(GameObject BuildingPrefab)
    {
        _buildingPrefab = BuildingPrefab;
    }

    public override void StartWork()
    {
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
        Instantiate(_buildingPrefab, transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        _buildEveryFrameSubscription.Clear();
    }
}
