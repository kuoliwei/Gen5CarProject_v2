using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

public class DisplaySwitch : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Canvas[] outCanvases;
    [SerializeField] Dropdown[] dropdowns;
    [SerializeField] Camera gndCamera;
    [SerializeField] Canvas uiCanvas;
    [SerializeField] Dropdown dropdownForUICanvas;
    [SerializeField] string jsonPath = "DisplayIndexData";
    [SerializeField] string jsonName = "DisplayIndexData.json";
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
        dropdownForUICanvas.options.Clear();
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
        dropdownForUICanvas.AddOptions(displayList.options);
        DisplayIndex displayIndex = await LoadDisplayIndex(jsonPath, jsonName);
        gndCamera.transform.localEulerAngles = new Vector3(0, 0, displayIndex.groundDir);
        for (int i = 0; i < dropdowns.Length; i++)
        {
            dropdowns[i].value = displayIndex.cornerScreenCamsIndex[i];
        }
        dropdownForUICanvas.value = displayIndex.uiIndex;
    }

    public void ChangeDisplay()
    {
        mainCamera.targetDisplay = dropdowns[0].value;
        for (int i = 1; i < dropdowns.Length; i++)
        {
            outCanvases[i - 1].targetDisplay = dropdowns[i].value;
        }
        uiCanvas.targetDisplay = dropdownForUICanvas.value;
    }
    public void TurnLeft()
    {
        gndCamera.transform.localEulerAngles = new Vector3(0, 0, (int)gndCamera.transform.localEulerAngles.z - 90);
    }
    public void TurnRight()
    {
        gndCamera.transform.localEulerAngles = new Vector3(0, 0, (int)gndCamera.transform.localEulerAngles.z + 90);
    }
    public async Task<DisplayIndex> LoadDisplayIndex(string jsonPath, string jsonName)
    {
        string savePath = Path.Combine(Application.dataPath, jsonPath);
        string fullPath = Path.Combine(savePath, jsonName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"[LoadDisplayIndex] File does not exist: {fullPath}");
            await SaveDisplayIndexData(jsonPath, jsonName);
        }
        var loaded = await JsonFileUtility.LoadFromFileAsync<DisplayIndex>(savePath, jsonName);

        if (!loaded.HasValue)
        {
            Debug.LogWarning("Failed to load calibration file.");
            return new DisplayIndex();
        }

        return loaded.Value;
    }
    async Task SaveDisplayIndexData(string jsonPath, string jsonName)
    {
        DisplayIndex displayIndex = new DisplayIndex
        {
            cornerScreenCamsIndex = new[] { mainCamera.targetDisplay, outCanvases[0].targetDisplay, outCanvases[1].targetDisplay, outCanvases[2].targetDisplay },
            uiIndex = dropdownForUICanvas.value,
            groundDir = (int)gndCamera.transform.localEulerAngles.z
        };
        string savePath = Path.Combine(Application.dataPath, jsonPath);
        // Àx¦s
        await JsonFileUtility.SaveToFileAsync(displayIndex, savePath, jsonName);
    }
    void OnApplicationQuit()
    {
        SaveDisplayIndexData(jsonPath, jsonName);
    }
}
