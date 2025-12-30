using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GodsHand : MonoBehaviour
{
    [SerializeField]
    private float speedOfClimb = 1f;
    [SerializeField]
    private float speedOfDrag = 1f;
    [SerializeField]
    private LayerMask charactersLayers;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue))
            {
                if((charactersLayers.value & (1 << raycastHit.collider.gameObject.layer)) != 0)
                {
                    Rigidbody target = raycastHit.rigidbody;
                    if(target!=null)
                    {
                        Vector3 screenOffset =
                            Camera.main.WorldToScreenPoint(target.position) - Input.mousePosition;
                        StartCoroutine(Drag(target, screenOffset));
                    }
                }
                else
                {
                    var anchorPosition = raycastHit.point;
                    StartCoroutine(MoveCamera(anchorPosition));
                }
            }
        }
    }

    IEnumerator MoveCamera(Vector3 anchorPosition)
    {
        Plane plane = new Plane(Vector3.up, anchorPosition);
        while (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(plane.Raycast(ray, out float enter))
            {
                var raycastHitPoint = ray.GetPoint(enter);
                GameRoot.Instance.CameraMove.Move(anchorPosition - raycastHitPoint);
            }

            yield return null;
        }
    }

    IEnumerator Drag(Rigidbody target, Vector3 screenOffset)
    {
        IDragHandlable dragHandlable = target.GetComponent<IDragHandlable>();
        dragHandlable?.OnDragBegin();
        float haight = target.position.y;

        while (Input.GetMouseButton(0))
        {
            haight -= Input.GetAxis("Mouse ScrollWheel") * speedOfClimb;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + screenOffset);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, ~charactersLayers))
            {
                haight = Mathf.Max(haight, raycastHit.point.y);
                Vector3 destination = new Vector3(raycastHit.point.x, haight, raycastHit.point.z);
                destination = Vector3.MoveTowards(target.position,
                    destination, speedOfDrag * Time.deltaTime);
                target.MovePositionFlowAround(destination);
            }

            yield return null;
        }

        dragHandlable?.OnDragEnd();
    }
}
