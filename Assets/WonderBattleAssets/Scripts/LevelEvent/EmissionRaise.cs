using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionRaise : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color m_ColTarget;

    [SerializeField] private Renderer[] m_TargetRenderer;
    [SerializeField]private float m_Duration;
    [SerializeField]private AnimationCurve m_Movement;

    public Color ColOrigin { get; set; }
    public Coroutine Process { get; set; }
    public bool Flipflop { get; set; }

    public void OnEnable()
    {
        Raise();
    }

    public void Raise()
    {
        if (Process == null)
        {
            Process = StartCoroutine(Delay());
        }
        
    }

    public IEnumerator Delay() {        
        int counter = 2;
        var Origin = m_TargetRenderer[0].material.GetColor("_EmissiveColor");
        var Target = m_ColTarget;

        while (counter > 0) { 

            var StartTime = Time.time;
            var endTime = m_Duration;
            print("for loop");

            while (Time.time - StartTime < endTime)
            {
                Color col = ColOrigin;

                if (!Flipflop)
                    col = Color.Lerp(Origin, Target, m_Movement.Evaluate( (Time.time - StartTime) / endTime) );
                if (Flipflop)
                    col = Color.Lerp(Target, Origin, m_Movement.Evaluate((Time.time - StartTime) / endTime));

                foreach (var obj in m_TargetRenderer)
                {
                    Material mat = obj.material;
                    mat.SetColor("_EmissiveColor", col);
                }                      

                yield return null;
            }

            //yield return new WaitForSeconds(endTime / 2);

            Flipflop = !Flipflop;

            --counter;

            yield return null;
        }
        Process = null;
    }
}
