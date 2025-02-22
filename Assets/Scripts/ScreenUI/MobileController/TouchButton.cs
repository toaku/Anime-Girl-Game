using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isInput
    {
        get;
        private set;
    } = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(isInput == false)
            isInput = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isInput == true)
            isInput = false;
    }
}
