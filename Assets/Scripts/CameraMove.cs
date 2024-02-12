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

    [Header("Camera constraints")]
    [SerializeField]
    private Vector3 Max;
    [SerializeField]
    private Vector3 Min;

    private Vector3 LOCAL_RIGHT;
    private Vector3 LOCAL_FORWARD;
    private float _targetHeight;
    private bool CanHandleInput { get; set; } = true;
    private const int SCREEN_ERROR = 5;

    void Start()
    {
        RefreshCameraSettings();
    }

    void Update()
    {
        Vector3 input = ReadInput();

        if(input!=Vector3.zero)
        {
            Move(input);
        }

        //smooth vertical movement
        transform.position = new Vector3(
            transform.position.x,
            Mathf.MoveTowards(transform.position.y, _targetHeight, MoveSpeed * Time.deltaTime),
            transform.position.z);
    }

    private Vector3 ReadInput()
    {
        Vector3 input = Vector3.zero;

        if (!CanHandleInput)
        {
            return input;
        }

        if (Input.mousePosition.x >= Screen.width - SCREEN_ERROR)
        {
            input.x = 1f;
        }
        else if (Input.mousePosition.x <= 0 + SCREEN_ERROR)
        {
            input.x = -1f;
        }

        if (Input.mousePosition.y >= Screen.height - SCREEN_ERROR)
        {
            input.y = 1f;
        }
        else if (Input.mousePosition.y <= 0 + SCREEN_ERROR)
        {
            input.y = -1f;
        }

        if (!Input.GetMouseButton(0))//so it doesn't conflict with the GodsHand
        {
            input.z = Input.GetAxis("Mouse ScrollWheel");
        }

        return input;
    }

    private void Move(Vector3 input)
    {
        transform.Translate(input.x * LOCAL_RIGHT * MoveSpeed * Time.deltaTime, Space.World);
        transform.Translate(input.y * LOCAL_FORWARD * MoveSpeed * Time.deltaTime, Space.World);
        ClampHorizontalPosition();

        _targetHeight -= input.z * HeightChangeStep;
        _targetHeight = Mathf.Clamp(_targetHeight, Min.y, Max.y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Min + Max) * 0.5f, Max - Min);
    }

    private void ClampHorizontalPosition()
    {
        float x = Mathf.Clamp(transform.position.x, Min.x, Max.x);
        float z = Mathf.Clamp(transform.position.z, Min.z, Max.z);
        transform.position = new Vector3(x, transform.position.y, z);
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
