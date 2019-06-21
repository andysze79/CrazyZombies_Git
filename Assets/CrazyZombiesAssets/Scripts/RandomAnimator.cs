using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimator : MonoBehaviour
{
    public string[] m_AnimatorPath;
    
    public void OnEnable()
    {
        var animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Resources.Load( m_AnimatorPath[Random.Range(0,m_AnimatorPath.Length)] ) as RuntimeAnimatorController;
    }


}
