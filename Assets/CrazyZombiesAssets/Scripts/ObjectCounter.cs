using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectCounter : MonoBehaviour
{
    public string m_ObjTag = "RedZombie";
    public float m_RefreshTime = 2f;
    public TextUpdater m_SpawnedAmountText;
    public TextUpdater m_KilledAmountText;
    public TextUpdater m_ResourcesText;
    public float m_GainResourcesEachKill;
    public float m_ResourcesAmount;

    public static ObjectCounter m_Instance;
    public static ObjectCounter Instance {
        get {
            if (m_Instance == null) {
                m_Instance = GameObject.FindObjectOfType<ObjectCounter>();
            }

            return m_Instance;
        }
    }

    public int CurrentKilled { get; set; }


    public void OnEnable()
    {
        StartCoroutine(UpdateAmount());
        InitializeUI();
    }

    public void InitializeUI() {
        if (m_KilledAmountText != null) m_KilledAmountText.ChangeText(CurrentKilled);
        if (m_ResourcesText != null) UpdateResources(m_GainResourcesEachKill);
    }

    public IEnumerator UpdateAmount(){
        var counter = GameObject.FindGameObjectsWithTag(m_ObjTag).Length;

        Debug.Log(counter + " Zombies awake!"); 

        yield return new WaitForSeconds(m_RefreshTime);

        StartCoroutine(UpdateAmount());
    }

    public int GetAmount() {
        var amount = GameObject.FindGameObjectsWithTag(m_ObjTag).Length;
        if(m_SpawnedAmountText != null) m_SpawnedAmountText.ChangeText(amount);

        return amount;
    }

    public void TargetKilled(string tag)
    {
        if (tag == m_ObjTag) {
            UpdateKilledAmount();            
            UpdateResources(m_GainResourcesEachKill);            
        }
    }

    public void UseResources(float value) {
        if (m_ResourcesAmount - value > 0) {
            UpdateResources(-value);
        }
    }

    public void CheckEnable(TextUpdater target) {
        if (!target.transform.parent.gameObject.activeSelf) {
            target.transform.parent.gameObject.SetActive(true);
        }
    }

    public void UpdateKilledAmount()
    {
        ++CurrentKilled;

        if (m_KilledAmountText != null)
        {
            CheckEnable(m_KilledAmountText);
            m_KilledAmountText.ChangeText(CurrentKilled);
        }     
    }

    public void UpdateResources(float addValue)
    {
        if (m_ResourcesText != null)
        {
            CheckEnable(m_ResourcesText);
            m_ResourcesAmount += addValue;
            m_ResourcesText.ChangeText(m_ResourcesAmount);
        }
    }


}
