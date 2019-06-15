using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenreChanger : MonoBehaviour
{

    public ScrollViewImageUpdater m_ItemsScrollMenu;
    public ItemGenre m_CurrentGenre;
    public Color m_ButtonColClicked;
    public Color m_ButtonColOrigin;
    

    public void ChangeGenre(int Target) {
        m_ItemsScrollMenu.m_CurrentFilter = (ItemGenre)Target;
        ChangeColor(Target);
    }

    public void ChangeColor(int Target) {
        var buttons = GetComponentsInChildren<Image>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].color = m_ButtonColOrigin;
        }

        transform.GetChild(Target).GetComponent<Image>().color = m_ButtonColClicked;
    }
}
