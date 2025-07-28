using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornorScreenCameraController : MonoBehaviour
{
    [SerializeField] Camera[] cameras;
    [SerializeField] Transform[] cornerScreenQuadRootTransforms;
    [SerializeField] Transform cornerScreenTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetOrthographicSize()
    {
        for (int i = 0; i < 3; i++)
        {
            cameras[i].orthographicSize = cornerScreenTransform.localScale.y * cornerScreenQuadRootTransforms[i].localScale.y / 2f;
        }
    }
}
