using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;

    [SerializeField]
    private RectTransform background;
    [SerializeField]
    private RectTransform stick;
    private float movementRange;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        movementRange = (Mathf.Abs(background.rect.width) - Mathf.Abs(stick.rect.width)) / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystick(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetJoystick();
    }

    private void ControlJoystick(Vector2 draggedPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, draggedPosition, null, out draggedPosition);
        if(Vector2.Distance(Vector2.zero, draggedPosition) > movementRange)
        {
            draggedPosition = draggedPosition.normalized * movementRange;
        }
        stick.anchoredPosition = draggedPosition;
    }

    private void ResetJoystick()
    {
        stick.anchoredPosition = Vector2.zero;
    }

    public Vector2 GetDirection()
    {
        return (stick.anchoredPosition - Vector2.zero).normalized;
    }
}
