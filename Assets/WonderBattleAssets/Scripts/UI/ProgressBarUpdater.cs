using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarUpdater : MonoBehaviour
{
    public TextMeshProUGUI m_Data;
    public float m_Max;
    public Image Bar { get; set; }

    public float CurrentAmount { get; set; }

    public void Awake()
    {
        Bar = GetComponent<Image>();

        CurrentAmount = int.Parse(m_Data.text);

        Bar.fillAmount = Mathf.Clamp(CurrentAmount / m_Max, 0, 1);

    }

    public void Update()
    {
        if (CurrentAmount != int.Parse(m_Data.text))
        {
            CurrentAmount = int.Parse(m_Data.text);

            Bar.fillAmount = Mathf.Clamp(CurrentAmount / m_Max,0,1);
        }
    }
}
