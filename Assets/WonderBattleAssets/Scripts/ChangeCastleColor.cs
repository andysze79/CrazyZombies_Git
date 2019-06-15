using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCastleColor : MonoBehaviour
{
    public Color m_PlayerColor;
    public Color m_EnemyColor;

    public float m_Time = 2;

    public Material m_TargetMat;

    public Coroutine process { get; set; }

    void Start()
    {
        ChangeMatColor(m_EnemyColor);
        gameObject.SetActive(false);
    }

    public void OnEnable()
    {
        ChangeMatColor(m_PlayerColor);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) {
            ChangeMatColor(m_EnemyColor);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeMatColor(m_PlayerColor);
        }
    }

    public void ChangeMatColor(Color col) {

        Material mat = (Material)Resources.Load("Enemy", typeof(Material));

        mat.shader = Shader.Find("HDRP/Lit");

        m_TargetMat = mat;

        //mat.SetColor("_BaseColor", col);

        StartCoroutine(ColorTransition(mat, col));
    }

    public IEnumerator ColorTransition(Material mat, Color col) {

        var startTime = Time.time;
        var endTime = m_Time;
        Color colOrigin = mat.color;

        while (Time.time - startTime < endTime) {
            var colTemp = Color.Lerp(colOrigin, col, (Time.time - startTime) / endTime);
            
            mat.SetColor("_BaseColor", colTemp);

            yield return null;
        }

    }
}
