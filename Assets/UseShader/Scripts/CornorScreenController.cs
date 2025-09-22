using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using System.Threading.Tasks;

//[ExecuteInEditMode]
public class CornorScreenController : MonoBehaviour
{
    [SerializeField] Camera[] outputCameras;
    [SerializeField] Transform[] screenTrans;
    [SerializeField] MeshFilter[] quadMeshes;
    [SerializeField] RenderTexture rt_Left;
    [SerializeField] RenderTexture rt_Right;
    [SerializeField] RenderTexture rt_Left_Width;
    [SerializeField] RenderTexture rt_Right_Width;
    [SerializeField] MeshRenderer quad_Left;
    [SerializeField] MeshRenderer quad_Right;
    [SerializeField] Camera cam_Left;
    [SerializeField] Camera cam_Right;
    [SerializeField] Camera cam_Ground;
    [SerializeField] Dropdown modeChanging;
    public enum Mode { _laboratory, _5GCar }
    public  Mode mode;
    enum Direction { up, down, left, right, none }
    Direction direction = Direction.none;
    enum MovementMode { displacement, rotation }
    MovementMode movementMode = MovementMode.displacement;
    [SerializeField] float displacementSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Text movementModeText;
    enum Zooming { plus, minus, none }
    Zooming zooming = Zooming.none;
    [SerializeField] float zoomingSpeed;
    public Text transformInfoText;
    [SerializeField] Dropdown sceneExchange;
    [SerializeField] CornerScreenFollower cornerScreenFollower;
    [SerializeField] RawImage refViewRawImage_Left;
    [SerializeField] RawImage refViewRawImage_Right;
    [SerializeField] OutputChange outputChange;
    CalibrationParameters calibrationParameters;
    [SerializeField] string jsonPath = "CalibrationData";
    [SerializeField] string jsonName = "CalibrationData.json";
    // Start is called before the first frame update
    void Start()
    {
        //if(Display.displays.Length > 0)
        //{
        //    for (int i = 0; i < Display.displays.Length; i++)
        //    {
        //        if (i > 7) break;
        //        Display.displays[i].Activate();
        //    }
        //}
        init();
    }
    private async void init()
    {
        // 使用 async 方法的最佳方式是這樣包起來呼叫
        calibrationParameters = await LoadProjectNamesToDropdown(Path.Combine(Application.dataPath, jsonPath), jsonName);
        // 提取 projectName 成為選單選項
        SetSceneExchangeOptions(calibrationParameters);
        SelectScene();
    }
    public void SetSceneExchangeOptions(CalibrationParameters calibrationParameters)
    {
        List<string> projectNames = new List<string>();
        foreach (var param in calibrationParameters.calibrationParameters)
        {
            if (!string.IsNullOrEmpty(param.projectName))
                projectNames.Add(param.projectName);
        }
        // 先清空原有選項
        sceneExchange.ClearOptions();

        // 加入新的選項
        sceneExchange.AddOptions(projectNames);
    }
    //private void init()
    //{
    //    // 呼叫非同步方法但不阻塞主線程
    //    string path = Path.Combine(Application.dataPath, "CalibrationData");
    //    string file = "CalibrationData.json";

