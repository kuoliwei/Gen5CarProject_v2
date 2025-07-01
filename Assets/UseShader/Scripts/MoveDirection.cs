using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MoveDirection : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        if (gameObject.name == "up")
        {
            controller.SetDirectionUP();
        }
        if (gameObject.name == "down")
        {
            controller.SetDirectionDOWN();
        }
        if (gameObject.name == "left")
        {
            controller.SetDirectionLEFT();
        }
        if (gameObject.name == "right")
        {
            controller.SetDirectionRIGHT();
        }
        cornerScreenFollower.SetCamPos();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        controller.SetDirectionNONE();
        cornerScreenFollower.StopSetCamPos();
    }
}
