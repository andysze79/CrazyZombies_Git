using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawerLinker : MonoBehaviour
{
    public GameObject m_Pen;

    [System.Serializable]
    public struct LinkedDrawer
    {
        public string m_EvenetName;
        public int m_EventNum;
        public FormationDrawer m_LinkedDrawer;
        public bool m_DrawerValue;
        public GameObject m_VFX;
        public bool m_VFXValue;
        public GameObject m_paintVFX;
        public bool m_paintVFXValue;        
        public int m_paintVFXIndex;
        public GameObject m_UI;
        public GameObject m_Spawner;
        public bool m_CursorClick;        
        public bool m_CursorHold;  
        public GameObject m_UIPointer;
        public Image m_CkickThisButton;
        public string m_SFXName;
    }

    public List<LinkedDrawer> m_LinkedEvents = new List<LinkedDrawer>();
    public FormationDrawer m_Listener;
    public int ShapeSelectorCurrent { get; set; }

    public void Awake()
    {
        ShapeSelectorCurrent = m_Listener.ShapeSelector;
    }

    public void Update()
    {
        if (ShapeSelectorCurrent != m_Listener.ShapeSelector) {

            ShapeSelectorCurrent = m_Listener.ShapeSelector;

            for (int i = 0; i < m_LinkedEvents.Count; i++)
            {
                if (ShapeSelectorCurrent == m_LinkedEvents[i].m_EventNum) {
                    CheckUpdateObjects(i);
                }
            }
        }
    }

    public void CheckUpdateObjects(int index) {
        if (m_LinkedEvents[index].m_LinkedDrawer != null) {
            m_LinkedEvents[index].m_LinkedDrawer.gameObject.SetActive(m_LinkedEvents[index].m_DrawerValue);
        }
        if (m_LinkedEvents[index].m_VFX != null) {
            m_LinkedEvents[index].m_VFX.SetActive(m_LinkedEvents[index].m_VFXValue);
        }
        if (m_LinkedEvents[index].m_paintVFX != null) {
            //m_LinkedEvents[index].m_paintVFX.GetComponent<RaycastCursor>().m_Index = m_LinkedEvents[index].m_paintVFXIndex;
            m_LinkedEvents[index].m_paintVFX.SetActive(m_LinkedEvents[index].m_paintVFXValue);
        }
        if (m_LinkedEvents[index].m_UI != null) {
            m_LinkedEvents[index].m_UI.SetActive(true);
        }
        if (m_LinkedEvents[index].m_Spawner != null) {
            m_LinkedEvents[index].m_Spawner.SetActive(true);
        }
        if(m_LinkedEvents[index].m_SFXName != ""){
            SoundsManager.Instance.PlaySFX(m_LinkedEvents[index].m_SFXName);
        }
        if(m_LinkedEvents[index].m_CursorClick){
            // Cursor Click
            m_Pen.transform.GetComponent<UIScaler>().StartScale();
            
            if(m_LinkedEvents[index].m_CkickThisButton != null){
                m_LinkedEvents[index].m_CkickThisButton.GetComponent<UIScaler>().StartScale();            
            }
            if(m_LinkedEvents[index].m_UIPointer != null){
                m_LinkedEvents[index].m_UIPointer.transform.position = m_LinkedEvents[index].m_CkickThisButton.transform.position;
                m_LinkedEvents[index].m_UIPointer.SetActive(true);
                m_LinkedEvents[index].m_UIPointer.GetComponent<UIScaler>().StartScale();     
            }
        }
        if(m_LinkedEvents[index].m_CursorHold)
        {                        
            m_Listener.m_Pen.transform.GetComponent<UIScaler>().StartHold(true);
        }
        if(!m_LinkedEvents[index].m_CursorHold)
        {                        
            m_Listener.m_Pen.transform.GetComponent<UIScaler>().StartHold(false);
        }

    }

}
