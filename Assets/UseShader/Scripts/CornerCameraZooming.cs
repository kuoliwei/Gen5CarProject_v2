using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CornerCameraZooming : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] CornerScreenFollower cornerScreenFollower;
    Image image;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        image.color = Color.gray;
        if (gameObject.name == "plus")
        {
            cornerScreenFollower.SetZoomingPLUS();
        }
        if (gameObject.name == "minus")
        {
            cornerScreenFollower.SetZoomingMINUS();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        cornerScreenFollower.SetZoomingNONE();
    }
}
