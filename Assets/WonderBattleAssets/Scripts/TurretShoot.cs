using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TurretShoot : MonoBehaviour
{

    [SerializeField] private GameObject m_Projecttile;
    [SerializeField] private Transform m_GeneratePos;
    [SerializeField]private float m_CoolDownTime;
    [SerializeField]private Transform m_ShootTarget;

    [SerializeField] private float m_Speed;
    [SerializeField] private int m_BufferSize;
    [SerializeField]private float m_TargetRadius;

    [SerializeField]private string m_SFXName;

    public List<GameObject> ProjectTiles = new List<GameObject>();// { get; set; }
    public Animator m_animator { get; set; }

    public void Awake()
    {
        Initialize();
    }

    public void Update()
    {
    }

    public void Initialize()
    {
        m_animator = GetComponent<Animator>();

        for (int i = 0; i < m_BufferSize; i++)
        {
            var clone = Instantiate(m_Projecttile, m_GeneratePos.position, Quaternion.identity);

            ProjectTiles.Add(clone);

            clone.SetActive(false);
        }
    }

    public GameObject ExtendBuffersize()
    {
        var clone = Instantiate(m_Projecttile, m_GeneratePos);

        ProjectTiles.Add(clone);

        clone.SetActive(false);

        return clone;
    }

    public GameObject GetProjectile()
    {

        GameObject target = gameObject;

        foreach (var each in ProjectTiles)
        {
            if (each.activeSelf != true)
            {
                target = each;
                break;
            }
        }

        if (target != gameObject)
        {
            return target;
        }
        else
        {
            return ExtendBuffersize();
        }

    }
       
    public void ShootProjectile()
    {
        var projecttile = GetProjectile();
        Vector3 Pos = Vector3.zero;
        Pos.x = m_ShootTarget.position.x + Random.Range(-m_TargetRadius, m_TargetRadius);
        Pos.z = m_ShootTarget.position.z + Random.Range(-m_TargetRadius, m_TargetRadius);
        var dir = Pos - m_GeneratePos.position;

        projecttile.transform.position = m_GeneratePos.position;
        projecttile.transform.forward = dir;

        projecttile.GetComponent<Rigidbody>().velocity = Vector3.zero;
        projecttile.SetActive(true);
        

        projecttile.GetComponent<Rigidbody>().AddForce(dir * m_Speed, ForceMode.VelocityChange);

        if (m_SFXName != "") {
            // Play sound
            SoundsManager.Instance.PlaySFX(m_SFXName);
        }

        StartCoroutine(CoolDown());
    }

    public IEnumerator CoolDown() {

        m_animator.SetBool("CoolDown", true);

        yield return new WaitForSeconds(m_CoolDownTime);

        m_animator.SetBool("CoolDown", false);
    }
}
