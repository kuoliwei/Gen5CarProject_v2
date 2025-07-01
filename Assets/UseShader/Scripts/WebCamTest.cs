using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
[ExecuteInEditMode]
public class WebCamTest : MonoBehaviour
{
    WebCamDevice[] webCamDevices;
    WebCamTexture currentDevice;
    [SerializeField] MeshRenderer[] renderers;
    // Start is called before the first frame update
    void Start()
    {
        init();
    }
    private void init()
    {
        webCamDevices = WebCamTexture.devices;
        foreach (WebCamDevice device in webCamDevices)
        {
            Debug.Log(device.name);
            if (device.name.Contains("OBS"))
            {
                currentDevice = new WebCamTexture(device.name);
                //renderer.material.mainTexture = new Texture2D(1920,1080);
                foreach (MeshRenderer renderer in renderers)
                {
                    renderer.sharedMaterial.mainTexture = currentDevice;
                    Debug.Log(renderer.sharedMaterial.mainTexture.width + "," + renderer.sharedMaterial.mainTexture.height);
                }
                currentDevice.Play();
                Debug.Log("Now using " + device.name);
                Debug.Log(currentDevice.width + "," + currentDevice.height);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
