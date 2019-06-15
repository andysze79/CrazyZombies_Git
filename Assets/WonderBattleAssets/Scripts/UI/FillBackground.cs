using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class FillBackground : MonoBehaviour
{

    public Image m_blueBar;
    public Image m_redBar;

    public void Awake()
    {
        m_redBar = GetComponent<Image>();
    }

    public void Update()
    {
        if(m_redBar.fillAmount != 0)
        m_redBar.fillAmount = 1 - m_blueBar.fillAmount;
    }

}
