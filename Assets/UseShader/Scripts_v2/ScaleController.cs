using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScaleController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Transform cornerScreenTransform;
    Image image;
    bool isPushing;
    [SerializeField][Range(0f, 0.1f)] float speed;
    [SerializeField] ReferenceCameraController referenceCameraController;
    [SerializeField] CornorScreenCameraController cornorScreenCameraController;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPushing)
        {
            if (gameObject.name == "plus")
            {
                cornerScreenTransform.localScale += Vector3.one * Time.deltaTime * speed;
            }
            if (gameObject.name == "minus")
            {
                cornerScreenTransform.localScale -= Vector3.one * Time.deltaTime * speed;
            }
            referenceCameraController.SetPosition();
            cornorScreenCameraController.SetOrthographicSize();
        }
    }
    public void SetScale(Vector3 scale)
    {
        cornerScreenTransform.localScale = scale;
        referenceCameraController.SetPosition();
        cornorScreenCameraController.SetOrthographicSize();
    }
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        image.color = Color.gray;
        isPushing = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        isPushing = false;
    }
}
