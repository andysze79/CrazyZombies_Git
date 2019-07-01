using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    public Animator m_GateAnimator;
    
    public void OpenTheGate(float duration)
    {
        StartCoroutine(ActivateTheGate(duration));
    }

    public IEnumerator ActivateTheGate(float duration) {

        m_GateAnimator.SetTrigger("Open");

        yield return new WaitForSeconds(duration);

        m_GateAnimator.SetTrigger("Close");
    }



}
