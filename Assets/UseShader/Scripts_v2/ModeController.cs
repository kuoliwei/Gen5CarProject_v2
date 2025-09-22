using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    [SerializeField] Dropdown dropdown;
    [SerializeField] RenderTexture rt_Left;
    [SerializeField] RenderTexture rt_Right;
    [SerializeField] RenderTexture rt_Left_Width;
    [SerializeField] RenderTexture rt_Right_Width;
    [SerializeField] RawImage outputCanvas_Left;
    [SerializeField] RawImage outputCanvas_Right;
    [SerializeField] RawImage referenceRawImage_Left;
    [SerializeField] RawImage referenceRawImage_Right;
    [SerializeField] Camera cornerCamera_Left;
    [SerializeField] Camera cornerCamera_Right;
    [SerializeField] Camera[] cornerCameras;
    [SerializeField] Transform[] cornerScreenQuadTransforms;
    [SerializeField] CornorScreenCameraController cornorScreenCameraController;
    [SerializeField] ReferenceCameraController referenceCameraController;
    public enum Mode { _laboratory, _5GCar }
    Mode mode;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetDropdownValue(int value)
    {
        if(dropdown.value != value)
        {
            dropdown.value = value;
        }
        else
        {
            ChangeMode();
        }
    }
    public int GetDropdownValue()
    {
        return dropdown.value;
    }
    public void ChangeMode()
    {
        switch (dropdown.value)
        {
            case 0:
                mode = Mode._laboratory;
                break;
            case 1:
                mode = Mode._5GCar;
                break;
        }
        SetRenderTexture(mode);
        cornorScreenCameraController.SetOrthographicSize();
        referenceCameraController.SetPosition();
    }
    void SetRenderTexture(Mode mode)
    {
        switch (mode)
        {
            case Mode._laboratory:
                cornerCamera_Left.targetTexture = rt_Left;
                cornerCamera_Right.targetTexture = rt_Right;
                outputCanvas_Left.texture = rt_Left;
                outputCanvas_Right.texture = rt_Right;
                referenceRawImage_Left.texture = rt_Left;
                referenceRawImage_Right.texture = rt_Right;
                for (int i = 0; i < 2; i++)
                {
                    cornerScreenQuadTransforms[i].localScale = new Vector3(1, 1, 1);
                }
                break;
            case Mode._5GCar:
                cornerCamera_Left.targetTexture = rt_Left_Width;
                cornerCamera_Right.targetTexture = rt_Right_Width;
                outputCanvas_Left.texture = rt_Left_Width;
                outputCanvas_Right.texture = rt_Right_Width;
                referenceRawImage_Left.texture = rt_Left_Width;
                referenceRawImage_Right.texture = rt_Right_Width;
                for (int i = 0; i < 2; i++)
                {
                    cornerScreenQuadTransforms[i].localScale = new Vector3(1, 10f / 16f, 1);
                }
                break;
        }
    }
}
