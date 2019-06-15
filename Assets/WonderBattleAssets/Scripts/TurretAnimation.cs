using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAnimation : MonoBehaviour
{

    public enum Animation {
        Default,
        Initialize,
        Shoot,
        Deactivated,
        Activate
    }

    public Animation m_AnimationCurrent = Animation.Default;
    public Material m_DeactivateMat;
    public Material ActivateMat { get; set; }
    public Animator Animator { get; set; }
    public LookAtObject FacingFunc { get; set; }
    Renderer[] meshs { get { return GetComponentsInChildren<Renderer>(); } }

    public void Awake()
    {
        Animator = GetComponent<Animator>();
        FacingFunc = GetComponent<LookAtObject>();
        ActivateMat = meshs[0].material;
    }

    public void Update()
    {
        switch (m_AnimationCurrent)
        {
            case Animation.Default:
                break;
            case Animation.Initialize:
                Animator.SetTrigger("Initialize");
                FacingFunc.Look();
                break;
            case Animation.Shoot:
                Animator.SetTrigger("Shoot");
                break;
            case Animation.Deactivated:
                Animator.SetTrigger("Deactivated");

                for (int i = 0; i < meshs.Length; i++)
                {
                    meshs[i].material = m_DeactivateMat;
                }
                break;
            case Animation.Activate:
                Animator.SetTrigger("Activated");
                
                for (int i = 0; i < meshs.Length; i++)
                {
                    meshs[i].material = ActivateMat;
                }
                break;
            default:
                break;
        }
    }

}
