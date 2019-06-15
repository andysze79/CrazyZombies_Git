using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsSwitcher : MonoBehaviour
{
    [System.Serializable]
    public struct PartsData {
        public ItemGenre Genre;
        public GameObject PartsObj;
        public int ItemCode;
    }

    public List<PartsData> m_CurrentParts = new List<PartsData>();

    public List<ItemData> m_CurrentActiveItem = new List<ItemData>();

    public int m_PowerCurrent;
    public int m_ShieldCurrent;

    public void ChangeParts(ItemGenre Genre, int Index) {

        List<PartsData> GenreParts = new List<PartsData>();

        for (int i = 0; i < m_CurrentParts.Count; i++)
        {
            if (m_CurrentParts[i].Genre == Genre) {
                if (m_CurrentParts[i].ItemCode == Index)
                {
                    m_CurrentParts[i].PartsObj.SetActive(true);            
                }
                else
                {
                    m_CurrentParts[i].PartsObj.SetActive(false);
                }
            }

        }

        UpdateActiveItemsList(Genre,Index);
        ScoreCalculator();
        /*for (int i = 0; i < m_CurrentParts.Count; i++)
        {
            if (i == Index)
                m_CurrentParts[i].PartsObj.SetActive(true);
            else
                m_CurrentParts[i].PartsObj.SetActive(false);
        }*/
    }

    public void UpdateActiveItemsList(ItemGenre Genre, int Index) {

        var Items = InventroyData.Instance.m_Items;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].Genre == Genre )
            {
                if (Items[i].ItemCode == Index) {
                    m_CurrentActiveItem.Add(Items[i]);
                }
                else
                {
                    m_CurrentActiveItem.Remove(Items[i]);
                }
            }            
        }
    }

    public void ScoreCalculator() {
        m_PowerCurrent = 0;
        m_ShieldCurrent = 0;

        foreach (var item in m_CurrentActiveItem)
        {
            m_PowerCurrent += item.Power;
            m_ShieldCurrent += item.Shield;
        }

        UIManager.Instance.UpdateStatus(m_PowerCurrent,m_ShieldCurrent);
    }
}
