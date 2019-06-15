using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISubject : MonoBehaviour
{
    public GameState m_ActiveAtThisState = GameState.Prepare;
    public float m_ActiveDelayTime;

    public void Awake()
    {
        UIObserver.Instance.AddToListen(this);
    }

    public void OnNotify(GameState state) {
        if(state == m_ActiveAtThisState){
            gameObject.SetActive(true);
            StartCoroutine(OpenUI());
            }
        else{
            StartCoroutine(CloseUI());
            }
    }

    public IEnumerator CloseUI(){
        if(GetComponent<Animator>() != null){
            GetComponent<Animator>().SetTrigger("Disable");
        }

        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("OnDisable"));
        yield return new WaitUntil(() => GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f);
        gameObject.SetActive(false);
    }   

    public IEnumerator OpenUI(){
        GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(m_ActiveDelayTime);
        GetComponent<Animator>().enabled = true;        
    }
    private void OnDestroy()
    {
        //UIObserver.Instance.RemoveFromListen(this);
    }  

}
