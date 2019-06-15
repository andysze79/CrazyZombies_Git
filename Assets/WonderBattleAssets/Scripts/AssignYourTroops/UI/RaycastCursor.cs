using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCursor : MonoBehaviour
{    
    public LayerMask m_RaycastTarget;
    public GameObject m_point1;
    
    public GameObject m_point2;
    
    public GameObject m_point3;
    public GameObject m_Cursor;

    public bool m_Trigger;
    public int m_Index;
    public Ray ray{get;set;}
    public GameObject CurrentPoint{get;set;}
    public Vector3 OriginalPos{get;set;}
    public void Awake()
    {
        OriginalPos = m_point1.transform.position;
        gameObject.SetActive(false);        
    }

    public void Update()
    {        
        if(m_Trigger){

            RaycastHit hitInfo;
            ray = Camera.main.ScreenPointToRay(m_Cursor.transform.position);

            switch(m_Index)
            {
                case 1:
                CurrentPoint = m_point1;
                break;
                case 2:                
                CurrentPoint = m_point2;
                break;
                case 3:
                CurrentPoint = m_point3;
                break;
                default:
                CurrentPoint = m_point1;
                break;
            }

            if(Physics.Raycast(ray, out hitInfo, 1000, m_RaycastTarget)){
                CurrentPoint.transform.position = hitInfo.point;
            }
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    public void OnDisable()
    {
        CurrentPoint.transform.position = OriginalPos;
    }
}
