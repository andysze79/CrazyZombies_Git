using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDrawer : MonoBehaviour
{
    [System.Serializable]
    public struct FormationShape {
        public string m_shapeName;
        public Transform[] m_WayPoints;        
        public float m_Duration;
    }

    public enum WayToDraw {
        Loop,
        Sequence
    }

    public GameObject m_Pen;
    [SerializeField]private FormationShape[] m_Shapes;
    [SerializeField]private int m_ShapeSelector;
    [SerializeField]private WayToDraw m_DrawingWay = WayToDraw.Loop;
    //[SerializeField]private bool m_Loop;
    [Tooltip("The delay time for drawing next shape or next loop")]
    [SerializeField]private float m_DelayToNextShape;

    [SerializeField]private bool m_Trigger;

    public static float m_TimeScaler = 0.5f;


    public float m_TotalDist { get; set; }
    public bool Trigger { get; set; }
    public Vector3 PenStartPos { get; set; }
    public int ShapeSelector { get { return m_ShapeSelector; } }

    public void Awake()
    {
        PenStartPos = m_Pen.transform.position;
        m_Pen.SetActive(false);
    }

    public void OnEnable()
    {
        //StartCoroutine(DrawShape());
    }

    public void Update()
    {
        if (Trigger != m_Trigger) {
            Trigger = m_Trigger;

            if (Trigger)
                StartCoroutine(DrawShape());
        }

    }

    public float CheckDistance(Vector3 a, Vector3 b) {
        return Vector3.Distance(a,b);
    }

    public float CheckTotalDist() {

        float TotalDist = 0;

        for (int i = 0; i < m_Shapes[m_ShapeSelector].m_WayPoints.Length - 1; i++)
        {
            TotalDist += CheckDistance(m_Shapes[m_ShapeSelector].m_WayPoints[i].position, m_Shapes[m_ShapeSelector].m_WayPoints[i + 1].position);
        }

        return TotalDist;
    }

    public IEnumerator DrawShape() {

        int i = 0;

        var startTime = Time.fixedTime;
        var endTime = m_Shapes[m_ShapeSelector].m_Duration * m_TimeScaler;
        var endTimeFrag = endTime;
        var TotalDist = CheckTotalDist();

        m_Pen.SetActive(true);
        m_Pen.transform.position = PenStartPos;
                
        while (i < m_Shapes[m_ShapeSelector].m_WayPoints.Length - 1)
        {

            var start = m_Shapes[m_ShapeSelector].m_WayPoints[i].position;
            var End = m_Shapes[m_ShapeSelector].m_WayPoints[i + 1].position;
            endTimeFrag = endTime * CheckDistance(start, End) / TotalDist;

            if (TotalDist == 0) {
                yield return new WaitForSeconds(m_Shapes[m_ShapeSelector].m_Duration);
                startTime = Time.fixedTime;
                ++i;
            }
            else
            {
                if (CheckDistance(m_Pen.transform.position, End) > 0.1f)
                {
                    m_Pen.transform.position = Vector3.Lerp(start, End, (Time.fixedTime - startTime) / endTimeFrag);
                }
                else
                {
                    startTime = Time.fixedTime;
                    ++i;
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(m_DelayToNextShape);


        switch (m_DrawingWay)
        {
            case WayToDraw.Loop:
                StartCoroutine(DrawShape());
                break;
            case WayToDraw.Sequence:

                if (m_ShapeSelector < m_Shapes.Length)
                {
                    ++m_ShapeSelector;
                }

                StartCoroutine(DrawShape());
                break;
            default:
                break;
        }

        /*if (m_Loop)
        {
            StartCoroutine(DrawShape());
        }
        else {
            //m_Pen.SetActive(false);
            //m_Pen.transform.position = m_Shapes[m_ShapeSelector].m_WayPoints[m_Shapes[m_ShapeSelector].m_WayPoints.Length - 1].position;
        }*/
    }

    public void OnDisable()
    {
        m_Pen.transform.position = PenStartPos;
        m_Pen.SetActive(false);
    }



}
