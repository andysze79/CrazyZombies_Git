using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    [System.Serializable]
    public struct Materials {
        public int m_index;
        public Material m_Mat;
    }

    [SerializeField] private List<Materials> m_Mats = new List<Materials>();
    [SerializeField] private Renderer[] m_TargetRenderer;
    [SerializeField] private float m_IntervalTime = 0.1f;

    [Header("Control by Timeline")]
    [SerializeField] private int m_TargetMat;

    public int CurrentMat { get; set; }

    public void Awake()
    {
        StartCoroutine(SwapTexInSequence(CurrentMat));
    }

    public void Update()
    {
        if (CurrentMat != m_TargetMat) {
            CurrentMat = m_TargetMat;
            StartCoroutine(SwapTexInSequence(CurrentMat));
        }
    }

    public Material CheckMaterial(int Target) {
        Material targetMat = m_Mats[0].m_Mat;

        foreach (var obj in m_Mats) {
            if (obj.m_index == Target) {
                targetMat = obj.m_Mat;
            }
        }

        return targetMat;
    }

    public IEnumerator SwapTexInSequence(int Target) {
        Material targetMat = CheckMaterial(Target);

        foreach (var obj in m_TargetRenderer)
        {
            obj.material = targetMat;
            yield return new WaitForSeconds(m_IntervalTime);
        }
    }

}
