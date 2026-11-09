using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;
using System.IO;

public class KahootEditorManager : MonoBehaviour
{
    [Header("General Information")]
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private TMP_InputField descriptionInput;

    [Header("Image Selector")]
    [SerializeField] private KahootEditorImageSelector imageSelector;

    [Header("Questions")]
    [SerializeField] private Transform questionsContent;
    [SerializeField] private QuestionEditorCard questionPrefab;

    private List<QuestionEditorCard> questions = new List<QuestionEditorCard>();

    private void Start()
    {
        switch (AppManager.Instance.EditorMode)
        {
            case KahootEditorMode.CREATE:
                CreateEmptyKahoot();
                break;

            case KahootEditorMode.EDIT:
                LoadKahoot();
                break;
        }
    }

    private void CreateEmptyKahoot()
    {
        titleInput.text = "";
        descriptionInput.text = "";
        
        AddQuestion();
    }

    private void LoadKahoot()
    {
        KahootData kahoot = AppManager.Instance.CurrentKahoot;

        titleInput.text = kahoot.titulo;
        descriptionInput.text = kahoot.descripcion;

        imageSelector.SetSelectedImage(kahoot.imagen);

        foreach (PreguntaData question in kahoot.preguntas)
        {
            AddQuestion(question);
        }
    }

    public void AddQuestion()
    {
        QuestionEditorCard newQuestion = Instantiate(questionPrefab, questionsContent);
        
        questions.Add(newQuestion);
    }

    private void AddQuestion(PreguntaData question)
    {
        QuestionEditorCard newQuestion = Instantiate(questionPrefab, questionsContent);

        newQuestion.SetQuestionData(question);

        questions.Add(newQuestion);
    }

    public void DeleteQuestion()
    {
        if (questions.Count <= 1)
        {
            return;
        }

        int lastIndex = questions.Count - 1;

        Destroy(questions[lastIndex].gameObject);
        questions.RemoveAt(lastIndex);
    }

    public void SaveKahoot()
    {
        if (string.IsNullOrWhiteSpace(titleInput.text))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(descriptionInput.text))
        {
            return;
        }

        KahootData kahoot = new KahootData();

        kahoot.titulo = titleInput.text;
        kahoot.descripcion = descriptionInput.text;
        kahoot.imagen = imageSelector.GetSelectedImage();
        kahoot.preguntas = new List<PreguntaData>();

        foreach (QuestionEditorCard card in questions)
        {
            if (!card.TryGetQuestionData(out PreguntaData question, out string error))
            {
                Debug.LogWarning(error);
                return;
            }

            kahoot.preguntas.Add(question);
        }

        string json = JsonUtility.ToJson(kahoot, true);

        string path = "";

        switch (AppManager.Instance.EditorMode)
        {
            case KahootEditorMode.CREATE:
            {
                string folder = Application.persistentDataPath + "/kahoots";

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string fileName = GetUniqueFileName(folder, kahoot.titulo);

                path = Path.Combine(folder, fileName + ".json");
                break;
            }

            case KahootEditorMode.EDIT:
            {
                path = AppManager.Instance.CurrentKahootPath;

                if (path.StartsWith("Resources/"))
                {
                    string folder = Application.persistentDataPath + "/kahoots";

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    path = Path.Combine(folder, kahoot.titulo + ".json");
                }
                break;
            }

            default:
                return;
        }

        File.WriteAllText(path, json);

        AppManager.Instance.SetCurrentKahoot(kahoot, json, path);

        ChangeSceneManager.Instance.GoToEditorMenu();
    }

    private string GetUniqueFileName(string folder, string title)
    {
        string fileName = string.Join("_", title.Split(Path.GetInvalidFileNameChars()));

        string uniqueName = fileName;
        int number = 1;

        while (File.Exists(Path.Combine(folder, uniqueName + ".json")))
        {
            uniqueName = fileName + "_" + number;
            number++;
        }

        return uniqueName;
    }

    public void DiscardKahoot()
    {
        ChangeSceneManager.Instance.GoToEditorMenu();
    }
}
