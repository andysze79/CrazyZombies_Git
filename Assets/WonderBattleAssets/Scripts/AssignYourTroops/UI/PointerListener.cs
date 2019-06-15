using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerListener : MonoBehaviour
{
    void OnEnable()
    {
        transform.parent.GetComponent<PointerSwitcher>().OnNotify(gameObject);    
    }
    
}
