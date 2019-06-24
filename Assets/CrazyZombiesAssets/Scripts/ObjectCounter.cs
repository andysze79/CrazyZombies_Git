using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCounter : MonoBehaviour
{
    public string m_ObjTag = "RedZombie";
    public float m_RefreshTime = 2f;

    public void OnEnable()
    {
        StartCoroutine(UpdateAmount());
    }

    public IEnumerator UpdateAmount(){
        var counter = GameObject.FindGameObjectsWithTag(m_ObjTag).Length;

        Debug.Log(counter + " Zombies awake!"); 

        yield return new WaitForSeconds(m_RefreshTime);

        StartCoroutine(UpdateAmount());
    }

    public int GetAmount() {
        return GameObject.FindGameObjectsWithTag(m_ObjTag).Length;
    }

}
