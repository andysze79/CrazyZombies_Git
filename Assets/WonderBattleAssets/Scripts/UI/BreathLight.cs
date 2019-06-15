using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BreathLight : MonoBehaviour
{

    public Image m_Glow;
    public AnimationCurve m_Behavior;
    public float m_Min;
    public float m_CycleTime;
    public bool FliFlop{get;set;}
    public void OnEnable()
    {
        StartCoroutine(Breath(1f));
    }

    public IEnumerator Breath(float targetValue) {

        var startTime = Time.time;
        var endTime = m_CycleTime;
        var col = m_Glow.color;
        var colOrigin = col;
        var colTarget = new Color(col.r, col.g, col.b, targetValue);


        while (Time.time - startTime < endTime) {

            col = Color.Lerp(colOrigin, colTarget, (Time.time - startTime) / endTime );
            m_Glow.color = col;
            //print(col);
            yield return null;
        }

        if (FliFlop){
        FliFlop = !FliFlop;
        StartCoroutine(Breath(1f));
        }
        
        else{
        FliFlop = !FliFlop;
        StartCoroutine(Breath(m_Min));
        }
        
    }

}
