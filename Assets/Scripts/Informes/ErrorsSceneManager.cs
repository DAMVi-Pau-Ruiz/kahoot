
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

public class ErrorsSceneManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform content;
    [SerializeField] private InformCard informCardPrefab;
    [SerializeField] private TMP_Dropdown sortDropdown;

    private List<ErrorLog> errors = new List<ErrorLog>();
    private List<GameObject> instantiatedCards = new List<GameObject>();

    private string errorFolder;

    private void Start()
    {
        errorFolder = Path.Combine(Application.persistentDataPath, "errors");

        ConfigureDropdown();
        LoadErrors();
        ShowErrors();
    }

    // Configura las opciones del Dropdown
    private void ConfigureDropdown()
    {
        sortDropdown.value = 0;
        sortDropdown.onValueChanged.AddListener(OnSortChanged);
    }

    // Carga todos los archivos de errores
    private void LoadErrors()
    {
        errors.Clear();

        if (!Directory.Exists(errorFolder))
            return;

        string[] files = Directory.GetFiles(
            errorFolder,
            "*_errors.json"
        );

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);

                ErrorLogList logList = JsonUtility.FromJson<ErrorLogList>(json);

                if (logList == null || logList.errors == null)
                {
                    Debug.LogError("Archivo de errores no válido: " + file);
                    continue;
                }

                foreach (ErrorLog error in logList.errors)
                {
                    if (error != null)
                        errors.Add(error);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(
                    "No se ha podido cargar el archivo de errores: "
                    + file + "\n" + e.Message
                );
            }
        }
    }

    // Muestra los errores correspondientes a la acción actual
    private void ShowErrors()
    {
        List<ErrorLog> errorsToShow = new List<ErrorLog>();

        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.ERRORS:
                errorsToShow.AddRange(errors);
                break;

            case AppAction.ERRORLOG:
                string kahootName = Path.GetFileNameWithoutExtension(
                    AppManager.Instance.CurrentKahootPath
                );

                foreach (ErrorLog error in errors)
                {
                    if (error.kahootName == kahootName)
                        errorsToShow.Add(error);
                }
                break;
        }

        SortErrors(errorsToShow, sortDropdown.value);
        CreateCards(errorsToShow);
    }

    // Ordena los errores según el Dropdown
    private void SortErrors(List<ErrorLog> errorsToSort, int option)
    {
        errorsToSort.Sort((a, b) =>
        {
            switch (option)
            {
                case 0:
                    return string.Compare(
                        a.kahootName,
                        b.kahootName,
                        StringComparison.OrdinalIgnoreCase
                    );

                case 1:
                    return string.Compare(
                        b.kahootName,
                        a.kahootName,
                        StringComparison.OrdinalIgnoreCase
                    );

                case 2:
                    return GetDate(b).CompareTo(GetDate(a));

                case 3:
                    return GetDate(a).CompareTo(GetDate(b));

                default:
                    return 0;
            }
        });
    }

    // Convierte la fecha del error a DateTime
    private DateTime GetDate(ErrorLog error)
    {
        DateTime date;

        DateTime.TryParseExact(
            error.date,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out date
        );

        return date;
    }

    // Genera las tarjetas en la interfaz
    private void CreateCards(List<ErrorLog> errorsToShow)
    {
        ClearCards();

        foreach (ErrorLog error in errorsToShow)
        {
            InformCard card = Instantiate(informCardPrefab, content);

            card.SetInformation(error);

            instantiatedCards.Add(card.gameObject);
        }
    }

    // Elimina las tarjetas anteriores
    private void ClearCards()
    {
        foreach (GameObject card in instantiatedCards)
        {
            Destroy(card);
        }

        instantiatedCards.Clear();
    }

    // Se ejecuta cuando cambia el Dropdown
    private void OnSortChanged(int option)
    {
        ShowErrors();
    }

    private void OnDestroy()
    {
        sortDropdown.onValueChanged.RemoveListener(OnSortChanged);
    }

    public void GoBack()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);

        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.ERRORS:
                ChangeSceneManager.Instance.GoToErrorsMenu();
                break;

            case AppAction.ERRORLOG:
                ChangeSceneManager.Instance.GoToKahootSelector();
                break;
        }
    }
}
