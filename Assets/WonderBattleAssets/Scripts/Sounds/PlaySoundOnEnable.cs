using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnEnable : MonoBehaviour
{
    [SerializeField]private string m_SFXName;

    public void OnEnable()
    {
        if(m_SFXName != "")
        SoundsManager.Instance.PlaySFX(m_SFXName);
    }
}
