using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 2f;

    private CharacterController _controller;
    private float _diagonalCoefficient = 1 / Mathf.Sqrt(2f);
    private Vector3 RIGHT;
    private Vector3 FORWARD;
    public Vector3 direction { get; private set; }

    void Start()
    {
        Quaternion cameraRotateY = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        RIGHT = cameraRotateY * Vector3.right;
        FORWARD = cameraRotateY * Vector3.forward;
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal") * RIGHT + Input.GetAxis("Vertical") * FORWARD;
        if (direction.sqrMagnitude > 0.01f)
        {
            if (direction.sqrMagnitude > 1.5f)
            {
                direction *= _diagonalCoefficient;
            }
            _controller.Move(direction * Time.deltaTime * _moveSpeed);
        }
    }
}