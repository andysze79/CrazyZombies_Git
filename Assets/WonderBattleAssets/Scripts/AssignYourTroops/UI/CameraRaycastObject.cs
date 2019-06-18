using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycastObject : MonoBehaviour
{
    public LayerMask m_RaycastTarget;

    public bool Trigger { get; set; }
    public GameObject Cursor { get; set; }
    public Ray Ray { get; set; }
    public Vector3 OriginalPos { get; set; }

    public void Awake() {
        OriginalPos = transform.position;
    }

    public void ActivateDragObject(GameObject cursor)
    {
        Trigger = true;
        Cursor = cursor;
        //gameObject.SetActive(false);        
    }

    public void DeactivateDragObject()
    {
        transform.position = OriginalPos;
        Cursor = null;
        Trigger = false;
    }

    public void Update()
    {
        if (Trigger)
        {
            RaycastHit hitInfo;
            Ray = Camera.main.ScreenPointToRay(Cursor.transform.position);

            if (Physics.Raycast(Ray, out hitInfo, 1000, m_RaycastTarget))
            {
                transform.position = hitInfo.point;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(Ray.origin, Ray.origin + Ray.direction * 1000, Color.blue);
    }


}
