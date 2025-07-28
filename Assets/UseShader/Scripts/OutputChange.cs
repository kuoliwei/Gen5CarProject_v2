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
    [SerializeField] CornorScreenController cornorScreenController;
    [SerializeField] Camera corCamGnd;
    [SerializeField] Canvas canvas;
    [SerializeField] Dropdown dropdownForCanvas;
    string jsonPath = "DisplayIndexData";
    string jsonName = "DisplayIndexData.json";
    // Start is called before the first frame update
    void Start()
    {
        init();
    }
    async void init()
    {
        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.options.Clear();
        }
        dropdownForCanvas.options.Clear();
        Dropdown.OptionDataList displayList = new Dropdown.OptionDataList();
#if !UNITY_EDITOR
        if (Display.displays.Length > 0)
        {
            for (int i = 0; i < Display.displays.Length; i++)
            {
                if (i > 7) break;
                Display.displays[i].Activate();
                Dropdown.OptionData displayData = new Dropdown.OptionData();
                displayData.text = i.ToString();
                Debug.Log(Display.displays[i].ToString());
                displayList.options.Add(displayData);
            }
        }
#endif
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
        DisplayIndex displayIndex = await LoadDisplayIndex(Path.Combine(Application.dataPath, jsonPath), jsonName);
        corCamGnd.transform.localEulerAngles = new Vector3(0, 0, displayIndex.groundDir);
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = displayIndex.cornerScreenCamsIndex[i];
        }
        dropdownForCanvas.value = displayIndex.uiIndex;
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
        corCamGnd.transform.localEulerAngles = new Vector3(0, 0, (int)corCamGnd.transform.localEulerAngles.z - 90);
    }
    public void TurnRight()
    {
        corCamGnd.transform.localEulerAngles = new Vector3(0, 0, (int)corCamGnd.transform.localEulerAngles.z + 90);
    }
    public async Task<DisplayIndex> LoadDisplayIndex(string savePath, string fileName)
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
            return new DisplayIndex();
        }

        return loaded.Value;
    }
    //public async Task LoadDisplayIndex(string savePath, string fileName)
    //{
    //    string fullPath = Path.Combine(savePath, fileName);
    //    if (!File.Exists(fullPath))
    //    {
    //        Debug.LogWarning($"[LoadDisplayIndex] File does not exist: {fullPath}");
    //        SaveDisplayIndexData();
    //    }
    //    var loaded = await JsonFileUtility.LoadFromFileAsync<DisplayIndex>(savePath, fileName);

    //    if (!loaded.HasValue)
    //    {
    //        Debug.LogWarning("Failed to load calibration file.");
    //        return;
    //    }

    //    DisplayIndex displayIndex = loaded.Value;

    //    for (int i = 0; i < dropdowns.Length; i++)
    //    {
    //        dropdowns[i].value = displayIndex.cornerScreenCamsIndex[i];
    //    }
    //    dropdownForCanvas.value = displayIndex.uiIndex;
    //}
    void SaveDisplayIndexData()
    {
        DisplayIndex displayIndex = new DisplayIndex
        {
            cornerScreenCamsIndex = new[] { cameras[0].targetDisplay, cameras[1].targetDisplay, cameras[2].targetDisplay, cameras[3].targetDisplay },
            uiIndex = dropdownForCanvas.value,
            groundDir = (int)corCamGnd.transform.localEulerAngles.z
        };

        // Àx¦s
        JsonFileUtility.SaveToFile(displayIndex, Path.Combine(Application.dataPath, jsonPath), jsonName);
    }
    void OnApplicationQuit()
    {
        SaveDisplayIndexData();
    }
}
