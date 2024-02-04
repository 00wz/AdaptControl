using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CastomGravity : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed = -2f;

    private Rigidbody _rigidbidy;
    private void Start()
    {
        _rigidbidy = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _rigidbidy.AddWorldOffsetSweep(new Vector3 { y = fallSpeed });
    }
}
