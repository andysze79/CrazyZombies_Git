using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    [System.Serializable]
    public struct Clips {
        public string m_SFXName;
        public bool m_Loop;
        public AudioClip[] m_Clip;
    }

    public Clips[] m_SFXclips;
    public AudioSource AudioPlayer;// { get; set; }

    public static SoundsManager _instance;
    public static SoundsManager Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<SoundsManager>();
                //Debug.Log(_instance.name);
            }

            return _instance;
        }
    }


    public void Awake()
    {
        //AudioPlayer = GetComponent<AudioSource>();
    }

    public (bool,AudioClip) Search(string SFXName) {
        bool Loop = false;
        AudioClip target = null;

        foreach (var item in m_SFXclips)
        {
            if (item.m_SFXName == SFXName)
            {
                var index = Random.Range(0, item.m_Clip.Length);
                target = item.m_Clip[index];
                Loop = item.m_Loop;
            }
        }

        if (target != null){
            return (Loop, target);            
            }
        else{
            return (Loop, null);
            }

    }

    public void PlaySFX(string SFXName) {

        var findClip = Search(SFXName);

        if (findClip.Item2 != null)
        {
            AudioPlayer.Stop();
            if(findClip.Item1){
            AudioPlayer.loop = true;
            AudioPlayer.clip = findClip.Item2;
            AudioPlayer.Play();
            }
            else
            {
            var SFXPlayer = GetPlayer(findClip.Item2);
            SFXPlayer.Stop();
            SFXPlayer.loop = false;                
            SFXPlayer.PlayOneShot(findClip.Item2);
            }
        }
        else{
            Debug.Log("SFXManager Couldn't find " + SFXName);
        }
    }

    public AudioSource GetPlayer(AudioClip clip){
        var clone = Instantiate(AudioPlayer, transform);

        Destroy(clone.gameObject, clip.length);

        return clone;
    }
    
}
