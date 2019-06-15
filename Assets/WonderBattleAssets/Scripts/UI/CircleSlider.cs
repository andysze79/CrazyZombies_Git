using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleSlider : MonoBehaviour
{
    public enum SliderMode{
        Circle,
        Horizontal
    }

    public SliderMode m_Mode = SliderMode.Horizontal;

    public Image m_Bar;
    public RectTransform m_Border;

    [Header("Horizontal Slider")]
    public RectTransform m_StartEdge;
    public RectTransform m_EndEdge;

    

    public void Update()
    {
        switch (m_Mode)
        {
            case SliderMode.Circle:
                HealthChange();
                break;
            case SliderMode.Horizontal:
                HorizontalBar();
                break;
            default:
                break;
        }
    }

    public void HealthChange() {
        
        float borderAngle = m_Bar.fillAmount * 360;

        if(!m_Bar.fillClockwise)
            m_Border.localEulerAngles = new Vector3(0, 0, borderAngle);
        else
            m_Border.localEulerAngles = new Vector3(0, 0, -borderAngle);
    }

    public void HorizontalBar() {

        float borderDist = m_Bar.fillAmount * Vector2.Distance(m_StartEdge.position, m_EndEdge.position);
               
        m_Border.position = new Vector3(m_EndEdge.position.x + borderDist, m_Border.position.y, m_Border.position.z);
    }

}
