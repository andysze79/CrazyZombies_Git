using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXPlayer : MonoBehaviour
{
    [SerializeField]private GameObject[] m_VFX;
    [SerializeField]private float m_Duration;

    [SerializeField]private string m_SFXName;
    [SerializeField] private bool m_StoreUnderThisObj = true;

    [Header("Real-Time generate")]
    [SerializeField]private bool m_InstantiateTheVFX;
    [SerializeField] private Transform m_GenerateSpot;
    public Coroutine Process { get; set; }
    public Transform OriginalParent { get; set; }

    public void Awake()
    {
        OriginalParent = transform.parent;
    }

    public void OnEnable()
    {
        if (m_SFXName != "")
            SoundsManager.Instance.PlaySFX(m_SFXName);
    }

    public void PlayVFX()
    {        
        if(Process == null)
            Process = StartCoroutine(Delayer());
        if (m_InstantiateTheVFX)
        {           
            GenerateVFX();
        }
    }

    public void GenerateVFX() {
        foreach (var item in m_VFX)
        {
            var clone = Instantiate(item.gameObject,m_GenerateSpot.position, transform.rotation);
            //clone.transform.position = transform.position;
            clone.GetComponent<ParticleSystem>().Play();
            Destroy(clone, m_Duration);
        }
    }

    public IEnumerator Delayer() {
        foreach (var item in m_VFX)
        {
            if (!m_StoreUnderThisObj) {
                item.transform.SetParent(transform.root);
            }
            item.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(m_Duration);

        foreach (var item in m_VFX)
        {
            //item.transform.SetParent(OriginalParent);
            item.gameObject.SetActive(false);
        }

        Process = null;
    }

}
