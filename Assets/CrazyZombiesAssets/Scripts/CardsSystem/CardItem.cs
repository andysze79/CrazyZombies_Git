using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler {

    public enum Mode {
        Card,
        Object,
        Placed
    }

    public GameObject m_PreviewObj;
    public GameObject m_ControlObj;

    public Mode m_CurrentMode = Mode.Card;
    
    public Vector3 OriginalPos {get;set; }
    public int OriginalSibling { get; set; }
    public Color OriginalCol { get; set; }
    public Image CardImage { get { return GetComponent<Image>(); }}
    public bool ResetProcess { get; set; }
    public Vector3 StartPos { get; set; }
    public float StartTime { get; set; }
    public bool DragInProgress;// { get; set; }
    
    #region Drag Func
    void InitializePos()
    {
        if (!ResetProcess)
        {
            // For drag from Card Field
            if (m_CurrentMode == Mode.Card)
            {
                OriginalPos = transform.position;
                OriginalSibling = transform.GetSiblingIndex();
            }
            // For drag from placing board
            else {
                SwitchMode(Mode.Object);
            }
        }
    }

    public void OnDragStart(){
        /*if (!ResetProcess)
        {
            transform.position = m_FollowingObject.transform.position;
            
        }*/

        InitializePos();
        DragInProgress = true;
    }

    public void DragUpdate(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void DragUpdateForTimeline(Transform Cursor)
    {
        transform.position = Cursor.position;       
    }

    public void OnDragEnd(){
        DragInProgress = false;

        if(m_CurrentMode == Mode.Object)
        SwitchMode(Mode.Placed);
    }

    #endregion

    #region DragInterface
    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDragStart();
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragUpdate(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd();
    }
    #endregion

    public void Start() {
        OriginalCol = CardImage.color;
    }

    public void OnEnable() {
        CardsManager.Instance.AddListener(this);
    }

    public void FixedUpdate() {
        if (ResetProcess) {

            if ( Vector3.Distance(transform.position, OriginalPos) > 0.1f)
            {
                transform.position = Vector3.Lerp(StartPos, OriginalPos, (Time.time - StartTime) * CardsManager.Instance.m_TransitionSpeed);
            }
            else
            {
                ResetProcess = false;

                RestoreCard();
            }
        }
    }

    #region CardField Trigger

    private void OnTriggerEnter(Collider other)
    {
        if (other == CardsManager.Instance.CardField)
        {            
            SwitchMode(Mode.Card);

            if(Time.time > 1)
            StartCoroutine(WaitForRestore());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == CardsManager.Instance.CardField)
        {
            SwitchMode(Mode.Object);
        }
    }

    #endregion

    public void SwitchMode(Mode mode) {

        m_CurrentMode = mode;

        switch (m_CurrentMode)
        {
            case Mode.Card:
                m_PreviewObj.SetActive(false);
                m_ControlObj.SetActive(false);
                CardImage.color = OriginalCol;
                
                m_PreviewObj.GetComponent<CameraRaycastObject>().DeactivateDragObject();
                break;
            case Mode.Object:
                // Open the preview object
                m_PreviewObj.SetActive(true);
                m_ControlObj.SetActive(false);

                // Close the Card image
                var col = OriginalCol;
                col.a = 0;
                CardImage.color = col;

                // Move out the card
                transform.SetParent(transform.root);

                // Start dragging the object
                m_PreviewObj.GetComponent<CameraRaycastObject>().ActivateDragObject(gameObject);
                break;
            case Mode.Placed:
                // Open the preview object
                m_PreviewObj.SetActive(false);
                m_ControlObj.SetActive(true);
                break;
            default:
                break;
        }

    }

    public void RestoreCard() {
        // Move in the card
        transform.SetParent(CardsManager.Instance.transform);
        transform.SetSiblingIndex(OriginalSibling);
    }

    public IEnumerator WaitForRestore() {
        yield return new WaitUntil(() => !DragInProgress);

        StartPos = transform.position;
        StartTime = Time.time;
        ResetProcess = true;
    }
}
