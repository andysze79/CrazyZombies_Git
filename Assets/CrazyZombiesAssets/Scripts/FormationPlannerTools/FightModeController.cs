using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaseAssets.Tools;
using BaseAssets.AI;

public class FightModeController : MonoBehaviour
{
    [Header("For ready animation")]
    public bool m_PlayReadyBeforeFight = false;
    public float m_TimeToReady = 1;
    [Range(0,20)]
    public int m_EnableAmountPerFrame = 1;
    public bool m_Offset = true;

    [Header("For fight mode")]
    public float m_TimeToAggressive = 3;
    public GameObject m_SpawnedTroopsContainer;


    public void OnEnable()
    {
        if(m_TimeToAggressive != 0)
        StartCoroutine(Delay());
    }

    public IEnumerator Delay()
    {
        if (m_PlayReadyBeforeFight) {
            yield return new WaitForSeconds(m_TimeToReady);

            var troops = m_SpawnedTroopsContainer.GetComponentsInChildren<Animator>();

            for (int i = 0; i < troops.Length; i++)
            {
                troops[i].GetComponent<Animator>().SetTrigger("Ready");

                if (m_Offset && i % m_EnableAmountPerFrame == 0)
                    yield return new WaitForEndOfFrame();
                    //yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(m_TimeToAggressive - m_TimeToReady);

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
