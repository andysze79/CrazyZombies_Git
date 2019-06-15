using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScrollViewImageUpdater : MonoBehaviour
{
    [Header("ScrollView")]
    public ItemGenre m_CurrentFilter;
    public int m_CurrentClick;
    public GameObject m_Content;

    public PartsSwitcher m_PartsSwitcher;

    public List<ItemData> m_Items { get { return InventroyData.Instance.m_Items; } }
    public List<Image> m_ContentSlots;
    public ItemGenre CurrentFilter { get; set; }
    public int CurrentClick { get; set; }
    public List<ItemData> CurrentItems = new List<ItemData>();
        

    public void Awake()
    {

        var slots = m_Content.GetComponentsInChildren<Image>();

        for (int i = 0; i < slots.Length; i++)
        {
            m_ContentSlots.Add(slots[i]);
        }

        UpdateImages();
    }

    public void Update()
    {
        if (CurrentFilter != m_CurrentFilter) {
            CurrentFilter = m_CurrentFilter;
            UpdateImages();
        }
        /*if (CurrentClick != m_CurrentClick) {
            CurrentClick = m_CurrentClick;

            
        }*/
    }

    public void UpdateImages() {

        if (CurrentItems.Count != 0)
        CurrentItems.Clear();
        
        for (int i = 0; i < m_Items.Count; i++)
        {
            if (m_Items[i].Genre == CurrentFilter) {
                CurrentItems.Add(m_Items[i]);
            }
        }

        for (int i = 0; i < m_ContentSlots.Count; i++)
        {
            if(i < CurrentItems.Count)
            m_ContentSlots[i].sprite = CurrentItems[i].Icon;
        }
    }

    public void OnContentClick(int ClickIndex) {
        m_CurrentClick = ClickIndex;

        CurrentClick = m_CurrentClick;
        // switch parts
        m_PartsSwitcher.ChangeParts(CurrentFilter, CurrentClick);
    }
}
