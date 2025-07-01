using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public static class CalibrationSaverAsync
{
    /// <summary>
    /// �x�s�ե���ƨ���w���|�P�ɦW
    /// </summary>
    public static async Task SaveToFileAsync(CalibrationParameters data, string savePath, string fileName)
    {
        string fullPath = Path.Combine(savePath, fileName);

        string json = JsonUtility.ToJson(data, true);

        // �T�O�ؿ��s�b
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        await File.WriteAllTextAsync(fullPath, json);
        Debug.Log($"Calibration data saved to: {fullPath}");
    }

    /// <summary>
    /// �q���w���|�P�ɦW���J�ե����
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
