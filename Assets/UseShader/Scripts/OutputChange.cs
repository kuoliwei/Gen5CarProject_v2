using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class OutputChange : MonoBehaviour
{
    [SerializeField] Camera[] cameras;
    [SerializeField] Dropdown[] dropdowns;
    [SerializeField] RawImage[] rawImages;
    [SerializeField] RenderTexture rt_Left;
    [SerializeField] RenderTexture rt_Right;
    [SerializeField] RenderTexture rt_Left_Width;
    [SerializeField] RenderTexture rt_Right_Width;
    [SerializeField] CornorScreenController cornorScreenController;
    [SerializeField] Camera corCamGnd;
    [SerializeField] Canvas canvas;
    [SerializeField] Dropdown dropdownForCanvas;
    // Start is called before the first frame update
    void Start()
    {
        init();
        string savePath = Path.Combine(Application.dataPath, "DisplayIndexData");
        string fileName = "DisplayIndexData.json";
        
        _ = LoadDisplayIndex(savePath, fileName);
    }
    void init()
    {
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.options.Clear();
        }
        dropdownForCanvas.options.Clear();
        Dropdown.OptionDataList displayList = new Dropdown.OptionDataList();
        //if (Display.displays.Length > 0)
        //{
        //    for (int i = 0; i < Display.displays.Length; i++)
        //    {
        //        if (i > 7) break;
        //        Display.displays[i].Activate();
        //        Dropdown.OptionData displayData = new Dropdown.OptionData();
        //        displayData.text = i.ToString();
        //        Debug.Log(Display.displays[i].ToString());
        //        displayList.options.Add(displayData);
        //    }
        //}
#if UNITY_EDITOR
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.options.Clear();
        }
        dropdownForCanvas.options.Clear();
        for (int i = 0; i < 4; i++)
        {
            Dropdown.OptionData displayData = new Dropdown.OptionData();
            displayData.text = i.ToString();
            displayList.options.Add(displayData);
        }
#endif
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.AddOptions(displayList.options);
        }
        dropdownForCanvas.AddOptions(displayList.options);
        //for (int i = 0; i < dropdowns.Length; i++)
        //{
        //    dropdowns[i].value = cameras[i].targetDisplay;
        //}
        //dropdownForCanvas.value = 0;
        if (cornorScreenController.mode == CornorScreenController.Mode._laboratory)
        {
            rawImages[1].texture = rt_Left;
            rawImages[2].texture = rt_Right;
        }
        else if (cornorScreenController.mode == CornorScreenController.Mode._5GCar)
        {
            rawImages[1].texture = rt_Left_Width;
            rawImages[2].texture = rt_Right_Width;
        }
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.onValueChanged.AddListener(delegate {
                ChangeDisplay();
            });
        }
        dropdownForCanvas.onValueChanged.AddListener(delegate { ChangeDisplay(); });
    }
    
    public void ChangeDisplay()
    {
        for(int i = 0;i < cameras.Length;i++)
        {
            cameras[i].targetDisplay = dropdowns[i].value;
        }
        canvas.targetDisplay = dropdownForCanvas.value;
    }
    public void TurnLeft()
    {
        corCamGnd.transform.localEulerAngles -= new Vector3(0, 0, 90);
    }
    public void TurnRight()
    {
        corCamGnd.transform.localEulerAngles += new Vector3(0, 0, 90);
    }
    public async Task LoadDisplayIndex(string savePath, string fileName)
    {
        string fullPath = Path.Combine(savePath, fileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"[LoadDisplayIndex] File does not exist: {fullPath}");
            SaveDisplayIndexData();
        }
        var loaded = await JsonFileUtility.LoadFromFileAsync<DisplayIndex>(savePath, fileName);

        if (!loaded.HasValue)
        {
            Debug.LogWarning("Failed to load calibration file.");
            return;
        }

        DisplayIndex displayIndex = loaded.Value;

        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = displayIndex.cornerScreenCamsIndex[i];
        }
        dropdownForCanvas.value = displayIndex.uiIndex;
    }
    void SaveDisplayIndexData()
    {
        DisplayIndex displayIndex = new DisplayIndex
        {
            cornerScreenCamsIndex = new[] { cameras[0].targetDisplay, cameras[1].targetDisplay, cameras[2].targetDisplay, cameras[3].targetDisplay },
            uiIndex = dropdownForCanvas.value
        };
        string savePath = Path.Combine(Application.dataPath, "DisplayIndexData");
        string fileName = "DisplayIndexData.json";

        // Àx¦s
        JsonFileUtility.SaveToFile(displayIndex, savePath, fileName);
    }
    void OnApplicationQuit()
    {
        SaveDisplayIndexData();
    }
}
