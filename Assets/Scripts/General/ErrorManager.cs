using UnityEngine;
using System;
using System.IO;

public class ErrorManager : MonoBehaviour
{
    public static ErrorManager Instance {  get; private set; }

    private string errorFolder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        errorFolder = Path.Combine(Application.persistentDataPath, "errors");

        if (!Directory.Exists(errorFolder))
        {
            Directory.CreateDirectory(errorFolder);
        }
    }

    public void RegisterError(string kahootName, string errorTitle, string errorDescription)
    {
        string safeName = kahootName;

        foreach(char character in Path.GetInvalidFileNameChars())
        {
            safeName = safeName.Replace(character, '_');
        }

        string filePath = Path.Combine(errorFolder, safeName + "_errors.json");

        ErrorLogList logList = new ErrorLogList();

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);

                ErrorLogList loadedList = JsonUtility.FromJson<ErrorLogList>(json);

                if (loadedList == null || loadedList.errors == null)
                {
                    Debug.LogError("El archivo de errores está mal formado: " + filePath);
                    return;
                }

                logList = loadedList;
            }
            catch (Exception e)
            {
                Debug.LogError("Error leyendo el achivo de errores: " + e.Message);
                return;
            }
        }

        ErrorLog newError = new ErrorLog
        {
            kahootName = kahootName,
            date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            errorTitle = errorTitle,
            errorDescription = errorDescription
        };

        logList.errors.Add(newError);

        try
        {
            string newJson = JsonUtility.ToJson(logList, true);

            File.WriteAllText(filePath, newJson);

            Debug.Log("Error registrado correctamente: " + errorTitle);
        }
        catch (Exception e)
        {
            Debug.LogError("Error guardando el registro: " + e.Message);
        }
    }
}
