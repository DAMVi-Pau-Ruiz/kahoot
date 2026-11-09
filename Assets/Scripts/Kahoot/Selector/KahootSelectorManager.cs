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
            CreateKahootCard(file.text, "Resources/DefaultKahoots/" + file.name + ".json");
        }
    }

    private void LoadUserKahoots()
    {
        string path = Application.persistentDataPath + "/kahoots";

        if (!Directory.Exists(path))
        {
            return;
        }

        string[] files = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            string json = File.ReadAllText(file);
            CreateKahootCard(json, file);
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
        }
    }

    public void GoToMenu()
    {
        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.EDITOR:
                ChangeSceneManager.Instance.GoToEditorMenu();
                break;

            default:
                ChangeSceneManager.Instance.GoToMainMenu();
                break;
        }
    }
}
