using UnityEngine;
using System.IO;

public class KahootSelectorManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform content;
    [SerializeField] private KahootCard kahootCardPrefab;

    [Header("Images")]
    [SerializeField] private string imageResourcesFolder = "KahootImages";

    private void Start()
    {
        LoadDefaultKahoots();
        LoadUserKahoots();
    }

    private void LoadDefaultKahoots()
    {
        TextAsset[] files = Resources.LoadAll<TextAsset>("DefaultKahoots");

        foreach (TextAsset file in files)
        {
            string path = "Resources/DefaultKahoots/" + file.name + ".json";

            try
            {
                string json = file.text;
                CreateKahootCard(json, path);
            }
            catch (System.Exception e)
            {
                ErrorManager.Instance.RegisterError(file.name, "Error accediendo al Kahoot", "No se ha podido leer el archivo predeterminado. " + e.Message);
            }
        }
    }

    private void LoadUserKahoots()
    {
        string path = Application.persistentDataPath + "/kahoots";

        if (!Directory.Exists(path))
            return;

        string[] files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            try
            {
                string json = File.ReadAllText(file);
                CreateKahootCard(json, file);
            }
            catch (System.Exception e)
            {
                ErrorManager.Instance.RegisterError(Path.GetFileNameWithoutExtension(file), "Error accediendo al Kahoot", "No se ha podido leer el archivo. " + e.Message);
            }
        }
    }

    private void CreateKahootCard(string json, string path)
    {
        KahootData kahoot = JsonUtility.FromJson<KahootData>(json);

        if (kahoot == null)
        {
            return;
        }

        Sprite portada = null;

        if (!string.IsNullOrEmpty(kahoot.imagen))
        {
            portada = Resources.Load<Sprite>(imageResourcesFolder + "/" + kahoot.imagen);
        }

        KahootCard card = Instantiate(kahootCardPrefab, content);

        card.SetData(kahoot, portada, () => {SelectKahoot(kahoot, json, path);});
    }

    private void SelectKahoot(KahootData kahoot, string json, string path)
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        AppManager.Instance.SetCurrentKahoot(kahoot, json, path);

        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.PLAY:

                Debug.Log("Seleccionado para jugar: " + kahoot.titulo);
                ChangeSceneManager.Instance.GoToKahootValidator();
                break;

            case AppAction.EDITOR:
                Debug.Log("Seleccionado para editar: " + kahoot.titulo);
                ChangeSceneManager.Instance.GoToKahootEditor();
                break;

            case AppAction.LEADERBOARD:
                LeaderboardManager.CreateLeaderboardIfNotExists(AppManager.Instance.CurrentKahootPath);
                ChangeSceneManager.Instance.GoToLeaderboard();
                break;

            case AppAction.ERRORLOG:
                Debug.Log("Seleccionado para consultar errores: " + kahoot.titulo);
                ChangeSceneManager.Instance.GoToErrors();
                break;
        }
    }

    public void GoToMenu()
    {
        AudioManager.Instance.PlaySFX(SFXType.BUTTON_CLICK);
        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.EDITOR:
                ChangeSceneManager.Instance.GoToEditorMenu();
                break;

            case AppAction.ERRORLOG:
                ChangeSceneManager.Instance.GoToErrorsMenu();
                break;

            default:
                ChangeSceneManager.Instance.GoToMainMenu();
                break;
        }
    }
}
