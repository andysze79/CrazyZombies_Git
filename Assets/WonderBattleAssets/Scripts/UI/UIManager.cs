using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]private DominationProgressBar m_PlayerTeam;
    [SerializeField]private DominationProgressBar m_EnemyTeam;
    [SerializeField]private HealthBarBehavior m_StatueHealth;
    [SerializeField]private HealthBarBehavior m_GateHealthBar;
    [SerializeField]private TextMeshProUGUI m_Power;
    [SerializeField]private TextMeshProUGUI m_Shield;
    [SerializeField]private TextMeshProUGUI m_PowerEnemy;
    [SerializeField]private TextMeshProUGUI m_ShieldEnemy;
    [SerializeField]private GameObject m_PowerArrow;
    [SerializeField] private GameObject m_ShieldArrow;

    public static UIManager _instance;

    public static UIManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<UIManager>();
                //Debug.Log(_instance.name);
            }

            return _instance;
        }
    }

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void UpdateStatueHealth(float damageAmount)
    {
        m_StatueHealth.GetDamage(damageAmount);
    }
    public void UpdateGateHealth(float damageAmount) {
        m_GateHealthBar.GetDamage(damageAmount);
    }
    public void UpdateStatus(int Power, int Shield) {
        m_Power.text = Power.ToString();
        m_Shield.text = Shield.ToString();

        if (int.Parse(m_Power.text) > int.Parse(m_PowerEnemy.text))
        {
            m_PowerArrow.gameObject.SetActive(true);
        }
        else {
            m_PowerArrow.gameObject.SetActive(false);
        }


        if (int.Parse(m_Shield.text) > int.Parse(m_ShieldEnemy.text))
        {
            m_ShieldArrow.gameObject.SetActive(true);
        }
        else
        {
            m_ShieldArrow.gameObject.SetActive(false);
        }
    }
}
