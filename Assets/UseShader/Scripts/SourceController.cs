using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SourceController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] MeshRenderer[] meshRenderers;
    Image image;
    [SerializeField] [Range(0f, 1f)] float ratio;
    bool isChanging = false;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChanging)
        {
            if (gameObject.name == "plus")
            {
                ChangeWidth(1 * ratio * Time.deltaTime);
            }
            if (gameObject.name == "minus")
            {
                ChangeWidth(-1 * ratio * Time.deltaTime);
            }
        }
    }
    public void SetWidth(float width)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.sharedMaterial.SetFloat("_Width", width);
        }
    }
    void ChangeWidth(float quantity)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.sharedMaterial.SetFloat("_Width", renderer.sharedMaterial.GetFloat("_Width") + quantity);
        }
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        image.color = Color.gray;
        isChanging = true;
    }

    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        image.color = Color.white;
        isChanging = false;
    }
}
