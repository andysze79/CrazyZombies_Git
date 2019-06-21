using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.Tools;

public class SlotsVisualizer : MonoBehaviour
{
    [System.Serializable]
    public struct VisualizeObj
    {
        public FormationPlanner m_Formation;
        public GameObject m_SlotColor;
        public Transform m_Organizer;
    }

    public VisualizeObj[] m_objs;

    public void OnEnable()
    {
        transform.position = m_objs[0].m_Formation.transform.position;

        for (int i = 0; i < m_objs.Length; i++)
        {
            var slots = m_objs[i].m_Formation.GetActiveList();

            for (int j = 0; j < slots.Count; j++)
            {
                var clone = Instantiate(m_objs[i].m_SlotColor,slots[j],Quaternion.identity);

                clone.transform.parent = m_objs[i].m_Organizer;
            }
        }
    }


}
