using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CornerScreenFollower : MonoBehaviour
{
    [SerializeField] MeshRenderer[] meshRenderers;
    [SerializeField] MeshFilter[] meshFilters;
    [SerializeField] Camera camera;
    [SerializeField] float zoomingSpeed;
    [SerializeField] Text camDis;
    enum Zooming { plus, minus, none }
    Zooming zooming = Zooming.none;
    bool isChenging = false;
    // Start is called before the first frame update
    void Start()
    {
        SendCamPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChenging)
        {
            SendCamPos();
        }
        if(zooming == Zooming.plus)
        {
            camera.transform.position -= new Vector3(0, 0, Time.deltaTime * zoomingSpeed);
            foreach (MeshRenderer renderer in meshRenderers)
            {
                SendCamPos();
            }
        }
        if (zooming == Zooming.minus)
        {
            camera.transform.position += new Vector3(0, 0, Time.deltaTime * zoomingSpeed);
            foreach (MeshRenderer renderer in meshRenderers)
            {
                SendCamPos();
            }
        }
    }
    public void SetCamPos()
    {
        isChenging = true;
    }
    public void StopSetCamPos()
    {
        isChenging = false;
    }
    public void SendCamPos()
    {
        float left = meshFilters[0].transform.TransformPoint(meshFilters[0].mesh.vertices[3]).x;
        float right = meshFilters[1].transform.TransformPoint(meshFilters[0].mesh.vertices[3]).x;
        float up = meshFilters[0].transform.TransformPoint(meshFilters[0].mesh.vertices[1]).y;
        float down = meshFilters[2].transform.TransformPoint(meshFilters[2].mesh.vertices[3]).y;
        float width = Mathf.Abs(left - right);
        float height = Mathf.Abs(up - down);
        Vector2 middle = new Vector2(left + width / 2f, down + height / 2f);
        camera.transform.position = new Vector3(middle.x, middle.y, camera.transform.position.z);
        SetFOV(height, width);
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.sharedMaterial.SetFloat("_CamPosX", camera.transform.position.x);
            renderer.sharedMaterial.SetFloat("_CamPosY", camera.transform.position.y);
            renderer.sharedMaterial.SetFloat("_CamPosZ", camera.transform.position.z);
        }
        this.camDis.text = camera.transform.position.z.ToString();
    }
    void SetFOV(float height, float width)
    {
        if (height > width)
        {
            camera.fieldOfView = Mathf.Atan(height / 2 / -camera.transform.position.z) * Mathf.Rad2Deg * 2;
        }
        else if (width > height)
        {
            camera.fieldOfView = Mathf.Atan(width / 2 / -camera.transform.position.z) * Mathf.Rad2Deg * 2;
        }
    }
    public void SetZoomingPLUS()
    {
        zooming = Zooming.plus;
    }
    public void SetZoomingMINUS()
    {
        zooming = Zooming.minus;
    }
    public void SetZoomingNONE()
    {
        zooming = Zooming.none;
    }
    public void SetCamDis(float dis)
    {
        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, dis);
        SendCamPos();
    }
    public float GetCamDis()
    {
        return camera.transform.position.z;
    }
}