    //    // 使用 async 方法的最佳方式是這樣包起來呼叫
    //    _ = LoadProjectNamesToDropdown(path, file);
    //    DisplayTransformInfo();
    //    cornerScreenFollower.SendCamPos();
    //}
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || direction == Direction.up)
        {
            if (Input.GetKey(KeyCode.LeftShift) || movementMode == MovementMode.rotation)
            {
                transform.eulerAngles += new Vector3(rotationSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.up * Time.deltaTime * displacementSpeed;
            }
            DisplayTransformInfo();
            cornerScreenFollower.SendCamPos();
        }
        if (Input.GetKey(KeyCode.DownArrow) || direction == Direction.down)
        {
            if (Input.GetKey(KeyCode.LeftShift) || movementMode == MovementMode.rotation)
            {
                transform.eulerAngles -= new Vector3(rotationSpeed, 0, 0) * Time.deltaTime;
            }
            else
            {
                transform.position -= Vector3.up * Time.deltaTime * displacementSpeed;
            }
            cornerScreenFollower.SendCamPos();
            DisplayTransformInfo();
        }
        if (Input.GetKey(KeyCode.LeftArrow) || direction == Direction.left)
        {
            if (Input.GetKey(KeyCode.LeftShift) || movementMode == MovementMode.rotation)
            {
                transform.eulerAngles += new Vector3(0, rotationSpeed, 0) * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.left * Time.deltaTime * displacementSpeed;
            }
            cornerScreenFollower.SendCamPos();
            DisplayTransformInfo();
        }
        if (Input.GetKey(KeyCode.RightArrow) || direction == Direction.right)
        {
            if (Input.GetKey(KeyCode.LeftShift) || movementMode == MovementMode.rotation)
            {
                transform.eulerAngles -= new Vector3(0, rotationSpeed, 0) * Time.deltaTime;
            }
            else
            {
                transform.position -= Vector3.left * Time.deltaTime * displacementSpeed;
            }
            cornerScreenFollower.SendCamPos();
            DisplayTransformInfo();
        }
        if (Input.GetKey(KeyCode.Equals) && Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.KeypadPlus) || zooming == Zooming.plus)
        {
            transform.localScale += Vector3.one * Time.deltaTime * zoomingSpeed;
            SetOutCamOrtSize();
            cornerScreenFollower.SendCamPos();
            DisplayTransformInfo();
        }
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) || zooming == Zooming.minus)
        {
            transform.localScale -= Vector3.one * Time.deltaTime * zoomingSpeed;
            SetOutCamOrtSize();
            cornerScreenFollower.SendCamPos();
            DisplayTransformInfo();
        }
    }
    public void ChangeMode()
    {
        switch (modeChanging.value)
        {
            case 0:
                mode = Mode._laboratory;
                cam_Left.targetTexture = rt_Left;
                quad_Left.sharedMaterial.mainTexture = rt_Left;
                cam_Right.targetTexture = rt_Right;
                quad_Right.sharedMaterial.mainTexture = rt_Right;
                refViewRawImage_Left.texture = rt_Left;
                refViewRawImage_Right.texture = rt_Right;
                for (int i = 0; i < 2; i++)
                {
                    screenTrans[i].localScale = new Vector3(1, 1, 1);

                }
                SetOutCamOrtSize();
                break;
            case 1:
                mode = Mode._5GCar;
                cam_Left.targetTexture = rt_Left_Width;
                quad_Left.sharedMaterial.mainTexture = rt_Left_Width;
                cam_Right.targetTexture = rt_Right_Width;
                quad_Right.sharedMaterial.mainTexture = rt_Right_Width;
                refViewRawImage_Left.texture = rt_Left_Width;
                refViewRawImage_Right.texture = rt_Right_Width;
                for (int i = 0; i < 2; i++)
                {
                    screenTrans[i].localScale = new Vector3(1, 9f / 16f, 1);
                }
                SetOutCamOrtSize();
                break;
        }
    }
    void SetOutCamOrtSize()
    {
        for (int i = 0; i < 3; i++)
        {
            outputCameras[i].orthographicSize = transform.localScale.y * screenTrans[i].localScale.y / 2f;
        }
    }
    void DisplayTransformInfo()
    {
        transformInfoText.text = transform.position.ToString() + "\n" + transform.eulerAngles.ToString() + "\n" + transform.localScale.ToString();
    }
    public void SetDirectionUP()
    {
        direction = Direction.up;
    }
    public void SetDirectionDOWN()
    {
        direction = Direction.down;
    }
    public void SetDirectionLEFT()
    {
        direction = Direction.left;
    }
    public void SetDirectionRIGHT()
    {
        direction = Direction.right;
    }
    public void SetDirectionNONE()
    {
        direction = Direction.none;
    }
    public void ToggleMovementMode()
    {
        if(movementMode==MovementMode.displacement)
        {
            movementMode = MovementMode.rotation;
            Debug.Log(movementMode);
            movementModeText.text = "旋轉";
        }
        else if (movementMode == MovementMode.rotation)
        {
            movementMode = MovementMode.displacement;
            Debug.Log(movementMode);
            movementModeText.text = "位移";
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
    public async Task<CalibrationParameters> LoadProjectNamesToDropdown(string savePath, string fileName)
    {
        string fullPath = Path.Combine(savePath, fileName);
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"[LoadProjectNames] File does not exist: {fullPath}");
            SaveCalibrationParameterData(false);
        }
        var loaded = await JsonFileUtility.LoadFromFileAsync<CalibrationParameters>(savePath, fileName);

        if (!loaded.HasValue)
        {
            Debug.LogWarning("Failed to load calibration file.");
            return new CalibrationParameters();
        }

        return loaded.Value;
    }
    //public async Task LoadProjectNamesToDropdown(string savePath, string fileName)
    //{
    //    string fullPath = Path.Combine(savePath, fileName);
    //    if (!File.Exists(fullPath))
    //    {
    //        Debug.LogWarning($"[LoadProjectNames] File does not exist: {fullPath}");
    //        SaveCalibrationParameterData(false);
    //    }
    //    var loaded = await JsonFileUtility.LoadFromFileAsync<CalibrationParameters>(savePath, fileName);

    //    if (!loaded.HasValue)
    //    {
    //        Debug.LogWarning("Failed to load calibration file.");
    //        return;
    //    }

    //    calibrationParameters = loaded.Value;

    //    // 提取 projectName 成為選單選項
    //    List<string> projectNames = new List<string>();
    //    foreach (var param in calibrationParameters.calibrationParameters)
    //    {
    //        if (!string.IsNullOrEmpty(param.projectName))
    //            projectNames.Add(param.projectName);
    //    }

    //    SetOptions(projectNames);
    //    Debug.Log("Dropdown options updated from calibration file.");
    //    SelectScene();
    //}

    public void SelectScene()
    {
        CalibrationParameter calibrationParameter = calibrationParameters.calibrationParameters[sceneExchange.value];
        if(modeChanging.value == calibrationParameter.mode)
        {
            ChangeMode();
        }
        else
        {
            modeChanging.value = calibrationParameter.mode;
        }

        transform.position = new Vector3(calibrationParameter.position.x, calibrationParameter.position.y, 0);
        transform.eulerAngles = new Vector3(calibrationParameter.eulerAngles.x, calibrationParameter.eulerAngles.y, 0);
        transform.localScale = calibrationParameter.localScale;

        cornerScreenFollower.SetCamDis(calibrationParameter.distance);
        SetOutCamOrtSize();
        DisplayTransformInfo();
    }
    void SaveCalibrationParameterData(bool isFileExists)
    {
        CalibrationParameters calibrationParameters = new CalibrationParameters();
        if (!isFileExists)
        {
            calibrationParameters.calibrationParameters = new CalibrationParameter[]
    {
                new CalibrationParameter
                {
                    projectName = "AutoSave",
                    position = new Vector3(transform.position.x, transform.position.y, 0),
                    eulerAngles =  new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0),
                    localScale = transform.localScale,
                    distance = cornerScreenFollower.GetCamDis(),
                    mode = modeChanging.value
                }
    };
        }
        else
        {
            calibrationParameters = this.calibrationParameters;
            calibrationParameters.calibrationParameters[0] = new CalibrationParameter
            {
                projectName = "AutoSave",
                position = new Vector3(transform.position.x, transform.position.y, 0),
                eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0),
                localScale = transform.localScale,
                distance = cornerScreenFollower.GetCamDis(),
                mode = modeChanging.value
            };
        }

        string savePath = Path.Combine(Application.dataPath, "CalibrationData");
        string fileName = "CalibrationData.json";

        // 儲存
        JsonFileUtility.SaveToFile(calibrationParameters, savePath, fileName);
    }
    void OnApplicationQuit()
    {
        //SaveCalibrationParameterData(true);
    }
}
