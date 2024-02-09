using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerSearchState : BaseState<Worker>
{
    private const float SEARCH_INTERVAL = 2f;
    private Coroutine searchCoroutine;

    public WorkerSearchState(Worker context, Action<Type> changeStateCallback) : base(context, changeStateCallback)
    {
    }

    public override void Enter()
    {
        searchCoroutine = context.StartCoroutine(SearchState());
    }

    public override void Exit()
    {
        //Sometimes StartCoroutine(Anything()) equals null if the coroutine is exist <= 1 frame
        if (searchCoroutine != null)
        {
            context.StopCoroutine(searchCoroutine);
        }
    }

    private IEnumerator SearchState()
    {
        List<Workplace> workplaces = new();

        //repeats at a SEARCH_INTERVAL interval until it finds a workplace
        while (true)
        {
            var colls = Physics.OverlapSphere(context.transform.position, context.SearchRadius);

            //Search all available Workplace objects
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].TryGetComponent<Workplace>(out Workplace workplace) && workplace.CanAcceptAnWorker)
                {
                    workplaces.Add(workplace);
                }
            }

            if (workplaces.Count > 0) break;

            yield return new WaitForSeconds(SEARCH_INTERVAL);
        }

        //Select closest Workplace object
        float minSqrDist = (workplaces[0].transform.position - context.transform.position).sqrMagnitude;
        Workplace closestWorkplace = workplaces[0];
        for (int i = 1; i < workplaces.Count; i++)
        {
            var sqrDist = (workplaces[i].transform.position - context.transform.position).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closestWorkplace = workplaces[i];
            }
        }

        context.CurrentWorkplace = closestWorkplace;
        ChangeState<WorkerGoingToWorkplaceState>();
    }
}
