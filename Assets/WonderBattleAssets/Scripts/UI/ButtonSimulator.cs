using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonSimulator : MonoBehaviour
{
    public enum ButtonState {
        Idle,
        Pressed,
        Disabled
    }

    [SerializeField]private SpriteState sprState = new SpriteState();
    [SerializeField]private Image m_ButtonImage;
    [SerializeField]private ButtonState m_ButtonStateCurrent = ButtonState.Idle;
    [SerializeField]private Image m_Glow;

    public float m_TargetScale = 0.28f;
    public float m_Duration = 0.6f;
    
    public bool Once { get; set; }

    public void Update()
    {
        switch (m_ButtonStateCurrent)
        {
            case ButtonState.Idle:
                //m_ButtonImage.sprite = sprState.highlightedSprite;
                //if(m_Glow != null)
                //    m_Glow.gameObject.SetActive(false);
                break;
            case ButtonState.Pressed:
                //m_ButtonImage.sprite = sprState.pressedSprite;
                if(!Once)
                Click();
                //if (m_Glow != null)
                //    m_Glow.gameObject.SetActive(true);
                break;
            case ButtonState.Disabled:
                //m_ButtonImage.sprite = sprState.disabledSprite;
                //if (m_Glow != null)
                //    m_Glow.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void Click() {
        Once = true;

        var info = GetComponent<RectTransform>();
        var infoOrigin = info.localScale;

        StartCoroutine(ScaleUpDown(infoOrigin, m_TargetScale * Vector3.one));
    }

    public IEnumerator ScaleUpDown(Vector3 start, Vector3 end) {
        var startTime = Time.time;
        var EndTime = m_Duration / 2;

        while (Time.time - startTime < EndTime)
        {
            GetComponent<RectTransform>().localScale = Vector3.Lerp(start, end, (Time.time - startTime) / EndTime);
            yield return null;
        }

        GetComponent<RectTransform>().localScale = end;

        StartCoroutine(ScaleUpDown(end, start));
    }


}
