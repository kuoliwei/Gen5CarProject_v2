using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementModeController : MonoBehaviour
{
    [SerializeField] Text text;
    public enum MovementMode { position, rotation }
    MovementMode mode = MovementMode.position;
    public MovementMode Mode => mode;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMovementMode()
    {
        if (mode == MovementMode.position)
        {
            mode = MovementMode.rotation;
            Debug.Log(mode);
            text.text = "±ÛÂà";
        }
        else if (mode == MovementMode.rotation)
        {
            mode = MovementMode.position;
            Debug.Log(mode);
            text.text = "¦ì²¾";
        }
    }
}
