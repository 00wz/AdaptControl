using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 10f;
    [SerializeField]
    private float HeightChangeStep = 5;

    private Vector3 LOCAL_RIGHT;
    private Vector3 LOCAL_FORWARD;
    private float _targetHeight;

    void Start()
    {
        RefreshCameraSettings();
    }

    void Update()
    {
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

    private void RefreshCameraSettings()
    {
        Quaternion cameraRotateY = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        LOCAL_RIGHT = cameraRotateY * Vector3.right;
        LOCAL_FORWARD = cameraRotateY * Vector3.forward;
        _targetHeight = transform.position.y;
    }
}
