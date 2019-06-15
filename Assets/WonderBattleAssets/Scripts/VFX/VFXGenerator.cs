using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXGenerator : MonoBehaviour
{
    public GameObject m_VFX;
    public Transform[] m_GenerateAtHere;
    public float m_Duration;

    [Header("Control by timeline")]
    public bool m_Triggered;
    public List<GameObject> VFXOnHold { get; set; }
    public List<Coroutine> Process { get; set; }

    public void Awake()
    {
        foreach (var obj in m_GenerateAtHere)
        {
            var clone = Instantiate(m_VFX, obj.position, Quaternion.identity);
            clone.transform.parent = transform.root;
            clone.gameObject.SetActive(false);
            VFXOnHold.Add(clone);
        }
    }

    public void Update()
    {
        if (m_Triggered && Process == null) {
            PlayEffects();
        }
    }

    public void PlayEffects() {
        foreach (var obj in VFXOnHold)
        {
            Process.Add(StartCoroutine(Delayer(obj.GetComponent<ParticleSystem>(), m_Duration)));
        }

        StartCoroutine(ClearProcess());
    }

    public IEnumerator Delayer(ParticleSystem item, float duration)
    {
        item.gameObject.SetActive(true);
        item.Play();
        
        yield return new WaitForSeconds(duration);

        item.gameObject.SetActive(false);
    }

    public IEnumerator ClearProcess() {     

        yield return new WaitForSeconds(m_Duration);

        Process.Clear();
    }
}
