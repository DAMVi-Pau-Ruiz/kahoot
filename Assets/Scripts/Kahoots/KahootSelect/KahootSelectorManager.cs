
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class KahootSelectorManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform content;
    [SerializeField] private KahootCard kahootCardPrefab;

    private string kahootsFolder;

    private void Start()
    {
        Sprite test = Resources.Load<Sprite>("KahootImages/CulturaGeneral");

        if (test == null)
        {
            Debug.LogError("Unity no puede cargar CulturaGeneral.");
        }
        else
        {
            Debug.Log("Imagen cargada correctamente: " + test.name);
        }


        kahootsFolder = Path.Combine(
            Application.persistentDataPath,
            "Kahoots"
        );

        // Creamos la carpeta si todavía no existe.
        Directory.CreateDirectory(kahootsFolder);

        LoadDefaultKahoots();
        LoadUserKahoots();
    }

    private void LoadDefaultKahoots()
    {
        // Cargamos todos los JSON incluidos en el juego.
        TextAsset[] jsonFiles =
            Resources.LoadAll<TextAsset>("DefaultKahoots");

        foreach (TextAsset jsonFile in jsonFiles)
        {
            KahootData data = JsonUtility.FromJson<KahootData>(
                jsonFile.text
            );

            string resourcePath =
                "RESOURCE:DefaultKahoots/" + jsonFile.name;

            CreateKahootCard(data, resourcePath);
        }
    }

    private void LoadUserKahoots()
    {
        // Buscamos todos los archivos JSON del usuario.
        string[] jsonFiles = Directory.GetFiles(
            kahootsFolder,
            "*.json"
        );

        foreach (string jsonPath in jsonFiles)
        {
            try
            {
                string json = File.ReadAllText(jsonPath);

                KahootData data =
                    JsonUtility.FromJson<KahootData>(json);

                CreateKahootCard(data, jsonPath);
            }
            catch (System.Exception exception)
            {
                Debug.LogWarning(
                    "No se pudo leer el Kahoot: " +
                    jsonPath + "\n" + exception.Message
                );
            }
        }
    }

    private void CreateKahootCard(
        KahootData data,
        string jsonPath
    )
    {
        // Comprobamos que tenga los datos obligatorios.
        if (data == null ||
            string.IsNullOrWhiteSpace(data.titulo) ||
            string.IsNullOrWhiteSpace(data.descripcion) ||
            string.IsNullOrWhiteSpace(data.imagen))
        {
            Debug.LogWarning(
                "Kahoot ignorado: faltan datos obligatorios."
            );

            return;
        }

        // Buscamos la imagen correspondiente.
        string imagePath = "KahootImages/" + data.imagen;

        Sprite icon = Resources.Load<Sprite>(imagePath);

        Debug.Log("Buscando imagen en: " + imagePath);

        if (icon == null)
        {
            Debug.LogError("No se ha encontrado la imagen: " + imagePath);
            return;
        }

        if (icon == null)
        {
            Debug.LogWarning(
                "No se encontró la imagen: " + data.imagen
            );

            return;
        }

        // Creamos la tarjeta.
        KahootCard newCard = Instantiate(
            kahootCardPrefab,
            content
        );

        newCard.SetKahoot(
            data.titulo,
            data.descripcion,
            icon,
            jsonPath
        );

        newCard.OnKahootSelected += OnKahootSelected;
    }

    private void OnKahootSelected(string jsonPath)
    {
        Debug.Log("Kahoot seleccionado: " + jsonPath);

        // Más adelante pasaremos a la escena de validación.
    }
}
