using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUpdateCounter : MonoBehaviour
{    
    public void OnDestroy()
    {
        ObjectCounter.Instance.TargetKilled(transform.tag);
    }
}
