using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationSlots : MonoBehaviour
{
    [System.Serializable]
    public struct Slot {
        public Transform m_Slot;
        public Transform m_User;
        public bool m_InUse;
    }

    public List<Slot> m_Slots = new List<Slot>();

    public int Index { get; set; }

    public void Awake()
    {
        var slots = GetComponentsInChildren<Transform>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != transform && slots[i].GetComponent<MeshRenderer>() != null)
            {
                Slot info;

                info.m_Slot = slots[i];
                info.m_InUse = false;
                info.m_User = null;

                m_Slots.Add(info);
            }
        }

        // start renting slot from the first one
        Index = 0;
    }

    public void OnGUI()
    {
        
    }

    public Transform GetSlot(Transform user) {

        Transform TargetSlot = null;

        if (!m_Slots[Index].m_InUse)
        {
            TargetSlot = m_Slots[Index].m_Slot;

            Slot info = m_Slots[Index];
            info.m_InUse = true;
            info.m_User = user;

            m_Slots[Index] = info;

            // Update the Index for next check to use
            ++Index;

            if (Index >= m_Slots.Count) {
                // Reset the Index if reach to the end
                Index = 0;
            }
        }
        else
        {
            for (int i = 0; i < m_Slots.Count; i++)
            {
                // If find a spare slot
                if (!m_Slots[i].m_InUse)
                {
                    TargetSlot = m_Slots[i].m_Slot;

                    Slot info = m_Slots[i];
                    info.m_InUse = true;
                    info.m_User = user;

                    m_Slots[i] = info;

                    // Update the Index for next check to use
                    Index = i + 1;

                    if (Index >= m_Slots.Count)
                    {
                        // Reset the Index if reach to the end
                        Index = 0;
                    }

                    break;
                }
                else
                {
                    // If find a slot that the user is not there anymore
                    if (m_Slots[i].m_User == null)
                    {
                        Slot info = m_Slots[i];
                        info.m_InUse = false;
                        info.m_User = null;

                        m_Slots[i] = info;

                        TargetSlot = m_Slots[i].m_Slot;


                        // Update the Index for next check to use
                        Index = i + 1;

                        if (Index >= m_Slots.Count)
                        {
                            // Reset the Index if reach to the end
                            Index = 0;
                        }
                    }
                }
            }
        }

        //if (TargetSlot != null)
            return TargetSlot;
        /*else
            return m_Slots[Random.Range(0, m_Slots.Count)].m_Slot;*/
    }

    public bool CheckSlotVacancy() {

        bool vacancy = false;

        //Debug.Log("Index is currently at : " + Index);

        if (!m_Slots[Index].m_InUse)
        {
            vacancy = true;
        }

        else
        {
            for (int i = 0; i < m_Slots.Count; i++)
            {
                if (!m_Slots[i].m_InUse)
                {
                    vacancy = true;

                    break;
                }
                else
                {
                    if (m_Slots[i].m_User == null)
                    {
                        Slot info = m_Slots[i];
                        info.m_InUse = false;
                        info.m_User = null;

                        m_Slots[i] = info;

                        vacancy = true;
                    }
                }
            }
        }

        return vacancy;
    }
}
