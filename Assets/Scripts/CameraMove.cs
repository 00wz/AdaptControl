using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 10f;
    [SerializeField]
    private float HeightChangeStep = 5;

    private Vector3 LOCAL_RIGHT;
    private Vector3 LOCAL_FORWARD;
    private float _targetHeight;
    private bool CanHandleInput { get; set; } = true;

    void Start()
    {
        RefreshCameraSettings();
    }

    void Update()
    {
        if(!CanHandleInput)
        {
            return;
        }

        if(Input.mousePosition.x >= Screen.width)
        {
            transform.Translate(LOCAL_RIGHT * MoveSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.x <= 0)
        {
            transform.Translate(LOCAL_RIGHT * -MoveSpeed * Time.deltaTime, Space.World);
        }        
        
        if(Input.mousePosition.y >= Screen.height)
        {
            transform.Translate(LOCAL_FORWARD * MoveSpeed * Time.deltaTime, Space.World);
        }
        else if (Input.mousePosition.y <= 0)
        {
            transform.Translate(LOCAL_FORWARD * -MoveSpeed * Time.deltaTime, Space.World);
        }

        if(Input.GetMouseButton(0))//so it doesn't conflict with the GodsHand
        {
            return;
        }
        _targetHeight -= Input.GetAxis("Mouse ScrollWheel") * HeightChangeStep;
        transform.position = new Vector3(
            transform.position.x,
            Mathf.MoveTowards(transform.position.y, _targetHeight, MoveSpeed * Time.deltaTime),
            transform.position.z
            );
    }

    public void ShowTarget(Transform target, Action onExecution, float showTime, bool refundable = false)
    {
        StopAllCoroutines();
        CanHandleInput = false;
        Observable.WhenAll( 
            Observable.FromCoroutine(_ =>ShowTarget(target.position, showTime, refundable)))
            .Subscribe(_ =>
            {
                onExecution?.Invoke();
                CanHandleInput = true;
            });
    }
        
    private IEnumerator ShowTarget(Vector3 targetPosition, float showTime, bool refundable)
    {
        Plane targetAltitude = new Plane(Vector3.down, targetPosition);
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));
        if (!targetAltitude.Raycast(ray, out float distance))
        {
            yield break;
        }

        Vector3 startPosition = transform.position;
        Vector3 endPosition =
            transform.position + (targetPosition - ray.GetPoint(distance));

        while (!transform.position.NearlyEqual(endPosition))
        {
            yield return null;
            transform.position =
                Vector3.MoveTowards(transform.position, endPosition, MoveSpeed * Time.deltaTime);
        }

        float remainingShowTime = showTime;
        while (remainingShowTime >= 0)
        {
            remainingShowTime -= Time.deltaTime;
            yield return null;
        }

        if (!refundable)
        {
            yield break;
        }

        while (!transform.position.NearlyEqual(startPosition))
        {
            transform.position =
                Vector3.MoveTowards(transform.position, startPosition, MoveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void RefreshCameraSettings()
    {
        Quaternion cameraRotateY = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        LOCAL_RIGHT = cameraRotateY * Vector3.right;
        LOCAL_FORWARD = cameraRotateY * Vector3.forward;
        _targetHeight = transform.position.y;
    }
}
