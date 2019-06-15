using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSwitcher : MonoBehaviour
{
    [System.Serializable]
    public struct HandleObject {
        public string m_MaterialName;
        public Texture m_previousTex, m_afterTex;
    }

    public HandleObject[] m_objs;
    public bool m_triggered;

    public void Awake()
    {
        for (int i = 0; i < m_objs.Length; i++)
        {
            Material mat = (Material)Resources.Load(m_objs[i].m_MaterialName, typeof(Material));

            if (mat != null) {
                mat.shader = Shader.Find("HDRP/Lit");

                mat.SetTexture("_BaseColorMap", m_objs[i].m_previousTex);
            }
        }
    }
    public void Update()
    {
        if (m_triggered) {
            SwitchTexture();
        }
    }
    public void SwitchTexture()
    {
        for (int i = 0; i < m_objs.Length; i++)
        {
            Material mat = (Material)Resources.Load(m_objs[i].m_MaterialName, typeof(Material));

            mat.shader = Shader.Find("HDRP/Lit");

            mat.SetTexture("_BaseColorMap", m_objs[i].m_afterTex);
        }
    }

}
