using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.Tools;
using BaseAssets.AI;

public class FightModeController : MonoBehaviour
{
    public float m_TimeToAggressive = 3;
    public GameObject m_SpawnedTroopsContainer;

    public void OnEnable()
    {
        if(m_TimeToAggressive != 0)
        StartCoroutine(Delay());
    }

    public IEnumerator Delay() {
        yield return new WaitForSeconds(m_TimeToAggressive);

        GetComponent<FormationPlanner>().fightingMode = BaseAssets.AI.AIDataHolder.FightingMode.Aggressive;

        NotifyMembers();
    }

    public void NotifyMembers() {
        var items = m_SpawnedTroopsContainer.GetComponentsInChildren<AIDataHolder>();

        foreach (var item in items)
        {
            item.fightingMode = AIDataHolder.FightingMode.Aggressive; 
        }
    }
}
