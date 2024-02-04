using System.Collections;
using UnityEngine;

public class GodsHand : MonoBehaviour
{
    [SerializeField]
    private float speedOfClimb = 1f;
    [SerializeField]
    private float speedOfDrag = 1f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                Rigidbody target = raycastHit.rigidbody;
                if(target!=null)
                {
                    Plane plane = new Plane(Vector3.down, raycastHit.point);
                    StartCoroutine(Drag(target, plane, target.position-raycastHit.point));
                }
            }
        }
    }

    IEnumerator Drag(Rigidbody target, Plane altitude, Vector3 targetOffset)
    {
        while (Input.GetMouseButton(0))
        {
            altitude.distance -= Input.GetAxis("Mouse ScrollWheel")*speedOfClimb;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (altitude.Raycast(ray, out float hitDist))
            {
                var destination = ray.GetPoint(hitDist) + targetOffset;
                //if (destination.y < target.position.y) destination.y = target.position.y;//
                destination = Vector3.MoveTowards(target.position,
                    destination, speedOfDrag * Time.deltaTime);
                target.MovePositionSweep(destination);
            }

            yield return null;
        }
    }
}
