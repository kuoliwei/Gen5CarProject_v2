using UnityEngine;
[System.Serializable]
public struct CalibrationParameters
{
    public CalibrationParameter[] calibrationParameters;
}
[System.Serializable]
public struct CalibrationParameter
{
    public string projectName;
    public Vector3 position;
    public Vector3 eulerAngles;
    public Vector3 localScale;
    public float distance;
}
