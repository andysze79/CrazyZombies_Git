using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbObject : MonoBehaviour
{
    public ClimbUpWall ClimbWall { get { return transform.parent.GetComponent<ClimbUpWall>(); } }


    public void OnEnable()
    {
        StartCoroutine(Delay());
    }

    public IEnumerator Delay() {
        yield return new WaitForSeconds(Random.Range(0 , ClimbWall.m_MaxDelayStartTime));

        ClimbWall.AddListener(transform);
        GetComponent<Animator>().SetTrigger("Walk");
    }
}

