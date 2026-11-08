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
            CreateKahootCard(file.text);
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
            CreateKahootCard(json);
        }
    }

    private void CreateKahootCard(string json)
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

        card.SetData(kahoot, portada, () => {SelectKahoot(kahoot);});
    }

    private void SelectKahoot(KahootData kahoot)
    {
        switch (AppManager.Instance.CurrentAction)
        {
            case AppAction.PLAY:
                Debug.Log("Seleccionado para jugar: " + kahoot.titulo);
                // Aquí verificaremos el JSON y comenzaremos la partida.
                break;

            case AppAction.EDITOR:
                // Aquí abriremos el editor.
                break;

            case AppAction.LEADERBOARD:
                // Aquí abriremos el leaderboard.
                break;
        }
    }
}
