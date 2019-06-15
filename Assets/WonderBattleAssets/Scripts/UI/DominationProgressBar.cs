using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class DominationProgressBar : MonoBehaviour
{
    [SerializeField]private float m_TotalEnemiesKilledAmount;
    [SerializeField]private float m_EnemiesKilledAmount;

    public TextMeshProUGUI m_Meter;
    public Image DominationProgress { get; set; }

    public void Awake()
    {
        DominationProgress = GetComponent<Image>();
                
        DominationProgress.fillAmount = 0;
    }


    public void UpdateScore() {

        ++m_EnemiesKilledAmount;

        float barAmount = Mathf.Clamp(m_EnemiesKilledAmount / m_TotalEnemiesKilledAmount, 0f, 1f);

        DominationProgress.fillAmount = barAmount;

        if (m_Meter != null) {
            int percentage = Mathf.FloorToInt(barAmount * 100);
            m_Meter.text = percentage.ToString() + " %";
        }
    }
    

}
