using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI m_Text { get { return GetComponent<TextMeshProUGUI>(); } }

    public void ChangeText(string data)
    {
        m_Text.text = data;
    }

    public void ChangeText(float data) {
        m_Text.text = Mathf.FloorToInt(data).ToString();
    }
}
