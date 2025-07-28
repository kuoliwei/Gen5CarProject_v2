using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DistanceController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Transform referenceCameraTransform;
    Image image;
    bool isPushing;
    [SerializeField][Range(0f, 0.1f)] float speed;
    [SerializeField] ReferenceCameraController referenceCameraController;
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
                referenceCameraTransform.position += new Vector3(0, 0, -1) * Time.deltaTime * speed;
            }
            if (gameObject.name == "minus")
            {
                referenceCameraTransform.position += new Vector3(0, 0, 1) * Time.deltaTime * speed;
            }
            referenceCameraController.SetPosition();
        }
    }
    public void SetDistance(Vector3 distance)
    {
        referenceCameraTransform.position = distance;
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
