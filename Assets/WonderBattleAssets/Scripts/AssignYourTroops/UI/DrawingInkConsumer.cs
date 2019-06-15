using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingInkConsumer : MonoBehaviour
{
    public float m_TotalInkConsume;
    public float m_ConsumeStep;
    public float m_EnableTime;
    public static float m_TimeScaler = 1.6f;
    public void OnEnable()
    {
        StartCoroutine(ConsumeInk());
    }

    public IEnumerator ConsumeInk() {

        var startTime = Time.time;
        var endTime = m_EnableTime * m_TimeScaler;

        while (Time.time - startTime < endTime) {
            UIManager.Instance.UpdateStatueHealth(m_ConsumeStep);
            //print(endTime / (m_TotalInkConsume / m_ConsumeStep));
            yield return new WaitForSeconds(endTime / (m_TotalInkConsume / m_ConsumeStep));
        }
               
    }

}
