using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReferenceCameraController : MonoBehaviour
{
    Camera referenceCamera;
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] MeshFilter[] meshFilters;
    // Start is called before the first frame update
    void Start()
    {
        referenceCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPosition()
    {
        Vector3 position = CalculatePosition(out float height, out float width);
        referenceCamera.transform.position = position;
        SetShaderParameter(position);
        SetFOV(height, width);
    }
    Vector3 CalculatePosition(out float height, out float width)
    {
        float left = meshFilters[0].transform.TransformPoint(meshFilters[0].mesh.vertices[3]).x;
        float right = meshFilters[1].transform.TransformPoint(meshFilters[0].mesh.vertices[3]).x;
        float up = meshFilters[0].transform.TransformPoint(meshFilters[0].mesh.vertices[1]).y;
        float down = meshFilters[2].transform.TransformPoint(meshFilters[2].mesh.vertices[3]).y;
        width = Mathf.Abs(left - right);
        height = Mathf.Abs(up - down);
        Vector2 middle = new Vector2(left + width / 2f, down + height / 2f);
        return new Vector3(middle.x, middle.y, transform.position.z);
    }
    void SetFOV(float height, float width)
    {
        if (height > width)
        {
            referenceCamera.fieldOfView = Mathf.Atan(height / 2 / -referenceCamera.transform.position.z) * Mathf.Rad2Deg * 2;
        }
        else if (width > height)
        {
            referenceCamera.fieldOfView = Mathf.Atan(width / 2 / -referenceCamera.transform.position.z) * Mathf.Rad2Deg * 2;
        }
    }
    void SetShaderParameter(Vector3 position)
    {
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.sharedMaterial.SetFloat("_CamPosX", position.x);
            renderer.sharedMaterial.SetFloat("_CamPosY", position.y);
            renderer.sharedMaterial.SetFloat("_CamPosZ", position.z);
        }
    }
}
