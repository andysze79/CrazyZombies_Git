using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbUpWall : MonoBehaviour
{
    public Transform m_Start;
    public Transform m_End;
    public float m_ClimbDuration = 3f;
    public float m_SlideDuration = 3f;
    public AnimationCurve m_ClimbCurve;
    public AnimationCurve m_SlideCurve;

    public float m_MaxDelayStartTime = 2f;
    public bool m_LockZAxis = false;

    public List<Transform> m_Objs = new List<Transform>();

    
    public void AddListener(Transform obj) {
        m_Objs.Add(obj);
        StartCoroutine(Climb(obj, m_Start, m_End, m_ClimbDuration));
    }

    public IEnumerator Climb(Transform Obj, Transform start, Transform end, float duration) {
        var startTime = Time.time;
        var endTime = duration;
        var PosZ = Obj.position.z;

        if (m_LockZAxis)
        {
            PosZ = start.position.z;
        }
        else
        {
            PosZ = Obj.position.z;
        }

        while (Time.time - startTime < endTime) {

            Obj.position = new Vector3(
                Obj.position.x,
                Mathf.Lerp(start.position.y, end.position.y, m_ClimbCurve.Evaluate((Time.time - startTime) / endTime)),
                PosZ
                );            

            yield return null;
        }

        StartCoroutine(Fall(Obj ,m_End, m_Start, m_SlideDuration));             
    }
    public IEnumerator Fall(Transform Obj, Transform start, Transform end, float duration)
    {
        var startTime = Time.time;
        var endTime = duration;
        var PosZ = Obj.position.z;

        if (m_LockZAxis)
        {
            PosZ = start.position.z;
        }
        else {
            PosZ = Obj.position.z;
        }

        while (Time.time - startTime < endTime)
        {

            Obj.position = new Vector3(
                Obj.position.x,
                Mathf.Lerp(start.position.y, end.position.y, m_SlideCurve.Evaluate((Time.time - startTime) / endTime)),
                PosZ
                );

            yield return null;
        }

        StartCoroutine(Climb(Obj, m_Start, m_End, m_ClimbDuration));

    }

}
