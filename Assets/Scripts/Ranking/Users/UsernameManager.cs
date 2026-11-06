using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UsernameManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInput;

    [Header("Background Shapes")]
    [SerializeField] private RectTransform backgroundShapes;
    [SerializeField] private float movementRadiusX = 80f;
    [SerializeField] private float movementRadiusY = 60f;
    [SerializeField] private float movementSpeed = 0.15f;
    [SerializeField] private float irregularity = 0.35f;

    private Vector2 initialPosition;
    private float randomOffset;

    private void Start()
    {
        initialPosition = backgroundShapes.anchoredPosition;
        randomOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        MoveBackgroundShapes();
    }

    private void MoveBackgroundShapes()
    {
        float time = Time.time * movementSpeed + randomOffset;

        float x = Mathf.Cos(time) +
                  Mathf.Sin(time * 2.3f) * irregularity;

        float y = Mathf.Sin(time) +
                  Mathf.Cos(time * 1.7f) * irregularity;

        Vector2 offset = new Vector2(
            x * movementRadiusX,
            y * movementRadiusY
        );

        backgroundShapes.anchoredPosition = initialPosition + offset;
    }

    public void StartGame()
    {
        string username = usernameInput.text.Trim();

        if (string.IsNullOrEmpty(username))
        {
            return;
        }

        PlayerSession.StartSession(username);

        SceneManager.LoadScene("KahootSelector");
    }

    public void ReturnToMainMenu()
    {
        UIManager.Instance.GoToMainMenu();
    }
}
