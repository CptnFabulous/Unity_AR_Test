using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onDown;
    public Color defaultColour = Color.white;
    public UnityEvent onUp;
    public Color pressedColour = Color.gray;

    Image i;
    public bool Held { get; private set; }

    void Awake()
    {
        i = GetComponent<Image>();
        i.color = defaultColour;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Held = true;
        onDown.Invoke();
        i.color = pressedColour;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Held = false;
        onUp.Invoke();
        i.color = defaultColour;
    }
}
