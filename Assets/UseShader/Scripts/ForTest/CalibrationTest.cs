using UnityEngine;
using System.Threading.Tasks;
using System.IO;

public class CalibrationTest : MonoBehaviour
{
    void Start()
    {
        //string savePath = Path.Combine(Application.dataPath, "CalibrationData");
        //string fileName = "Test.json";

        //// 儲存
        //_ = JsonFileUtility.SaveToFileAsync(CreateTestData(), savePath, fileName);

        //// 載入
        //_ = LoadCalibrationParametersAsync(savePath, fileName);

        string savePath = Path.Combine(Application.dataPath, "DisplayIndexData");
        string fileName = "Test.json";

        // 儲存
        _ = JsonFileUtility.SaveToFileAsync(CreateDisplayTestData(), savePath, fileName);

        // 載入
        _ = LoadDisplayIndexAsync(savePath, fileName);
    }

    CalibrationParameters CreateTestData()
    {
        return new CalibrationParameters
        {
            calibrationParameters = new CalibrationParameter[]
            {
                new CalibrationParameter
                {
                    projectName = "Camera_A",
                    position = new Vector3(1, 2, 3),
                    eulerAngles = new Vector3(0, 45, 0),
                    localScale = Vector3.one,
                    distance = 5.0f
                }
            }
        };
    }
    DisplayIndex CreateDisplayTestData()
    {
        return new DisplayIndex
        {
            cornerScreenCamsIndex = new[] { 0, 1, 3, 2 },
            uiIndex = 0
        };
    }

    async Task LoadCalibrationParametersAsync(string path, string file)
    {
        var loaded = await JsonFileUtility.LoadFromFileAsync<CalibrationParameters>(path, file);
        if (loaded.HasValue)
        {
            CalibrationParameters data = loaded.Value;

            Debug.Log($"Loaded {data.calibrationParameters.Length} calibration entries:");

            for (int i = 0; i < data.calibrationParameters.Length; i++)
            {
                CalibrationParameter p = data.calibrationParameters[i];
                Debug.Log($"--- Entry {i} ---\n" +
                          $"Project Name : {p.projectName}\n" +
                          $"Position     : {p.position}\n" +
                          $"EulerAngles  : {p.eulerAngles}\n" +
                          $"LocalScale   : {p.localScale}\n" +
                          $"Distance     : {p.distance}");
            }
        }
        else
        {
            Debug.LogWarning("Failed to load calibration data.");
        }
    }
    async Task LoadDisplayIndexAsync(string path, string file)
    {
        var loaded = await JsonFileUtility.LoadFromFileAsync<DisplayIndex>(path, file);
        string msg = "";
        if (loaded.HasValue)
        {
            DisplayIndex data = loaded.Value;

            for (int i = 0; i < data.cornerScreenCamsIndex.Length; i++)
            {
                msg += data.cornerScreenCamsIndex[i] + ", ";
            }
            msg += data.uiIndex;
            Debug.Log($"{msg}");
        }
        else
        {
            Debug.LogWarning("Failed to load DisplayIndex data.");
        }
    }
}
