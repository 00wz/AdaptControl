using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerNavMashMove : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 2f;
    
    private NavMeshAgent _navMeshAgent;
    private float _diagonal—oefficient = 1 / Mathf.Sqrt(2f);
    private Vector3 RIGHT;
    private Vector3 FORWARD;
    public Vector3 direction { get; private set; }
    
    void Start()
    {
        Quaternion cameraRotateY= Quaternion.Euler(0,Camera.main.transform.eulerAngles.y, 0);
        RIGHT =cameraRotateY* Vector3.right;
        FORWARD = cameraRotateY * Vector3.forward;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        direction = Input.GetAxis("Horizontal") * RIGHT + Input.GetAxis("Vertical")*FORWARD;
        if (direction.sqrMagnitude > 0.01f)
        {
            if (direction.sqrMagnitude > 1.5f)
            {
                direction *= _diagonal—oefficient;
            }
            _navMeshAgent.Move(direction * Time.deltaTime * MoveSpeed);
            return;
        }
    }
}
