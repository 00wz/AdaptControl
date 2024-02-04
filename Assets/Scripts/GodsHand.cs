using System.Collections;
using UnityEngine;

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
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, charactersLayers))
            {
                Rigidbody target = raycastHit.rigidbody;
                if(target!=null)
                {
                    Vector3 screenOffset =
                        Camera.main.WorldToScreenPoint(target.position) - Input.mousePosition;
                    StartCoroutine(Drag(target, screenOffset));
                }
            }
        }
    }

    IEnumerator Drag(Rigidbody target, Vector3 screenOffset)
    {
        float haight = 0f;

        while (Input.GetMouseButton(0))
        {
            var newHeight = haight - Input.GetAxis("Mouse ScrollWheel") * speedOfClimb;
            haight = Mathf.Max(newHeight, 0f);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition + screenOffset);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, ~charactersLayers))
            {
                var destination = raycastHit.point + (haight * Vector3.up);
                destination = Vector3.MoveTowards(target.position,
                    destination, speedOfDrag * Time.deltaTime);
                target.MovePositionSweep(destination);
            }

            yield return null;
        }
    }
}
