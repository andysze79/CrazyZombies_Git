using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    public float m_ScaleAfterClicked;
    public float m_Duration;
    public Vector3 OriginalSize{get;set;}
    public Coroutine Process{get;set;}

    public bool AutoRelease{get;set;}
    public bool Once{get;set;}
     
     
    public void Awake()
    {
        OriginalSize = transform.localScale;
    }
    public void StartScale(){
        AutoRelease = true;
        Once = false;
        StartCoroutine(Scaling(transform.localScale, OriginalSize * m_ScaleAfterClicked ));        
    }

    public void StartHold(bool onOff){
        StopAllCoroutines();

        AutoRelease = false;
        
        if(onOff){
            Once = false;
            //StartCoroutine(Scaling(transform.localScale, OriginalSize * m_ScaleAfterClicked ));
            transform.localScale = OriginalSize * m_ScaleAfterClicked;
        }
        else
        {       
            Once = false;
            //StartCoroutine(Scaling(transform.localScale, OriginalSize));
            transform.localScale = OriginalSize;
        }
    }

    public bool CheckSize(){
        if(transform.localScale == OriginalSize)
            return true;
        else            
            return false;    
    }

    public IEnumerator Scaling(Vector3 start, Vector3 end){
        
        var startTime = Time.time;
        var endTime = m_Duration;
        
        while(Time.time - startTime < endTime){

            transform.localScale = Vector3.Lerp(start, end, (Time.time - startTime) / endTime);

            yield return null;
        }

        if(AutoRelease && !Once){
        StartCoroutine(Scaling(end, start));
        Once = true;
        }
    }

}
