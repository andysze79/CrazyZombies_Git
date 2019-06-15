using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehavior : MonoBehaviour
{    

    [SerializeField] private float m_HealthAmount;
    [SerializeField] private float m_BufferSpeed;

    [SerializeField] private Image m_Buffer;
    [SerializeField] private Image m_Health;
    [SerializeField] private bool m_HideBarWhenNoHealth;

    public float HealthCurrent { get; set; }

    public void Awake()
    {
        HealthCurrent = m_HealthAmount;
    }

    public void Update()
    {
        if (m_Buffer != null) {
            if (m_Buffer.fillAmount != m_Health.fillAmount)
            {
                m_Buffer.fillAmount = Mathf.Lerp(m_Buffer.fillAmount, m_Health.fillAmount, Time.deltaTime * m_BufferSpeed);
            }
        }
    }

    public void GetDamage(float DamageAmount)
    {
        HealthCurrent -= DamageAmount;
        m_Health.fillAmount = Mathf.Clamp(HealthCurrent / m_HealthAmount, 0f, 1f);

        if (m_Health.fillAmount == 0 && m_HideBarWhenNoHealth)
            HideBar();
    }

    public void HideBar()
    {
        gameObject.SetActive(false);
    }

}

