using UnityEditor;
using UnityEngine;

public class KahootValidatorManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject errorPanel;

    private void Start()
    {
        ShowLoading();

        string json = AppManager.Instance.CurrentKahootJson;

        ValidateKahoot(json);
    }

    public void ValidateKahoot(string json)
    {
        ShowLoading();

        bool isValid = KahootValidator.TryValidate(json, out string error);

        if (isValid)
        {
            ShowSuccess();
            Debug.Log("El kahoot es valido");
        }
        else
        {
            ShowError();
            Debug.Log(error);
        }
    }

    private void ShowLoading()
    {
        loadingPanel.SetActive(true);
        successPanel.SetActive(false);
        errorPanel.SetActive(false);
    }

    private void ShowSuccess()
    {
        loadingPanel.SetActive(false);
        successPanel.SetActive(true);
        errorPanel.SetActive(false);
    }

    private void ShowError()
    {
        loadingPanel.SetActive(false);
        successPanel.SetActive(false);
        errorPanel.SetActive(true);
    }

    public void GoToKahootSelect()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToKahootSelector();
    }

    public void GoToKahootPlay()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        ChangeSceneManager.Instance.GoToKahootGame();
    }

}
