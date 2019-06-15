using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXLifeTime : MonoBehaviour
{
    public float m_LifeTime;
    public bool m_Destroy;

    public void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    public IEnumerator AutoDisable() {
        yield return new WaitForSeconds(m_LifeTime);
        gameObject.SetActive(false);
        if(m_Destroy)
            Destroy(gameObject);
    }


}
