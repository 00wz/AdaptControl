using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Worker : Character, IDragHandlable
{
    [SerializeField]
    public float SearchRadius = 10f;
    [SerializeField]
    private GameObject WorkerMainStateView;

    private GameObject _workStateView;
    //private Workplace _currentWorkplace;
    private const float SEARCH_INTERVAL = 2f;

    private void Start()
    {
        StartCoroutine(SearchState());
    }

    private IEnumerator SearchState()
    {
        List<Workplace> workplaces = new();

        while (true)
        {
            var colls = Physics.OverlapSphere(transform.position, SearchRadius);

            //Search all Workplace objects
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].TryGetComponent<Workplace>(out Workplace workplace))
                {
                    workplaces.Add(workplace);
                }
            }

            if (workplaces.Count > 0) break;

            yield return new WaitForSeconds(SEARCH_INTERVAL);
        }

        //Select closest Workplace object
        float minSqrDist = (workplaces[0].transform.position - transform.position).sqrMagnitude;
        Workplace closestWorkplace = workplaces[0];
        for (int i = 1; i < workplaces.Count; i++)
        {
            var sqrDist = (workplaces[i].transform.position - transform.position).sqrMagnitude;
            if(sqrDist<minSqrDist)
            {
                minSqrDist = sqrDist;
                closestWorkplace = workplaces[i];
            }
        }

        GoToStationary(closestWorkplace.gameObject,
            () => ShowWorkStateView(closestWorkplace),
            () => StartCoroutine(SearchState()));
    }

    private void ShowWorkStateView(Workplace workplace)
    {
        WorkerMainStateView.SetActive(false);
        _workStateView = Instantiate<GameObject>(workplace.WorkStateView, transform.position,
            transform.rotation, this.transform);
    }

    private void ShowMainStateView()
    {
        if (_workStateView != null)
        {
            Destroy(_workStateView);
        }
        WorkerMainStateView.SetActive(true);
    }

    public void OnDragBegin()
    {
        StopAllCoroutines();
        StopNavigation();
        ShowMainStateView();
    }

    public void OnDragEnd()
    {
        StartCoroutine(SearchState());
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, SearchRadius);
    }

}
