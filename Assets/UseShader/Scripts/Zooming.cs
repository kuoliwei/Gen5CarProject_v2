using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Zooming : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CornorScreenController controller;
    [SerializeField] CornerScreenFollower cornerScreenFollower;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = Color.gray;
        if (gameObject.name == "plus")
        {
            controller.SetZoomingPLUS();
        }
        if (gameObject.name == "minus")
        {
            controller.SetZoomingMINUS();
        }
        cornerScreenFollower.SetCamPos();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        controller.SetZoomingNONE();
        cornerScreenFollower.StopSetCamPos();
    }
}
