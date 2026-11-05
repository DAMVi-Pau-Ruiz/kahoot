
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KahootValidationManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject validPanel;
    [SerializeField] private GameObject invalidPanel;

    private void Start()
    {
        ValidateSelectedKahoot();
    }

    private void ValidateSelectedKahoot()
    {
        string path = KahootSession.SelectedKahootPath;

        if (string.IsNullOrEmpty(path))
        {
            ShowInvalidPanel();
            Debug.LogError("No hay ningún Kahoot seleccionado.");
            return;
        }

        string json = "";

        // Comprobar si es un Kahoot de Resources
        if (path.StartsWith("RESOURCE:"))
        {
            string resourcePath = path.Replace("RESOURCE:", "");

            TextAsset jsonFile = Resources.Load<TextAsset>(
                resourcePath
            );

            if (jsonFile == null)
            {
                ShowInvalidPanel();
                Debug.LogError("No se ha encontrado el Kahoot.");
                return;
            }

            json = jsonFile.text;
        }
        else
        {
            // Kahoot creado por el usuario
            if (!File.Exists(path))
            {
                ShowInvalidPanel();
                Debug.LogError("No existe el archivo: " + path);
                return;
            }

            json = File.ReadAllText(path);
        }

        // Revisar el JSON
        List<string> errors = KahootJSONValidator.Validate(json);

        if (errors.Count == 0)
        {
            KahootSession.CurrentKahoot =
                JsonUtility.FromJson<KahootData>(json);

            ShowValidPanel();
        }
        else
        {
            ShowInvalidPanel();

            foreach (string error in errors)
            {
                Debug.LogWarning(error);
            }
        }
    }

    private void ShowValidPanel()
    {
        validPanel.SetActive(true);
        invalidPanel.SetActive(false);
    }

    private void ShowInvalidPanel()
    {
        validPanel.SetActive(false);
        invalidPanel.SetActive(true);
    }

    public void StartKahoot()
    {
        UIManager.Instance.GoToKahootPlay();
    }

    public void BackToSelector()
    {
        UIManager.Instance.GoToKahootSelector();
    }
}
