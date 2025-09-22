using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransformInfoController : MonoBehaviour
{
    [SerializeField] Text text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetTransformInfo(Transform transform)
    {
        text.text = transform.position.ToString() + "\n" + transform.eulerAngles.ToString() + "\n" + transform.localScale.ToString();
    }
}
