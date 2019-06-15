using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCheck : MonoBehaviour
{
    public Animator[] m_TestObjs;
    public string[] m_AnimationParameters;
    public float m_Randomize;
    public string m_AnimationCurrent;
    public int Index{get;set;}
    
    public void Awake()
    {
        m_TestObjs = GetComponentsInChildren<Animator>();
    }
    
    public void Update()
    {
        if(Input.GetKeyUp(KeyCode.A)){            
            if(Index > 0){
                StopAllCoroutines();
                --Index;            
                StartCoroutine(UpdateAnimation());
            }
        }
        if(Input.GetKeyUp(KeyCode.D)){            
            if(Index < m_AnimationParameters.Length - 1)
            {
                StopAllCoroutines();
                ++Index;
                StartCoroutine(UpdateAnimation());
            }
        }
    }

    public IEnumerator UpdateAnimation(){
        for (int i = 0; i < m_TestObjs.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0, m_Randomize));
            m_TestObjs[i].SetTrigger(m_AnimationParameters[Index]);
        }
        m_AnimationCurrent = m_AnimationParameters[Index];
    }

}
