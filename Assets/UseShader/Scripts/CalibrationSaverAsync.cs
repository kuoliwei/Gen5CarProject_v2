using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class CalibrationSaverAsync
{
    /// <summary>
    /// 儲存校正資料到指定路徑與檔名
    /// </summary>
    public static async Task SaveToFileAsync(CalibrationParameters data, string savePath, string fileName)
    {
        string fullPath = Path.Combine(savePath, fileName);

        string json = JsonUtility.ToJson(data, true);

        // 確保目錄存在
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        await File.WriteAllTextAsync(fullPath, json);
        Debug.Log($"Calibration data saved to: {fullPath}");
    }

    /// <summary>
    /// 從指定路徑與檔名載入校正資料
    /// </summary>
    public static async Task<CalibrationParameters?> LoadFromFileAsync(string savePath, string fileName)
    {
        string fullPath = Path.Combine(savePath, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"File not found: {fullPath}");
            return null;
        }

        string json = await File.ReadAllTextAsync(fullPath);
        CalibrationParameters data = JsonUtility.FromJson<CalibrationParameters>(json);
        return data;
    }
}
