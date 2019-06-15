using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerSwitcher : MonoBehaviour
{
    public GameObject[] m_Pointers;
    public GameObject m_CurrentPointer; 

    public void OnNotify(GameObject TargetPointer){
        foreach(var obj in m_Pointers){
            if(obj == TargetPointer){
                m_CurrentPointer = obj;                                
            }            
            else
            {
                obj.SetActive(false);
            }
        }
    }

   
}
