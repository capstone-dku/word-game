using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovement : MonoBehaviour
{
    private Vector3 origin;
    private Vector3 diff;
    private Vector3 reset;

    private bool drag;

    private void Awake()
    {
        reset = Camera.main.transform.position;
    }

    private void LateUpdate()
    {
        if(Input.GetMouseButton(0))
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

        if (drag)
        {
            Camera.main.transform.position = origin - diff;
        }
    }
}
