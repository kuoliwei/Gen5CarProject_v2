using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] PositionController positionController;
    [SerializeField] RotationController rotationController;
    [SerializeField] ScaleController scaleController;
    [SerializeField] DistanceController distanceController;
    [SerializeField] ModeController modeController;
    [SerializeField] Dropdown dropdown;
    CalibrationParameters calibrationParameters;
    [SerializeField] string jsonPath = "CalibrationData";
    [SerializeField] string jsonName = "CalibrationData.json";
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async void Init()
    {
        calibrationParameters = await LoadProjectNamesToDropdown(jsonPath, jsonName);
        SetDropdownOptions(calibrationParameters);
        SetScene(calibrationParameters.calibrationParameters[0]);
    }
    public void OnSceneSelected()
    {
        SetScene(calibrationParameters.calibrationParameters[dropdown.value]);
    }
    void SetScene(CalibrationParameter calibrationParameter)
    {
        positionController.SetPosition(calibrationParameter.position);
        rotationController.SetRotation(calibrationParameter.eulerAngles);
        scaleController.SetScale(calibrationParameter.localScale);
        distanceController.SetDistance(calibrationParameter.distance);
        modeController.SetDropdownValue(calibrationParameter.mode);
    }
    async Task<CalibrationParameters> LoadProjectNamesToDropdown(string jsonPath, string jsonName)
    {
        string savePath = Path.Combine(Application.dataPath, jsonPath);
        string fullPath = Path.Combine(savePath, jsonName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"[LoadProjectNames] File does not exist: {fullPath}");
            await SaveCalibrationParameterData(jsonPath, jsonName, false);
        }
        var loaded = await JsonFileUtility.LoadFromFileAsync<CalibrationParameters>(savePath, jsonName);

        if (!loaded.HasValue)
        {
            Debug.LogWarning("Failed to load calibration file.");
            return new CalibrationParameters();
        }

        return loaded.Value;
    }
    async Task SaveCalibrationParameterData(string jsonPath, string jsonName, bool isFileExists)
    {
        string savePath = Path.Combine(Application.dataPath, jsonPath);
        Vector3 position = positionController.GetPosition();
        Vector3 rotation = rotationController.GetRotation();
        Vector3 scale = scaleController.GetScale();
        CalibrationParameters calibrationParameters = new CalibrationParameters();
        if (!isFileExists)
        {
            calibrationParameters.calibrationParameters = new CalibrationParameter[]
    {
                new CalibrationParameter
                {
                    projectName = "AutoSave",
                    position = new Vector3(position.x, position.y, 0),
                    eulerAngles =  new Vector3(rotation.x, rotation.y, 0),
                    localScale = scale,
                    distance = distanceController.GetDistance(),
                    mode = modeController.GetDropdownValue()
                },
                new CalibrationParameter
                {
                    projectName = "Default",
                    position = new Vector3(0, 0, 0),
                    eulerAngles =  new Vector3(330, 0, 0),
                    localScale = new Vector3(0.5f, 0.5f, 0.5f),
                    distance = -0.8660254f,
                    mode = 0
                }
    };
        }
        else
        {
            calibrationParameters = this.calibrationParameters;
            calibrationParameters.calibrationParameters[0] = new CalibrationParameter
            {
                projectName = "AutoSave",
                position = new Vector3(position.x, position.y, 0),
                eulerAngles = new Vector3(rotation.x, rotation.y, 0),
                localScale = scale,
                distance = distanceController.GetDistance(),
                mode = modeController.GetDropdownValue()
            };
        }
        // 儲存
        await JsonFileUtility.SaveToFileAsync(calibrationParameters, savePath, jsonName);
    }
    void SetDropdownOptions(CalibrationParameters calibrationParameters)
    {
        List<string> projectNames = new List<string>();
        foreach (var param in calibrationParameters.calibrationParameters)
        {
            if (!string.IsNullOrEmpty(param.projectName))
                projectNames.Add(param.projectName);
        }
        // 先清空原有選項
        dropdown.ClearOptions();
        // 加入新的選項
        dropdown.AddOptions(projectNames);
    }
    void OnApplicationQuit()
    {
        SaveCalibrationParameterData(jsonPath, jsonName, true);
    }
}
