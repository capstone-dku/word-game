using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 diff;
    private Vector3 reset;

    private bool drag;
    [SerializeField]private GraphicRaycaster raycaster;
    private PointerEventData eventData;
    private void Awake()
    {
        reset = Camera.main.transform.position;
        eventData = new PointerEventData(null);
    }

    private void LateUpdate()
    {
        if(Input.GetMouseButton(0))
        {
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(eventData, results);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            if (results.Count <= 0 && hit.transform == null)
            {
                diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
                if (drag == false)
                {
                    drag = true;
                    origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else
            {
                drag = false;
            }
        }
        else
        {
            drag = false;
        }

        if (drag)
        {
            Camera.main.transform.position = origin - diff;
        }
        Camera.main.orthographicSize -= Input.mouseScrollDelta.y * 1f;
    }
}
