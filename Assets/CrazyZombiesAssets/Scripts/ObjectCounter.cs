using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameResource {
    Heart, Diamond, Gold
}

public class ObjectCounter : MonoBehaviour
{
    [System.Serializable]
    public struct ResourceGroup {
        public GameResource Type;
        public TextUpdater Text;
        public float GainResourcesEachKill;
        public float Amount;
    }

    public string m_ObjTag = "RedZombie";
    public float m_RefreshTime = 2f;
    public TextUpdater m_SpawnedAmountText;
    public TextUpdater m_KilledAmountText;

    public List<ResourceGroup> m_ResourcesGroups = new List<ResourceGroup>();


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

        if (m_ResourcesGroups.Count != 0)
        {
            for (int i = 0; i < m_ResourcesGroups.Count; i++)
            {
                UpdateResources(m_ResourcesGroups[i].Type, m_ResourcesGroups[i].GainResourcesEachKill);
            }
        }
    }

    public IEnumerator UpdateAmount(){
        var counter = GameObject.FindGameObjectsWithTag(m_ObjTag).Length;

        //Debug.Log(counter + " Zombies awake!"); 

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

            if(m_ResourcesGroups.Count != 0)
            for (int i = 0; i < m_ResourcesGroups.Count; i++)
            {
                UpdateResources(m_ResourcesGroups[i].Type, m_ResourcesGroups[i].GainResourcesEachKill);
            }
        }
    }

    public void UseResources(GameResource Type, float value) {
        //Debug.Log("Use " + value + " " + Type);
        UpdateResources(Type, -value);        
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

    public void UpdateResources(GameResource ResourceType ,float addValue)
    {
        for (int i = 0; i < m_ResourcesGroups.Count; i++)
        {
            if (m_ResourcesGroups[i].Type == ResourceType)
            {
                // Calculate   
                var resource = m_ResourcesGroups[i];
                resource.Amount += addValue;

                //Debug.Log("Current " + resource.Type + " " + resource.Amount);

                if (resource.Amount >= 0)
                m_ResourcesGroups[i] = resource;

                // Update UI
                CheckEnable(m_ResourcesGroups[i].Text);
                m_ResourcesGroups[i].Text.ChangeText(m_ResourcesGroups[i].Amount);

            }
        }        
    }


}
