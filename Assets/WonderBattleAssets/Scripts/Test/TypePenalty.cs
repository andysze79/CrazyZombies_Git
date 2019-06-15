using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TypePenalty : MonoBehaviour
{
    public enum TroopTypes {
        Infantry,
        Archer,
        Calavry
    }

    [SerializeField]private TroopTypes m_CurrentType = TroopTypes.Infantry;
    [SerializeField]private TroopTypes m_WeakType = TroopTypes.Archer;
    [SerializeField]private TroopTypes m_AdvType = TroopTypes.Archer;
    [SerializeField]private float m_PaneltyAmount = 2f;
    [SerializeField]private float m_AdvantageAmount = 2f;

    public float CheckTypePanelty(TroopTypes Attacker) {
        if (Attacker == m_WeakType)
        {
            return m_PaneltyAmount;
        }
        else {
            return 1;
        }
    }

    public float CheckTypeAdv(TroopTypes Attacker) {
        if (Attacker == m_AdvType)
        {
            return m_AdvantageAmount;
        }
        else
        {
            return 1;
        }
    }

}
