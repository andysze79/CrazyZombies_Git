using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.Tools;

public class SpawnTriggerFormation : MonoBehaviour
{
    public SpawnManager SpawnManager { get { return GetComponent<SpawnManager>(); } }

    public void OnEnable()
    {
        SpawnManager.formations[0].transform.parent.gameObject.SetActive(true); ;
    }


}
