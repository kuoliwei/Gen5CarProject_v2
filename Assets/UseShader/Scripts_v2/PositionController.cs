using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static MovementModeController;

public class PositionController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Transform cornerScreenTransform;
    Image image;
    bool isPushing;
    [SerializeField][Range(0f, 0.1f)] float speed;
    [SerializeField] MovementModeController movementModeController;
    [SerializeField] ReferenceCameraController referenceCameraController;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPushing && movementModeController.Mode == MovementMode.position)
        {
            if (gameObject.name == "up")
            {
                cornerScreenTransform.position += Vector3.up * Time.deltaTime * speed;
            }
            if (gameObject.name == "down")
            {
                cornerScreenTransform.position += Vector3.down * Time.deltaTime * speed;
            }
            if (gameObject.name == "left")
            {
                cornerScreenTransform.position += Vector3.left * Time.deltaTime * speed;
            }
            if (gameObject.name == "right")
            {
                cornerScreenTransform.position += Vector3.right * Time.deltaTime * speed;
            }
            referenceCameraController.SetPosition();
        }
    }
    public void SetPosition(Vector3 position)
    {
        cornerScreenTransform.position = position;
        referenceCameraController.SetPosition();
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
