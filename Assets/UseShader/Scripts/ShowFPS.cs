using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
[ExecuteInEditMode]
public class ShowFPS : MonoBehaviour
{
    [SerializeField] Text fpsText;
    float _fps;
    private void Start()
    {
        Time.fixedDeltaTime = 0.1f;
    }
    private void Update()
    {
        _fps = 1.0f / Time.deltaTime;
    }
    private void FixedUpdate()
    {
        fpsText.text = Mathf.Ceil(_fps).ToString();
    }
}
