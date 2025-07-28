using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static MovementModeController;

public class RotationController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Transform cornerScreenTransform;
    Image image;
    bool isPushing;
    [SerializeField][Range(0f, 10f)] float speed;
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
        if (isPushing && movementModeController.Mode == MovementMode.rotation)
        {
            if (gameObject.name == "up")
            {
                cornerScreenTransform.eulerAngles += new Vector3(1, 0, 0) * Time.deltaTime * speed;
            }
            if (gameObject.name == "down")
            {
                cornerScreenTransform.eulerAngles += new Vector3(-1, 0, 0) * Time.deltaTime * speed;
            }
            if (gameObject.name == "left")
            {
                cornerScreenTransform.eulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * speed;
            }
            if (gameObject.name == "right")
            {
                cornerScreenTransform.eulerAngles += new Vector3(0, -1, 0) * Time.deltaTime * speed;
            }
            referenceCameraController.SetPosition();
        }
    }
    public void SetPosition(Vector3 eulerAngles)
    {
        cornerScreenTransform.eulerAngles = eulerAngles;
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
