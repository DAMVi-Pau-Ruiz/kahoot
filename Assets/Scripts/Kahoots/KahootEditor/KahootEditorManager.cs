
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class KahootEditorManager : MonoBehaviour
{
    [Header("Question Editor")]
    [SerializeField] private GameObject questionPrefab;
    [SerializeField] private Transform content;

    [Header("Kahoot Information")]
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField descriptionInput;
    [SerializeField] private KahootImageSelector imageSelector;

    [Header("Save Feedback")]
    [SerializeField] private TMP_Text feedbackText;

    private int questionCount = 0;

    private void Start()
    {
        if (KahootEditorSession.IsEditing)
        {
            LoadExistingKahoot();
        }
        else
        {
            AddQuestion();
        }
    }

    public void AddQuestion()
    {
        Instantiate(questionPrefab, content);
        questionCount++;
    }

    public void DeleteQuestion()
    {
        if (questionCount <= 1)
        {
            return;
        }

        QuestionEditor[] questions =
            content.GetComponentsInChildren<QuestionEditor>();

        if (questions.Length > 1)
        {
            Destroy(questions[questions.Length - 1].gameObject);
            questionCount--;
        }
    }

    private void LoadExistingKahoot()
    {
        string filePath = KahootEditorSession.FilePath;

        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            Debug.LogError("No se encuentra el archivo del Kahoot.");
            feedbackText.text = "No se ha podido cargar el Kahoot.";
            AddQuestion();
            return;
        }

        string json = File.ReadAllText(filePath);

        KahootData kahoot =
            JsonUtility.FromJson<KahootData>(json);

        if (kahoot == null)
        {
            Debug.LogError("No se ha podido leer el JSON.");
            feedbackText.text = "Error al cargar el Kahoot.";
            AddQuestion();
            return;
        }

        // Cargamos la información general.
        titleInput.text = kahoot.titulo;
        descriptionInput.text = kahoot.descripcion;
        imageSelector.SetSelectedImage(kahoot.imagen);

        // Cargamos todas las preguntas.
        foreach (PreguntaData questionData in kahoot.preguntas)
        {
            GameObject newQuestion =
                Instantiate(questionPrefab, content);

            QuestionEditor questionEditor =
                newQuestion.GetComponent<QuestionEditor>();

            questionEditor.SetQuestionData(questionData);

            questionCount++;
        }

        // Si el Kahoot no tenía preguntas, creamos una.
        if (questionCount == 0)
        {
            AddQuestion();
        }
    }

    public void SaveKahoot()
    {
        KahootData kahoot = new KahootData();

        kahoot.titulo = titleInput.text.Trim();
        kahoot.descripcion = descriptionInput.text.Trim();
        kahoot.imagen = imageSelector.GetSelectedImage();

        kahoot.preguntas = new List<PreguntaData>();

        QuestionEditor[] questions =
            content.GetComponentsInChildren<QuestionEditor>();

        foreach (QuestionEditor question in questions)
        {
            kahoot.preguntas.Add(question.GetQuestionData());
        }

        // Convertimos a JSON.
        string json = JsonUtility.ToJson(kahoot, true);

        // Validamos.
        List<string> errors = KahootJSONValidator.Validate(json);

        if (errors.Count > 0)
        {
            feedbackText.text = "Hay errores en el Kahoot.";

            foreach (string error in errors)
            {
                Debug.LogError(error);
            }

            return;
        }

        string filePath;

        if (KahootEditorSession.IsEditing)
        {
            // EDITAR: utilizamos la ruta original.
            filePath = KahootEditorSession.FilePath;
        }
        else
        {
            // CREAR: generamos un archivo nuevo.
            string folderPath = Path.Combine(
                Application.persistentDataPath, "Kahoots");

            Directory.CreateDirectory(folderPath);

            string fileName =
                MakeSafeFileName(kahoot.titulo) + ".json";

            filePath = Path.Combine(folderPath, fileName);
        }

        // Guardamos o reemplazamos el archivo.
        File.WriteAllText(filePath, json);

        // A partir de ahora, si vuelve a guardar,
        // modificará el mismo archivo.
        KahootEditorSession.StartEditing(filePath);

        feedbackText.text = "¡Kahoot guardado correctamente!";

        Debug.Log("Kahoot guardado en: " + filePath);

        UIManager.Instance.GoToMainMenu();
    }

    private string MakeSafeFileName(string name)
    {
        foreach (char character in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(character, '_');
        }

        return name;
    }
}
